using System;

namespace LiveRates.Client
{
    /// <summary>
    /// Represents a live-rates.com rate symbol.
    /// </summary>
    public sealed class LiveRateSymbol
    {
        #region Fields

        private string _symbol;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a default instance of the class, with the symbol supplied.
        /// </summary>
        /// <param name="symbol">The symbol.</param>
        /// <param name="updated">The date and time the rate was last updated.</param>
        internal LiveRateSymbol(string symbol, DateTime updated)
        {
            Symbol = symbol;
            Updated = updated;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// The symbol.
        /// </summary>
        public string Symbol
        {
            get => _symbol;
            private set
            {
                var symbol = value;
                if (!string.IsNullOrWhiteSpace(symbol))
                {
                    if (symbol.IndexOf('_') > -1 && symbol.Length == 7)
                    {
                        string[] curreny = symbol.Split('_');
                        if (curreny[0].Length == 3 && curreny[1].Length == 3)
                        {
                            IsCurrency = true;
                            FirstCurrency = curreny[0];
                            SecondCurrency = curreny[1];
                            RequestSymbol = $"{curreny[0]}_{curreny[1]}";
                            PlainSymbol = $"{curreny[0]}{curreny[1]}";
                            _symbol = $"{curreny[0]}/{curreny[1]}";
                        }
                        else
                        {
                            IsCurrency = false;
                            RequestSymbol = symbol;
                            PlainSymbol = symbol;
                            _symbol = symbol;
                        }
                    }
                    else
                    {
                        IsCurrency = false;
                        RequestSymbol = symbol;
                        PlainSymbol = symbol;
                        _symbol = symbol;
                    }
                }
            }
        }

        /// <summary>
        /// The symbol formatted for request API endpoints.
        /// </summary>
        public string RequestSymbol { get; private set; }

        /// <summary>
        /// The symbol without formatting.
        /// </summary>
        public string PlainSymbol { get; private set; }

        /// <summary>
        /// The first currency in a FX Currency rate pair.
        /// </summary>
        public string FirstCurrency { get; private set; }

        /// <summary>
        /// The second currency in a FX Currency rate pair.
        /// </summary>
        public string SecondCurrency { get; private set; }

        /// <summary>
        /// Denotes this symbol is a FX Currency symbol.
        /// </summary>
        public bool IsCurrency { get; private set; }

        /// <summary>
        /// UTC time-stamp when the rate was last updated.
        /// </summary>
        public DateTime Updated { get; private set; }

        #endregion

        #region Object Overrides

        /// <summary>
        /// A string view of the object.
        /// </summary>
        public override string ToString()
        {
            return Symbol;
        }

        #endregion
    }
}
