using System;
using System.Collections.Generic;
using Covid19Analysis.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestCovid19Analysis.TestOverallCovidStatisticsCalculator
{
    /// <summary>
    /// Input({Positive Cases} in CovidStatistic list.) Expected output
    /// {}                                          InvalidOperationException
    /// {100}                                       50
    /// {100,200}                                   50
    /// {100,200,300}                               50
    /// </summary>
    [TestClass]
    public class FindOverallPositivityRate
    {
       
        /// <summary>
        /// Tests the average positive tests when there are no items in the list.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestNoItemsInList()
        {
            var statistics = new List<CovidStatistic>();
            var overallCalculator = new OverallCovidStatisticsCalculator(statistics);
            Assert.ThrowsException<InvalidOperationException>(() => overallCalculator.FindOverallPositivityRate());
        }

        /// <summary>
        /// Tests when one item in list.
        /// </summary>
        [TestMethod]
        public void TestOneItemInList()
        {
            var statistics = new List<CovidStatistic>
            {
                new CovidStatistic(new DateTime(2020, 10, 17), 100, 100, 100, 100)
            };
            var overallCalculator = new OverallCovidStatisticsCalculator(statistics);
            var result = overallCalculator.FindOverallPositivityRate();
            Assert.AreEqual(50, result);
        }

        /// <summary>
        /// Tests when two items in list.
        /// </summary>
        [TestMethod]
        public void TestTwoItemsInList()
        {
            var statistics = new List<CovidStatistic>
            {
                new CovidStatistic(new DateTime(2020, 10, 17), 100, 100, 100, 100),
                new CovidStatistic(new DateTime(2020, 10, 17), 200, 200, 200, 200)
            };
            var overallCalculator = new OverallCovidStatisticsCalculator(statistics);
            var result = overallCalculator.FindOverallPositivityRate();
            Assert.AreEqual(50, result);
        }

        /// <summary>
        /// Tests when multiple item in list.
        /// </summary>
        [TestMethod]
        public void TestMultipleItemInList()
        {
            var statistics = new List<CovidStatistic>
            {
                new CovidStatistic(new DateTime(2020, 10, 17), 100, 100, 100, 100),
                new CovidStatistic(new DateTime(2020, 10, 17), 200, 200, 200, 200),
                new CovidStatistic(new DateTime(2020, 10, 17), 300, 300, 300, 300)
            };
            var overallCalculator = new OverallCovidStatisticsCalculator(statistics);
            var result = overallCalculator.FindOverallPositivityRate();
            Assert.AreEqual(50, result);
        }
    }
}
