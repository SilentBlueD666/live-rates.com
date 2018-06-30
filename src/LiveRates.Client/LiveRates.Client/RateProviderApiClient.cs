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
    public class RateProviderApiClient : IRateProviderApiClient, IDisposable
    {
        #region Fields

        private HttpClientHandler _webApiHandler = null;
        private HttpClient _webApiCaller = null;
        private readonly string _baseUri = "https://www.live-rates.com/";

        #endregion

        #region Public Properties

        public string Key { get; set; }

        #endregion

        #region Constructors

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

        public RateProviderApiClient(string key)
            : this()
        {
            Key = key;
        }

        #endregion

        #region IRateProviderApiClient Implementation

        public Task<IEnumerable<LiveRate>> GetPriceAsync(IEnumerable<string> symbols)
        {
            return GetPriceAsync(symbols, CancellationToken.None);
        }

        public Task<IEnumerable<LiveRate>> GetPriceAsync(IEnumerable<string> symbols, CancellationToken cancellationToken)
        {
            var queryStringParams = new NameValueCollection(2)
            {
                { "rate", string.Join(",", symbols) }
            };

            HttpRequestMessage request = BuildRequest(
                apiName: "api/price",
                parameters: queryStringParams
                );

            return SendRequestAsync(request, cancellationToken);
        }

        public Task<IEnumerable<LiveRate>> GetPriceAsync(IEnumerable<LiveRateSymbol> symbols)
        {
            return GetPriceAsync(symbols, CancellationToken.None);
        }

        public Task<IEnumerable<LiveRate>> GetPriceAsync(IEnumerable<LiveRateSymbol> symbols, CancellationToken cancellationToken)
        {
            return GetPriceAsync(symbols.Select(s => s.RequestSymbol), cancellationToken);
        }

        public Task<IEnumerable<LiveRate>> GetRatesAsync()
        {
            return GetRatesAsync(CancellationToken.None);
        }

        public Task<IEnumerable<LiveRate>> GetRatesAsync(CancellationToken cancellationToken)
        {
            return SendRequestAsync(BuildRequest("/rates"), cancellationToken);
        }

        public Task<IEnumerable<LiveRateSymbol>> GetSymbolsAsync()
        {
            return GetSymbolsAsync(CancellationToken.None);
        }

        public async Task<IEnumerable<LiveRateSymbol>> GetSymbolsAsync(CancellationToken cancellationToken)
        {
            var rates = await GetRatesAsync(cancellationToken).ConfigureAwait(false);

            return from r in rates
                   select new LiveRateSymbol(r.Currency);
        }

        public Task<IEnumerable<LiveRateSymbol>> GetCurrencySymbolsAsync()
        {
            return GetCurrencySymbolsAsync(CancellationToken.None);
        }

        public async Task<IEnumerable<LiveRateSymbol>> GetCurrencySymbolsAsync(CancellationToken cancellationToken)
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
        /// <param name="method">The HttpMethod the default is GET.</param>
        /// <param name="apiRequest">ApiRequest base object values to apply to the request.</param>
        /// <returns>A HttpRequestMessage setup with defaults and specified values.</returns>
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
        /// <param name="apiName">API function name.</param>
        /// <returns>A Uri object that contains the full endpoint information.</returns>
        protected virtual Uri BuildRequestUri(string apiName, NameValueCollection parameters = null)
        {
            var builder = new UriBuilder(_baseUri)
            {
                Path = $"{apiName.TrimEnd('/')}/"
            };

            string key = Key;
            if (!string.IsNullOrWhiteSpace(key))
            {
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

        protected virtual async Task<IEnumerable<LiveRate>> SendRequestAsync(HttpRequestMessage request, CancellationToken cancellationToken)
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
                           select new LiveRate
                           {
                               Ask = r.Ask.ToDecimal(),
                               Bid = r.Bid.ToDecimal(),
                               Currency = r.Currency,
                               High = r.High.ToDecimal(),
                               Low = r.Low.ToDecimal(),
                               Rate = r.Rate,
                               TimeStamp = r.TimeStamp
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

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
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
