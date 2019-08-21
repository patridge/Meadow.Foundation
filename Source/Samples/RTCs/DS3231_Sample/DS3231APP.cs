using Meadow;
using Meadow.Devices;
using Meadow.Foundation.RTCs;
using Meadow.Hardware;
using System;
using System.Threading;

namespace DS3231_Sample
{
    public class DS3231App : App<F7Micro, DS3231App>
    {
        IDigitalOutputPort _blueLED;
        IDigitalOutputPort _redLED;
        IDigitalOutputPort _greenLED;

        DS3231 ds3231;

        public DS3231App()
        {
            _redLED = Device.CreateDigitalOutputPort(Device.Pins.OnboardLedRed);
            _redLED.State = true;

            _greenLED = Device.CreateDigitalOutputPort(Device.Pins.OnboardLedGreen);

            ds3231 = new DS3231(Device, Device.Pins.I2C_SCL, Device.Pins.I2C_SDA);

            TestDS3231();
        }

        protected void TestDS3231()
        {
            var state = false;

            _redLED.State = false;
            //ds3231.CurrentDateTime = DateTime.Now;

            while (true)
            {
                Console.WriteLine(ds3231.CurrentDateTime);

                _greenLED.State = true;
                Thread.Sleep(1000);
                _greenLED.State = false;
            }
        }
    }
}