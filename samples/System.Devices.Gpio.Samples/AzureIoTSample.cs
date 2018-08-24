// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Azure.EventHubs;
using Microsoft.Azure.EventHubs.Processor;
using Newtonsoft.Json;
using System.Configuration;
using System.Devices.I2c;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace System.Devices.Gpio.Samples
{
    public class AzureIoTSample
    {
        private struct Message
        {
            public DateTime TimeStamp;
            public float TemperatureInCelsius;
            public float TemperatureInFahrenheit;
            public float PressureInHectopascals;
            public float AltitudeInMeters;
            public float AltitudInFeet;
            public float Humidity;
            public byte ColorR;
            public byte ColorG;
            public byte ColorB;
            public float ColorBrightness;
            public float ColorHue;
            public float ColorSaturation;
            public float Luminosity;
            public float ColorTemperature;
        }

        public void StartSendingData()
        {
            EventHubClient eventHubClient = CreateEventHubClient("DataEventHubConnectionString");
            Stopwatch watch = Stopwatch.StartNew();

            var pressureSettings = new I2cConnectionSettings(Program.I2cBusId, PressureTemperatureHumiditySensor.DefaultI2cAddress);
            var colorSettings = new I2cConnectionSettings(Program.I2cBusId, RgbColorSensor.DefaultI2cAddress);

            using (var pressureSensor = new PressureTemperatureHumiditySensor(pressureSettings))
            using (var colorSensor = new RgbColorSensor(colorSettings))
            {
                pressureSensor.SeaLevelPressureInHectopascals = 1013.25f;
                bool ok = pressureSensor.Begin();

                if (!ok)
                {
                    Console.WriteLine($"Error initializing pressure sensor");
                    return;
                }

                ok = colorSensor.Begin();

                if (!ok)
                {
                    Console.WriteLine($"Error initializing color sensor");
                    return;
                }

                while (watch.Elapsed.TotalDays < 1)
                {
                    pressureSensor.ReadSensor();
                    colorSensor.ReadSensor();

                    Console.WriteLine($"Pressure:    {pressureSensor.PressureInHectopascals:0.00} hPa");
                    Console.WriteLine($"Humdity:     {pressureSensor.Humidity:0.00} %");
                    Console.WriteLine($"Temperature: {pressureSensor.TemperatureInCelsius:0.00} C, {pressureSensor.TemperatureInFahrenheit:0.00} F");
                    Console.WriteLine($"Altitude:    {pressureSensor.AltitudeInMeters:0.00} m, {pressureSensor.AltitudInFeet:0.00} ft");
                    Console.WriteLine();

                    Console.WriteLine($"Color:       {Program.ToRgbString(colorSensor.Color)}");
                    Console.WriteLine($"Temperature: {colorSensor.Temperature:0.00} K");
                    Console.WriteLine($"Luminosity:  {colorSensor.Luminosity:0.00} lux");
                    Console.WriteLine();

                    Console.WriteLine("Sending sensors data to Azure IoT Hub...");
                    Console.WriteLine();

                    CreateMessage(pressureSensor, colorSensor, out Message message);
                    SendMessage(eventHubClient, ref message);

                    Thread.Sleep(TimeSpan.FromSeconds(1));
                }
            }
        }

        private static void CreateMessage(PressureTemperatureHumiditySensor pressureSensor, RgbColorSensor colorSensor, out Message message)
        {
            message = new Message()
            {
                TimeStamp = DateTime.Now,
                TemperatureInCelsius = pressureSensor.TemperatureInCelsius,
                TemperatureInFahrenheit = pressureSensor.TemperatureInFahrenheit,
                PressureInHectopascals = pressureSensor.PressureInHectopascals,
                AltitudeInMeters = pressureSensor.AltitudeInMeters,
                AltitudInFeet = pressureSensor.AltitudInFeet,
                Humidity = pressureSensor.Humidity,

                ColorR = colorSensor.Color.R,
                ColorG = colorSensor.Color.G,
                ColorB = colorSensor.Color.B,
                ColorBrightness = colorSensor.Color.GetBrightness(),
                ColorHue = colorSensor.Color.GetHue(),
                ColorSaturation = colorSensor.Color.GetSaturation(),

                ColorTemperature = colorSensor.Temperature,
                Luminosity = colorSensor.Luminosity,
            };
        }

        private static void SendMessage(EventHubClient eventHubClient, ref Message message)
        {
            string serializedMessage = JsonConvert.SerializeObject(message);
            var data = new EventData(Encoding.UTF8.GetBytes(serializedMessage));

            try
            {
                eventHubClient.SendAsync(data).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{DateTime.Now} > Exception: {ex.Message}");
            }
        }

        private static EventHubClient CreateEventHubClient(string eventHubConnectionStringKeyName)
        {
            string connectionString = ConfigurationManager.AppSettings[eventHubConnectionStringKeyName];

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new Exception($"Did not find {eventHubConnectionStringKeyName} key in appsettings (app.config)");
            }

            return EventHubClient.CreateFromConnectionString(connectionString);
        }

        public void StartSendingCommands()
        {
            Console.WriteLine("Commands:");
            Console.WriteLine("        - led");
            Console.WriteLine("        - temp");
            Console.WriteLine("        - color");
            Console.WriteLine("        - lcd <text>");
            Console.WriteLine();
            Console.WriteLine("Write commands to send. Press ENTER to stop.");

            EventHubClient eventHubClient = CreateEventHubClient("CommandEventHubConnectionString");
            string command = Console.ReadLine();

            while (!string.IsNullOrWhiteSpace(command))
            {
                SendCommand(eventHubClient, command);
                command = Console.ReadLine();
            }
        }

        private static void SendCommand(EventHubClient eventHubClient, string command)
        {
            var data = new EventData(Encoding.UTF8.GetBytes(command));

            try
            {
                eventHubClient.SendAsync(data).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{DateTime.Now} > Exception: {ex.Message}");
            }
        }

        public void StartReceivingCommands()
        {
            StartReceivingCommandsAsync().GetAwaiter().GetResult();
        }

        private static async Task StartReceivingCommandsAsync()
        {
            Console.WriteLine("Registering EventProcessor...");

            EventProcessorHost eventProcessorHost = CreateEventProcessorHost();

            // Registers the Event Processor Host and starts receiving messages
            await eventProcessorHost.RegisterEventProcessorAsync<EventProcessor>();

            Console.WriteLine("Receiving. Press ENTER to stop.");
            Console.ReadLine();

            // Disposes of the Event Processor Host
            await eventProcessorHost.UnregisterEventProcessorAsync();
        }

        private static EventProcessorHost CreateEventProcessorHost()
        {
            string eventHubName = ConfigurationManager.AppSettings["CommandEventHubName"];

            if (string.IsNullOrEmpty(eventHubName))
            {
                throw new Exception("Did not find CommandEventHubName key in appsettings (app.config)");
            }

            string eventHubConnectionString = ConfigurationManager.AppSettings["CommandEventHubConnectionString"];

            if (string.IsNullOrEmpty(eventHubConnectionString))
            {
                throw new Exception("Did not find CommandEventHubConnectionString key in appsettings (app.config)");
            }

            string storageConnectionString = ConfigurationManager.AppSettings["CommandStorageConnectionString"];

            if (string.IsNullOrEmpty(storageConnectionString))
            {
                throw new Exception("Did not find CommandStorageConnectionString key in appsettings (app.config)");
            }

            string storageContainerName = ConfigurationManager.AppSettings["CommandStorageContainerName"];

            if (string.IsNullOrEmpty(storageContainerName))
            {
                throw new Exception("Did not find CommandStorageContainerName key in appsettings (app.config)");
            }

            return new EventProcessorHost(
                eventHubName,
                PartitionReceiver.DefaultConsumerGroupName,
                eventHubConnectionString,
                storageConnectionString,
                storageContainerName);
        }
    }
}
