using System;
using System.Collections.Generic;
using Covid19Analysis.Model;

namespace Covid19Analysis.Utils

{
    internal class OverallCovidStatisticsReportBuilder
    {
        #region Data members

        private readonly OverallCovidStatisticsCalculator overallCalculator;
        private readonly CovidStatisticsHistogramReportBuilder histogramBuilder;
        private const string IntegerFormat = "N0";
        private const string DoubleFormat = "#,#.##";
        

        #endregion

        #region Properties

        /// <summary>
        ///     Gets or sets the lower bound input.
        /// </summary>
        /// <value>
        ///     The lower bound input.
        /// </value>
        public int LowerBoundInput { get; set; }

        /// <summary>
        ///     Gets or sets the upper bound input.
        /// </summary>
        /// <value>
        ///     The upper bound input.
        /// </value>
        public int UpperBoundInput { get; set; }

        /// <summary>
        ///     Gets or sets the histogram bin size.
        /// </summary>
        /// <value>
        ///     The histogram bin size.
        /// </value>
        public int HistogramBinSizeInput { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="OverallCovidStatisticsReportBuilder" /> class.
        /// </summary>
        /// <param name="statistics">The statistics list.</param>
        public OverallCovidStatisticsReportBuilder(IList<CovidStatistic> statistics)
        {
            this.overallCalculator = new OverallCovidStatisticsCalculator(statistics);
            this.histogramBuilder = new CovidStatisticsHistogramReportBuilder(statistics);
        }

        #endregion

        #region Methods

        private string firstPositiveDateToString()
        {
            var output = string.Empty;
            var firstDate = this.overallCalculator.FindDateOfFirstPositiveCase();
            output += Environment.NewLine + "Date of First Positive Increase in Cases: " +
                      firstDate.ToShortDateString();
            return output;
        }

        private string highestRecordedPositiveCasesToString()
        {
            var foundStatistic = this.overallCalculator.FindHighestRecordedPositiveCases();
            var highestPositiveIncrease = foundStatistic.PositiveIncrease;
            var dateOccurOn = foundStatistic.Date.ToShortDateString();
            var output = Environment.NewLine + "Highest Recorded Positive Increase in Cases: " +
                         highestPositiveIncrease.ToString(OverallCovidStatisticsReportBuilder.IntegerFormat) + " cases, on " + dateOccurOn;
            return output;
        }

        private string highestRecordedNegativeCasesToString()
        {
            var foundStatistic = this.overallCalculator.FindHighestRecordedNegativeCases();
            var highestNegativeIncrease = foundStatistic.NegativeIncrease;
            var dateOccurOn = foundStatistic.Date.ToShortDateString();
            var output = Environment.NewLine + "Highest Recorded Negative Increase in Cases: " +
                         highestNegativeIncrease.ToString(OverallCovidStatisticsReportBuilder.IntegerFormat) + " cases, on " + dateOccurOn;
            return output;
        }

        private string highestRecordedTotalTestsToString()
        {
            var foundStatistic = this.overallCalculator.FindHighestNumberOfTests();
            var totalTests = foundStatistic.PositiveIncrease + foundStatistic.NegativeIncrease;
            var dateOccurOn = foundStatistic.Date.ToShortDateString();
            var output = Environment.NewLine + "Highest Recorded Tests Taken: " +
                         totalTests.ToString(OverallCovidStatisticsReportBuilder.IntegerFormat) + " tests, on " + dateOccurOn;
            return output;
        }

        private string highestRecordedDeathsToString()
        {
            var foundStatistic = this.overallCalculator.FindHighestRecordedDeaths();
            var highestDeaths = foundStatistic.Deaths;
            var dateOccurOn = foundStatistic.Date.ToShortDateString();
            var output = Environment.NewLine + "Highest Recorded Deaths: " +
                         highestDeaths.ToString(OverallCovidStatisticsReportBuilder.IntegerFormat) + " deaths, on " + dateOccurOn;
            return output;
        }

        private string highestRecordedHospitalizationsToString()
        {
            var foundStatistic = this.overallCalculator.FindHighestRecordedHospitalizations();
            var highestHospitalizations = foundStatistic.Hospitalized;
            var dateOccurOn = foundStatistic.Date.ToShortDateString();
            var output = Environment.NewLine + "Highest Recorded Hospitalizations: " +
                         highestHospitalizations.ToString(OverallCovidStatisticsReportBuilder.IntegerFormat) + " deaths, on " + dateOccurOn;
            return output;
        }

        private string highestPercentPositiveIncreaseToString()
        {
            var foundStatistic = this.overallCalculator.FindHighestRecordedPositiveIncreasePercentage();
            var highestPercentPositive = foundStatistic.PercentPositiveIncrease;
            var dateOccurOn = foundStatistic.Date.ToShortDateString();
            var output = Environment.NewLine + "Highest Recorded Percent Positive Increase in Cases: " +
                         highestPercentPositive.ToString(OverallCovidStatisticsReportBuilder.DoubleFormat) + "% , on " + dateOccurOn;
            return output;
        }

        private string averageNumberPositivePerDayToString()
        {
            var averagePositive = this.overallCalculator.FindAveragePositiveCasesSinceStart();
            var output = Environment.NewLine + "Average Positive Cases Per Day: " +
                         averagePositive.ToString(OverallCovidStatisticsReportBuilder.DoubleFormat) + " cases";
            return output;
        }

        private string overallPositivityRateToString()
        {
            var averagePositive = this.overallCalculator.FindOverallPositivityRate();
            var output = Environment.NewLine + "Overall Positivity Rate : " +
                         averagePositive.ToString(OverallCovidStatisticsReportBuilder.DoubleFormat) + "%";
            return output;
        }

        private string numberOfDaysFromFirstTestAboveNumberToString(int upperBoundInput)
        {
            var daysCount = this.overallCalculator.FindNumberCasesHigherThanLowerBound(upperBoundInput);
            var output = Environment.NewLine + "Number of Days With More Than " +
                         upperBoundInput.ToString(OverallCovidStatisticsReportBuilder.IntegerFormat) + " Cases: " +
                         daysCount.ToString(OverallCovidStatisticsReportBuilder.IntegerFormat) + " days";
            return output;
        }

        private string numberOfDaysFromFirstTestBelowNumberToString(int lowerBoundInput)
        {
            var daysCount = this.overallCalculator.FindNumberCasesLowerThanUpperBound(lowerBoundInput);
            var output = Environment.NewLine + "Number of Days With Less Than " +
                         lowerBoundInput.ToString(OverallCovidStatisticsReportBuilder.IntegerFormat) + " Cases: " +
                         daysCount.ToString(OverallCovidStatisticsReportBuilder.IntegerFormat) + " days";
            return output;
        }

        /// <summary>
        ///     Gets the overall data string representation.
        /// </summary>
        /// <returns>the string to set the summary to</returns>
        public string OverallDataToString()
        {
            var output = Environment.NewLine + "Overall Statistics" + Environment.NewLine;
            output += this.firstPositiveDateToString();
            output += this.highestRecordedPositiveCasesToString();
            output += this.highestRecordedNegativeCasesToString();
            output += this.highestRecordedTotalTestsToString();
            output += this.highestRecordedDeathsToString();
            output += this.highestRecordedHospitalizationsToString();
            output += this.highestPercentPositiveIncreaseToString();
            output += this.averageNumberPositivePerDayToString();
            output += this.overallPositivityRateToString();
            output += this.numberOfDaysFromFirstTestAboveNumberToString(this.UpperBoundInput);
            output += this.numberOfDaysFromFirstTestBelowNumberToString(this.LowerBoundInput);
            output += Environment.NewLine;
            output += this.histogramBuilder.BuildPositiveIncreaseHistogram(this.HistogramBinSizeInput);
            return output;
        }

        #endregion
    }
}