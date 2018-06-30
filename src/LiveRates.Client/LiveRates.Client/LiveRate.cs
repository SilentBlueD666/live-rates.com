using System;

namespace LiveRates.Client
{
    /// <summary>
    /// Represents Live-Rates.com rate data item.
    /// </summary>
    public class LiveRate
    {
        #region Public Properties

        /// <summary>
        /// The rate symbol.
        /// </summary>
        public LiveRateSymbol Symbol { get; private set; }

        /// <summary>
        /// The rate price.
        /// </summary>
        public decimal Rate { get; internal set; }

        /// <summary>
        /// The sell price.
        /// </summary>
        public decimal Bid { get; internal set; }

        /// <summary>
        /// The buy price.
        /// </summary>
        public decimal Ask { get; internal set; }

        /// <summary>
        /// The high price.
        /// </summary>
        public decimal High { get; internal set; }

        /// <summary>
        /// The low price.
        /// </summary>
        public decimal Low { get; internal set; }

        /// <summary>
        /// The rate UTC time-stamp.
        /// </summary>
        public DateTime TimeStamp { get; internal set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a default instance of the class.
        /// </summary>
        /// <param name="symbol">The symbol.</param>
        internal LiveRate(LiveRateSymbol symbol)
        {
            Symbol = symbol;
        }

        #endregion

        #region Object Overrides

        /// <summary>
        /// A string view of the object.
        /// </summary>
        public override string ToString()
        {
            return $"Symbol: {Symbol.Symbol}, Rate: {Rate}";
        }

        #endregion
    }
}