// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Linq;
using System.Runtime.CompilerServices;

namespace System.Devices.Gpio.Samples
{
    //
    /// <summary>
    /// Supports HD44780 LCD controller
    /// </summary>
    public class LcdController
    {
        // Commands
        private const byte LCD_CLEARDISPLAY = 0x01;
        private const byte LCD_RETURNHOME = 0x02;
        private const byte LCD_ENTRYMODESET = 0x04;
        private const byte LCD_DISPLAYCONTROL = 0x08;
        private const byte LCD_CURSORSHIFT = 0x10;
        private const byte LCD_FUNCTIONSET = 0x20;
        private const byte LCD_SETCGRAMADDR = 0x40;
        private const byte LCD_SETDDRAMADDR = 0x80;

        // Flags for display entry mode
        private const byte LCD_ENTRYRIGHT = 0x00;
        private const byte LCD_ENTRYLEFT = 0x02;
        private const byte LCD_ENTRYSHIFTINCREMENT = 0x01;
        private const byte LCD_ENTRYSHIFTDECREMENT = 0x00;

        // Flags for display on/off control
        private const byte LCD_DISPLAYON = 0x04;
        private const byte LCD_DISPLAYOFF = 0x00;
        private const byte LCD_CURSORON = 0x02;
        private const byte LCD_CURSOROFF = 0x00;
        private const byte LCD_BLINKON = 0x01;
        private const byte LCD_BLINKOFF = 0x00;

        // Flags for display/cursor shift
        private const byte LCD_DISPLAYMOVE = 0x08;
        private const byte LCD_CURSORMOVE = 0x00;
        private const byte LCD_MOVERIGHT = 0x04;
        private const byte LCD_MOVELEFT = 0x00;

        // Flags for function set
        private const byte LCD_8BITMODE = 0x10;
        private const byte LCD_4BITMODE = 0x00;
        private const byte LCD_2LINE = 0x08;
        private const byte LCD_1LINE = 0x00;
        private const byte LCD_5x10DOTS = 0x04;
        private const byte LCD_5x8DOTS = 0x00;

        // When the display powers up, it is configured as follows:
        //
        // 1. Display clear
        // 2. Function set: 
        //    DL = 1; 8-bit interface data 
        //    N = 0; 1-line display 
        //    F = 0; 5x8 dot character font 
        // 3. Display on/off control: 
        //    D = 0; Display off 
        //    C = 0; Cursor off 
        //    B = 0; Blinking off 
        // 4. Entry mode set: 
        //    I/D = 1; Increment by 1 
        //    S = 0; No shift 
        //
        // Note, however, that resetting the device doesn't reset the LCD, so we
        // can't assume that its in that state when a sketch starts (and the
        // LiquidCrystal constructor is called).

        private readonly GpioPin _rsPin; // LOW: command.  HIGH: character.
        private readonly GpioPin _rwPin; // LOW: write to LCD.  HIGH: read from LCD.
        private readonly GpioPin _enablePin; // Activated by a HIGH pulse.
        private readonly GpioPin[] _dataPins;

        private byte _displayFunction;
        private byte _displayControl;
        private byte _displayMode;

        private byte _numLines;
        private readonly byte[] _rowOffsets;

        public LcdController(GpioPin registerSelect, GpioPin enable, params GpioPin[] data)
            : this(registerSelect, null, enable, data)
        {
            // Do nothing
        }

        public LcdController(GpioPin registerSelect, GpioPin readWrite, GpioPin enable, params GpioPin[] data)
        {
            _rwPin = readWrite;
            _rsPin = registerSelect ?? throw new ArgumentNullException(nameof(registerSelect));
            _enablePin = enable ?? throw new ArgumentNullException(nameof(enable));
            _dataPins = data ?? throw new ArgumentNullException(nameof(data));

            if (data.Any(pin => pin == null))
            {
                throw new ArgumentNullException(nameof(data));
            }

            _rowOffsets = new byte[4];

            _displayFunction = LCD_1LINE | LCD_5x8DOTS;

            if (data.Length == 4)
            {
                _displayFunction |= LCD_4BITMODE;
            }
            else if (data.Length == 8)
            {
                _displayFunction |= LCD_8BITMODE;
            }
            else
            {
                throw new ArgumentException($"The length of the array given to parameter {nameof(data)} must be 4 or 8");
            }

            Begin(16, 1);
        }

