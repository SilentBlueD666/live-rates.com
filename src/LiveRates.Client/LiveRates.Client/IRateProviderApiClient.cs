using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace LiveRates.Client
{
    /// <summary>
    /// Live-rates.com API endpoint contract.
    /// </summary>
    public interface IRateProviderApiClient
    {
        #region Information Methods

        /// <summary>
        /// Gets a list of available rate symbols.
        /// </summary>
        /// <returns>An <see cref="Task"/> that represents an asynchronous operation that contains a list of <see cref="LiveRateSymbol"/> objects.</returns>
        Task<IEnumerable<LiveRateSymbol>> GetSymbolsAsync();

        /// <summary>
        /// Gets a list of available rate symbols.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
        /// <returns>An <see cref="Task"/> that represents an asynchronous operation that contains a list of <see cref="LiveRateSymbol"/> objects.</returns>
        Task<IEnumerable<LiveRateSymbol>> GetSymbolsAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Gets a list of available FX Currency rate symbols.
        /// </summary>
        /// <returns>An <see cref="Task"/> that represents an asynchronous operation that contains a list of <see cref="LiveRateSymbol"/> objects.</returns>
        Task<IEnumerable<LiveRateSymbol>> GetCurrencySymbolsAsync();

        /// <summary>
        /// Get a list of available FX Currency rate symbols.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
        /// <returns>An <see cref="Task"/> that represents an asynchronous operation that contains a list of <see cref="LiveRateSymbol"/> objects.</returns>
        Task<IEnumerable<LiveRateSymbol>> GetCurrencySymbolsAsync(CancellationToken cancellationToken);

        #endregion

        #region API Endpoint Methods

        /// <summary>
        /// Get all live rates available.
        /// </summary>
        /// <returns>An <see cref="Task"/> that represents an asynchronous operation that contains a list of <see cref="LiveRate"/> objects.</returns>
        Task<IEnumerable<LiveRate>> GetRatesAsync();

        /// <summary>
        /// Get all live rates available.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
        /// <returns>An <see cref="Task"/> that represents an asynchronous operation that contains a list of <see cref="LiveRate"/> objects.</returns>
        Task<IEnumerable<LiveRate>> GetRatesAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Get a single rate based on the supplied symbol.
        /// </summary>
        /// <param name="symbol">The price symbol for the rate. FX Currency rate symbols should contain an underscore between the currencies i.e. GBP_EUR.</param>
        /// <returns>An <see cref="Task"/> that represents an asynchronous operation that contains a list of <see cref="LiveRate"/> objects.</returns>
        Task<LiveRate> GetPriceAsync(string symbol);

        /// <summary>
        /// Get a single rate based on the supplied symbol.
        /// </summary>
        /// <param name="symbol">The price symbol for the rate. FX Currency rate symbols should contain an underscore between the currencies i.e. GBP_EUR.</param>
        /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
        /// <returns>An <see cref="Task"/> that represents an asynchronous operation that contains a list of <see cref="LiveRate"/> objects.</returns>
        Task<LiveRate> GetPriceAsync(string symbol, CancellationToken cancellationToken);

        /// <summary>
        /// Get a single rate based on the supplied symbol.
        /// </summary>
        /// <param name="symbol">The price symbol for the rate.</param>
        /// <returns>An <see cref="Task"/> that represents an asynchronous operation that contains a list of <see cref="LiveRate"/> objects.</returns>
        Task<LiveRate> GetPriceAsync(LiveRateSymbol symbol);

        /// <summary>
        /// Get a single rate based on the supplied symbol.
        /// </summary>
        /// <param name="symbol">The price symbol for the rate.</param>
        /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
        /// <returns>An <see cref="Task"/> that represents an asynchronous operation that contains a list of <see cref="LiveRate"/> objects.</returns>
        Task<LiveRate> GetPriceAsync(LiveRateSymbol symbol, CancellationToken cancellationToken);

        /// <summary>
        /// Get rates based on the symbols supplied.
        /// </summary>
        /// <param name="symbols">A list containing symbols, FX Currency rate symbols should contain an underscore between the currencies i.e. GBP_EUR.</param>
        /// <returns>An <see cref="Task"/> that represents an asynchronous operation that contains a list of <see cref="LiveRate"/> objects.</returns>
        Task<IEnumerable<LiveRate>> GetPricesAsync(IEnumerable<string> symbols);

        /// <summary>
        /// Get rates based on the symbols supplied.
        /// </summary>
        /// <param name="symbols">A list containing symbols, FX Currency rate symbols should contain an underscore between the currencies i.e. GBP_EUR.</param>
        /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
        /// <returns>An <see cref="Task"/> that represents an asynchronous operation that contains a list of <see cref="LiveRate"/> objects.</returns>
        Task<IEnumerable<LiveRate>> GetPricesAsync(IEnumerable<string> symbols, CancellationToken cancellationToken);

        /// <summary>
        /// Get rates based on the symbols supplied.
        /// </summary>
        /// <param name="symbols">A list containing symbols.</param>
        /// <returns>An <see cref="Task"/> that represents an asynchronous operation that contains a list of <see cref="LiveRate"/> objects.</returns>
        Task<IEnumerable<LiveRate>> GetPricesAsync(IEnumerable<LiveRateSymbol> symbols);

        /// <summary>
        /// Get rates based on the symbols supplied.
        /// </summary>
        /// <param name="symbols">A list containing symbols.</param>
        /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
        /// <returns>An <see cref="Task"/> that represents an asynchronous operation that contains a list of <see cref="LiveRate"/> objects.</returns>
        Task<IEnumerable<LiveRate>> GetPricesAsync(IEnumerable<LiveRateSymbol> symbols, CancellationToken cancellationToken);

        #endregion
    }
}
