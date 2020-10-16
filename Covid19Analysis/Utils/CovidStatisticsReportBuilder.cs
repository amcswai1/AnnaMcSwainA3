using System;
using System.Collections.Generic;
using Covid19Analysis.Model;

namespace Covid19Analysis.Utils
{
    class CovidStatisticsStringBuilder
    {
        private readonly OverallCovidStatisticsStringBuilder overallCovidStatisticsStringBuilder;
        private readonly MonthlyCovidStatisticsStringBuilder monthlyCovidStatisticsStringBuilder;
        /// <summary>
        /// Initializes a new instance of the <see cref="CovidStatisticsStringBuilder"/> class.
        /// </summary>
        /// <param name="statisticsList">The statistics list.</param>
        public CovidStatisticsStringBuilder(List<CovidStatistic> statisticsList)
        {
            this.overallCovidStatisticsStringBuilder = new OverallCovidStatisticsStringBuilder(statisticsList);
            this.monthlyCovidStatisticsStringBuilder = new MonthlyCovidStatisticsStringBuilder(statisticsList);
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
