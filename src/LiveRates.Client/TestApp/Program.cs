using System;
using System.Collections.Generic;
using LiveRates.Client;

namespace TestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            using (var client = new RateProviderApiClient())
            {
                var symbols = new List<string>()
                {
                    "EUR/GBP",
                    "USD_GBP"
                };

                var liveRateSymbls = client.GetSymbolsAsync().Result;
                var prices0 = client.GetPricesAsync(symbols).Result;
                var currencyRateSymbols = client.GetCurrencySymbolsAsync().Result;
                var prices1 = client.GetPricesAsync(currencyRateSymbols).Result;

                Console.ReadLine();
            }
        }
    }
}
