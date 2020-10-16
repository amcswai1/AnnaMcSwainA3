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
        private readonly List<CovidStatistic> statisticsList;
        private readonly DateTime startingDate;
        private readonly DateTime endingDate;
        
        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MonthlyCovidStatisticsCalculator"/> class.
        /// </summary>
        /// <param name="statisticsList">The statistics list.</param>
        public MonthlyCovidStatisticsCalculator(List<CovidStatistic> statisticsList)
        {
            this.startingDate = statisticsList.Min(statistic => statistic.Date);
            this.endingDate = statisticsList.Max(statistic => statistic.Date);
            this.statisticsList = statisticsList;
          
        }

        #endregion

        #region Public Methods        
        /// <summary>
        /// Finds the highest number of positive tests.
        /// </summary>
        /// <param name="statisticsOfMonth">The statistics of month.</param>
        /// <returns>the list of matching highest number of positive tests recorded</returns>
        public List<CovidStatistic> FindAllHighestNumberOfPositiveTests(List<CovidStatistic> statisticsOfMonth)
        {
            int highestPositiveIncrease = statisticsOfMonth.Max(statistic => statistic.PositiveIncrease);
            List<CovidStatistic> foundStatistic = this.statisticsList.FindAll(statistic => statistic.PositiveIncrease == highestPositiveIncrease);
            return foundStatistic;
        }
        /// <summary>
        /// Finds the lowest number of positive tests.
        /// </summary>
        /// <param name="statisticsOfMonth">The statistics of month.</param>
        /// <returns>the list of matching lowest number of positive tests recorded</returns>
        public List<CovidStatistic> FindAllLowestNumberOfPositiveTests(List<CovidStatistic> statisticsOfMonth)
        {
            int lowestPositiveIncrease = statisticsOfMonth.Min(statistic => statistic.PositiveIncrease);
            List<CovidStatistic> foundStatistic = this.statisticsList.FindAll(statistic => statistic.PositiveIncrease == lowestPositiveIncrease);
            return foundStatistic;
        }
        /// <summary>
        /// Finds the highest total of tests.
        /// </summary>
        /// <param name="statisticsOfMonth">The statistics of month.</param>
        /// <returns>the list of matching highest tests recorded</returns>
        public List<CovidStatistic> FindAllHighestTotalOfTests(List<CovidStatistic> statisticsOfMonth)
        {
            int totalTests = statisticsOfMonth.Max(statistic => statistic.PositiveIncrease + statistic.NegativeIncrease);
            List<CovidStatistic> foundStatistic = this.statisticsList.FindAll(statistic => statistic.PositiveIncrease + statistic.NegativeIncrease == totalTests);
            return foundStatistic;
        }
        /// <summary>
        /// Finds the lowest total of tests.
        /// </summary>
        /// <param name="statisticsOfMonth">The statistics of month.</param>
        /// <returns>the list of matching lowest recorded tests</returns>
        public List<CovidStatistic> FindAllLowestTotalOfTests(List<CovidStatistic> statisticsOfMonth)
        {
            var totalTests = statisticsOfMonth.Min(statistic => statistic.PositiveIncrease + statistic.NegativeIncrease);
            List<CovidStatistic> foundStatistic = this.statisticsList.FindAll(statistic => statistic.PositiveIncrease + statistic.NegativeIncrease == totalTests);
            return foundStatistic;
        }
        /// <summary>
        /// Finds the average positive tests.
        /// </summary>
        /// <param name="statisticsOfMonth">The statistics of month.</param>
        /// <returns>the average positive tests taken recorded</returns>
        public double FindAveragePositiveTests(List<CovidStatistic> statisticsOfMonth)
        {
            var positiveIncreases = new List<int>();
            foreach (CovidStatistic statistic in statisticsOfMonth)
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
        public double FindAverageTotalTests(List<CovidStatistic> statisticsOfMonth)
        {
            var totalTests = new List<int>();
            foreach (CovidStatistic statistic in statisticsOfMonth)
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
        public int FindYearAssociatedWithMonth(List<CovidStatistic> statisticsOfMonth)
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
        public int FindMissingMonthsYear(List<CovidStatistic> monthStatistics)
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
        public List<CovidStatistic> FindAllStatisticsInMonth(int month)
        {
            List<CovidStatistic> statisticsInMonth = this.statisticsList.FindAll(statistic => statistic.Date.Month == month);
            return statisticsInMonth;
        }
      

        #endregion

    }
}
