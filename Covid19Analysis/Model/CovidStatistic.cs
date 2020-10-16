using System;

namespace Covid19Analysis.Model
{
    /// <summary>
    /// Represents an instance of a statistic in the Covid records.
    /// </summary>
    public class CovidStatistic
    {
        #region Fields

        /// <summary>
        /// Gets or sets the date of the recorded statistic.
        /// </summary>
        /// <value>
        /// The date.
        /// </value>
        public DateTime Date { get; set; }
        /// <summary>
        /// Gets or sets the positive increase of the recorded statistic.
        /// </summary>
        /// <value>
        /// The positive increase in cases.
        /// </value>
        public int PositiveIncrease { get; set; }
        /// <summary>
        /// Gets or sets the negative increase of the recorded statistic.
        /// </summary>
        /// <value>
        /// The negative increase in cases.
        /// </value>
        public int NegativeIncrease { get; set;  }
        /// <summary>
        /// Gets or sets the deaths of the recorded statistic.
        /// </summary>
        /// <value>
        /// The deaths recorded.
        /// </value>
        public int Deaths { get; set; }
        /// <summary>
        /// Gets or sets the hospitalized of the recorded statistic.
        /// </summary>
        /// <value>
        /// The number of hospitalized patients recorded.
        /// </value>
        public int Hospitalized { get; set; }
        /// <summary>
        /// Gets percent positive increase in cases of the recorded statistic.
        /// </summary>
        /// <value>
        /// The percent positive increase in cases recorded.
        /// </value>
        public double PercentPositiveIncrease { get; set; }
        
        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CovidStatistic"/> class.
        /// </summary>
        public CovidStatistic(DateTime date, int positiveIncrease, int negativeIncrease, int deaths, int hospitalized)
        {
            Date = date;
            PositiveIncrease = positiveIncrease;
            NegativeIncrease = negativeIncrease;
            Deaths = deaths;
            Hospitalized = hospitalized;
            PercentPositiveIncrease = this.GetPercentPositiveIncrease();
        }
        
        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the numerical value of the percent positive increase in covid cases.
        /// </summary>
        /// <returns>The numerical percentage value of the percent of positive covid cases</returns>
        public double GetPercentPositiveIncrease()
        {

            return (Convert.ToDouble(this.PositiveIncrease)) / (Convert.ToDouble(this.PositiveIncrease + this.NegativeIncrease)) * 100;
        }
        
        #endregion
    }
}
