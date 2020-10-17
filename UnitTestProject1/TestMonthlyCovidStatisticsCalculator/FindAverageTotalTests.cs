using System;
using System.Collections.Generic;
using Covid19Analysis.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestCovid19Analysis.TestMonthlyCovidStatisticsCalculator
{
    /// <summary>
    /// Input({Total Cases} in CovidStatistic list.) Expected output
    /// {}                                          InvalidOperationException
    /// {200}                                       200
    /// {200,600}                                   400     
    /// </summary>
    [TestClass]
    public class FindAverageTotalTests
    {
        /// <summary>
        /// Tests the average total tests when there are no items in the list.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestNoItemsInList()
        {
            var statistics = new List<CovidStatistic>();
            var monthlyCalculator = new MonthlyCovidStatisticsCalculator(statistics);
            Assert.ThrowsException<InvalidOperationException>(() => monthlyCalculator.FindAverageTotalTests(statistics));
        }

        /// <summary>
        /// Tests the average total tests when there is only one item in the list.
        /// </summary>
        [TestMethod]
        public void TestOneItemsInList()
        {
            var statistics = new List<CovidStatistic>
            {
                new CovidStatistic(new DateTime(2020, 10, 17), 100, 100, 100, 100)
            };
            var monthlyCalculator = new MonthlyCovidStatisticsCalculator(statistics);
            var result = monthlyCalculator.FindAverageTotalTests(statistics);
            Assert.AreEqual(200, result);
        }

        /// <summary>
        /// Tests the average total tests when there are multiple items in the list.
        /// </summary>
        [TestMethod]
        public void TestMultipleItemsInList()
        {
            var statistics = new List<CovidStatistic> {
                new CovidStatistic(new DateTime(2020, 10, 17), 100, 100, 100, 100),
                new CovidStatistic(new DateTime(2020, 10, 17), 300, 300, 100, 300)
            };
            var monthlyCalculator = new MonthlyCovidStatisticsCalculator(statistics);
            var result = monthlyCalculator.FindAverageTotalTests(statistics);
            Assert.AreEqual(400, result);
        }
    }
}
