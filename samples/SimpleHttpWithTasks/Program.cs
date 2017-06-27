﻿#define USE_ASYNC_SEND
#define USE_ASYNC_RECEIVE
// #define USING_TASK_SOCKETS
//#define USE_BUFFER_BLOCKCOPY
//#define USE_SMALL_RESPONSE
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using SimpleHttpServer;

namespace SimpleHttpServer
{
    public class Program
    {
        public static readonly byte[] s_contentText = Encoding.UTF8.GetBytes("Hello World!\r\n");

        public static void Main(string[] args)
        {
            var server = new Server(5000, UserCode);
            server.Run();
        }

        public static void UserCode(HttpContext context)
        {
            if (context.Request.PathEquals("/plaintext"))
                context.Response.WriteUtf8(s_contentText);
        }
    }
}

public class Server
{
    public Server(int port, Action<HttpContext> body)
    {
        Console.WriteLine("Listening at port {0}", port);
        _body = body;
        _listenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        _listenSocket.Bind(new IPEndPoint(IPAddress.Any, port));
    }

    /// <summary>
    /// Listen to the HTTP port and  respond to requests.  
    /// </summary>
    public void Run()
    {
        _listenSocket.Listen(1000);
        for (;;)
        {
            // Console.WriteLine("** Trying to get a new connection");
            var connectionSocket = _listenSocket.Accept();

            // This automatically inserts itself into _activeConnections. 
            new Connection(connectionSocket, this);
        }
    }

    #region private 
    class Connection
    {
        internal Connection(Socket connectionSocket, Server server)
        {
            _server = server;
            _socket = connectionSocket;
            _socket.NoDelay = true;
#if USING_TASK_SOCKETS
            _readTask = ProcessRequests();
#else
            ProcessRequests();
#endif
            // TODO insert into _server._activeConnections list;
        }

#if USING_TASK_SOCKETS
        async Task ProcessRequests()
        {
            using (_socket)
            {
                byte[] buffer = new byte[1024];

                for (;;)
                {
                    int bytesRead = await _socket.ReceiveAsync(new ArraySegment<byte>(buffer), SocketFlags.None);
                    // Console.WriteLine("****** REQUEST {0} bytes: {1}", bytesRead, buffer.AsString(bytesRead).Replace("\r\n", "\r\n      "));
                    if (bytesRead == 0)
                    {
                        // Console.WriteLine("** Closing Connection");
                        break;
                    }
                    ParseAndReply(buffer, 0, bytesRead);
                }
            }
            // TODO remove from _server._activeConnectons list.  
        }
#else
        void ProcessRequests()
        {
            _readArgs = new SocketAsyncEventArgs();
            _readArgs.SetBuffer(new byte[1024], 0, 1024);
            _readArgs.Completed += OnRead;
            bool pending = _socket.ReceiveAsync(_readArgs);
            if (!pending)
                OnRead(null, _readArgs);
        }

        private void OnRead(object sender, SocketAsyncEventArgs e)
        {
            for (;;)
            {
                if (e.SocketError != SocketError.Success)
                {
                    if (e.SocketError == SocketError.ConnectionReset)
                    {
                        _socket.Dispose();
                        return;
                    }
                    throw new Exception(string.Format("read failed, error = {0}", e.SocketError));
                }

                int bytesRead = e.BytesTransferred;
                if (bytesRead == 0)
                {
                    _socket.Dispose();
                    return;
                }
                ParseAndReply(e.Buffer, e.Offset, bytesRead);

                // If it is not pending we can loop and process the next one.  
                var pending = _socket.ReceiveAsync(_readArgs);
                if (pending)
                    break;
            }
        }

#endif

        void ParseAndReply(byte[] buffer, int offset, int bytesRead)
        {

            Request request = new Request() { _buffer = buffer };
            Response response = new Response();
            HttpContext context = new HttpContext() { Request = request, Response = response };

            // Parse;
            int startParse = offset;
            do
            {
                int endParsed = RequestParser.ParseRequest(buffer, startParse, bytesRead, ref request.parseResult);
                if (!request.parseResult._parseOK)
                {
                    response.WriteUtf8(s_badRequestText);
                    Console.WriteLine("bad request");
                    break;
                }

                // Do the callback 
                _server._body(context);

                if (response.Length == 0)
                {
                    response.WriteUtf8(s_notFoundText);
                    Console.WriteLine("not found");
                }

                // Send reply
                response.UpdateContentLength();
                // Console.WriteLine("  ** REPLYING {0} bytes : {1}", response.Length, response.Buffer.AsString(response.Length).Replace("\r\n", "\r\n      "));
                _socket.Send(response.Buffer, response.Length, SocketFlags.None);

                startParse = endParsed;
                request.parseResult.Reset();
                response.Clear();
            }
            while (startParse < bytesRead);
        }

