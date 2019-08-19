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
        IDigitalOutputPort _redLED;
        IDigitalOutputPort _greenLED;

        DS3231 ds3231;

        public DS3231App()
        {
            _redLED = Device.CreateDigitalOutputPort(Device.Pins.OnboardLedRed);
            _redLED.State = true;

            _greenLED = Device.CreateDigitalOutputPort(Device.Pins.OnboardLedGreen);

            IPin[] i2cPins = new IPin[2] { Device.Pins.I2C_SCL, Device.Pins.I2C_SDA };
            ds3231 = new DS3231(Device, i2cPins);
        }

        protected void TestDS3231()
        {
            ds3231.CurrentDateTime = DateTime.Now;

            Thread.Sleep(3000);

            _redLED.State = false;
            _greenLED.State = true;
        }
    }
}
