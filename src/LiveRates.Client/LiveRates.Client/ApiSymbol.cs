namespace LiveRates.Client
{
    /// <summary>
    /// Represents a live-rates.com symbol.
    /// </summary>
    internal class ApiSymbol
    {
        #region Public Properties

        /// <summary>
        /// The symbol.
        /// </summary>
        public string Currency { get; set; }

        /// <summary>
        /// UTC time-stamp when the rate was last updated as milliseconds.
        /// </summary>
        public string Updated { get; set; }

        #endregion
    }
}
