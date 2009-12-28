/*
This file is part of LightWol.

LightWol is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

LightWol is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with LightWol.  If not, see <http://www.gnu.org/licenses/>.
 */
namespace LightWol
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Sockets;
    using System.Threading;

    /// <summary>
    /// This is the main class of the LightWol application. It contains
    /// the main method which is called on program start and all other program
    /// logic.
    /// </summary>
    internal class Program
    {
        /// <summary>
        /// Contains the number of packets to send.
        /// </summary>
        private static int packetCount = 1;

        /// <summary>
        /// Contains the time to wait between sending packets (seconds).
        /// </summary>
        private static int packetInterval = 1;

        /// <summary>
        /// Main method called on program start.
        /// </summary>
        /// <param name="args">Command line arguments passed to the application.</param>
        public static void Main(string[] args)
        {
            byte[] targetMac;
            try
            {
                targetMac = ParseArgs(args);
            }
            catch (MacAddressException e)
            {
                Console.WriteLine("ERROR: " + e.Message);
                return;
            }
            catch (UsageException)
            {
                Usage();
                return;
            }

            byte[] buffer = CreateWolPacket(targetMac);

            IPEndPoint ep = new IPEndPoint(IPAddress.Broadcast, 7);

            UdpClient client = new UdpClient();
            while (packetCount > 0)
            {
                Console.WriteLine("Sending packet...");
                client.Send(buffer, buffer.Length, ep);

                if (packetCount > 1)
                {
                    Thread.Sleep(packetInterval * 1000);
                }

                packetCount--;
            }
        }

        /// <summary>
        /// Displays some usage information to the user.
        /// </summary>
        private static void Usage()
        {
            Console.WriteLine("LightWol - Wake-On-Lan command-line utility\n");
            Console.WriteLine("usage:\n\tLightWol <mac-address> [<count> [<interval>]]\n");
            Console.WriteLine(" mac-address MAC-Address of the host to wake up.");
            Console.WriteLine(" count       Sets the number of packets that should be sent (default = 1).");
            Console.WriteLine(" interval    Sets the interval between the packets in seconds (default = 1s).\n");
            Console.WriteLine("example:\n\tLightWol 00:11:22:33:44:55");
        }

        /// <summary>
        /// Parses the command line arguments and sets the class variables
        /// accordingly. Will throw exceptions, if the command line arguments
        /// are not valid.
        /// </summary>
        /// <param name="args">Command line arguments passed to application.</param>
        /// <returns>MAC address to wake up.</returns>
        /// <exception cref="UsageException">If the number of arguments is not valid.</exception>
        /// <exception cref="MacAddressException">If the MAC address is not valid.</exception>
        private static byte[] ParseArgs(string[] args)
        {
            if (args.Length == 0 || args.Length > 3)
            {
                throw new UsageException("Invalid number of arguments!");
            }

            if (args.Length > 1)
            {
                packetCount = Int32.Parse(args[1]);
                if (args.Length > 2)
                {
                    packetInterval = Int32.Parse(args[2]);
                }
            }

            string[] macParts = args[0].Split(':', '-');
            if (macParts.Length != 6)
            {
                throw new MacAddressException(args[0]);
            }

            byte[] mac = new byte[6];
            for (int i = 0; i < 6; i++)
            {
                mac[i] = Byte.Parse(macParts[i], System.Globalization.NumberStyles.HexNumber);
            }

            return mac;
        }

        /// <summary>
        /// Creates a byte array containing a "magic packet" to wake up a host via
        /// wake-on-lan.
        /// </summary>
        /// <param name="targetMac">MAC address to wake up.</param>
        /// <returns>Byte array containing "magic packet".</returns>
        private static byte[] CreateWolPacket(byte[] targetMac)
        {
            List<byte> result = new List<byte>();
            for (int i = 0; i < 6; i++)
            {
                result.Add(0xFF);
            }

            for (int i = 0; i < 16; i++)
            {
                for (int j = 0; j < targetMac.Length; j++)
                {
                    result.Add(targetMac[j]);
                }
            }

            return result.ToArray();
        }
    }
}
