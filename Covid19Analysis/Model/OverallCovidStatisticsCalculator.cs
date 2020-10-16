using System;
using System.Collections.Generic;
using System.Linq;

namespace Covid19Analysis.Model
{
    /// <summary>
    /// Represents the overall statistics calculator for all given Covid statistics
    /// </summary>
    public class OverallCovidStatisticsCalculator
    {
        #region Data members

        private readonly IList<CovidStatistic> statistics;
        private readonly DateTime startingDate;

        #endregion

        #region Constructors

        /// <summary>
        ///     Creates a StatisticsCalculator object
        /// </summary>
        /// <param name="statistics">The list of statistics from the CovidStatisticsList</param>
        public OverallCovidStatisticsCalculator(IList<CovidStatistic> statistics)
        {
            this.startingDate = statistics.Min(statistic => statistic.Date);
            this.statistics = statistics;
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Finds the first positive date recorded for cases.
        /// </summary>
        /// <returns>the first positive date</returns>
        public DateTime FindDateOfFirstPositiveCase()
        {
            var datesWithIncrease =
                this.statistics.ToList().FindAll(statistic => statistic.PositiveIncrease > 0);
            DateTime firstDate = datesWithIncrease.Min(statistic => statistic.Date);
            return firstDate;
        }

        /// <summary>
        ///     Finds the highest recorded positive cases.
        /// </summary>
        /// <returns>the statistic with the highest recorded positive cases</returns>
        public CovidStatistic FindHighestRecordedPositiveCases()
        {
            var highestPositiveIncrease =
                this.statistics.Max(statistic => statistic.PositiveIncrease);
            var foundStatistic =
                this.statistics.ToList().Find(statistic =>
                    statistic.PositiveIncrease == highestPositiveIncrease);
            return foundStatistic;
        }

        /// <summary>
        ///     Finds the highest recorded negative cases.
        /// </summary>
        /// <returns>the statistic with the highest recorded negative cases</returns>
        public CovidStatistic FindHighestRecordedNegativeCases()
        {
            var highestNegativeIncrease =
                this.statistics.Max(statistic => statistic.NegativeIncrease);
            var foundStatistic =
                this.statistics.ToList().Find(statistic =>
                    statistic.NegativeIncrease == highestNegativeIncrease);
            return foundStatistic;
        }

        /// <summary>
        ///     Finds the highest number of tests.
        /// </summary>
        /// <returns>the statistic with the highest recorded number of tests</returns>
        public CovidStatistic FindHighestNumberOfTests()
        {
            var combinedTests =
                this.statistics.Max(statistic =>
                    statistic.NegativeIncrease + statistic.PositiveIncrease);
            var foundStatistic = this.statistics.ToList().Find(statistic =>
                statistic.NegativeIncrease + statistic.PositiveIncrease == combinedTests);
            return foundStatistic;
        }

        /// <summary>
        ///     Finds the highest recorded deaths.
        /// </summary>
        /// <returns>the statistic with the highest recorded number of deaths</returns>
        public CovidStatistic FindHighestRecordedDeaths()
        {
            var highestDeaths = this.statistics.Max(statistic => statistic.Deaths);
            var foundStatistic = this.statistics.ToList().Find(statistic => statistic.Deaths == highestDeaths);
            return foundStatistic;
        }

        /// <summary>
        ///     Finds the highest recorded hospitalizations.
        /// </summary>
        /// <returns>the statistic with the highest recorded number of hospitalizations</returns>
        public CovidStatistic FindHighestRecordedHospitalizations()
        {
            var hospitalizations = this.statistics.Max(statistic => statistic.Hospitalized);
            var foundStatistic =
                this.statistics.ToList().Find(statistic => statistic.Hospitalized == hospitalizations);
            return foundStatistic;
        }

        /// <summary>
        ///     Finds the highest recorded positive increase percentage.
        /// </summary>
        /// <returns>the statistic with the highest recorded percent positive increase</returns>
        public CovidStatistic FindHighestRecordedPositiveIncreasePercentage()
        {
            var maxPercentPositive =
                this.statistics.Max(statistic => statistic.GetPercentPositiveIncrease());
            var foundStatistic = this.statistics.ToList().Find(statistic =>
                statistic.GetPercentPositiveIncrease().Equals(maxPercentPositive));
            return foundStatistic;
        }

        /// <summary>
        ///     Finds the average positive cases since start date.
        /// </summary>
        /// <returns>the average positive cases</returns>
        public double FindAveragePositiveCasesSinceStart()
        {
            var firstPositiveDate = this.FindDateOfFirstPositiveCase();
            var positiveCases = new List<int>();
            var foundStatistics =
                this.statistics.ToList().FindAll(statistic =>
                    statistic.Date >= firstPositiveDate);
            foreach (var statistic in foundStatistics)
            {
               positiveCases.Add(statistic.PositiveIncrease);
            }
            return positiveCases.Average();
        }

        /// <summary>
        ///     Finds the overall positivity rate.
        /// </summary>
        /// <returns>the positivity rate</returns>
        public double FindOverallPositivityRate()
        {
            double allPositives = this.statistics.Sum(statistic => statistic.PositiveIncrease);
            double totalTests =
                this.statistics.Sum(statistic =>
                    statistic.NegativeIncrease + statistic.PositiveIncrease);
            var percentage = allPositives / totalTests * 100;
            return percentage;
        }

        /// <summary>
        ///     Finds the number cases higher than lower bound.
        /// </summary>
        /// <param name="lowerBound">The lower bound.</param>
        /// <returns>the number of cases higher than the lower bound</returns>
        public int FindNumberCasesHigherThanLowerBound(int lowerBound)
        {
            var foundStatistics =
                this.statistics.ToList().FindAll(statistic =>
                    statistic.Date >= this.startingDate);
            var statisticsGreaterThan = foundStatistics.FindAll(statistic => statistic.PositiveIncrease > lowerBound);
            return statisticsGreaterThan.Count;
        }

        /// <summary>
        ///     Finds the number cases lower than upper bound.
        /// </summary>
        /// <param name="upperBound">The upper bound.</param>
        /// <returns>the number of cases lower than the upper bound</returns>
        public int FindNumberCasesLowerThanUpperBound(int upperBound)
        {
            var foundStatistics =
                this.statistics.ToList().FindAll(statistic => statistic.Date >= this.FindDateOfFirstPositiveCase());
            var statisticsLowerThan = foundStatistics.FindAll(statistic => statistic.PositiveIncrease < upperBound);
            return statisticsLowerThan.Count;
        }

        #endregion
    }
}