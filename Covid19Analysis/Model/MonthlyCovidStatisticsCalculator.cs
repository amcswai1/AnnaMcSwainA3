using System;
using System.Collections.Generic;
using System.Linq;

namespace Covid19Analysis.Model 
{
    /// <summary>
    /// Represents the monthly statistics calculator for all given Covid statistics
    /// </summary>
    public class MonthlyCovidStatisticsCalculator
    {
        #region Fields
        
        private readonly IList<CovidStatistic> statistics;
        private readonly DateTime startingDate;
        private readonly DateTime endingDate;
        
        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MonthlyCovidStatisticsCalculator"/> class.
        /// </summary>
        /// <param name="statistics">The statistics list.</param>
        public MonthlyCovidStatisticsCalculator(IList<CovidStatistic> statistics)
        {
            this.startingDate = statistics.Min(statistic => statistic.Date);
            this.endingDate = statistics.Max(statistic => statistic.Date);
            this.statistics = statistics;
          
        }

        #endregion

        #region Public Methods  
        
        /// <summary>
        /// Finds the highest number of positive tests.
        /// </summary>
        /// <param name="statisticsOfMonth">The statistics of month.</param>
        /// <returns>the list of matching highest number of positive tests recorded</returns>
        public IList<CovidStatistic> FindAllHighestNumberOfPositiveTests(IList<CovidStatistic> statisticsOfMonth)
        {
            var highestPositiveIncrease = statisticsOfMonth.Max(statistic => statistic.PositiveIncrease);
            var foundStatistic = this.statistics.ToList().FindAll(statistic => statistic.PositiveIncrease == highestPositiveIncrease);
            return foundStatistic;
        }
        /// <summary>
        /// Finds the lowest number of positive tests.
        /// </summary>
        /// <param name="statisticsOfMonth">The statistics of month.</param>
        /// <returns>the list of matching lowest number of positive tests recorded</returns>
        public IList<CovidStatistic> FindAllLowestNumberOfPositiveTests(IList<CovidStatistic> statisticsOfMonth)
        {
            var lowestPositiveIncrease = statisticsOfMonth.Min(statistic => statistic.PositiveIncrease);
            var foundStatistic = this.statistics.ToList().FindAll(statistic => statistic.PositiveIncrease == lowestPositiveIncrease);
            return foundStatistic;
        }
        /// <summary>
        /// Finds the highest total of tests.
        /// </summary>
        /// <param name="statisticsOfMonth">The statistics of month.</param>
        /// <returns>the list of matching highest tests recorded</returns>
        public IList<CovidStatistic> FindAllHighestTotalOfTests(IList<CovidStatistic> statisticsOfMonth)
        {
            var totalTests = statisticsOfMonth.Max(statistic => statistic.PositiveIncrease + statistic.NegativeIncrease);
            var foundStatistic = this.statistics.ToList().FindAll(statistic => statistic.PositiveIncrease + statistic.NegativeIncrease == totalTests);
            return foundStatistic;
        }
        /// <summary>
        /// Finds the lowest total of tests.
        /// </summary>
        /// <param name="statisticsOfMonth">The statistics of month.</param>
        /// <returns>the list of matching lowest recorded tests</returns>
        public IList<CovidStatistic> FindAllLowestTotalOfTests(IList<CovidStatistic> statisticsOfMonth)
        {
            var totalTests = statisticsOfMonth.Min(statistic => statistic.PositiveIncrease + statistic.NegativeIncrease);
            var foundStatistic = this.statistics.ToList().FindAll(statistic => statistic.PositiveIncrease + statistic.NegativeIncrease == totalTests);
            return foundStatistic;
        }
        /// <summary>
        /// Finds the average positive tests.
        /// </summary>
        /// <param name="statisticsOfMonth">The statistics of month.</param>
        /// <returns>the average positive tests taken recorded</returns>
        public double FindAveragePositiveTests(IList<CovidStatistic> statisticsOfMonth)
        {
            var positiveIncreases = new List<int>();
            foreach (var statistic in statisticsOfMonth)
            {
                positiveIncreases.Add(statistic.PositiveIncrease);
            }
            return positiveIncreases.Average();
        }
        /// <summary>
        /// Finds the average total tests.
        /// </summary>
        /// <param name="statisticsOfMonth">The statistics of month.</param>
        /// <returns>the average total of tests taken recorded</returns>
        public double FindAverageTotalTests(IList<CovidStatistic> statisticsOfMonth)
        {
            var totalTests = new List<int>();
                foreach (var statistic in statisticsOfMonth)
                {
                    totalTests.Add(statistic.PositiveIncrease + statistic.NegativeIncrease);
                }
            return totalTests.Average();

        }
        /// <summary>
        /// Finds the starting month integer.
        /// </summary>
        /// <returns>data starting month integer representation</returns>
        public int FindStartingMonthInteger()
        {
            return this.startingDate.Month;
        }
        /// <summary>
        /// Finds the year associated with month.
        /// </summary>
        /// <param name="statisticsOfMonth">The statistics of month.</param>
        /// <returns>the year associated with the month's integer</returns>
        public int FindYearAssociatedWithMonth(IList<CovidStatistic> statisticsOfMonth)
        {
            return statisticsOfMonth.First().Date.Year;
        }
        /// <summary>
        /// Finds the number of months in data.
        /// </summary>
        /// <returns>the number of months recorded for the data</returns>
        public int FindNumberOfMonthsInData()
        {
            return ((this.startingDate.Year - this.endingDate.Year) * 12) + this.endingDate.Month - this.startingDate.Month;
        }
        /// <summary>
        /// Finds the missing months year.
        /// </summary>
        /// <param name="monthStatistics">The month statistics.</param>
        /// <returns>the month with no recorded data year</returns>
        public int FindMissingMonthsYear(IList<CovidStatistic> monthStatistics)
        {
            
            if (monthStatistics.Count == 0)
            {
                if (this.startingDate.Year - this.endingDate.Year == 0)
                {
                    return startingDate.Year;
                }
            }
            return startingDate.Year;
        }
        /// <summary>
        /// Finds all statistics in month.
        /// </summary>
        /// <param name="month">The month.</param>
        /// <returns>all statistics associated with the months integer</returns>
        public IList<CovidStatistic> FindAllStatisticsInMonth(int month)
        {
            var statisticsInMonth = this.statistics.ToList().FindAll(statistic => statistic.Date.Month == month);
            return statisticsInMonth;
        }

        #endregion

    }
}
