using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Covid19Analysis.Model;

namespace Covid19Analysis.Utils

{
    internal class MonthlyCovidStatisticsReportBuilder
    {
        #region Data members

        private readonly MonthlyCovidStatisticsCalculator monthlyCalculator;
        private readonly string integerFormat = "N0";
        private readonly string doubleFormat = "N";

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="MonthlyCovidStatisticsReportBuilder" /> class.
        /// </summary>
        /// <param name="statistics">The statistics statistics.</param>
        public MonthlyCovidStatisticsReportBuilder(IList<CovidStatistic> statistics)
        {
            this.monthlyCalculator = new MonthlyCovidStatisticsCalculator(statistics);
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Dates the with suffix to string.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns>the string representation of the day followed by it's suffix</returns>
        private static string dateWithSuffixToString(DateTime date)
        {
            var day = date.Day;
            switch (day)
            {
                case 1:
                case 21:
                case 31:
                    return day + "st";
                case 2:
                case 22:
                    return day + "nd";
                case 3:
                case 23:
                    return day + "rd";
                default:
                    return day + "th";
            }
        }

        private string highestNumberOfPositiveCasesToString(IList<CovidStatistic> monthlyStatistics)
        {
            var statistics = this.monthlyCalculator.FindAllHighestNumberOfPositiveTests(monthlyStatistics);
            var highestNumberPositiveCases = statistics.First().PositiveIncrease;
            var output = Environment.NewLine + "Highest Recorded Positive Increase in Cases: " +
                         highestNumberPositiveCases.ToString(this.integerFormat) + " cases, on the " +
                         this.formatMultipleDaysToString(statistics.ToList());
            return output;
        }

        private string lowestNumberOfPositiveCasesToString(IList<CovidStatistic> monthlyStatistics)
        {
            var statistics = this.monthlyCalculator.FindAllLowestNumberOfPositiveTests(monthlyStatistics);
            var lowestNumberPositiveCases = statistics.First().PositiveIncrease;
            var output = Environment.NewLine + "Lowest Recorded Positive Increase in Cases: " +
                         lowestNumberPositiveCases.ToString(this.integerFormat) + " cases, on the " +
                         this.formatMultipleDaysToString(statistics.ToList());
            return output;
        }

        private string highestTotalTestsToString(IList<CovidStatistic> monthlyStatistics)
        {
            var statistics = this.monthlyCalculator.FindAllHighestTotalOfTests(monthlyStatistics);
            var highestTotalTests = statistics.First().PositiveIncrease;
            var output = Environment.NewLine + "Highest Total Cases: " +
                         highestTotalTests.ToString(this.integerFormat) + " cases, on the " +
                         this.formatMultipleDaysToString(statistics.ToList());
            return output;
        }

        private string lowestTotalTestsToString(IList<CovidStatistic> monthlyStatistics)
        {
            var statistics = this.monthlyCalculator.FindAllLowestTotalOfTests(monthlyStatistics);
            var lowestTotalTests = statistics.First().PositiveIncrease;
            var output = Environment.NewLine + "Lowest Total Cases: " +
                         lowestTotalTests.ToString(this.integerFormat) + " cases, on the " +
                         this.formatMultipleDaysToString(statistics);
            return output;
        }

        private string averageNumberPositiveTestsPerDayToString(IList<CovidStatistic> monthlyStatistics)
        {
            var statistic = this.monthlyCalculator.FindAveragePositiveTests(monthlyStatistics.ToList());
            var output = Environment.NewLine + "Average number of positive tests per day: " +
                         statistic.ToString(this.doubleFormat);
            return output;
        }

        private string averageTotalNumberTestsPerDayToString(IList<CovidStatistic> monthlyStatistics)
        {
            var statistic = this.monthlyCalculator.FindAverageTotalTests(monthlyStatistics);
            var output = Environment.NewLine + "Average number of total tests per day: " +
                         statistic.ToString(this.doubleFormat);
            return output;
        }

        private string findCovidDataByMonthInteger(int month)
        {
            var monthStatistics = this.monthlyCalculator.FindAllStatisticsInMonth(month);
            var output = "(Number of days loaded: " + monthStatistics.Count + ")";
            if (monthStatistics.Count == 0)
            {
                return output + Environment.NewLine + "No statistics were recorded for this month." +
                       Environment.NewLine;
            }
            output += this.highestNumberOfPositiveCasesToString(monthStatistics.ToList());
            output += this.lowestNumberOfPositiveCasesToString(monthStatistics.ToList());
            output += this.highestTotalTestsToString(monthStatistics.ToList());
            output += this.lowestTotalTestsToString(monthStatistics.ToList());
            output += this.averageNumberPositiveTestsPerDayToString(monthStatistics.ToList());
            output += this.averageTotalNumberTestsPerDayToString(monthStatistics.ToList());
            output += Environment.NewLine;
            return output;
        }

        private string formatMultipleDaysToString(IList<CovidStatistic> statistics)
        {
            var output = string.Empty;
            if (statistics.Count > 1)
            {
                foreach (var statistic in statistics)
                {
                    if (statistics.Last().Equals(statistic))
                    {
                        var day = statistic.Date;
                        output += "and the " + dateWithSuffixToString(day);
                    }
                    else
                    {
                        var day = statistic.Date;
                        output += dateWithSuffixToString(day) + ", ";
                    }
                }
            }
            else
            {
                var day = statistics.First().Date;
                output += dateWithSuffixToString(day);
            }
            return output;
        }

        /// <summary>
        ///     Gets the monthly data summary and returns the string that the text box should print.
        /// </summary>
        /// <returns>the total string of monthly summarized data</returns>
        public string MonthlyDataToString()
        {
            var currentMonth = this.monthlyCalculator.FindStartingMonthInteger();
            var output = Environment.NewLine + Environment.NewLine + "Monthly Statistics" + Environment.NewLine;
            for (var i = 0; i <= this.monthlyCalculator.FindNumberOfMonthsInData(); i++)
            {
                var monthStatistics = this.monthlyCalculator.FindAllStatisticsInMonth(currentMonth);
                if (monthStatistics.Count != 0)
                {
                    output += Environment.NewLine +
                              CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(currentMonth) + " " +
                              this.monthlyCalculator.FindYearAssociatedWithMonth(monthStatistics);
                    output += this.findCovidDataByMonthInteger(currentMonth);
                }
                else
                {
                    output += Environment.NewLine +
                              CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(currentMonth) + " " +
                              this.monthlyCalculator.FindMissingMonthsYear(monthStatistics);
                    output += this.findCovidDataByMonthInteger(currentMonth);
                }
                currentMonth++;
            }
            return output;
        }

        #endregion
    }
}