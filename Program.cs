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

namespace LightWol
{
    class Program
    {
        static void Main(string[] args)
        {
            byte[] targetMac;
            try
            {
                targetMac = ParseArgs(args);
            }
            catch (ArgumentException)
            {
                Usage();
                return;
            }

            byte[] buffer = CreateWolPacket(targetMac);

            IPEndPoint ep = new IPEndPoint(IPAddress.Broadcast, 7);

            Console.WriteLine("Sending packet...");

            UdpClient client = new UdpClient();
            client.Send(buffer, buffer.Length, ep);
        }

        private static void Usage()
        {
            Console.WriteLine("LightWol - Wake-On-Lan command-line utility\n");
            Console.WriteLine("usage:\n\tLightWol <mac-address>\n");
            Console.WriteLine("example:\n\tLightWol 00:11:22:33:44:55");
        }

        private static byte[] ParseArgs(string[] args)
        {
            if (args.Length != 1)
                throw new ArgumentException();

            string[] macParts = args[0].Split(':', '-');
            if (macParts.Length != 6)
                throw new ArgumentException();

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
