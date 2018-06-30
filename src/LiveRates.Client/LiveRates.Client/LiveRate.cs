using System;

namespace LiveRates.Client
{
    /// <summary>
    /// Represents Live-Rates.com rate data item.
    /// </summary>
    public class LiveRate
    {
        #region Public Properties

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

        public LiveRateSymbol Symbol { get; private set; }

        public string FirstCurrency { get; private set; }

        public string SecondCurrency { get; private set; }

        public bool IsCurrency { get => Symbol.IsCurrency; }

        public decimal Rate { get; set; }

        public decimal Bid { get; set; }

        public decimal Ask { get; set; }

        public decimal High { get; set; }

        public decimal Low { get; set; }

        public DateTime TimeStamp { get; set; }

        #endregion

        #region Object Overrides

        public override string ToString()
        {
            return $"Symbol: {Symbol}, Rate:{Rate}";
        }

        #endregion
    }
}