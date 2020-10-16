using System;
using System.Collections.Generic;
using System.Linq;

namespace Covid19Analysis.Model
{
    /// <summary>
    /// Represents the covid statistics histogram data calculator
    /// </summary>
    class CovidStatisticsHistogramCalculator
    {
        private readonly List<CovidStatistic> statisticsList;
        private const int NeutralizeBuffer = 5;
        public CovidStatisticsHistogramCalculator(List<CovidStatistic> statisticsList)
        {
            this.statisticsList = statisticsList;
        }
        /// <summary>
        /// Counts the statistics positive cases within given range.
        /// </summary>
        /// <param name="range">The range threshold</param>
        /// <returns></returns>
        public int[] CountStatisticsPositiveCasesWithinRanges(double range)
        {
            double maxPositiveCases = this.statisticsList.Max(statistic => statistic.PositiveIncrease);
            int segments = Convert.ToInt32(Math.Ceiling(maxPositiveCases/range));
            if (segments == 0)
            {
                segments = 1;
            }
            int[] positiveIncreaseCount = new int[segments];
            foreach(var statistic in this.statisticsList)
            {
                var positiveIncreaseValue = statistic.PositiveIncrease;
                int segmentNumber = (int)((positiveIncreaseValue - CovidStatisticsHistogramCalculator.NeutralizeBuffer) / range);
                positiveIncreaseCount[segmentNumber]++;
            }
            return positiveIncreaseCount;
        }
    }
}
