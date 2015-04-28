// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

internal class Program
{
    private static void Main(string[] args)
    {
        if (args.Length == 1 && args[0] == "linux")
        {
            DrawLinux();
        }
        else if (args.Length == 1 && args[0] == "freebsd")
        {
            DrawFreeBSD();
        }
        else if (args.Length == 1 && args[0] == "mac")
        {
            DrawMac();
        }
        else
        {
            DrawWindows();
        }

        Console.WriteLine();
        Console.WriteLine("Press ENTER to exit ...");
        Console.ReadLine();
    }

    private static void DrawWindows()
    {
        Console.WriteLine("Hello, Windows...");

        const int squareSize = 20;

        var colors = new[] { ConsoleColor.Red, ConsoleColor.Green, ConsoleColor.Blue, ConsoleColor.Yellow };
        for (int row = 0; row < 2; row++)
        {
            for (int i = 0; i < squareSize / 2; i++)
            {
                Console.WriteLine();
                Console.Write("  ");
                for (int col = 0; col < 2; col++)
                {
                    Console.BackgroundColor = colors[row * 2 + col];
                    Console.ForegroundColor = colors[row * 2 + col];
                    for (int j = 0; j < squareSize; j++) Console.Write('@');
                    Console.ResetColor();
                }
            }
        }
        Console.WriteLine();
    }

    private static void DrawLinux()
    {
        Console.WriteLine("Hello, Linux...");

        const string Penguin = @"
                                                     
                        @@@@@                        
                      @@@@@@@@@@                     
                    @@@@@@@@@@@@@                    
                    @@@@@@@@@@@@@@                   
                   @@@@@@@@@@@@@@@@                  
                   @@@@@@@@@@@@@@@@                  
                   @@@@@@@@@@@@@@@@@                 
                  @@@@@@@@@@@@@@@@@@                 
                  @@@@@@@@@@@@@@@@@@                 
                  @@@ @@@@@@@  @@@@@                 
                  @@   @@@@    @@@@@                 
                  @@ @@ @@  @@  @@@@                 
                  @@ @@ @@@ @@@ @@@@                 
                   @ @@---- @@@ @@@@                 
                   @ @-------@  @@@@@                
                   @------------@@@@@                
                   @------------@@@@@                
                   @------------@@@@@                
                   @------------@@@@@                
                   @ ---------  @@@@@@               
                   @  ------    @@@@@@@              
                  @@    --       @@@@@@              
                 @@@             @@@@@@@             
                 @@               @@@@@@             
                @@@               @@@@@@@            
               @@@                 @@@@@@@           
              @@@@                 @@@@@@@@          
             @@@@@                 @@@@@@@@@         
             @@@@@                  @@@@@@@@         
             @@@@                    @@@@@@@@        
            @@@@             *       @@@@@@@@        
            @@@@            ****     @@@@@@@@        
            @@@            *****      @@@@@@@@       
           @@@@     *     ******      @@@@@@@@       
           @@@      **   *** ***      @@@@@@@@       
          @@@@      *******  ***      @@@@@@@@@      
          @@@@      * ****   ***      @@@@@@@@@      
         @@@@@      *******  ***      @@@@@@@@@      
         @@@@@      **   *** ***      @@@@@@@@@      
         @@@@@      *     ******      @@@@@@@@@      
         @@@@@             *****      @@@@@@@@@      
         ---@@              ****      @@@@@@@@       
        -----@@              *      ---@@@@@@@       
        ------@@                   ----@@@@@@--      
   ------------@@                   ---@@@@@@--      
   ------------@@@@                ----@@@@----      
   -------------@@@@               -------------     
   --------------@@@@              --------------    
   --------------@@@@              ---------------   
   ---------------@@@             @----------------  
   ----------------              @@----------------- 
   ----------------             @@@----------------- 
  ------------------          @@@@@----------------  
  ------------------@@     @@@@@@@@--------------    
  -------------------@@@@@@@@@@@@@@------------      
   ------------------@@@@@@@@@@@@@@----------        
      --------------@@@@@@@@@@@@@@@---------         
          @---------@              @-------          
             @----@@               @@-----           
                @@                   @@@             
                                                     
";
        foreach (char c in Penguin)
        {
            if (c == '\n')
            {
                Console.ResetColor();
                Console.WriteLine();
            }
            else
            {
                ConsoleColor cc =
                    c == '*' ? ConsoleColor.Blue :
                    c == '@' ? ConsoleColor.Black :
                    c == '-' ? ConsoleColor.Yellow :
                    ConsoleColor.White;
                Console.BackgroundColor = cc;
                Console.ForegroundColor = cc;
                Console.Write(" ");
            }
        }

        Console.ResetColor();
        Console.WriteLine();
    }

    private static void DrawFreeBSD()
    {
        Console.WriteLine("Hello, FreeBSD...");
        
        const string daemon_pic = @"
               ,        ,
              /(        )`
              \ \___   / |
              /- _  `-/  '
             (/\/ \ \   /\
             / /   | `    \
             O O   ) /    |
             `-^--'`<     '
            (_.)  _  )   /
             `.___/`    /
               `-----' /
  <----.     __ / __   \
  <----|====O)))==) \) /====
  <----'    `--' `.__,' \
               |        |
                \       /       /\
           ______( (_  / \______/
         ,'  ,-----'   |
         `--{__________)
";

        const string daemon_attr = @"
               R        R
              RR        RR
              R RRRR   R R
              RR W  RRR  R
             RWWW W R   RR
             W W   W R    R
             B B   W R    R
             WWWWWWRR     R
            RRRR  R  R   R
             RRRRRRR    R
               RRRRRRR R
  YYYYYY     RR R RR   R
  YYYYYYYYYYRRRRYYR RR RYYYY
  YYYYYY    RRRR RRRRRR R
               R        R
                R       R       RR
           CCCCCCR RR  R RRRRRRRR
         CC  CCCCCCC   C
         CCCCCCCCCCCCCCC
";

        for (int i=0; i < daemon_pic.Length; i++)
        {
            var symbol = daemon_pic[i];
            var attr =   daemon_attr[i];

            ConsoleColor color =
                attr == 'R' ? ConsoleColor.Red :
                attr == 'W' ? ConsoleColor.White :
                attr == 'B' ? ConsoleColor.Blue :
                attr == 'Y' ? ConsoleColor.Yellow :
                attr == 'C' ? ConsoleColor.Cyan :
                ConsoleColor.White;
            
            Console.ForegroundColor = color;
            Console.Write(symbol);
        }

        Console.ResetColor();
        Console.WriteLine();
    }

    private static void DrawMac()
    {
        Console.WriteLine("Hello, Mac...");

        const string Apple = @"
                                  @@@@ 
                               @@@@@@
                             @@@@@@@@
                           @@@@@@@@@
                          @@@@@@@@@
                         @@@@@@@@
                          @@@@
                          
          @@@@@@@@@@@         @@@@@@@@@@@@@
       @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
     @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
   @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
  @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
 @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
 @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
 @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
 @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
 @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
 @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
 @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
 @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
  @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
  @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
   @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
    @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
     @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
      @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
       @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
         @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
           @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
             @@@@@@@             @@@@@@@";


        foreach (char c in Apple)
        {
            if (c == '@')
            {
                Console.BackgroundColor = ConsoleColor.White;
                Console.Write(" ");
            }
            else
            {
                Console.ResetColor();
                Console.Write(c);
            }

        }

        Console.ResetColor();
        Console.WriteLine();
    }
}
