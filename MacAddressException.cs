using System;
using System.Collections.Generic;
using System.Text;

namespace LightWol
{
    class MacAddressException: ArgumentException
    {
        public MacAddressException(String address)
            : base(address + " is not a valid MAC-Address!")
        {
        }
    }
}
