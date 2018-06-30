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

        Task<IEnumerable<LiveRate>> GetPriceAsync(IEnumerable<string> symbols);

        Task<IEnumerable<LiveRate>> GetPriceAsync(IEnumerable<string> symbols, CancellationToken cancellationToken);

        Task<IEnumerable<LiveRate>> GetPriceAsync(IEnumerable<LiveRateSymbol> symbols);

        Task<IEnumerable<LiveRate>> GetPriceAsync(IEnumerable<LiveRateSymbol> symbols, CancellationToken cancellationToken);

        #endregion
    }
}
