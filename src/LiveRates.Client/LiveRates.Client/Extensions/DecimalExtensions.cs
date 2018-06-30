using System;
using System.Collections.Generic;
using System.Text;

namespace LiveRates.Client.Extensions
{
    internal static class DecimalExtensions
    {
        public static decimal ToDecimal(this string value)
        {
            if(decimal.TryParse(value, out decimal result))
            {
                return result;
            }

            return 0;
        }
    }
}
