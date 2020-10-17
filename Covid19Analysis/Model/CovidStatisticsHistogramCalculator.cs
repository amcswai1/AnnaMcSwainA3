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
        #region Fields

        private readonly IList<CovidStatistic> statistics;
        private const int NeutralizeBuffer = 5;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CovidStatisticsHistogramCalculator"/> class.
        /// </summary>
        /// <param name="statistics">The statistics.</param>
        public CovidStatisticsHistogramCalculator(IList<CovidStatistic> statistics)
        {
            this.statistics = statistics;
        }

        #endregion

        #region Public Methods 

        /// <summary>
        /// Counts the statistics positive cases within given range.
        /// </summary>
        /// <param name="range">The range threshold</param>
        /// <returns></returns>
        public int[] CountStatisticsPositiveCasesWithinRanges(double range)
        {
            double maxPositiveCases = this.statistics.Max(statistic => statistic.PositiveIncrease);
            int segments = Convert.ToInt32(Math.Ceiling(maxPositiveCases / range));
            if (segments == 0)
            {
                segments = 1;
            }
            int[] positiveIncreaseCount = new int[segments];
            foreach (var statistic in this.statistics)
            {
                var positiveIncreaseValue = statistic.PositiveIncrease;
                int segmentNumber = (int)((positiveIncreaseValue - CovidStatisticsHistogramCalculator.NeutralizeBuffer) / range);
                positiveIncreaseCount[segmentNumber]++;
            }
            return positiveIncreaseCount;
        }

        #endregion
    }
}
