using System;
using System.Collections.Generic;
using System.Text;

namespace LightWol
{
    class UsageException: ArgumentOutOfRangeException
    {
        public UsageException(String message)
            : base(message)
        {
        }
    }
}