        public void Begin(byte cols, byte lines, byte dotSize = LCD_5x8DOTS)
        {
            if (lines > 1)
            {
                _displayFunction |= LCD_2LINE;
            }

            _numLines = lines;

            SetRowOffsets(0x00, 0x40, (byte)(0x00 + cols), (byte)(0x40 + cols));

            // for some 1 line displays you can select a 10 pixel high font
            if ((dotSize != LCD_5x8DOTS) && (lines == 1))
            {
                _displayFunction |= LCD_5x10DOTS;
            }

            _rsPin.Mode = PinMode.Output;
            // we can save 1 pin by not using RW. Indicate by passing null instead of a pin
            if (_rwPin != null)
            {
                _rwPin.Mode = PinMode.Output;
            }
            _enablePin.Mode = PinMode.Output;

            // Do this just once, instead of every time a character is drawn (for speed reasons).
            for (int i = 0; i < _dataPins.Length; ++i)
            {
                _dataPins[i].Mode = PinMode.Output;
            }

            // SEE PAGE 45/46 FOR INITIALIZATION SPECIFICATION!
            // according to datasheet, we need at least 40ms after power rises above 2.7V
            // before sending commands. Arduino can turn on way before 4.5V so we'll wait 50
            DelayMicroseconds(50000);
            // Now we pull both RS and R/W low to begin commands
            DigitalWrite(_rsPin, PinValue.Low);
            DigitalWrite(_enablePin, PinValue.Low);

            if (_rwPin != null)
            {
                DigitalWrite(_rwPin, PinValue.Low);
            }

            //put the LCD into 4 bit or 8 bit mode
            if (HasFlag(_displayFunction, LCD_8BITMODE))
            {
                // this is according to the hitachi HD44780 datasheet
                // page 45 figure 23

                // Send function set command sequence
                Command(LCD_FUNCTIONSET | _displayFunction);
                DelayMicroseconds(4500);  // wait more than 4.1ms

                // second try
                Command(LCD_FUNCTIONSET | _displayFunction);
                DelayMicroseconds(150);

                // third go
                Command(LCD_FUNCTIONSET | _displayFunction);
            }
            else
            {
                // this is according to the hitachi HD44780 datasheet
                // figure 24, pg 46

                // we start in 8bit mode, try to set 4 bit mode
                Write4bits(0x03);
                DelayMicroseconds(4500); // wait min 4.1ms

                // second try
                Write4bits(0x03);
                DelayMicroseconds(4500); // wait min 4.1ms

                // third go!
                Write4bits(0x03);
                DelayMicroseconds(150);

                // finally, set to 4-bit interface
                Write4bits(0x02);
            }

            // finally, set # lines, font size, etc.
            Command(LCD_FUNCTIONSET | _displayFunction);

            // turn the display on with no cursor or blinking default
            _displayControl = LCD_DISPLAYON | LCD_CURSOROFF | LCD_BLINKOFF;
            Display();

            // clear it off
            Clear();

            // Initialize to default text direction (for romance languages)
            _displayMode = LCD_ENTRYLEFT | LCD_ENTRYSHIFTDECREMENT;
            // set the entry mode
            Command(LCD_ENTRYMODESET | _displayMode);
        }

        public void SetRowOffsets(byte row0, byte row1, byte row2, byte row3)
        {
            _rowOffsets[0] = row0;
            _rowOffsets[1] = row1;
            _rowOffsets[2] = row2;
            _rowOffsets[3] = row3;
        }

        /********** high level commands, for the user! */
        public void Clear()
        {
            Command(LCD_CLEARDISPLAY);  // clear display, set cursor position to zero
            DelayMicroseconds(2000);  // this command takes a long time!
        }

        public void Home()
        {
            Command(LCD_RETURNHOME);  // set cursor position to zero
            DelayMicroseconds(2000);  // this command takes a long time!
        }

        public void SetCursor(byte col, byte row)
        {
            if (row >= _rowOffsets.Length)
            {
                row = (byte)(_rowOffsets.Length - 1);    // we count rows starting w/0
            }
            if (row >= _numLines)
            {
                row = (byte)(_numLines - 1);    // we count rows starting w/0
            }

            Command(LCD_SETDDRAMADDR | (col + _rowOffsets[row]));
        }

        // Turn the display on/off (quickly)
        public void NoDisplay()
        {
            _displayControl = (byte)(_displayControl & ~LCD_DISPLAYON);
            Command(LCD_DISPLAYCONTROL | _displayControl);
        }

        public void Display()
        {
            _displayControl |= LCD_DISPLAYON;
            Command(LCD_DISPLAYCONTROL | _displayControl);
        }

        // Turns the underline cursor on/off
        public void NoCursor()
        {
            _displayControl = (byte)(_displayControl & ~LCD_CURSORON);
            Command(LCD_DISPLAYCONTROL | _displayControl);
        }

        public void Cursor()
        {
            _displayControl |= LCD_CURSORON;
            Command(LCD_DISPLAYCONTROL | _displayControl);
        }

