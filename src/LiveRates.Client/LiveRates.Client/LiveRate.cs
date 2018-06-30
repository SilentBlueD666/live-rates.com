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
        /// The currency the rate relates to.
        /// </summary>
        public string Currency
        {
            get { return Symbol.Symbol; }
            set
            {
                var symbol = value;
                if (!string.IsNullOrWhiteSpace(symbol))
                {
                    if (symbol.IndexOf('/') > -1)
                    {
                        string[] curreny = symbol.Split('/');
                        FirstCurrency = curreny[0];
                        SecondCurrency = curreny[1];
                    }
                    else
                    {
                        FirstCurrency = null;
                        SecondCurrency = null;
                    }

                    Symbol = new LiveRateSymbol(symbol);
                }
            }
        }

        /// <summary>
        /// The rate symbol.
        /// </summary>
        public LiveRateSymbol Symbol { get; private set; }

        /// <summary>
        /// The first currency in a FX Currency rate pair.
        /// </summary>
        public string FirstCurrency { get; private set; }

        /// <summary>
        /// The second currency in a FX Currency rate pair.
        /// </summary>
        public string SecondCurrency { get; private set; }

        /// <summary>
        /// Denotes this rate is a FX Currency rate.
        /// </summary>
        public bool IsCurrency { get => Symbol.IsCurrency; }

        /// <summary>
        /// The rate price.
        /// </summary>
        public decimal Rate { get; set; }

        /// <summary>
        /// The sell price.
        /// </summary>
        public decimal Bid { get; set; }

        /// <summary>
        /// The buy price.
        /// </summary>
        public decimal Ask { get; set; }

        /// <summary>
        /// The high price.
        /// </summary>
        public decimal High { get; set; }

        /// <summary>
        /// The low price.
        /// </summary>
        public decimal Low { get; set; }

        /// <summary>
        /// The rate time-stamp.
        /// </summary>
        public DateTime TimeStamp { get; set; }

        #endregion

        #region Object Overrides

        /// <summary>
        /// A string view of the object.
        /// </summary>
        public override string ToString()
        {
            return $"Symbol: {Symbol}, Rate:{Rate}";
        }

        #endregion
    }
}