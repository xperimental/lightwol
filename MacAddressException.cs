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

    /// <summary>
    /// This exception is raised by the argument parser, if the provided
    /// MAC-Address is not valid.
    /// </summary>
    internal class MacAddressException : ArgumentException
    {
        /// <summary>
        /// Initializes a new instance of the MacAddressException class. The message
        /// contains the provided MAC-Address.
        /// </summary>
        /// <param name="address">Erroneous MAC-Address.</param>
        public MacAddressException(string address)
            : base(address + " is not a valid MAC-Address!")
        {
        }
    }
}
