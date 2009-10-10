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
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace LightWol
{
    class Program
    {
        static int packetCount = 1;
        static int packetInterval = 1;

        static void Main(string[] args)
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
                    Thread.Sleep(packetInterval * 1000);
                packetCount--;
            }
        }

        private static void Usage()
        {
            Console.WriteLine("LightWol - Wake-On-Lan command-line utility\n");
            Console.WriteLine("usage:\n\tLightWol <mac-address> [<count> [<interval>]]\n");
            Console.WriteLine(" mac-address MAC-Address of the host to wake up.");
            Console.WriteLine(" count       Sets the number of packets that should be sent (default = 1).");
            Console.WriteLine(" interval    Sets the interval between the packets in seconds (default = 1s).\n");
            Console.WriteLine("example:\n\tLightWol 00:11:22:33:44:55");
        }

        private static byte[] ParseArgs(string[] args)
        {
            if (args.Length == 0 || args.Length > 3)
                throw new UsageException("Invalid number of arguments!");

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
                throw new MacAddressException(args[0]);

            byte[] mac = new byte[6];
            for (int i = 0; i < 6; i++)
            {
                mac[i] = Byte.Parse(macParts[i], System.Globalization.NumberStyles.HexNumber);
            }
            return mac;
        }

        private static byte[] CreateWolPacket(byte[] targetMac)
        {
            List<byte> result = new List<byte>();
            for (int i = 0; i < 6; i++)
                result.Add(0xFF);
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
