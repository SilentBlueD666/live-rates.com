using System;
using System.Collections.Specialized;

namespace LiveRates.Client.Extensions
{
    internal static class CollectionExtensions
    {
        /// <summary>
        /// Constructs an HTML encoded query string.
        /// </summary>
        /// <param name="parameters">NameValueCollection object containing the key value pairs to encode.</param>
        /// <returns>A HTML encoded query string.</returns>
        public static string ToQueryString(this NameValueCollection parameters)
        {
            string queryString = string.Empty;

            int paramCount = parameters.Count;
            if (paramCount > 0)
            {
                string[] queryParams = new string[paramCount];
                for (int i = 0; i < paramCount; i++)
                {
                    string value = parameters.Get(i);
                    if (value.IndexOf(',') < 0)
                        value = Uri.EscapeDataString(value);

                    queryParams[i] = string.Concat(parameters.GetKey(i), "=", value);
                }
                queryString = string.Join("&", queryParams);
            }

            return queryString;
        }
    }
}
