using Covid19Analysis.Model;
using System;
using System.Collections.Generic;

namespace Covid19Analysis.Utils


{
    /// <summary>
    /// Represents the string representation for the covid statistics histogram data
    /// </summary>
    class CovidStatisticsHistogramStringBuilder
    {

        private readonly CovidStatisticsHistogramCalculator histogramCalculator;

        /// <summary>
        /// Initializes a new instance of the <see cref="CovidStatisticsHistogramStringBuilder"/> class.
        /// </summary>
        /// <param name="statisticsList">The statistics list.</param>
        public CovidStatisticsHistogramStringBuilder(List<CovidStatistic> statisticsList)
        {
            this.histogramCalculator = new CovidStatisticsHistogramCalculator(statisticsList);
        }
        /// <summary>
        /// Builds the positive increase histogram string.
        /// </summary>
        /// <param name="range">The range threshold</param>
        /// <returns></returns>
        public String BuildPositiveIncreaseHistogram(double range)
        {
            int[] segments = this.histogramCalculator.CountStatisticsPositiveCasesWithinRanges(range);
            string output = Environment.NewLine + "Positive Case Histogram Breakdown: " + Environment.NewLine;
            double rangeStart = 0;
            double rangeEnd = range;
            for (int i = 0; i < segments.Length; i++)
            {
                output += $"{rangeStart,10} - {rangeEnd,-10}: {segments[i],-10}\n";
                rangeStart = rangeEnd + 1;
                rangeEnd = (range * (i + 2));
            }
            
            return output;
        }
    }
}
