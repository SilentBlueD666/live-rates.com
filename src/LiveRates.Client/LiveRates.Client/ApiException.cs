using System;
using System.Collections.Generic;

namespace LiveRates.Client
{
    public sealed class ApiException : Exception
    {
        #region Public Properties

        public string BodyContent { get; }

        public IEnumerable<ApiErrorMessage> ApiErrorMessages { get; }

        #endregion

        #region Construtors

        public ApiException(string message, IEnumerable<ApiErrorMessage> apiErrorMessages, string bodyContent)
            : base(message)
        {
            ApiErrorMessages = apiErrorMessages;
            BodyContent = bodyContent;
        }

        #endregion
    }
}
