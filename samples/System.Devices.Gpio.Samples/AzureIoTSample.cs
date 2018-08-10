// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Azure.EventHubs;
using Newtonsoft.Json;
using System.Configuration;
using System.Devices.I2c;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Threading;

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

        public void Start()
        {
            EventHubClient eventHubClient = CreateEventHubClient();
            Stopwatch watch = Stopwatch.StartNew();

            var pressureSettings = new I2cConnectionSettings(1, PressureTemperatureHumiditySensor.DefaultI2cAddress);
            var colorSettings = new I2cConnectionSettings(1, RgbColorSensor.DefaultI2cAddress);

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

        private void CreateMessage(PressureTemperatureHumiditySensor pressureSensor, RgbColorSensor colorSensor, out Message message)
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

        private void SendMessage(EventHubClient eventHubClient, ref Message message)
        {
            var serializedMessage = JsonConvert.SerializeObject(message);
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

        private static EventHubClient CreateEventHubClient()
        {
            string connectionString = ConfigurationManager.AppSettings["EventHubConnectionString"];

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new Exception("Did not find EventHubConnectionString key in appsettings (app.config)");
            }

            return EventHubClient.CreateFromConnectionString(connectionString);
        }
    }
}
