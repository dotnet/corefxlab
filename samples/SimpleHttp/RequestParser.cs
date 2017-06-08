#define SAFECODE
using System;
using System.Runtime.CompilerServices;

namespace SimpleHttpServer
{
    enum HttpMethod
    {
        Get,
    }
    struct ParseResult
    {
        public HttpMethod _method;
        public bool _parseOK;
        public int _pathStart;
        public int _pathEnd;
        public int _headerCount;
        public int[] _lineStart;
        public int[] _valueStart;

        void GrowArray(ref int[] a, int length)
        {
            if (a == null)
                a = new int[5];
            if (a.Length <= length)
            {
                int[] n = new int[length * 2];
                for (int i = 0; i < a.Length; i++)
                {
                    n[i] = a[i];
                }
                a = n;
            }
        }
        public void GrowHeaders()
        {
            GrowArray(ref _lineStart, _headerCount);
            GrowArray(ref _valueStart, _headerCount);
        }

        public void Reset()
        {
            _parseOK = false;
            _headerCount = 0;
        }

        public bool PathEquals(byte[] buffer, byte[] knownPath)
        {
            if (_pathEnd - _pathStart != knownPath.Length)
                return false;

            for (int i = 0; i < knownPath.Length; i++)
            {
                if (buffer[_pathStart + i] != knownPath[i])
                    return false;
            }
            return true;
        }
    }
    class RequestParser
    {
#if SAFECODE
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static int ParseRequest(byte[] buffer, int start, int end, ref ParseResult parseResult)
        {
            int cur = start;

            // "GET "?
            if (end - cur >= 4 && buffer[cur + 0] == (byte)'G' && buffer[cur + 1] == (byte)'E' && buffer[cur + 2] == (byte)'T' && buffer[cur + 3] == (byte)' ')
            {
                cur += 4;

                parseResult._method = HttpMethod.Get;

                parseResult._pathStart = cur;

                // parse the URI
                while (cur < end)
                {
                    // loop 3x unrolled to reduce number of taken branches
                    byte b = buffer[cur++];
                    if (b > (byte)' ')
                    {
                        if (cur >= end)
                            break;
                        b = buffer[cur++];
                        if (b > (byte)' ')
                        {
                            if (cur >= end)
                                break;
                            b = buffer[cur++];
                            if (b > (byte)' ')
                                continue;
                        }
                    }

                    if (b == (byte)' ')
                        break;

                    return cur;
                }
                parseResult._pathEnd = cur-1;


                // check for "HTTP/1.1"
                if (end - cur >= 8 && buffer[cur + 0] == (byte)'H' && buffer[cur + 1] == (byte)'T' && buffer[cur + 2] == (byte)'T' && buffer[cur + 3] == (byte)'P'
                                   && buffer[cur + 4] == (byte)'/' && buffer[cur + 5] == (byte)'1' && buffer[cur + 6] == (byte)'.' && buffer[cur + 7] == (byte)'1')
                {
                    cur += 8;
                }
                else
                {
                    return cur;
                }

                // check for \r\n
                if (end - cur >= 2 && buffer[cur + 0] == (byte)'\r' && buffer[cur + 1] == (byte)'\n')
                    cur += 2;
                else
                    return cur;

                // parse header lines
                int line = 0;
                int lineStart = cur;
                bool colonSeen = false;
                int valueStart = 0;
                while (cur < end)
                {
                    // loop 3x unrolled to reduce number of taken branches
                    byte b = buffer[cur++];
                    if (b > (byte)':')
                    {
                        if (cur >= end)
                            break;
                        b = buffer[cur++];
                        if (b > (byte)':')
                        {
                            if (cur >= end)
                                break;
                            b = buffer[cur++];
                            if (b > (byte)':')
                                continue;
                        }
                    }
                    if (b == (byte)':')
                    {
                        if (lineStart < cur)
                        {
                            colonSeen = true;
                            valueStart = cur;
                        }
                        else
                        {
                            Console.WriteLine("empty key");
                            return cur;
                        }
                    }
                    if (b < (byte)' ')
                    {
                        if (b == (byte)'\r')
                        {
                            if (lineStart == cur - 1 && cur < end && buffer[cur++] == (byte)'\n')
                            {
                                if (parseResult._lineStart == null || parseResult._lineStart.Length <= parseResult._headerCount)
                                    parseResult.GrowHeaders();
                                parseResult._lineStart[parseResult._headerCount] = lineStart;
                                parseResult._parseOK = true;
                                return cur;
                            }

                            if (!colonSeen || valueStart == cur)
                            {
                                Console.WriteLine("empty value {0} {1} {2} {3}", lineStart, colonSeen, valueStart, cur);
                                return cur;
                            }
                            if (parseResult._lineStart == null || parseResult._lineStart.Length <= parseResult._headerCount)
                                parseResult.GrowHeaders();
                            parseResult._lineStart[parseResult._headerCount] = lineStart;
                            parseResult._valueStart[parseResult._headerCount] = valueStart;
                            parseResult._headerCount++;
                            colonSeen = false;
                            if (cur < end && buffer[cur++] == (byte)'\n')
                                line++;
                            else
                                break;
                            lineStart = cur;
                            continue;
                        }
                        break;
                    }
                }
            }
            return cur;
        }
#else
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static bool ParseRequest(byte[] buffer, int start, int end)
        {
            unsafe
            {
                fixed (byte* pRequest = buffer)
                {
                    return ParseRequest(pRequest + start, pRequest + end);
                }
            }
        }

