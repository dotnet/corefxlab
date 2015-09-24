// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;

namespace dotnet
{
    internal class Log
    {
        public bool IsEnabled = false;

        public void WriteLine(string format, params object[] args)
        {
            if (!IsEnabled) return;
            Console.WriteLine(format, args);
        }

        public void Write(string format, params object[] args)
        {
            if (!IsEnabled) return;
            Console.Write(format, args);
        }

        public void WriteList(List<string> list, string listName)
        {
            if (!IsEnabled) return;
            WriteLine("{0}:", listName);
            foreach (var str in list)
            {
                Write("\t");
                WriteLine(str);
            }
        }

        public void Error(string format, params object[] args)
        {
            if (!IsEnabled) return;
            var old = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(format, args);
            Console.ForegroundColor = old;
        }
    }
}
