# `System.Devices.Gpio`

## Introduction

`System.Devices.Gpio` is an experimental package that allows projects to access General Purpose IO (GPIO) pins for the Raspberry Pi 3
(Broadcom BCM2837), ODROID-XU4, and BeagleBone Black (AM3358/9).

`System.Devices.Gpio` is currently a summer internship project being developed by @edgardozoppi.

## Goals
1. Research GPIO and gain a firm understanding of the practices and common use cases
2. Plan out an API for a CoreFX implementation of basic GPIO methods and take it through API Review
3. Functional (positive, negative) tests for 100% of Public APIs
4. Complete a workable implementation of GPIO functionality
5. BenchmarkDotNet performance tests written for 100% of Public APIs
6. Complete documentation for 100% of Public APIs, including usage examples

## Research
- Understand the basic protocols and vocabulary
    - Analog vs Digital pin numbering
    - BCM vs Board numbering on a raspberry pi
    - Pulse-Width Modulation (PWM)
    - Pullup/Pulldown resistors
	- Multi-pin communication protocols
		- SPI
		- I2C
		- UART (SerialPort)
- Look at other notable GPIO implementations documentation
	- [Windows.Devices.Gpio namespace (Windows IoT OS -only)](https://docs.microsoft.com/en-us/uwp/api/windows.devices.gpio)
	- [PI.IO (Corefxdev team hackathon project)](https://github.com/Petermarcu/Pi)
	- [Wiring Pi (GPL)](http://wiringpi.com/)
	- [RPI.GPIO (MIT)](http://tieske.github.io/rpi-gpio/modules/GPIO.html)

- Check out some of these helpful articles:
    - [A Web developers guide to communication Protcols](https://tessel.io/blog/108840925797/a-web-developers-guide-to-communication-protocols)
    - [.NET Core on RPi](https://github.com/dotnet/core/blob/master/samples/RaspberryPiInstructions.md)
    - [PI.IO sample for reading barometric pressure, temperature, and more](https://github.com/Petermarcu/Pi/blob/master/IotSample/Program.cs)
    - [Sample program to use Alexa to control a Pi through Azure](https://github.com/Petermarcu/AlexaDotnetPi)
    - [Controlling a device through Azure IOT hub](https://docs.microsoft.com/en-us/azure/iot-hub/quickstart-control-device-dotnet)
    - [Pinout - A visual guide to an rpi board](https://pinout.xyz)
- Consider specific scenarios in which GPIO would be used in .NET Core. Some examples:
    - IoT Device Sends Data to Azure
		- Example: A Raspberry Pi3 device collects sensor data (room temperature, soil temperature, soil moisture) and uploads it to Azure
			- Azure IoT collects the data and turns the log data into graphs for an Azure hosted .NET Core Website (ASP.NET Core Razer page) and a Xamarin iOS/Android App
	- IoT Device Receives Data from Azure
		- Example: Azure IoT Hub / Azure Web Site (and phone app) can set alert triggers and events based on the data
			- When temperature is high/low, it sends the device a control signal to turn on/off a fan
            - When the moisture level is low/high, it sends the device a control signal to turn on/off a water supply
     - Connect to a smart home device and read its information e.g. temperature, open/closed, etc.

## GPIO - Basic Implementation
Most GPIO implementations share a core set of functionality to allow basic on/off control
of pins. At the very least, we should support these:
- Open a object that is a representation of a pin with the given pin number
- Support closing a pin to release the resources owned by that pin object
- Represent the mode of the pin that details how the pin handles reads and writes (e.g. Input, Output)
- Allow a resistor to be added to a pin such that it can be set as pullup or pulldown (or no pull)
- Support setting a PWM value on a pin

### Rough API Shape
```
namespace System.Devices.Gpio
{
    public partial class GPIOPin : IDisposable
    {
        int PinNumber { get { } }
        bool Read() { }
        void Write(bool value) { }
        void PWMWrite(int value) { }
        void Dispose() { }
        GPIOPin(GPIOController controller, int pinNumber, GPIOPinMode pinMode = GPIOPinMode.Input) { }

        GPIOPinMode PinMode { get { } set { } }
        bool IsPinModeSupported(GPIOPinMode pinMode) { }
        int PWMValue { get { } set { } }
    }

    public partial class GPIOController : IDisposable
    {
        GPIOPin this[int pinNumber] { get { } }
        GPIOPin OpenPin(int pinNumber, GPIOPinMode mode) { }
        void ClosePin(int pinNumber) { }
        void ClosePin(GPIOPin pin) { }
        int PinCount() { }
        IEnumerable<GPIOPin> ConnectedPins { get { } }
        void Dispose() { }
    }

    public enum GPIOPinMode
    {
        Pull_Down,
        Pull_Up,
        PWM,
        Input,
        Output
    }
}
```

### Examples
```
// Blink a simple LED
public void BlinkBasic()
{
    using (GPIOController controller = new GPIOController()) // BOARD numbering by default
    using (GPIOPin ledPin = controller.OpenPin(7, GPIOPinMode.Output))
    {
        while(true)
        {
            ledPin.Write(true);
            Thread.Sleep(500);
            ledPin.Write(false);
            Thread.Sleep(500);
        }
    }
}

// Use pullup/pulldown with basic reads
public void PUD()
{
    using (GPIOController controller = new GPIOController()) // BOARD numbering by default
    using (GPIOPin pullUpPin = controller.OpenPin(7, GPIOPinMode.Input))
    {
        if (pullUpPin.IsPinModeSupported(GPIOPinMode.Pull_Up))
        {
            pullUpPin.PinMode = GPIOPinMode.Pull_Up | GPIOPinMode.Input;
        }

        while (true)
        {
            Console.WriteLine(pullUpPin.Read());
            Thread.Sleep(1000);
        }
    }
}
```

### Scenario 1
The best way to dip ones toes into GPIO and RaspberryPi development is to set up a single blinking LED light. This is the "Hello World" of GPIO and there are guides available for every GPIO implementation on how to accomplish this. Our goal for the first stage of the "basic" API should be to set up our own article or blog post on how to do a blink with .net core on an rpi3.

[Here's an example of a blinking guide written using node.js](https://www.w3schools.com/nodejs/nodejs_raspberrypi_blinking_led.asp)


## GPIO - Intermediate Implementation
Beyond the basic set of functionality are a set of functions that are supported by *almost* every implementation out there. They are:
- Waiters - Instead of manually polling a Read, a Waiter will handle the polling until the desired Read value is reached
- Listeners - There should be some way to listen for a change and respond accordingly using callbacks.
- Edge Detection - Used with listeners/eventing as a way of defining the circumstances under which an event/callback will be raised
- Allow setting a Debounce duration to ignore quickly occuring events during some timespan.

### Stub API that builds on top of the Basic API:
```
namespace System.Devices.Gpio
{
    public partial class GPIOPin
    {      
        // Waiters
        public bool ReadWait(TimeSpan timeout) { }

        // Listeners
        // TODO

        // Edge Detection
        // TODO

        // Debounce
        public TimeSpan Debounce { get { }  set { } }
    }

}
```

### Examples
```
// Poll a read until the pin is set to HIGH/on
public void PollRead()
{
    using (GPIOController controller = new GPIOController()) // BOARD numbering by default
    using (GPIOPin pollPin = controller.OpenPin(7, GPIOPinMode.Input))
    {
        pollPin.ReadWait(true, new TimeSpan(0, 1, 0));
    }
}
```

### Scenario 2
With eventing set up we can start making more practical programs with .net core on an rpi. One such use case would be to set up a temperature sensor and write a .net app that sets up eventing so that whenever the temperature changes, something happens. Maybe we blink an LED whenever the temperature is below a certain value, for example.

[Here's a guide on setting up a temperature sensor program in Python that prints out the temperature every interval. We should be able to do the same thing using .net core and make a blog post with the instructions.](https://pimylifeup.com/raspberry-pi-temperature-sensor/)


## GPIO - Advanced Implementation
Though not available everywhere, these functions provide high value to raspberry pi users and add some quality of life additions to everyone:
- Choose between BCM or BOARD pin numbering
- Bit shifting - Add helpers to allow easily working with more usable data types
- Advanced PWM functions can be added to allow setting range, rpi mode, etc.
- Analog Reads and Writes - Most GPIO works with digital pins, but sometimes analog pins are used. The difference in the 
    Analog pins is that they have a range of potential values instead of just being on/off like the digital pins.
    
### Stub API that builds on top of the previous API:
```
namespace System.Devices.Gpio
{
    // BCM vs BOARD
    public partial class GPIOController
    {
        GPIOController(GPIOBoardMode numbering = GPIOBoardMode.BOARD) { }
    }

    public enum GPIOBoardMode
    {
        BOARD,
        BCM
    }

    public partial class GPIOPin
    {
        // Analog
        int AnalogRead() { }
        void AnalogWrite(int value) { }
        public int AnalogReadWait(TimeSpan timeout) { }

        // Bit-Shifts and writer helpers
        // TODO

        // Advanced PWM
        public int PWMRange { get { } }
        public PWMMode PWMMode { get { } }
        
        // Frequency and DutyCycle might be useful to add also to allow finer control of the PWM controller
        // public int PWMFrequency { get { } set { } }
        // public int PWMDutyCycle{ get { } set { } }
        
        // Software PWM is another interesting idea that we could implement.
    }

    public enum PWMMode
    {
        MARK_SPACE,
        BALANCED
    }
}
```

### Examples
```
public void PWM()
{
    using (GPIOController controller = new GPIOController()) // BOARD numbering by default
    using (GPIOPin pwmPin = controller.OpenPin(12, GPIOPinMode.Output | GPIOPinMode.PWM))
    {
        pwmPin.PWMMode = PWMMode.MARK_SPACE;
        pwmPin.PWMRange = 100; // Splits up the max frequency into 100 equal segments 
        pwmPin.PWMFrequency = 10 * 1000 * 1000 ; // Sets the max frequency (i.e. at 100% duty cycle) to 10 mhz

        // Cycle from 0 to the max PWM frequency
        for (int i = 0; i < pwmPin.PWMRange; i++)
        {
            pwmPin.WritePWM(i);
            pwmPin.Sleep(100);
        }
    }
}
```

### Scenario 3
With the advanced API completed we have a truly functional implementation of GPIO with an API surface area at parity with most other implementations. As such, we should be able to translate most other "how to do X on raspberry pi" guides to .net core as long as they're using single pin communication.

Our goal for this scenario should be a little bit different than the other ones, then. I'd like to see us complete either a new scenario or a previous scenario but do so fully from end to end using Azure. For example, maybe we take our temperature reading program from the previous step but hook it up to Azure IoT so we can track temperature changes using the IoT hub tools. Bonus points if we also add an Azure hosted .NET Core website or Xamarin mobile app to view that data.


# Stretch - Multi-Pin Connections
This section holds connection types where more than one pin is used to transmit data. There are a quite a few of these, but the most commonly supported are SPI, I2C, and UART/SerialPort. It would be great if we could support at least one of these off-the-bat, but they aren't required to have a functional GPIO implementation.

### Stub API:
```
namespace System.Devices.Gpio
{
    // Add new members to the GPIOPinMode enum for the new pin types
    public enum GPIOPinMode
    {
        Pull_Down,
        Pull_Up,
        PWM,
        Input,
        Output,
        SPI,
        I2C,
        UART, // serialport
        Unknown
    }

    public partial class SpiConnection
    {
        // https://github.com/Petermarcu/Pi/tree/master/Pi.IO.SerialPeripheralInterface
    }

    public partial class I2cConnection
    {
        // https://github.com/Petermarcu/Pi/tree/master/Pi.IO.InterIntegratedCircuit
    }

    public class UARTConnection
    {
        // A Linux or platform-agnostic serial port library would likely have to be distinct from our 
        // existing bloated Windows implementation. That wouldn't necessarily be a bad thing, though, as
        // we could add basic functionality like read/write/open/close without the weight of the Windows
        // implementation
    }
    
    public class HomeAutomation
    {
        // It'd be cool if we could have a library to enable easy interactions with common household IoT automation devies.
        // Some possible scenarios for this:
        // - Raspberry Pi Twitter Bot (reads sensor data -> tweets; receives tweets -> adjusts gpio devices)
        // - Raspberry Pi Weather Station using Sensors
        // - Raspberry Pi Stock Market Orb (glow green or red based on stock market data)
        // - Raspberry Pi Google Voice Assistant / Cortana Voice Assistant / Amazon Echo
        // - Raspberry Pi Vending Machine / Badge Reader
    }
}
```

### Scenario 4
Depending on the multi-pin support we add, we'll have different options for a scenario to implement. I personally think the coolest one would be to implement the SPI protocol so we can communicate with RFID chips. We could then create a program that connects to an RFID reader and prints out RFID data whenever a tag is read from the reader.

[Here's an example of such a scenario implemented with Python](https://pimylifeup.com/raspberry-pi-rfid-rc522/)

# Out of Scope - Advanced Connection Types
Though there are a bunch of useful connections types, we can't feasibly implement them all at once. This section lists some more types that we should keep in the back of our mind and pursue after the above are complete.
```
namespace System.Devices.Gpio
{
    public class USBConnection
    {
        // We could include discovery of USB and even allow hot-swapping potentially. In addition to
        // allowing easy communication over the port, of course.
    }

    public class BluetoothConnection
    {
        // Communicating with devices over bluetooth has been a highly requested feature for a while now. 
    }
    
    // MQTTS 
    
    // AMQPS 
}
```

## Implementation Details
- We should aim to have an answer for GPIO cross-platform, so we will need to design our API and behavior to be sensible across platforms. We should consider leveraging the [WinRT API](https://docs.microsoft.com/en-us/uwp/api/windows.devices.gpio), either by recommending people use that on Windows or by building a Windows version of our API on top of it. For the parts of our API not supported by WinRT, we will need to investigate what win32 APIs are available to use.
- Compare performance of using basic File apis vs using MemoryMappedFiles
- We can hit /dev/gpiomem without root access if we want to allow some functions to run non-root.

# 12-Week Completion Plan
Our goal should be to hit the ground running and check-in on the first day and every subsequent day. No matter how small, every day should have a code check-in.
### Sprint 1: 06/04 - 06/22
#### Week 1: 06/04 - 06/08
- First check-in of an example usage or test case to corefxlab on Day1.
- Write a basic blinking LED program in any language by Day2
- First-Round API review for all GPIO phases by first Friday. 
#### Week 2: 06/11 - 06/15
- Check-in functional tests for 100% of the "Basic Implementation" APIs
- Check-in a Windows "Basic Implementation" that successfully passes all new tests
- All "Basic Implementation" public APIs have accompanying doc comments
#### Week 3: 06/18 - 06/22
- Check-in a Unix "Basic Implementation" that successfully passes all new tests
- Write a blog post or instructional guide on how to blink an LED using .net core on a Raspberry pi 3 (Scenario 1 above)
### Sprint 2: 06/25 - 07/13
#### Week 4: 06/25 - 06/29
- Check-in functional tests for 100% of the "Intermediate Implementation" APIs
- Check-in a Windows "Intermediate Implementation" that successfully passes all new tests
- All "Intermediate Implementation" public APIs have accompanying doc comments
#### Week 5: 07/02 - 07/06
- Check-in a Unix "Intermediate Implementation" that successfully passes all new tests
- Write a blog post or instructional guide on how to listen for temperature changes on a sensor using .net core on a Raspberry pi 3 (Scenario 2 above)
#### Week 6: 07/09 - 07/13
- BenchmarkDotNet performance tests written for 100% of Basic and Intermediate Public APIs.
- Share performance results with the team in either a detailed email or a blog post.
### Sprint 3: 07/16 - 08/03
#### Week 7: 07/16 - 07/20
- Check-in functional tests for 100% of the "Advanced Implementation" APIs
- Check-in a Windows "Advanced Implementation" that successfully passes all new tests
- All "Advanced Implementation" public APIs have accompanying doc comments
#### Week 8: 07/23 - 07/27
- Check-in a Unix "Advanced Implementation" that successfully passes all new tests
#### Week 9: 07/30 - 08/03
- Write a blog post or instructional guide on how to integrate a rpi app into Azure tools using .net core on a Raspberry pi 3 (Scenario 3 above). There's a lot of room for flexibility and creativity here to make something cool to show off to the team.
### Sprint 4: 08/06 - 08/24
#### Week 10: 08/06 - 08/10
- (Stretch Goal) Design and plan an API for one of the multi-pin connection types and take it through an API review meeting.
#### Week 11: 08/13 - 08/17
- (Stretch Goal) Check-in functional tests for 100% of the public multi-pin type APIs
- (Stretch Goal) Check-in a Windows multi-pin implementation that successfully passes all new tests
- (Stretch Goal) All multi-pin implementation public APIs have accompanying doc comments
#### Week 12: 08/20 - 08/24
- (Stretch Goal) Check-in a Unix multi-pin implementation that successfully passes all new tests
- (Stretch Goal) Write a blog post or instructional guide on how to use the new multi-pin APIs, maybe doing something interesting with an RFID reader for example (Scenario 4 above).
- Do any final cleanup necessary and close out remaining bugs/issues assigned

# Documentation and Examples
- Examples should be in a form that is easily recognizable for someone familiar with rpi developement, regardless of their current programming environment.
- Documentation could include a blog post with examples of some cool stuff you can do with GPIO on .net core.


