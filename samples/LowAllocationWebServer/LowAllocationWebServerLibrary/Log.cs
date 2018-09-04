// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Net;
using System.Buffers;
using static System.Buffers.Text.Encodings;

namespace System.Diagnostics
{
    public abstract class Log
    {
        public Level CurrentLevel;
        public bool IsVerbose { get { return CurrentLevel >= Level.Verbose; } }

        public abstract void LogMessage(Level level, string message);
        public virtual void LogMessage(Level level, string format, params object[] args)
        {
            LogMessage(level, String.Format(format, args));
        }

        public void LogVerbose(string message)
        {
            if (CurrentLevel >= Level.Verbose)
            {
                LogMessage(Level.Verbose, message);
            }
        }
        public void LogWarning(string message)
        {
            if (CurrentLevel >= Level.Warning)
            {
                LogMessage(Level.Warning, message);
            }
        }
        public void LogError(string message)
        {
            if (CurrentLevel >= Level.Error)
            {
                LogMessage(Level.Error, message);
            }
        }

        public enum Level
        {
            Off = -1,
            Error = 0,
            Warning = 10,
            Verbose = 20,
        }
    }

    public class ConsoleLog : Log
    {
        object s_lock = new object();

        public ConsoleLog(Log.Level level)
        {
            CurrentLevel = level;
        }
        public override void LogMessage(Level level, string message)
        {
            lock (s_lock)
            {
                ConsoleColor oldColor = Console.ForegroundColor;
                switch (level)
                {
                    case Level.Error:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("ERROR: ");
                        break;
                    case Level.Warning:
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write("WARNING: ");
                        break;
                }

                Console.WriteLine(message);
                Console.ForegroundColor = oldColor;
            }
        }
    }

    public static class HttpLogExtensions
    {
        public static void LogRequest(this Log log, HttpRequest request, ReadOnlySequence<byte> body)
        {
            if (log.IsVerbose)
            {
                // TODO: this is much ceremony. We need to do something with this. ReadOnlyBytes.AsUtf8 maybe?
                log.LogMessage(Log.Level.Verbose, "\tMethod:       {0}", request.Method);
                log.LogMessage(Log.Level.Verbose, "\tRequest-URI:  {0}", request.Path);
                log.LogMessage(Log.Level.Verbose, "\tHTTP-Version: {0}", request.Version);

                log.LogMessage(Log.Level.Verbose, "\tHttp Headers:");
                var headers = request.Headers;
                for (int i = 0; i < headers.Length; i++)
                {
                    var header = headers[i];
                    log.LogMessage(Log.Level.Verbose, "\t\t{0}: {1}", Utf8.ToString(header.Name), Utf8.ToString(header.Value));
                }

                // TODO: Logger should support Utf8Span
                log.LogMessage(Log.Level.Verbose, Utf8.ToString(body.First.Span));
            }
        }
    }
}
