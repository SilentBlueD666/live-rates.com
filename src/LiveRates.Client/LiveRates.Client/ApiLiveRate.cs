namespace LiveRates.Client
{
    /// <summary>
    /// Represents a rate from live-rates.com API endpoints
    /// </summary>
    internal class ApiLiveRate
    {
        #region Public Properties

        /// <summary>
        /// The symbol the rate represents.
        /// </summary>
        public string Currency { get; set; }

        /// <summary>
        /// The price of the symbol.
        /// </summary>
        public decimal Rate { get; set; }

        /// <summary>
        /// The sell price.
        /// </summary>
        public string Bid { get; set; }

        /// <summary>
        /// The buy price.
        /// </summary>
        public string Ask { get; set; }

        /// <summary>
        /// The high price.
        /// </summary>
        public string High { get; set; }

        /// <summary>
        /// The low price.
        /// </summary>
        public string Low { get; set; }

        /// <summary>
        /// The rate time-stamp.
        /// </summary>
        public string TimeStamp { get; set; }

        #endregion

        #region Object Overrides

        /// <summary>
        /// A string view of the object.
        /// </summary>
        public override string ToString()
        {
            return $"Currency: {Currency}, Rate:{Rate}";
        }

        #endregion
    }
}
