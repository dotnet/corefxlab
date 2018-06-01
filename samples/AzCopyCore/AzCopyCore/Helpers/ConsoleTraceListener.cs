// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace System.Diagnostics
{
    class ConsoleTraceListener : TraceListener
    {
        public override void Write(string message)
            => Console.Write(message);

        public override void WriteLine(string message)
            => Console.WriteLine(message);

        public override void Fail(string message)
        {
            base.Fail(message);
        }
        public override void Fail(string message, string detailMessage)
        {
            base.Fail(message, detailMessage);
        }
        public override bool IsThreadSafe => false;

        public override void TraceData(TraceEventCache eventCache, string source, TraceEventType eventType, int id, params object[] data)
        {
            ConsoleColor color = default;
            if (eventType == TraceEventType.Error || eventType == TraceEventType.Critical)
            {
                color = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Red;
            }

            Console.WriteLine(eventType.ToString());

            if (eventType == TraceEventType.Error || eventType == TraceEventType.Critical)
            {
                Console.ForegroundColor = color;
            }

            foreach (object item in data)
            {
                Console.Write("\t");
                Console.WriteLine(data);
            }
        }

        public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id, string format, params object[] args)
        {
            ConsoleColor color = default;
            if (eventType == TraceEventType.Error || eventType == TraceEventType.Critical)
            {
                color = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Red;
            }

            Console.WriteLine(format, args);

            if (eventType == TraceEventType.Error || eventType == TraceEventType.Critical)
            {
                Console.ForegroundColor = color;
            }
        }
    }
}