        static unsafe bool ParseRequest(byte* request, byte* requestEnd)
        {
            // "GET "?
            if (requestEnd - request >= 4 && request[3] == (byte)' ' && request[0] == (byte)'G' && request[1] == (byte)'E' && request[2] == (byte)'T')
            {
                request += 4;

                // parse the URI
                while (request < requestEnd)
                {
                    // loop 3x unrolled to reduce number of taken branches
                    byte b = *request++;
                    if (b > (byte)' ')
                    {
                        if (request >= requestEnd)
                            break;
                        b = *request++;
                        if (b > (byte)' ')
                        {
                            if (request >= requestEnd)
                                break;
                            b = *request++;
                            if (b > (byte)' ')
                                continue;
                        }
                    }

                    if (b == (byte)' ')
                        break;

                    return false;
                }


                // check for "HTTP/1.1"
                if (requestEnd - request >= 8 && request[0] == (byte)'H' && request[1] == (byte)'T' && request[2] == (byte)'T' && request[3] == (byte)'P'
                                              && request[4] == (byte)'/' && request[5] == (byte)'1' && request[6] == (byte)'.' && request[7] == (byte)'1')
                {
                    request += 8;
                }
                else
                {
                    return false;
                }

                // check for \r\n
                if (requestEnd - request >= 2 && request[0] == (byte)'\r' && request[1] == (byte)'\n')
                {
                    request += 2;
                }
                else
                    return false;

                // parse header lines
                int line = 0;
                byte* lineStart = request;
                bool colonSeen = false;
                byte* valueStart = null;
                while (request < requestEnd)
                {
                    // loop 3x unrolled to reduce number of taken branches
                    byte b = *request++;
                    if (b > (byte)':')
                    {
                        if (request >= requestEnd)
                            break;
                        b = *request++;
                        if (b > (byte)':')
                        {
                            if (request >= requestEnd)
                                break;
                            b = *request++;
                            if (b > (byte)':')
                                continue;
                        }
                    }
                    if (b == (byte)':')
                    {
                        if (lineStart < request - 1)
                        {
                            colonSeen = true;
                            valueStart = request - 1;
                        }
                        else
                        {
                            Console.WriteLine("empty key");
                            return false;
                        }
                    }
                    if (b < (byte)' ')
                    {
                        if (b == (byte)'\r')
                        {
                            if (lineStart == request - 1 && request < requestEnd && *request++ == (byte)'\n')
                                return request == requestEnd;

                            if (!colonSeen || valueStart == request - 1)
                            {
                                Console.WriteLine("empty value {0} {1} {2} {3}", (long)lineStart, colonSeen, (long)valueStart, (long)request);
                                return false;
                            }
                            colonSeen = false;
                            if (request < requestEnd && *request++ == (byte)'\n')
                            {
                                line++;
                            }
                            else
                                break;
                            lineStart = request;
                            continue;
                        }
                        break;
                    }
                }
            }
            return false;
        }
#endif

    }
}
