// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Azure.EventHubs;
using Microsoft.Azure.EventHubs.Processor;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace System.Devices.Gpio.Samples
{
    public class EventProcessor : IEventProcessor
    {
        public Task CloseAsync(PartitionContext context, CloseReason reason)
        {
            //Console.WriteLine($"{nameof(EventProcessor)} shutting down. Partition: '{context.PartitionId}', Reason: '{reason}'.");
            return Task.CompletedTask;
        }

        public Task OpenAsync(PartitionContext context)
        {
            //Console.WriteLine($"{nameof(EventProcessor)} initialized. Partition: '{context.PartitionId}'");
            return Task.CompletedTask;
        }

        public Task ProcessErrorAsync(PartitionContext context, Exception error)
        {
            Console.WriteLine($"Error. Partition: {context.PartitionId}, Message: {error.Message}");
            return Task.CompletedTask;
        }

        public Task ProcessEventsAsync(PartitionContext context, IEnumerable<EventData> messages)
        {
            foreach (var eventData in messages)
            {
                string command = Encoding.UTF8.GetString(eventData.Body.Array, eventData.Body.Offset, eventData.Body.Count);

                Console.WriteLine();
                Console.WriteLine($"Command received: '{command}'");

                ExecuteCommand(command);

                Console.WriteLine("Done");
            }

            return context.CheckpointAsync();
        }

        private static void ExecuteCommand(string command)
        {
            string lowerCommand = command.ToLowerInvariant();

            switch (lowerCommand)
            {
                case "led":
                    BlinkLed();
                    break;

                case "temp":
                    ReadTemperatureSensor();
                    break;

                case "color":
                    ReadColorSensor();
                    break;

                default:
                    if (lowerCommand.StartsWith("lcd "))
                    {
                        string message = command.Substring(4);
                        DisplayLcdMessage(message);
                    }
                    else
                    {
                        Console.WriteLine("Unknown command");
                    }
                    break;
            }
        }

        private static void BlinkLed()
        {
            Program.Unix_BlinkingLed();
        }

        private static void ReadTemperatureSensor()
        {
            Program.Unix_I2c_Pressure_Lcd();
        }

        private static void ReadColorSensor()
        {
            Program.Unix_I2c_Color_Lcd();
        }

        private static void DisplayLcdMessage(string message)
        {
            Program.Lcd(message);
        }
    }
}
