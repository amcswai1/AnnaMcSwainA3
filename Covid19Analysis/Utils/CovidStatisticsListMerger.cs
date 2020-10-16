using System.Collections.Generic;
using System.Linq;
using Covid19Analysis.Model;

namespace Covid19Analysis.Utils

{
    class CovidStatisticsListMerger
    {
        private readonly List<CovidStatistic> existingStatistics;
        private readonly List<CovidStatistic> incomingStatistics;
        private readonly List<CovidStatistic> duplicateStatistics;

        /// <summary>
        /// Initializes a new instance of the <see cref="CovidStatisticsListMerger"/> class.
        /// </summary>
        /// <param name="existingStatistics">The existing list.</param>
        /// <param name="incomingStatistics">The list to merge.</param>
        /// <param name="duplicateStatistics">The duplicates list.</param>
        public CovidStatisticsListMerger(List<CovidStatistic> existingStatistics, List<CovidStatistic> incomingStatistics, List<CovidStatistic> duplicateStatistics)
        {
            this.existingStatistics = existingStatistics;
            this.duplicateStatistics = duplicateStatistics;
            this.incomingStatistics = incomingStatistics;
        }

        /// <summary>
        /// Merges the existing data and new data without duplicates.
        /// </summary>
        /// <returns></returns>
        public IList<CovidStatistic> KeepAllExistingData()
        {
            IList<CovidStatistic> toRemove = new List<CovidStatistic>();

            foreach (var duplicate in this.duplicateStatistics)
            {
                var existingDuplicate = this.existingStatistics.Find(statistic => statistic.Date == duplicate.Date);
                toRemove.Add(existingDuplicate);
            }

            var combinedStatistics = this.existingStatistics.Concat(this.incomingStatistics).ToList();
            System.Diagnostics.Debug.Write(combinedStatistics.ToString());
            foreach (var duplicate in toRemove)
            {
                combinedStatistics.Remove(duplicate);
            }

            return combinedStatistics.ToList();

        }

        /// <summary>
        /// Overrides existing duplicate data with new data
        /// </summary>
        public IList<CovidStatistic> ReplaceAllExistingDuplicateDataWithNewData()
        {
            IList<CovidStatistic> toRemove = new List<CovidStatistic>();
            foreach(var duplicate in this.duplicateStatistics)
            {
                var existingDuplicate = this.existingStatistics.Find(statistic => statistic.Date == duplicate.Date);
                toRemove.Add(existingDuplicate);
            }
            
            var combinedStatistics = this.existingStatistics.Concat(this.incomingStatistics).ToList();
            foreach (var duplicate in toRemove){
                combinedStatistics.Remove(duplicate);
            }
            return combinedStatistics.ToList();

        }
        
        /// <summary>
        /// Keeps the existing data for current duplicate.
        /// </summary>
        /// <param name="duplicateItem">The duplicate item.</param>
        /// <returns></returns>
        public void KeepExistingDataForCurrentDuplicate(CovidStatistic duplicateItem)
        {
            this.incomingStatistics.Remove(duplicateItem);
           
        }

        /// <summary>
        /// Replaces the current item in list with duplicate item in new list.
        /// </summary>
        /// <param name="duplicateItem">The duplicate item.</param>
        public void ReplaceOneCurrentDataWithNewData(CovidStatistic duplicateItem)
        {
            var existingDuplicate = this.existingStatistics.Find(statistic => statistic.Date == duplicateItem.Date);
            this.existingStatistics.Remove(existingDuplicate);
        }

    }
}
