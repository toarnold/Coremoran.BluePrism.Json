using Coremoran.BluePrism.Json;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Data;

namespace BluePrismTests
{
    [TestClass()]
    public class JsonObjectTests
    {
        [TestMethod()]
        public void EmptyObjectTest()
        {
            var json = "{}";

            var result = BluePrismConverter.JsonToCollection(json);

            Assert.IsNotNull(result, "result is null");
            Assert.AreEqual(0, result.Columns.Count, "result contains columns");
            Assert.AreEqual(1, result.Rows.Count, "result didn't have 1 rows");
            Assert.IsTrue(result.IsSingleRow(), "result is not a single row");
        }

        [TestMethod()]
        public void SimpleObjectDecimalTest()
        {
            var json = "{ \"value\": 1 }";

            var result = BluePrismConverter.JsonToCollection(json);

            Assert.IsNotNull(result, "result is null");
            Assert.AreEqual(1, result.Columns.Count, "result no exactly one column");
            Assert.AreEqual("value", result.Columns[0].ColumnName, "result has not a 'value' column");
            Assert.AreEqual(1, result.Rows.Count, "result didn't contain exactly one row");
            Assert.AreEqual(1m, result.Rows[0]["value"], "the row value isn't 1.");
            Assert.IsTrue(result.IsSingleRow(), "result is not a single row");
        }

        [TestMethod()]
        public void SimpleObjectDecimalFractionTest()
        {
            var json = "{ \"value\": 3.14159265 }";

            var result = BluePrismConverter.JsonToCollection(json);

            Assert.IsNotNull(result, "result is null");
            Assert.AreEqual(1, result.Columns.Count, "result no exactly one column");
            Assert.AreEqual("value", result.Columns[0].ColumnName, "result has not a 'value' column");
            Assert.AreEqual(1, result.Rows.Count, "result didn't contain exactly one row");
            Assert.AreEqual(3.14159265m, result.Rows[0]["value"], "the row value isn't 3.14159265");
            Assert.IsTrue(result.IsSingleRow(), "result is not a single row");
        }

        [TestMethod()]
        public void SimpleObjectStringTest()
        {
            var json = "{ \"value\": \"test\" }";

            var result = BluePrismConverter.JsonToCollection(json);

            Assert.IsNotNull(result, "result is null");
            Assert.AreEqual(1, result.Columns.Count, "result no exactly one column");
            Assert.AreEqual("value", result.Columns[0].ColumnName, "result has not a 'value' column");
            Assert.AreEqual(1, result.Rows.Count, "result didn't contain exactly one row");
            Assert.AreEqual("test", result.Rows[0]["value"], "the row value isn't 'test'.");
            Assert.IsTrue(result.IsSingleRow(), "result is not a single row");
        }

        [TestMethod()]
        public void SimpleObjectNullTest()
        {
            var json = "{ \"value\": null }";

            var result = BluePrismConverter.JsonToCollection(json);

            Assert.IsNotNull(result, "result is null");
            Assert.AreEqual(1, result.Columns.Count, "result no exactly one column");
            Assert.AreEqual("value", result.Columns[0].ColumnName, "result has not a 'value' column");
            Assert.AreEqual(1, result.Rows.Count, 1, "result didn't contain exactly one row");
            Assert.AreEqual(DBNull.Value, result.Rows[0]["value"], "the row value isn't null.");
            Assert.IsTrue(result.IsSingleRow(), "result is not a single row");
        }

        [TestMethod()]
        public void SimpleObject2ValuesTest()
        {
            var json = "{ \"value1\": 4711, \"value2\": \"val\" }";

            var result = BluePrismConverter.JsonToCollection(json);

            Assert.IsNotNull(result, "result is null");
            Assert.AreEqual(2, result.Columns.Count, "result no exactly two columns");
            Assert.IsNotNull(result.Columns["value1"], "result has not a 'value1' column");
            Assert.IsNotNull(result.Columns["value2"], "result has not a 'value2' column");
            Assert.AreEqual(1, result.Rows.Count, 1, "result didn't contain exactly one row");
            Assert.AreEqual(4711m, result.Rows[0]["value1"], "the row value1 isn't 4711.");
            Assert.AreEqual("val", result.Rows[0]["value2"], "the row value2 isn't 'val'.");
            Assert.IsTrue(result.IsSingleRow(), "result is not a single row");
        }

        [TestMethod()]
        public void ObjectWithNestedArrayEmptyTest()
        {
            var json = "{ \"theArray\": [] }";

            var result = BluePrismConverter.JsonToCollection(json);

            Assert.IsNotNull(result, "result is null");
            Assert.AreEqual(1, result.Columns.Count, "result didn't contain 1 column");
            Assert.AreEqual("theArray", result.Columns[0].ColumnName, "result didn't contains column name 'theArray'");
            Assert.AreEqual(1, result.Rows.Count, "result didn't have 1 rows");
            Assert.IsTrue(result.IsSingleRow(), "result is not a single row");

            var secondTable = result.Rows[0]["theArray"] as DataTable;

            Assert.IsNotNull(secondTable, "row 0 col 0 isn't a DataTable");
            Assert.AreEqual(0, secondTable.Columns.Count, "secondTable contains columns");
            Assert.AreEqual(0, secondTable.Rows.Count, "secondTable didn't have 0 rows");
            Assert.IsFalse(secondTable.IsSingleRow(), "secondTable is a single row");
        }

