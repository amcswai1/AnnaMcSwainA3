using System;
using System.Collections.Generic;
using Covid19Analysis.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestCovid19Analysis.TestOverallCovidStatisticsCalculator
{
    /// <summary>
    /// Input({Positive Cases} in CovidStatistic list.) Expected output
    /// {}                                          InvalidOperationException
    /// {100}                                       0
    /// {200}                                       0
    /// {200,100}                                   0
    /// {201}                                       1
    /// {201,200}                                   1
    /// {202,600}                                   2    
    /// </summary>
    [TestClass]
    public class FindNumberCasesHigherThanLowerBound
    {
        /// <summary>
        /// Tests when no items in list.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestNoItemsInList()
        {
            var statistics = new List<CovidStatistic>();
            var overallCalculator = new OverallCovidStatisticsCalculator(statistics);
            Assert.ThrowsException<InvalidOperationException>(() => overallCalculator.FindNumberCasesHigherThanLowerBound(200));
        }

        /// <summary>
        /// Tests when one items in list no match well below boundary.
        /// </summary>
        [TestMethod]
        public void TestOneItemsInListNoMatchBelowBoundary()
        {
            var statistics = new List<CovidStatistic>
            {
                new CovidStatistic(new DateTime(2020, 10, 17), 100, 100, 100, 100)
            };
            var overallCalculator = new OverallCovidStatisticsCalculator(statistics);
            var result = overallCalculator.FindNumberCasesHigherThanLowerBound(200);
            Assert.AreEqual(0,result);
        }

        /// <summary>
        /// Tests when one items in list no match at boundary.
        /// </summary>
        [TestMethod] public void TestOneItemsInListNoMatchAtBoundary()
        {
            var statistics = new List<CovidStatistic>
            {
                new CovidStatistic(new DateTime(2020, 10, 17), 200, 200, 200, 200)
            };
            var overallCalculator = new OverallCovidStatisticsCalculator(statistics);
            var result = overallCalculator.FindNumberCasesHigherThanLowerBound(200);
            Assert.AreEqual(0, result);
        }

        /// <summary>
        /// Tests when multiple items in list no match below and at boundary.
        /// </summary>
        [TestMethod]
        public void TestMultipleItemsInListNoMatchBelowAndAtBoundary()
        {
            var statistics = new List<CovidStatistic> {
                new CovidStatistic(new DateTime(2020, 10, 17), 100, 100, 100, 100),
                new CovidStatistic(new DateTime(2020, 10, 17), 200, 200, 200, 200)
            };
            var overallCalculator = new OverallCovidStatisticsCalculator(statistics);
            var result = overallCalculator.FindNumberCasesHigherThanLowerBound(200);
            Assert.AreEqual(0, result);
        }

        /// <summary>
        /// Tests when one items in list match Right above boundary.
        /// </summary>
        [TestMethod]
        public void TestOneItemsInListMatchAboveBoundary()
        {
            var statistics = new List<CovidStatistic>
            {
                new CovidStatistic(new DateTime(2020, 10, 17), 201, 201, 201, 201)
            };
            var overallCalculator = new OverallCovidStatisticsCalculator(statistics);
            var result = overallCalculator.FindNumberCasesHigherThanLowerBound(200);
            Assert.AreEqual(1, result);
        }

        /// <summary>
        /// Tests when there are  multiple items in list and only one items match below and at boundary.
        /// </summary>
        [TestMethod]
        public void TestMultipleItemsInListMultipleOneMatchBelowAndAtBoundary()
        {
            var statistics = new List<CovidStatistic> {
                new CovidStatistic(new DateTime(2020, 10, 17), 201, 201, 201, 201),
                new CovidStatistic(new DateTime(2020, 10, 17), 200, 200, 200, 200)
            };
            var overallCalculator = new OverallCovidStatisticsCalculator(statistics);
            var result = overallCalculator.FindNumberCasesHigherThanLowerBound(200);
            Assert.AreEqual(1, result);
        }

        /// <summary>
        /// Tests when there are  multiple items in list and multiple items match below and at boundary.
        /// </summary>
        [TestMethod]
        public void TestMultipleItemsInListMultipleMatchBelowAndAtBoundary()
        {
            var statistics = new List<CovidStatistic> {
                new CovidStatistic(new DateTime(2020, 10, 17), 201, 201, 201, 201),
                new CovidStatistic(new DateTime(2020, 10, 17), 600, 600, 600, 600)
            };
            var overallCalculator = new OverallCovidStatisticsCalculator(statistics);
            var result = overallCalculator.FindNumberCasesHigherThanLowerBound(200);
            Assert.AreEqual(2, result);
        }
    }
}
