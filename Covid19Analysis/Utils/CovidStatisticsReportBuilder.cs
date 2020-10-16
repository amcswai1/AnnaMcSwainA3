using System;
using System.Collections.Generic;
using Covid19Analysis.Model;

namespace Covid19Analysis.Utils
{
    class CovidStatisticsReportBuilder
    {
        private readonly OverallCovidStatisticsReportBuilder overallCovidStatisticsStringBuilder;
        private readonly MonthlyCovidStatisticsReportBuilder monthlyCovidStatisticsStringBuilder;
        /// <summary>
        /// Initializes a new instance of the <see cref="CovidStatisticsReportBuilder"/> class.
        /// </summary>
        /// <param name="statisticsList">The statistics list.</param>
        public CovidStatisticsReportBuilder(List<CovidStatistic> statisticsList)
        {
            this.overallCovidStatisticsStringBuilder = new OverallCovidStatisticsReportBuilder(statisticsList);
            this.monthlyCovidStatisticsStringBuilder = new MonthlyCovidStatisticsReportBuilder(statisticsList);
        }
        /// <summary>
        /// Builds the string for output to the summary.
        /// </summary>
        /// <returns>the string to set the summary box to</returns>
        public string BuildStringForOutput()
        {
            string output = String.Empty;
            output += this.overallCovidStatisticsStringBuilder.OverallDataToString();
            output += this.monthlyCovidStatisticsStringBuilder.MonthlyDataToString();
            return output;
        }
        /// <summary>
        /// Sets the upper and lower bound limits for the methods in overall data.
        /// </summary>
        /// <param name="upperLimit">The upper limit.</param>
        /// <param name="lowerLimit">The lower limit.</param>
        public void SetUpperAndLowerBoundLimits(int upperLimit, int lowerLimit)
        {
            this.overallCovidStatisticsStringBuilder.LowerBoundInput = lowerLimit;
            this.overallCovidStatisticsStringBuilder.UpperBoundInput = upperLimit;
        }
    }
}
