using System;
using System.Collections.Generic;

namespace LiveRates.Client
{
    /// <summary>
    /// Represents an exception for when an error is returned from the API.
    /// </summary>
    public sealed class ApiException : Exception
    {
        #region Public Properties

        /// <summary>
        /// The original response body content from the API.
        /// </summary>
        public string BodyContent { get; }

        /// <summary>
        /// A list of API error messages.
        /// </summary>
        public IEnumerable<ApiErrorMessage> ApiErrorMessages { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a default instance of the class.
        /// </summary>
        /// <param name="message">The error message.</param>
        /// <param name="apiErrorMessages">A list of error messages from the API.</param>
        /// <param name="bodyContent">The original body content from the API.</param>
        public ApiException(string message, IEnumerable<ApiErrorMessage> apiErrorMessages, string bodyContent)
            : base(message)
        {
            ApiErrorMessages = apiErrorMessages;
            BodyContent = bodyContent;
        }

        #endregion
    }
}
