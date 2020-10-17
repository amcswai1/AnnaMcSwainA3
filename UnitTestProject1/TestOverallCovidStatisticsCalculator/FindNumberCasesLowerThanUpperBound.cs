using System;
using System.Collections.Generic;
using Covid19Analysis.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestCovid19Analysis.TestOverallCovidStatisticsCalculator
{
    [TestClass]
    public class FindNumberCasesLowerThanUpperBound
    {
        /// <summary>
        /// Input({Positive Cases} in CovidStatistic list.) Expected output
        /// {}                                          InvalidOperationException
        /// {500}                                       0
        /// {501}                                       0
        /// {500,600}                                   0
        /// {499}                                       1
        /// {499,500}                                   1    
        /// {499,300}                                   2    
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
                Assert.ThrowsException<InvalidOperationException>(() =>
                    overallCalculator.FindNumberCasesLowerThanUpperBound(500));
            }

            /// <summary>
            /// Tests when one items in list no match well below boundary.
            /// </summary>
            [TestMethod]
            public void TestOneItemsInListNoMatchBelowBoundary()
            {
                var statistics = new List<CovidStatistic> {
                    new CovidStatistic(new DateTime(2020, 10, 17), 500, 500, 500, 500)
                };
                var overallCalculator = new OverallCovidStatisticsCalculator(statistics);
                var result = overallCalculator.FindNumberCasesLowerThanUpperBound(500);
                Assert.AreEqual(0, result);
            }

            /// <summary>
            /// Tests when one items in list no match at boundary.
            /// </summary>
            [TestMethod]
            public void TestOneItemsInListNoMatchAtBoundary()
            {
                var statistics = new List<CovidStatistic> {
                    new CovidStatistic(new DateTime(2020, 10, 17), 501, 501, 501, 501)
                };
                var overallCalculator = new OverallCovidStatisticsCalculator(statistics);
                var result = overallCalculator.FindNumberCasesLowerThanUpperBound(500);
                Assert.AreEqual(0, result);
            }

            /// <summary>
            /// Tests when multiple items in list no match below and at boundary.
            /// </summary>
            [TestMethod]
            public void TestMultipleItemsInListNoMatchBelowAndAtBoundary()
            {
                var statistics = new List<CovidStatistic> {
                    new CovidStatistic(new DateTime(2020, 10, 17), 500, 500, 500, 500),
                    new CovidStatistic(new DateTime(2020, 10, 17), 600, 600, 600, 600)
                };
                var overallCalculator = new OverallCovidStatisticsCalculator(statistics);
                var result = overallCalculator.FindNumberCasesLowerThanUpperBound(500);
                Assert.AreEqual(0, result);
            }

            /// <summary>
            /// Tests when one items in list match Right above boundary.
            /// </summary>
            [TestMethod]
            public void TestOneItemsInListMatchAboveBoundary()
            {
                var statistics = new List<CovidStatistic> {
                    new CovidStatistic(new DateTime(2020, 10, 17), 499, 499, 499, 499)
                };
                var overallCalculator = new OverallCovidStatisticsCalculator(statistics);
                var result = overallCalculator.FindNumberCasesLowerThanUpperBound(500);
                Assert.AreEqual(1, result);
            }

            /// <summary>
            /// Tests when there are  multiple items in list and multiple items match below and at boundary.
            /// </summary>
            [TestMethod]
            public void TestMultipleItemsInListMultipleOneMatchBelowAndAtBoundary()
            {
                var statistics = new List<CovidStatistic> {
                    new CovidStatistic(new DateTime(2020, 10, 17), 499, 499, 499, 499),
                    new CovidStatistic(new DateTime(2020, 10, 17), 500, 500, 500, 500)
                };
                var overallCalculator = new OverallCovidStatisticsCalculator(statistics);
                var result = overallCalculator.FindNumberCasesLowerThanUpperBound(500);
                Assert.AreEqual(1, result);
            }

            /// <summary>
            /// Tests when there are  multiple items in list and multiple items match below and at boundary.
            /// </summary>
            [TestMethod]
            public void TestMultipleItemsInListMultipleAllMatchBelowAndAtBoundary()
            {
                var statistics = new List<CovidStatistic> {
                    new CovidStatistic(new DateTime(2020, 10, 17), 499, 499, 499, 499),
                    new CovidStatistic(new DateTime(2020, 10, 17), 300, 300, 300, 300)
                };
                var overallCalculator = new OverallCovidStatisticsCalculator(statistics);
                var result = overallCalculator.FindNumberCasesLowerThanUpperBound(500);
                Assert.AreEqual(2, result);
            }
        }
    }
}
