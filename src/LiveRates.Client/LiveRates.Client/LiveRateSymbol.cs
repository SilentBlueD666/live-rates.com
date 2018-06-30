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
        public LiveRateSymbol(string symbol)
        {
            Symbol = symbol;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// The symbol.
        /// </summary>
        public string Symbol
        {
            get { return _symbol; }
            set
            {
                var symbol = value;
                if (!string.IsNullOrWhiteSpace(symbol))
                {
                    if (symbol.IndexOf('/') > -1)
                    {
                        string[] curreny = symbol.Split('/');
                        IsCurrency = true;
                        RequestSymbol = $"{curreny[0]}_{curreny[1]}";
                        PlainSymbol = $"{curreny[0]}{curreny[1]}";
                    }
                    else
                    {
                        IsCurrency = false;
                        RequestSymbol = symbol;
                        PlainSymbol = symbol;
                    }

                    _symbol = symbol;
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
        /// Denotes this symbol is a FX Currency symbol.
        /// </summary>
        public bool IsCurrency { get; private set; }

        #endregion

        #region Object Overrides

        /// <summary>
        /// A string view of the object.
        /// </summary>
        public override string ToString()
        {
            return PlainSymbol;
        }

        #endregion
    }
}
