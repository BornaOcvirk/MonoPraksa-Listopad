using System;
using System.Collections.Generic;
using System.Text;

namespace prva_web_aplikacija.Common
{
    public class BuisnessExeptions : Exception
    {
        public BuisnessExeptions(string message) : base(message)
        {
        }
    }
}
