using System.Collections.Generic;
using Covid19Analysis.Model;

namespace Covid19Analysis.IO
{
    /// <summary>
    /// Represents the Covid CSV File Saver
    /// </summary>
    internal class CovidCsvSaver
    {
        #region Fields

        private readonly IList<CovidStatistic> statistics;
        private const string State = "GA";

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CovidCsvSaver"/> class.
        /// </summary>
        /// <param name="statistics">The statistics currently loaded.</param>
        public CovidCsvSaver(IList<CovidStatistic> statistics)
        {
            this.statistics = statistics;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Converts the currently loaded statistics to a string form able to be saved to a CSV file.
        /// </summary>
        /// <returns> the string representation of all statistics in csv form </returns>
        public string BuildCsvFileContents()
        {
            var output = "date,state,positiveIncrease,negativeIncrease,deathIncrease,hospitalizedIncrease\n";
            foreach (var statistic in this.statistics)
            {
                output += statistic.Date.ToString("yyyyMMdd") + "," + CovidCsvSaver.State + "," +
                          statistic.PositiveIncrease + "," + statistic.NegativeIncrease + "," + statistic.Deaths + "," +
                          statistic.Hospitalized + "\n";
            }
            return output;
        }

        #endregion

    }
}
