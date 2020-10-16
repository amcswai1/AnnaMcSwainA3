namespace Covid19Analysis.Model
{
    /// <summary>
    /// Represents an Error in the Covid Statistics Data
    /// </summary>
    public class CovidStatisticsError
    {
        /// <summary>
        /// Gets the error line contents.
        /// </summary>
        /// <value>
        /// The error line contents.
        /// </value>
        public string ErrorLineContents { get; }
        /// <summary>
        /// Gets the line error was found.
        /// </summary>
        /// <value>
        /// The line error was found.
        /// </value>
        public int LineErrorWasFound { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CovidStatisticsError"/> class.
        /// </summary>
        /// <param name="errorLineContents">The error line contents.</param>
        /// <param name="lineErrorWasFound">The line error was found.</param>
        public CovidStatisticsError(string[] errorLineContents, int lineErrorWasFound)
        {
            this.ErrorLineContents = string.Join(",",errorLineContents);
            this.LineErrorWasFound = lineErrorWasFound;
        }
    }


}
