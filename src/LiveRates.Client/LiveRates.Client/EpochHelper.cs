using System;

namespace LiveRates.Client
{
    /// <summary>
    /// Static class with Unix epoch helper methods.
    /// </summary>
    internal static class EpochHelper
    {
        /// <summary>
        /// Converts a DateTime to the long representation which is the number of milliseconds since the Unix epoch.
        /// </summary>
        /// <param name="dateTime">A DateTime to convert to epoch time.</param>
        /// <returns>The long number of seconds since the Unix epoch.</returns>
        public static long ToEpoch(DateTime dateTime) => (long)(dateTime - new DateTime(1970, 1, 1)).TotalMilliseconds;

        /// <summary>
        /// Converts a long representation of time since the Unix epoch to a DateTime.
        /// </summary>
        /// <param name="epoch">The number of milliseconds since Jan 1, 1970.</param>
        /// <returns>A DateTime representing the time since the epoch.</returns>
        public static DateTime FromEpoch(long epoch) => new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified).AddMilliseconds(epoch);

        /// <summary>
        /// Converts a DateTime? to the long? representation which is the number of milliseconds since the Unix epoch.
        /// </summary>
        /// <param name="dateTime">A DateTime? to convert to epoch time.</param>
        /// <returns>The long? number of seconds since the Unix epoch.</returns>
        public static long? ToEpoch(DateTime? dateTime) => dateTime.HasValue ? (long?)ToEpoch(dateTime.Value) : null;

        /// <summary>
        /// Converts a long? representation of time since the Unix epoch to a DateTime?.
        /// </summary>
        /// <param name="epoch">The number of milliseconds since Jan 1, 1970.</param>
        /// <returns>A DateTime? representing the time since the epoch.</returns>
        public static DateTime? FromEpoch(long? epoch) => epoch.HasValue ? (DateTime?)FromEpoch(epoch.Value) : null;
    }
}
