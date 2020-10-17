using System;
using System.Collections.Generic;
using Covid19Analysis.Model;

namespace Covid19Analysis.IO

{
    /// <summary>
    /// Interprets the input of a CSV or TXT file for recorded Covid statistics in Georgia
    /// </summary>
    public class CovidCsvParser
    {
        #region Fields

        /// <summary>
        /// Gets the errors in the file.
        /// </summary>
        /// <value>
        /// The errors.
        /// </value>
        public IList<CovidStatisticsError> Errors { get; }

        private const int DateIndex = 0;
        private const int PositiveIncreaseIndex = 2;
        private const int NegativeIncreaseIndex = 3;
        private const int DeathsIndex = 4;
        private const int HospitalizedIndex = 5;
        private const string StateFilter = "GA";

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CovidCsvParser"/> class.
        /// </summary>
        public CovidCsvParser()
        {
            this.Errors = new List<CovidStatisticsError>();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Parses the text of the given CSV or TXT file text into their respective data lists.
        /// </summary>
        public IList<CovidStatistic> ParseText(string text)
        {
            IList<CovidStatistic> statistics = new List<CovidStatistic>();
            string[] values = text.Split(new[] {"\r\n", "\r", "\n"},
                StringSplitOptions.None);
            for (int i = 1; i < values.Length - 1; i++)
            {
                string[] contents = values[i].Split(",");
                try
                { 
                    DateTime date = DateTime.ParseExact(contents[CovidCsvParser.DateIndex], "yyyyMMdd", null);
                    int positiveIncrease = int.Parse(contents[CovidCsvParser.PositiveIncreaseIndex]);
                    int negativeIncrease = int.Parse(contents[CovidCsvParser.NegativeIncreaseIndex]);
                    int deaths = int.Parse(contents[CovidCsvParser.DeathsIndex]);
                    int hospitalized = int.Parse(contents[CovidCsvParser.HospitalizedIndex]);
                    if (contents[1].Contains(CovidCsvParser.StateFilter))
                    {
                        CovidStatistic statistic = new CovidStatistic(date, positiveIncrease, negativeIncrease, deaths,
                            hospitalized);
                        statistics.Add(statistic);
                    }
                }
                catch (FormatException)
                {
                    this.Errors.Add(new CovidStatisticsError(contents, i));
                }
            }
            return statistics;
        }

        #endregion
    }
}
   

