using System;
using System.Collections.Generic;
using System.Text;

namespace LiveRates.Client
{
    public sealed class LiveRateSymbol
    {
        private string _symbol;

        public LiveRateSymbol(string symbol)
        {
            Symbol = symbol;
        }

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

        public string RequestSymbol { get; private set; }

        public string PlainSymbol { get; private set; }

        public bool IsCurrency { get; private set; }

        public override string ToString()
        {
            return PlainSymbol;
        }
    }
}
