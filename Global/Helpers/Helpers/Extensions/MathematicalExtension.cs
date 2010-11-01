using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Helpers.Extensions
{
    public static class MathematicalExtension
    {
        public static int ToInt(this double Source)
        {
            return Int32.Parse(Math.Floor(Source).ToString());
            
        }

        public static int ToInt(this float Source)
        {
            return Int32.Parse(Math.Floor(Source).ToString());

        }
    }
}
