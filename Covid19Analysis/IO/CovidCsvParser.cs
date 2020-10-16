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
        public List<CovidStatisticsError> Errors { get; }


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
        public List<CovidStatistic> ParseText(string text)
        {
            List<CovidStatistic> statisticsList = new List<CovidStatistic>();
            string[] values = text.Split(new[] {"\r\n", "\r", "\n"},
                StringSplitOptions.None);

            for (int i = 1; i < values.Length - 1; i++)
            {

                string[] contents = values[i].Split(",");
                try
                {

                    DateTime date = DateTime.ParseExact(contents[0], "yyyyMMdd", null);
                    int positiveIncrease = int.Parse(contents[2]);
                    int negativeIncrease = int.Parse(contents[3]);
                    int deaths = int.Parse(contents[4]);
                    int hospitalized = int.Parse(contents[5]);

                    if (contents[1].Contains("GA"))
                    {
                        CovidStatistic statistic = new CovidStatistic(date, positiveIncrease, negativeIncrease, deaths,
                            hospitalized);
                        statisticsList.Add(statistic);
                    }
                }
                catch (FormatException)
                {
                    this.Errors.Add(new CovidStatisticsError(contents.ToString(), i));
                }



            }

            return statisticsList;
        }

        #endregion
    }
}
   