        public static readonly byte[] s_badRequestText = Encoding.UTF8.GetBytes("HTTP/1.1 400 Bad Request\r\n\r\n");
        public static readonly byte[] s_notFoundText = Encoding.UTF8.GetBytes("HTTP/1.1 404 Page not found\r\n\r\n");
#if USING_TASK_SOCKETS
        Task _readTask;
#else
        SocketAsyncEventArgs _readArgs;
#endif
        Server _server;  // The server that this connection came from
        Socket _socket;  // Where to read and write to. 

        // Connection _next;   // for linked list
        // Connection _prev;
    }

    // internal Connection _activeConnections;         // Doubly Linked list of connnections;
    internal Socket _listenSocket;
    internal Action<HttpContext> _body;
    #endregion
}

public class HttpContext
{
    public Request Request { get; internal set; }
    public Response Response { get; internal set; }
}

public class Request
{
    public bool PathEquals(string path) { return parseResult.PathEquals(_buffer, path); }

    #region private
    internal ParseResult parseResult;
    internal byte[] _buffer;
    #endregion
}

public class Response
{
    public void WriteUtf8(byte[] b)
    {
        if (_first)
        {
            _first = false;
            AppendResponseHeader();
        }

        if (_responseBuffer.Length < _responseLength + b.Length)
            GrowBuffer(_responseLength + b.Length);
#if USE_BUFFER_BLOCKCOPY
                Buffer.BlockCopy(b, 0, _responseBuffer, _responseLength, b.Length);
#else
        byte[] responseBuffer = _responseBuffer;
        int offset = _responseLength;
        for (int i = 0; i < b.Length; i++)
            responseBuffer[offset + i] = b[i];
#endif
        _responseLength += b.Length;
    }

    #region private
    internal Response()
    {
        _responseBuffer = new byte[1024];
        Clear();
    }
    internal byte[] Buffer { get { return _responseBuffer; } }

    internal int Length { get { return _responseLength; } }

    internal void Clear()
    {
        _responseLength = 0;
        _contentLengthOffset = -1;
        _first = true;
    }

    internal void UpdateContentLength()
    {
        int length = _responseLength - _contentStart;
        do
        {
            _responseBuffer[--_contentLengthOffset] = (byte)(length % 10 + '0');
            length = length / 10;
        } while (0 < length);
    }

    void AppendResponseHeader()
    {
        WriteUtf8(s_okText);
        WriteUtf8(s_serverText);
        WriteUtf8(s_contentTypePlainText);
        WriteUtf8(s_contentLengthText);
        _contentLengthOffset = _responseLength - 2; // Right before \r\n
        AppendDate();
        WriteUtf8(s_crlf);
        _contentStart = _responseLength;
    }

    internal void AppendDate()
    {
        int tickCount = Environment.TickCount;
        if (tickCount != s_lastTickCount || s_lastNowText == null)
        {
            DateTime now = DateTime.Now;
            string nowString = now.ToString();
            s_lastNowText = Encoding.UTF8.GetBytes(nowString);
            s_lastTickCount = tickCount;
        }
        WriteUtf8(s_dateText);
        WriteUtf8(s_lastNowText);
        WriteUtf8(s_crlf);
    }

    void GrowBuffer(int minLength)
    {
        byte[] oldBuffer = _responseBuffer;
        int newLength = oldBuffer.Length * 2 < minLength ? minLength : oldBuffer.Length * 2;
        byte[] newBuffer = new byte[newLength];
        for (int i = 0; i < oldBuffer.Length; i++)
            newBuffer[i] = oldBuffer[i];
        _responseBuffer = newBuffer;
    }

    public static readonly byte[] s_okText = Encoding.UTF8.GetBytes("HTTP/1.1 200 OK\r\n");
    static readonly byte[] s_serverText = Encoding.UTF8.GetBytes("Server: TestServer\r\n");
    static readonly byte[] s_contentTypePlainText = Encoding.UTF8.GetBytes("Content-Type: text/plain\r\n");
    static readonly byte[] s_contentLengthText = Encoding.UTF8.GetBytes("Content-Length:      \r\n");
    static readonly byte[] s_dateText = Encoding.UTF8.GetBytes("Date: ");
    static readonly byte[] s_crlf = Encoding.UTF8.GetBytes("\r\n");

    static int s_lastTickCount;
    static byte[] s_lastNowText;

    byte[] _responseBuffer;
    int _responseLength;
    int _contentLengthOffset;       // Points to just past the ContentLength LEAST significand digit goes (max 5 digits)
    int _contentStart;              // Where content starts. 
    bool _first;
    #endregion
}

// For debugging, make it easy to see byte[] as UTF8 strings.  
public static class Extensions
{
    public static string AsUtf8(this byte[] bytes, int len = -1)
    {
        if (len < 0)
            len = bytes.Length;
        return Encoding.UTF8.GetString(bytes, 0, len);
    }

    public static string Utf8At(this byte[] bytes, int offset, int len = -1)
    {
        if (len < 0)
            len = bytes.Length - offset;
        return Encoding.UTF8.GetString(bytes, offset, len);
    }
}
