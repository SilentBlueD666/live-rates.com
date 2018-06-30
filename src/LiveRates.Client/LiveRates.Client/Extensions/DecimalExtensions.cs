namespace LiveRates.Client.Extensions
{
    /// <summary>
    /// Decimal extension method helpers.
    /// </summary>
    internal static class DecimalExtensions
    {
        /// <summary>
        /// Try's to converts a string to a decimal, if unsuccessful the result will be 0 (zero).
        /// </summary>
        /// <param name="value">The string to convert.</param>
        /// <returns>A <see cref="decimal"/>, if unsuccessful the result will be 0 (zero)</returns>
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
