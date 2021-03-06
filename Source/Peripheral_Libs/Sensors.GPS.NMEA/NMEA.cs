﻿using System;
using System.Collections;
using Meadow.Hardware;
using Meadow.Foundation.Communications;
using Meadow.Foundation.Helpers;

namespace Meadow.Foundation.Sensors.GPS
{
    /// <summary>
    ///     Generic NMEA GPS object.
    /// </summary>
    public class NMEA
    {
        #region Member variables / fields

        /// <summary>
        ///     NMEA decoders available to the GPS.
        /// </summary>
        private readonly Hashtable _decoders = new Hashtable();

        /// <summary>
        ///     GPS serial input.
        /// </summary>
        private readonly SerialTextFile _gps;

        #endregion Member variables / fields

        #region Constructors

        /// <summary>
        ///     Default constructor for a NMEA GPS object, this is private to prevent the user from
        ///     using it.
        /// </summary>
        private NMEA()
        {
        }

        /// <summary>
        ///     Create a new NMEA GPS object and attach to the specified serial port.
        /// </summary>
        /// <param name="port">Serial port attached to the GPS.</param>
        /// <param name="baudRate">Baud rate.</param>
        /// <param name="parity">Parity.</param>
        /// <param name="dataBits">Number of data bits.</param>
        /// <param name="stopBits">Number of stop bits.</param>
        public NMEA(string port, int baudRate, ParityType parity, int dataBits, NumberOfStopBits stopBits)
        {
            _gps = new SerialTextFile(port, baudRate, parity, dataBits, stopBits, "\r\n");
            _gps.OnLineReceived += _gps_OnLineReceived;
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        ///     Add a new NMEA decoder to the GPS.
        /// </summary>
        /// <param name="decoder">NMEA decoder.</param>
        public void AddDecoder(NMEADecoder decoder)
        {
            if (_decoders.Contains(decoder.Prefix))
            {
                throw new Exception(decoder.Prefix + " already registered.");
            }
            _decoders.Add(decoder.Prefix, decoder);
        }

        /// <summary>
        ///     Open the connection to the GPS and start processing data.
        /// </summary>
        public void Open()
        {
            _gps.Open();
        }

        /// <summary>
        ///     Close the connection to the GPS and stop processing data.
        /// </summary>
        public void Close()
        {
            _gps.Close();
        }

        #endregion Methods

        #region Interrupts

        /// <summary>
        ///     GPS message ready for processing.
        /// </summary>
        /// <remarks>
        ///     Unknown message types will be discarded.
        /// </remarks>
        /// <param name="line">GPS text for processing.</param>
        private void _gps_OnLineReceived(object sender, string line)
        {
            if (line.Length > 0)
            {
                var checksumLocation = line.LastIndexOf('*');
                if (checksumLocation > 0)
                {
                    var checksumDigits = line.Substring(checksumLocation + 1);
                    var actualData = line.Substring(0, checksumLocation);
                    if (DebugInformation.Hexadecimal(Checksum.XOR(actualData.Substring(1))) == ("0x" + checksumDigits))
                    {
                        var elements = actualData.Split(',');
                        if (elements.Length > 0)
                        {
                            var decoder = (NMEADecoder) _decoders[elements[0]];
                            if (decoder != null)
                            {
                                decoder.Process(elements);
                            }
                        }
                    }
                }
            }
        }

        #endregion Interrupts
    }
}