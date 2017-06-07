#define USE_ASYNC_SEND
#define USE_ASYNC_RECEIVE
//#define USE_BUFFER_BLOCKCOPY
//#define USE_SMALL_RESPONSE
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace SimpleHttpServer
{
    public class Program
    {
        public static Socket s_listenSocket;
        public const bool s_trace = false;

        public static readonly byte[] s_plainTextPath = Encoding.UTF8.GetBytes("/plaintext");
        public static readonly byte[] s_okText = Encoding.UTF8.GetBytes("HTTP/1.1 200 OK\r\n");
        public static readonly byte[] s_serverText = Encoding.UTF8.GetBytes("Server: TestServer\r\n");
        public static readonly byte[] s_contentTypePlainText = Encoding.UTF8.GetBytes("Content-Type: text/plain\r\n");
        public static readonly byte[] s_contentLengthText = Encoding.UTF8.GetBytes("Content-Length: ");
        public static readonly byte[] s_crlf = Encoding.UTF8.GetBytes("\r\n");
        public static readonly byte[] s_dateText = Encoding.UTF8.GetBytes("Date: ");
        public static readonly byte[] s_contentText = Encoding.UTF8.GetBytes("Hello World!\r\n");
        public static readonly byte[] s_badRequestText = Encoding.UTF8.GetBytes("HTTP/1.1 400 Bad Request\r\n\r\n");
        public static readonly byte[] s_notFoundText = Encoding.UTF8.GetBytes("HTTP/1.1 404 Page not found\r\n\r\n");

        class Response
        {
            byte[] _responseBuffer;
            int _responseLength;

            public Response(byte[] responseBuffer)
            {
                _responseBuffer = responseBuffer;
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

            public void AppendBytes(byte[] b)
            {
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

            void AppendByte(byte b)
            {
                if (_responseBuffer.Length < _responseLength + 1)
                    GrowBuffer(_responseLength + 1);

                _responseBuffer[_responseLength] = b;
                _responseLength += 1;
            }

            void AppendUnsigned(uint i)
            {
                if (i >= 10)
                    AppendUnsigned(i / 10);
                AppendByte((byte)(0x30 + (i % 10)));
            }

            public void AppendServer()
            {
                AppendBytes(s_serverText);
            }

            public void AppendContentLength(int length)
            {
                if (length <= 0)
                    throw new Exception("internal error");
                AppendBytes(s_contentLengthText);
                AppendUnsigned((uint)length);
                AppendBytes(s_crlf);
            }

            public void AppendContentTypePlainText()
            {
                AppendBytes(s_contentTypePlainText);
            }

            static int s_lastTickCount;
            static byte[] s_lastNowText;

            public void AppendDate()
            {
                int tickCount = Environment.TickCount;
                if (tickCount != s_lastTickCount || s_lastNowText == null)
                {
                    DateTime now = DateTime.Now;
                    string nowString = now.ToString();
                    s_lastNowText = Encoding.UTF8.GetBytes(nowString);
                    s_lastTickCount = tickCount;
                }
                AppendBytes(s_dateText);
                AppendBytes(s_lastNowText);
                AppendBytes(s_crlf);
            }

            public byte[] GetBuffer()
            {
                return _responseBuffer;
            }

            public int GetLength()
            {
                return _responseLength;
            }
        }

        class Connection
        {
            private SocketAsyncEventArgs _acceptEventArgs;
            private SocketAsyncEventArgs _readEventArgs;
            private SocketAsyncEventArgs _writeEventArgs;

            private Socket _socket;
            // buffer?

            public Connection()
            {
                _acceptEventArgs = new SocketAsyncEventArgs();
                _acceptEventArgs.Completed += OnAccept;


                _readEventArgs = new SocketAsyncEventArgs();
                _readEventArgs.SetBuffer(new byte[4096], 0, 4096);
                _readEventArgs.Completed += OnRead;

                _writeEventArgs = new SocketAsyncEventArgs();
                _writeEventArgs.SetBuffer(new byte[4096], 0, 4096);
                _writeEventArgs.Completed += OnWrite;
            }

            public void DoAccept()
            {
#if ASYNC_ACCEPT
                bool pending = s_listenSocket.AcceptAsync(_acceptEventArgs);
                if (!pending)
                    OnAccept(null, _acceptEventArgs);
#else
                _socket = s_listenSocket.Accept();

                QueueConnectionHandler();

                _socket.NoDelay = true;

                DoRead();
#endif
            }

            private void OnAccept(object sender, SocketAsyncEventArgs e)
            {
                if (e.SocketError != SocketError.Success)
                {
                    throw new Exception("accept failed");
                }

                if (s_trace)
                {
                    Console.WriteLine("Connection accepted");
                }

                // Spawn another work item to handle next connection
                QueueConnectionHandler();

                _socket = e.AcceptSocket;
                _socket.NoDelay = true;

                DoRead();
            }

            private int FindEndRequestBackwards(byte[] buffer, int len)
            {
                while (len >= 4)
                {
                    if (buffer[len - 1] == (byte)'\n' && buffer[len - 2] == (byte)'\r' &&
                        buffer[len - 3] == (byte)'\n' && buffer[len - 4] == (byte)'\r')
                    {
                        return len;
                    }
                    len -= 1;
                }
                return 0;
            }

            private void DoRead()
            {
#if USE_ASYNC_RECEIVE
                bool pending = _socket.ReceiveAsync(_readEventArgs);
                if (!pending)
                {
                    if (s_trace)
                    {
                        Console.WriteLine("Read completed synchronously");
                    }

                    OnRead(null, _readEventArgs);
                }
#else
                try
                {
                    int tailBytes = _readEventArgs.Offset;
                    while (true)
                    {
                        int bytesRead = _socket.Receive(_readEventArgs.Buffer, tailBytes, _readEventArgs.Buffer.Length - tailBytes, SocketFlags.None);
                        if (bytesRead == 0)
                        {
                            if (s_trace)
                            {
                                Console.WriteLine("Connection closed by client");
                            }

                            _socket.Dispose();
                            return;
                        }

                        if (s_trace)
                        {
                            Console.WriteLine("Read complete, bytesRead = {0}", bytesRead);
                        }
                        int bytesInBuffer = tailBytes + bytesRead;
                        int endRequest = FindEndRequestBackwards(_readEventArgs.Buffer, bytesInBuffer);
                        Response response = null;
                        if (endRequest > 0)
                            response = ParseRequestsBuildResponse(_readEventArgs.Buffer, endRequest, _writeEventArgs.Buffer);
                        if (endRequest < bytesInBuffer)
                        {
                            // ok, we have an incomplete request at the end - copy it to the beginning of the buffer
                            tailBytes = bytesInBuffer - endRequest;
                            Buffer.BlockCopy(_readEventArgs.Buffer, endRequest, _readEventArgs.Buffer, 0, tailBytes);
                        }
                        else
                        {
                            tailBytes = 0;
                        }
                        if (response != null)
                        {
                            if (s_trace)
                            {
                                byte[] b = response.GetBuffer();
                                int len = response.GetLength();
                                for (int i = 0; i < len; i++)
                                {
                                    Console.Write((char)b[i]);
                                }
                            }

#if USE_ASYNC_SEND
                            byte[] buffer = _readEventArgs.Buffer;
                            _readEventArgs.SetBuffer(buffer, tailBytes, buffer.Length - tailBytes);
                            _writeEventArgs.SetBuffer(response.GetBuffer(), 0, response.GetLength());
                            bool pending = _socket.SendAsync(_writeEventArgs);
                            if (!pending)
                            {
                                if (s_trace)
                                {
                                    Console.WriteLine("Write completed synchronously");
                                }

                                OnWrite(null, _writeEventArgs);
                            }
                            break;
#else
                            // Do write now
                            _socket.Send(response.GetBuffer(), response.GetLength(), SocketFlags.None);
#endif
                        }
                    }
                }
                catch (System.Net.Sockets.SocketException)
                {
                }
#endif
            }

#if USE_SMALL_RESPONSE
            // minimum reasonable response
            private void AppendPlainTextResponse(Response response)
            {
                response.AppendBytes(s_okText);
                response.AppendContentLength(s_contentText.Length);
                response.AppendBytes(s_crlf);
                response.AppendBytes(s_contentText);
            }
#else
            // realistic response
            private void AppendPlainTextResponse(Response response)
            {
                response.AppendBytes(s_okText);
                response.AppendServer();
                response.AppendContentTypePlainText();
                response.AppendContentLength(s_contentText.Length);
                response.AppendDate();
                response.AppendBytes(s_crlf);
                response.AppendBytes(s_contentText);
            }
#endif
            private Response ParseRequestsBuildResponse(byte[] requestBuffer, int bytesRead, byte[] responseBuffer)
            {
                ParseResult parseResult = new ParseResult();
                Response response = new Response(responseBuffer);
                int startParse = 0;
                do
                {
                    int endParsed = RequestParser.ParseRequest(requestBuffer, startParse, bytesRead, ref parseResult);
                    if (!parseResult._parseOK)
                    {
                        response.AppendBytes(s_badRequestText);
                        Console.WriteLine("bad request");
                        break;
                    }
                    else if (parseResult.PathEquals(requestBuffer, s_plainTextPath))
                    {
                        AppendPlainTextResponse(response);
                    }
                    else
                    {
                        response.AppendBytes(s_notFoundText);
                        Console.WriteLine("not found");
                    }
                    startParse = endParsed;
                    parseResult.Reset();
                }
                while (startParse < bytesRead);

                return response;
            }

            private void OnRead(object sender, SocketAsyncEventArgs e)
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
                    if (s_trace)
                    {
                        Console.WriteLine("Connection closed by client");
                    }

                    _socket.Dispose();
                    return;
                }

                if (s_trace)
                {
                    Console.WriteLine("Read complete, bytesRead = {0}", bytesRead);
                }

                int bytesInBuffer =_readEventArgs.Offset + bytesRead;
                int endRequest = FindEndRequestBackwards(_readEventArgs.Buffer, bytesInBuffer);
                Response response = null;
                if (endRequest > 0)
                    response = ParseRequestsBuildResponse(_readEventArgs.Buffer, endRequest, _writeEventArgs.Buffer);
                byte[] buffer = _readEventArgs.Buffer;
                if (endRequest < bytesInBuffer)
                {
                    // ok, we have an incomplete request at the end - copy it to the beginning of the buffer
                    int tailBytes = bytesInBuffer - endRequest;
                    Buffer.BlockCopy(buffer, endRequest, buffer, 0, tailBytes);
                    _readEventArgs.SetBuffer(buffer, tailBytes, buffer.Length - tailBytes);
                }
                else
                {
                    _readEventArgs.SetBuffer(buffer, 0, buffer.Length);
                }

                if (response != null)
                {
                    _writeEventArgs.SetBuffer(response.GetBuffer(), 0, response.GetLength());

                    if (s_trace)
                    {
                        byte[] b = response.GetBuffer();
                        int len = response.GetLength();
                        for (int i = 0; i < len; i++)
                        {
                            Console.Write((char)b[i]);
                        }
                    }

                    // Do write now
#if USE_ASYNC_SEND
                    bool pending = _socket.SendAsync(_writeEventArgs);
                    if (!pending)
                    {
                        if (s_trace)
                        {
                            Console.WriteLine("Write completed synchronously");
                        }

                        OnWrite(null, _writeEventArgs);
                    }
#else
                    _socket.Send(response.GetBuffer(), response.GetLength(), SocketFlags.None);
                    DoRead();
#endif
                }
                else
                {
                    // we didn't have complete request - keep reading
                    DoRead();
                }
            }

            private void OnWrite(object sender, SocketAsyncEventArgs e)
            {
                if (e.SocketError != SocketError.Success)
                {
                    throw new Exception("write failed");
                }

                int bytesWritten = e.BytesTransferred;

                if (false/*bytesWritten != s_responseMessage.Length*/)
                {
                    throw new Exception(string.Format("unexpected write size, bytesWritten = {0}", bytesWritten));
                }

                if (s_trace)
                {
                    Console.WriteLine("Write complete, bytesWritten = {0}", bytesWritten);
                }

                DoRead();
            }
        }

        private static void HandleConnection(object state)
        {
            var c = new Connection();
            c.DoAccept();
        }

        private static void QueueConnectionHandler()
        {
            ThreadPool.QueueUserWorkItem(HandleConnection);
        }

        private static void Start()
        {
            s_listenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            s_listenSocket.Bind(new IPEndPoint(IPAddress.Any, 5000));
            s_listenSocket.Listen(1000);

            QueueConnectionHandler();
        }

        public static void Main(string[] args)
        {
            Start();

            Console.WriteLine("Server Running");
            Console.ReadLine();
        }
    }
}
