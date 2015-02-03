// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

internal class Program
{
    private static void Main(string[] args)
    {
        int squareSize = 20;

        // If the user specified an argument, we'll use it as the size
        // of the squares.

        if (args.Length == 1)
        {
            int.TryParse(args[0], out squareSize);
            squareSize = Math.Max(1, Math.Min(40, squareSize));
        }

        // Draw flag

        const char filled = '@';

        var colors = new[] { ConsoleColor.Red, ConsoleColor.Green, ConsoleColor.Blue, ConsoleColor.Yellow };
        for (int row = 0; row < 2; row++)
        {
            for (int i = 0; i < squareSize / 2; i++)
            {
                Console.WriteLine();
                Console.Write("  ");
                for (int col = 0; col < 2; col++)
                {
                    Console.ForegroundColor = colors[row * 2 + col];
                    for (int j = 0; j < squareSize; j++) Console.Write(filled);
                    Console.ResetColor();
                    Console.Write(" ");
                }
            }
        }
        Console.WriteLine();
        Console.WriteLine();
        Console.WriteLine("Press ENTER to exit ...");
        Console.ReadLine();
    }
}