using Coremoran.BluePrism.Json;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Data;

namespace BluePrismTests
{
    [TestClass()]
    public class JsonValueTests
    {
        [TestMethod()]
        public void SingleValueArrayTest()
        {
            var json = "1";

            var result = BluePrismConverter.JsonToCollection(json);

            Assert.IsNotNull(result, "result is null");
            Assert.AreEqual(1, result.Columns.Count, "result didn't contain 1 column");
            Assert.AreEqual(SpecialValues.JsonUnnamedValue, result.Columns[0].ColumnName, "result didn't contains column name " + SpecialValues.JsonUnnamedValue);
            Assert.AreEqual(1, result.Rows.Count, "result didn't have 1 rows");
            Assert.AreEqual(1m, result.Rows[0][SpecialValues.JsonUnnamedValue], "result didn't have value 1 in rows");
            Assert.IsTrue(result.IsSingleRow(), "result is a single row");
        }

        [TestMethod()]
        public void SingleValueStringArrayTest()
        {
            var json = "\"1\"";

            var result = BluePrismConverter.JsonToCollection(json);

            Assert.IsNotNull(result, "result is null");
            Assert.AreEqual(1, result.Columns.Count, "result didn't contain 1 column");
            Assert.AreEqual(SpecialValues.JsonUnnamedValue, result.Columns[0].ColumnName, "result didn't contains column name " + SpecialValues.JsonUnnamedValue);
            Assert.AreEqual(1, result.Rows.Count, "result didn't have 1 rows");
            Assert.AreEqual("1", result.Rows[0][SpecialValues.JsonUnnamedValue], "result didn't have value '1' in rows");
            Assert.IsTrue(result.IsSingleRow(), "result is a single row");
        }

        [TestMethod()]
        public void SingleValueNullTest()
        {
            var json = "null";

            var result = BluePrismConverter.JsonToCollection(json);

            Assert.IsNotNull(result, "result is null");
            Assert.AreEqual(0, result.Columns.Count, "result didn't contain 1 column");
            Assert.AreEqual(0, result.Rows.Count, "result didn't contain 1 row");
        }

        [TestMethod()]
        public void LocalDateTimeSingleValue()
        {
            var now = DateTime.Now;
            Assert.AreEqual(DateTimeKind.Local, now.Kind);
            var json = JsonConvert.SerializeObject(now);

            var result = BluePrismConverter.JsonToCollection(json);

            Assert.AreEqual(DataSetDateTime.Utc, result.Columns[SpecialValues.JsonUnnamedValue].DateTimeMode);
            Assert.AreEqual(now, result.Rows[0][SpecialValues.JsonUnnamedValue]);
        }
    }
}
