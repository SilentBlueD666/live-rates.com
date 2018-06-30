namespace LiveRates.Client
{
    /// <summary>
    /// Represents an API error message.
    /// </summary>
    public sealed class ApiErrorMessage
    {
        #region Public Properties

        /// <summary>
        /// Error status.
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Error message.
        /// </summary>
        public string Error { get; set; }

        #endregion
    }
}
