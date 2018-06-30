using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace LiveRates.Client
{
    public interface IRateProviderApiClient
    {
        #region Information Methods

        Task<IEnumerable<LiveRateSymbol>> GetSymbolsAsync();

        Task<IEnumerable<LiveRateSymbol>> GetSymbolsAsync(CancellationToken cancellationToken);

        Task<IEnumerable<LiveRateSymbol>> GetCurrencySymbolsAsync();

        Task<IEnumerable<LiveRateSymbol>> GetCurrencySymbolsAsync(CancellationToken cancellationToken);

        #endregion

        #region API Endpoint Methods

        Task<IEnumerable<LiveRate>> GetRatesAsync();

        Task<IEnumerable<LiveRate>> GetRatesAsync(CancellationToken cancellationToken);

        Task<LiveRate> GetPriceAsync(string symbol);

        Task<LiveRate> GetPriceAsync(string symbol, CancellationToken cancellationToken);

        Task<LiveRate> GetPriceAsync(LiveRateSymbol symbol);

        Task<LiveRate> GetPriceAsync(LiveRateSymbol symbol, CancellationToken cancellationToken);

        Task<IEnumerable<LiveRate>> GetPricesAsync(IEnumerable<string> symbols);

        Task<IEnumerable<LiveRate>> GetPricesAsync(IEnumerable<string> symbols, CancellationToken cancellationToken);

        Task<IEnumerable<LiveRate>> GetPricesAsync(IEnumerable<LiveRateSymbol> symbols);

        Task<IEnumerable<LiveRate>> GetPricesAsync(IEnumerable<LiveRateSymbol> symbols, CancellationToken cancellationToken);

        #endregion
    }
}