        // Turn on and off the blinking cursor
        public void NoBlink()
        {
            _displayControl = (byte)(_displayControl & ~LCD_BLINKON);
            Command(LCD_DISPLAYCONTROL | _displayControl);
        }

        public void Blink()
        {
            _displayControl |= LCD_BLINKON;
            Command(LCD_DISPLAYCONTROL | _displayControl);
        }

        // These commands scroll the display without changing the RAM
        public void ScrollDisplayLeft()
        {
            Command(LCD_CURSORSHIFT | LCD_DISPLAYMOVE | LCD_MOVELEFT);
        }

        public void ScrollDisplayRight()
        {
            Command(LCD_CURSORSHIFT | LCD_DISPLAYMOVE | LCD_MOVERIGHT);
        }

        // This is for text that flows Left to Right
        public void LeftToRight()
        {
            _displayMode |= LCD_ENTRYLEFT;
            Command(LCD_ENTRYMODESET | _displayMode);
        }

        // This is for text that flows Right to Left
        public void RightToLeft()
        {
            _displayMode = (byte)(_displayMode & ~LCD_ENTRYLEFT);
            Command(LCD_ENTRYMODESET | _displayMode);
        }

        // This will 'right justify' text from the cursor
        public void Autoscroll()
        {
            _displayMode |= LCD_ENTRYSHIFTINCREMENT;
            Command(LCD_ENTRYMODESET | _displayMode);
        }

        // This will 'left justify' text from the cursor
        public void NoAutoscroll()
        {
            _displayMode = (byte)(_displayMode & ~LCD_ENTRYSHIFTINCREMENT);
            Command(LCD_ENTRYMODESET | _displayMode);
        }

        // Allows us to fill the first 8 CGRAM locations
        // with custom characters
        public void CreateChar(byte location, params byte[] charmap)
        {
            if (charmap.Length != 8)
            {
                throw new ArgumentException(nameof(charmap));
            }

            location &= 0x7; // we only have 8 locations 0-7
            Command(LCD_SETCGRAMADDR | (location << 3));

            for (int i = 0; i < 8; i++)
            {
                Write(charmap[i]);
            }
        }

        public void Print(string value)
        {
            for (int i = 0; i < value.Length; ++i)
            {
                Write(value[i]);
            }
        }

        /*********** mid level commands, for sending data/cmds */

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Command(int value)
        {
            Command((byte)value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Command(byte value)
        {
            Send(value, PinValue.Low);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Write(char value)
        {
            Write((byte)value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Write(byte value)
        {
            Send(value, PinValue.High);
        }

        /************ low level data pushing commands **********/

        // write either command or data, with automatic 4/8-bit selection
        private void Send(byte value, PinValue mode)
        {
            DigitalWrite(_rsPin, mode);

            // if there is a RW pin indicated, set it low to Write
            if (_rwPin != null)
            {
                DigitalWrite(_rwPin, PinValue.Low);
            }

            if (HasFlag(_displayFunction, LCD_8BITMODE))
            {
                Write8bits(value);
            }
            else
            {
                Write4bits((byte)(value >> 4));
                Write4bits(value);
            }
        }

        private void PulseEnable()
        {
            DigitalWrite(_enablePin, PinValue.Low);
            DelayMicroseconds(1);
            DigitalWrite(_enablePin, PinValue.High);
            DelayMicroseconds(1);    // enable pulse must be >450ns
            DigitalWrite(_enablePin, PinValue.Low);
            DelayMicroseconds(100);   // commands need > 37us to settle
        }

        private void Write4bits(byte value)
        {
            for (int i = 0; i < 4; i++)
            {
                DigitalWrite(_dataPins[i], value >> i);
            }

            PulseEnable();
        }

        private void Write8bits(byte value)
        {
            for (int i = 0; i < 8; i++)
            {
                DigitalWrite(_dataPins[i], value >> i);
            }

            PulseEnable();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void DigitalWrite(GpioPin pin, int value)
        {
            const int True = 1;
            PinValue state = HasFlag(value, True) ? PinValue.High : PinValue.Low;
            DigitalWrite(pin, state);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void DigitalWrite(GpioPin pin, PinValue state)
        {
            pin.Write(state);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool HasFlag(int value, int flag)
        {
            return (value & flag) == flag;
        }

        private static void DelayMicroseconds(int microseconds)
        {
            var sw = Diagnostics.Stopwatch.StartNew();
            long v = (microseconds * Diagnostics.Stopwatch.Frequency) / 1000000;
            while (sw.ElapsedTicks < v)
            {
                // Do nothing
            }
        }
    }
}
