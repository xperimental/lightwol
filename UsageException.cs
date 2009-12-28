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
    /// This exception is thrown by the argument parser if there is an
    /// invalid number of arguments and the usage info should be displayed
    /// to the user.
    /// </summary>
    internal class UsageException : ArgumentOutOfRangeException
    {
        /// <summary>
        /// Initializes a new instance of the UsageException class. The provided
        /// message is ignored by the application.
        /// </summary>
        /// <param name="message">Exception message (ignored)</param>
        public UsageException(string message)
            : base(message)
        {
        }
    }
}
