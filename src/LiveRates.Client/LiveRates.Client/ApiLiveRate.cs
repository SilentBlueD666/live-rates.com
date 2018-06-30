using System;
using System.Collections.Generic;
using System.Text;

namespace LiveRates.Client
{
    internal class ApiLiveRate
    {
        #region Public Properties

        public string Currency { get; set; }

        public decimal Rate { get; set; }

        public string Bid { get; set; }

        public string Ask { get; set; }

        public string High { get; set; }

        public string Low { get; set; }

        public string TimeStamp { get; set; }

        #endregion

        #region Object Overrides

        public override string ToString()
        {
            return $"Currency: {Currency}, Rate:{Rate}";
        }

        #endregion
    }
}
