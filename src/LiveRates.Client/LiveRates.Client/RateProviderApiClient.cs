using LiveRates.Client.Extensions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace LiveRates.Client
{
    /// <summary>
    /// Represent live-rates.com API Client.
    /// </summary>
    public class RateProviderApiClient : IRateProviderApiClient, IDisposable
    {
        #region Fields

        protected HttpClientHandler _webApiHandler = null;
        protected HttpClient _webApiCaller = null;
        protected readonly string _baseUri = "https://www.live-rates.com/";

        #endregion

        #region Public Properties

        /// <summary>
        /// The API access Key.
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Determines if a <see cref="Key"/> has been specified.
        /// </summary>
        public bool HasKey { get => !string.IsNullOrWhiteSpace(Key); }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a default instance of the class.
        /// </summary>
        public RateProviderApiClient()
        {
            _webApiHandler = new HttpClientHandler
            {
                CookieContainer = new CookieContainer(),
                UseCookies = false,
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
            };

            _webApiCaller = new HttpClient(_webApiHandler);
            _webApiCaller.DefaultRequestHeaders.ExpectContinue = false;
            _webApiCaller.DefaultRequestHeaders.ConnectionClose = false;
        }

        /// <summary>
        /// Creates a default instance of the class with an API Key.
        /// </summary>
        /// <param name="key">The API access Key.</param>
        public RateProviderApiClient(string key)
            : this()
        {
            Key = key;
        }

        #endregion

        #region IRateProviderApiClient Implementation

        /// <inheritdoc/>
        public Task<LiveRate> GetPriceAsync(string symbol)
        {
            return GetPriceAsync(symbol, CancellationToken.None);
        }

        /// <inheritdoc/>
        public async Task<LiveRate> GetPriceAsync(string symbol, CancellationToken cancellationToken)
        {
            if (!HasKey)
                throw new InvalidOperationException("API access Key is required for access to prices.");

            var queryStringParams = new NameValueCollection(2)
            {
                { "rate", symbol }
            };

            HttpRequestMessage request = BuildRequest(
                apiName: "api/price",
                parameters: queryStringParams
                );

            var rates = await SendRateRequestAsync(request, cancellationToken).ConfigureAwait(false);

            return rates.FirstOrDefault();
        }

        /// <inheritdoc/>
        public Task<LiveRate> GetPriceAsync(LiveRateSymbol symbol)
        {
            return GetPriceAsync(symbol, CancellationToken.None);
        }

        /// <inheritdoc/>
        public Task<LiveRate> GetPriceAsync(LiveRateSymbol symbol, CancellationToken cancellationToken)
        {
            return GetPriceAsync(symbol.RequestSymbol, cancellationToken);
        }

        /// <inheritdoc/>
        public Task<IEnumerable<LiveRate>> GetPricesAsync(IEnumerable<string> symbols)
        {
            return GetPricesAsync(symbols, CancellationToken.None);
        }

        /// <inheritdoc/>
        public Task<IEnumerable<LiveRate>> GetPricesAsync(IEnumerable<string> symbols, CancellationToken cancellationToken)
        {
            if (!HasKey)
                throw new InvalidOperationException("API access Key is required for access to prices.");

            var queryStringParams = new NameValueCollection(2)
            {
                { "rate", string.Join(",", symbols) }
            };

            HttpRequestMessage request = BuildRequest(
                apiName: "api/price",
                parameters: queryStringParams
                );

            return SendRateRequestAsync(request, cancellationToken);
        }

        /// <inheritdoc/>
        public virtual Task<IEnumerable<LiveRate>> GetPricesAsync(IEnumerable<LiveRateSymbol> symbols)
        {
            return GetPricesAsync(symbols, CancellationToken.None);
        }

        /// <inheritdoc/>
        public virtual Task<IEnumerable<LiveRate>> GetPricesAsync(IEnumerable<LiveRateSymbol> symbols, CancellationToken cancellationToken)
        {
            return GetPricesAsync(symbols.Select(s => s.RequestSymbol), cancellationToken);
        }

        /// <inheritdoc/>
        public virtual Task<IEnumerable<LiveRate>> GetRatesAsync()
        {
            return GetRatesAsync(CancellationToken.None);
        }

        /// <inheritdoc/>
        public virtual Task<IEnumerable<LiveRate>> GetRatesAsync(CancellationToken cancellationToken)
        {
            return SendRateRequestAsync(BuildRequest("/rates"), cancellationToken);
        }

        /// <inheritdoc/>
        public virtual Task<IEnumerable<LiveRateSymbol>> GetSymbolsAsync()
        {
            return GetSymbolsAsync(CancellationToken.None);
        }

        /// <inheritdoc/>
        public virtual async Task<IEnumerable<LiveRateSymbol>> GetSymbolsAsync(CancellationToken cancellationToken)
        {
            if (!HasKey)
                throw new InvalidOperationException("API access Key is required to access list of rate symbols.");

            using (HttpResponseMessage response = await _webApiCaller.SendAsync(BuildRequest("/api/rates"), cancellationToken).ConfigureAwait(false))
            {
                response.EnsureSuccessStatusCode();
                if (response.Content != null)
                {
                    string jsonContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                    var symbols = JsonConvert.DeserializeObject<ApiSymbol[]>(jsonContent);
                    if (symbols.All(r => r.Currency == null))
                    {
                        var errors = JsonConvert.DeserializeObject<ApiErrorMessage[]>(jsonContent);
                        throw new ApiException(string.Join(", ", errors.Select(x => x.Error)), errors, jsonContent);
                    }

                    return from s in symbols
                           select new LiveRateSymbol(s.Currency, EpochHelper.FromEpoch(Convert.ToInt64(s.Updated)));
                }
            }

            return new List<LiveRateSymbol>(0);
        }

        /// <inheritdoc/>
        public virtual Task<IEnumerable<LiveRateSymbol>> GetCurrencySymbolsAsync()
        {
            return GetCurrencySymbolsAsync(CancellationToken.None);
        }

        /// <inheritdoc/>
        public virtual async Task<IEnumerable<LiveRateSymbol>> GetCurrencySymbolsAsync(CancellationToken cancellationToken)
        {
            var symbols = await GetSymbolsAsync(cancellationToken);
            return symbols.Where(s => s.IsCurrency == true);
        }

        #endregion

        #region Request Helper Methods

        /// <summary>
        /// Builds a Http request message with API client defaults.
        /// </summary>
        /// <param name="apiName">The API endpoint to call.</param>
        /// <param name="parameters">A <see cref="NameValueCollection"/> for creating a query string.</param>
        /// <returns>A <see cref="HttpRequestMessage"/> set-up with defaults and specified values.</returns>
        protected virtual HttpRequestMessage BuildRequest(string apiName, NameValueCollection parameters = null)
        {
            var request = new HttpRequestMessage()
            {
                RequestUri = BuildRequestUri(apiName, parameters),
                Method = HttpMethod.Get,
                Content = null
            };

            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            return request;
        }

        /// <summary>
        /// Build a HTTP Uri endpoint request.
        /// </summary>
        /// <param name="apiName">The API endpoint to call.</param>
        /// <param name="parameters">A <see cref="NameValueCollection"/> for creating a query string.</param>
        /// <returns>A Uri object that contains the full endpoint information.</returns>
        protected virtual Uri BuildRequestUri(string apiName, NameValueCollection parameters = null)
        {
            var builder = new UriBuilder(_baseUri)
            {
                Path = $"{apiName.TrimEnd('/')}/"
            };

            if (HasKey)
            {
                string key = Key;
                if (parameters == null)
                {
                    parameters = new NameValueCollection(1)
                    {
                        { "key", key }
                    };
                }
                else
                    parameters.Add("key", key);
            }

            if (parameters != null)
                builder.Query = parameters.ToQueryString();

            return builder.Uri;
        }

        #endregion

        #region Http Helpers

        /// <summary>
        /// Sends a request to the API endpoint.
        /// </summary>
        /// <param name="request">The <see cref="HttpRequestMessage"/> to send.</param>
        /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
        /// <returns>An <see cref="Task"/> that represents an asynchronous operation that contains a list of <see cref="LiveRate"/> objects.</returns>
        protected virtual async Task<IEnumerable<LiveRate>> SendRateRequestAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            using (HttpResponseMessage response = await _webApiCaller.SendAsync(request, cancellationToken).ConfigureAwait(false))
            {
                response.EnsureSuccessStatusCode();
                if (response.Content != null)
                {
                    string jsonContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                    var rates = JsonConvert.DeserializeObject<ApiLiveRate[]>(jsonContent);
                    if (rates.All(r => r.Currency == null))
                    {
                        var errors = JsonConvert.DeserializeObject<ApiErrorMessage[]>(jsonContent);
                        throw new ApiException(string.Join(", ", errors.Select(x => x.Error)), errors, jsonContent);
                    }

                    return from r in rates
                           where r.Currency != null
                           select new LiveRate(new LiveRateSymbol(r.Currency.Replace('/', '_'), EpochHelper.FromEpoch(Convert.ToInt64(r.TimeStamp))))
                           {
                               Ask = r.Ask.ToDecimal(),
                               Bid = r.Bid.ToDecimal(),
                               High = r.High.ToDecimal(),
                               Low = r.Low.ToDecimal(),
                               Rate = r.Rate,
                               TimeStamp = EpochHelper.FromEpoch(Convert.ToInt64(r.TimeStamp))
                           };
                }
            }

            return new List<LiveRate>(0);
        }

        #endregion

        #region IDisposable Support

        private bool _disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    Key = null;
                }

                _webApiCaller?.Dispose();
                _webApiCaller = null;

                _webApiHandler?.Dispose();
                _webApiHandler = null;

                _disposedValue = true;
            }
        }

        ~RateProviderApiClient()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(false);
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
