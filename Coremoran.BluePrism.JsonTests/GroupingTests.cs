using Microsoft.VisualStudio.TestTools.UnitTesting;
using Coremoran.BluePrism.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Coremoran.BluePrism.Json.Tests
{
    [TestClass()]
    public class GroupingTests
    {
        [TestMethod()]
        public void SimpleGroupTest()
        {
            var source = Enumerable.Range(1, 10).Select(i => new { Index = i, Text = (char)(i + 65) });
            var data = source.GroupBy(g => g.Index % 2 == 0);

            var actual = BluePrismConverter.ObjectToCollection(data);

            Assert.IsNotNull(actual, "actual is null");
            Assert.AreEqual(2, actual.Columns.Count, "actual contains columns");
            Assert.AreEqual(2, actual.Rows.Count, "actual didn't have 2 rows");
            Assert.IsTrue(actual.Columns.Contains(SpecialValues.GroupingSingleKeyColumnName), "There is no 'Key' column");
            Assert.IsTrue(actual.Columns.Contains(SpecialValues.GroupingGroupColumnName), "There is no 'Group' column");
        }
    }
}