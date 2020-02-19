﻿using System;
using System.Threading;
using Meadow;
using Meadow.Devices;
using Meadow.Foundation.Transceivers;

namespace MeadowApp
{
    public class MeadowApp : App<F7Micro, MeadowApp>
    {
        SX127x radio;

        public MeadowApp()
        {
            InitHardware();
        }

        public void InitHardware()
        {
            Console.WriteLine("Initialize...");
            var bus = Device.CreateSpiBus();
            var chipSelect = Device.CreateDigitalOutputPort(Device.Pins.D00);
            radio = new SX127x(bus, chipSelect);

            while (true)
            {
                foreach (var spd in bus.SupportedSpeeds)
                {
                    Console.WriteLine($"{spd}");

                    try
                    {
                        bus.Configuration.SpeedKHz = spd;

                        Console.WriteLine($" @{bus.Configuration.SpeedKHz} Silicon Revision: 0x{radio.GetVersion():x2}");
                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine($" {ex.Message}");
                    }

                    Thread.Sleep(2000);
                }
            }
        }
    }
}