        [TestMethod()]
        public void ObjectWithNestedArraySimpleTest()
        {
            var json = "{ \"theArray\": [ 1,2 ] }";

            var result = BluePrismConverter.JsonToCollection(json);

            Assert.IsNotNull(result, "result is null");
            Assert.AreEqual(1, result.Columns.Count, "result didn't contain 1 column");
            Assert.AreEqual("theArray", result.Columns[0].ColumnName, "result didn't contains column name 'theArray'");
            Assert.AreEqual(1, result.Rows.Count, "result didn't have 1 rows");
            Assert.IsTrue(result.IsSingleRow(), "result is not a single row");

            var secondTable = result.Rows[0]["theArray"] as DataTable;

            Assert.IsNotNull(secondTable, "row 0 col 0 isn't a DataTable");
            Assert.AreEqual(1, secondTable.Columns.Count, "secondTable contains columns");
            Assert.AreEqual(SpecialValues.JsonUnnamedValue, secondTable.Columns[0].ColumnName, "secondTable didn't contains column name " + SpecialValues.JsonUnnamedValue);
            Assert.AreEqual(2, secondTable.Rows.Count, "secondTable didn't have 2 row");
            Assert.AreEqual(1m, secondTable.Rows[0][SpecialValues.JsonUnnamedValue], "secondTable didn't have value 1 in row 1");
            Assert.AreEqual(2m, secondTable.Rows[1][SpecialValues.JsonUnnamedValue], "secondTable didn't have value 2 in row 2");
            Assert.IsFalse(secondTable.IsSingleRow(), "secondTable is a single row");
        }

        [TestMethod()]
        public void ObjectWithNestedArrayObjectTest()
        {
            var json = "{ \"theArray\": [ { \"value\": 1 },{ \"value\": 2 } ] }";

            var result = BluePrismConverter.JsonToCollection(json);

            Assert.IsNotNull(result, "result is null");
            Assert.AreEqual(1, result.Columns.Count, "result didn't contain 1 column");
            Assert.AreEqual("theArray", result.Columns[0].ColumnName, "result didn't contains column name 'theArray'");
            Assert.AreEqual(1, result.Rows.Count, "result didn't have 1 rows");
            Assert.IsTrue(result.IsSingleRow(), "result is not a single row");

            var secondTable = result.Rows[0]["theArray"] as DataTable;

            Assert.IsNotNull(secondTable, "row 0 col 0 isn't a DataTable");
            Assert.AreEqual(1, secondTable.Columns.Count, "secondTable contains columns");
            Assert.AreEqual("value", secondTable.Columns[0].ColumnName, "secondTable didn't contains column name 'value'");
            Assert.AreEqual(2, secondTable.Rows.Count, "secondTable didn't have 2 row");
            Assert.AreEqual(1m, secondTable.Rows[0]["value"], "secondTable didn't have value 1 in row 1");
            Assert.AreEqual(2m, secondTable.Rows[1]["value"], "secondTable didn't have value 2 in row 2");
            Assert.IsFalse(secondTable.IsSingleRow(), "secondTable is a single row");
        }

        [TestMethod()]
        public void SimpleObjectNestedTest()
        {
            var json = "{ \"theObject\": { \"value\": 4711 }  }";

            var result = BluePrismConverter.JsonToCollection(json);

            Assert.IsNotNull(result, "result is null");
            Assert.AreEqual(1, result.Columns.Count, "result no exactly 1 column");
            Assert.AreEqual(1, result.Rows.Count, "result no exactly 1 row");
            Assert.IsTrue(result.IsSingleRow(), "result is not a single row");

            var secondTable = result.Rows[0]["theObject"] as DataTable;

            Assert.IsNotNull(secondTable, "row 0 col 0 isn't a DataTable");
            Assert.AreEqual(1, secondTable.Columns.Count, "secondTable no exactly one column");
            Assert.AreEqual("value", secondTable.Columns[0].ColumnName, "secondTable has not a 'value' column");
            Assert.AreEqual(1, secondTable.Rows.Count, "secondTable didn't contain exactly one row");
            Assert.AreEqual(4711m, secondTable.Rows[0]["value"], "the row value isn't 4711.");
            Assert.IsTrue(secondTable.IsSingleRow(), "secondTable is not a single row");
        }

        [TestMethod()]
        public void DateTimeValue()
        {
            var json = "{\"value\":\"1970-12-26T08:15:00Z\"}";

            var result = BluePrismConverter.JsonToCollection(json);

            Assert.AreEqual(DataSetDateTime.Utc, result.Columns["value"].DateTimeMode);
            Assert.AreEqual(new DateTime(1970, 12, 26, 8, 15, 0), result.Rows[0]["value"]);
        }


        [TestMethod()]
        public void LocalDateTimeObject()
        {
            var now = DateTime.Now;
            Assert.AreEqual(DateTimeKind.Local, now.Kind);
            var json = JsonConvert.SerializeObject(new { Datum = now });

            var result = BluePrismConverter.JsonToCollection(json);

            Assert.AreEqual(DataSetDateTime.Utc, result.Columns["Datum"].DateTimeMode);
            Assert.AreEqual(now, result.Rows[0]["Datum"]);
        }

    }
}
