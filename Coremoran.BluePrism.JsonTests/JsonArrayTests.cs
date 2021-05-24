using Coremoran.BluePrism.Json;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Data;

namespace BluePrismTests
{
    [TestClass()]
    public class JsonArrayTests
    {
        [TestMethod()]
        public void EmptyArrayTest()
        {
            var json = "[]";

            var result = BluePrismConverter.JsonToCollection(json);

            Assert.IsNotNull(result, "result is null");
            Assert.AreEqual(0, result.Columns.Count, "result contains columns");
            Assert.AreEqual(0, result.Rows.Count, "result didn't have 0 rows");
            Assert.IsFalse(result.IsSingleRow(), "result is a single row");
        }

        [TestMethod()]
        public void SimpleArray1Test()
        {
            var json = "[ 1 ]";

            var result = BluePrismConverter.JsonToCollection(json);

            Assert.IsNotNull(result, "result is null");
            Assert.AreEqual(1, result.Columns.Count, "result didn't contain 1 column");
            Assert.AreEqual(SpecialValues.JsonUnnamedValue, result.Columns[0].ColumnName, "result didn't contains column name " + SpecialValues.JsonUnnamedValue);
            Assert.AreEqual(1, result.Rows.Count, "result didn't have 1 rows");
            Assert.AreEqual(1m, result.Rows[0][SpecialValues.JsonUnnamedValue], "result didn't have value 1 in rows");
            Assert.IsFalse(result.IsSingleRow(), "result is a single row");
        }

        [TestMethod()]
        public void SimpleArray2Test()
        {
            var json = "[ 1,2 ]";

            var result = BluePrismConverter.JsonToCollection(json);

            Assert.IsNotNull(result, "result is null");
            Assert.AreEqual(1, result.Columns.Count, "result didn't contain 1 column");
            Assert.AreEqual(SpecialValues.JsonUnnamedValue, result.Columns[0].ColumnName, "result didn't contains column name " + SpecialValues.JsonUnnamedValue);
            Assert.AreEqual(2, result.Rows.Count, "result didn't have 2 rows");
            Assert.AreEqual(1m, result.Rows[0][SpecialValues.JsonUnnamedValue], "result didn't have value 1 in row 1");
            Assert.AreEqual(2m, result.Rows[1][SpecialValues.JsonUnnamedValue], "result didn't have value 1 in row 2");
            Assert.IsFalse(result.IsSingleRow(), "result is a single row");
        }


        [TestMethod()]
        public void SimpleArrayNullTest()
        {
            var json = "[ null ]";

            var result = BluePrismConverter.JsonToCollection(json);

            Assert.IsNotNull(result, "result is null");
            Assert.AreEqual(0, result.Columns.Count, "result didn't contain 1 column");
            Assert.AreEqual(1, result.Rows.Count, "result didn't have 1 rows");
            Assert.IsFalse(result.IsSingleRow(), "result is a single row");
        }

        [TestMethod()]
        public void ArrayWith1NestedObjectTest()
        {
            var json = "[ { \"value\": 1 } ]";

            var result = BluePrismConverter.JsonToCollection(json);

            Assert.IsNotNull(result, "result is null");
            Assert.AreEqual(1, result.Columns.Count, "result didn't contain 1 column");
            Assert.AreEqual("value", result.Columns[0].ColumnName, "result didn't contains column name 'value'");
            Assert.AreEqual(1, result.Rows.Count, "result didn't have 1 rows");
            Assert.AreEqual(1m, result.Rows[0]["value"], "result didn't have value 1 in row 1");
            Assert.IsFalse(result.IsSingleRow(), "result is a single row");
        }

        [TestMethod()]
        public void ArrayWith2NestedObjectTest()
        {
            var json = "[ { \"value\": 1 },{ \"value\": 2 } ]";

            var result = BluePrismConverter.JsonToCollection(json);

            Assert.IsNotNull(result, "result is null");
            Assert.AreEqual(1, result.Columns.Count, "result didn't contain 1 column");
            Assert.AreEqual("value", result.Columns[0].ColumnName, "result didn't contains column name 'value'");
            Assert.AreEqual(2, result.Rows.Count, "result didn't have 2 rows");
            Assert.AreEqual(1m, result.Rows[0]["value"], "result didn't have value 1 in row 1");
            Assert.AreEqual(2m, result.Rows[1]["value"], "result didn't have value 2 in row 2");
            Assert.IsFalse(result.IsSingleRow(), "result is a single row");
        }

        [TestMethod()]
        public void ArrayWith2NestedObjectsWithUndefinedValuesTest()
        {
            var json = "[ { \"value1\": 1, \"value2\": true },{ \"value1\": 3, \"value3\": \"foo\" } ]";

            var result = BluePrismConverter.JsonToCollection(json);

            Assert.IsNotNull(result, "result is null");
            Assert.AreEqual(3, result.Columns.Count, "result didn't contain 3 columns");
            Assert.AreEqual("value1", result.Columns[0].ColumnName, "result didn't contain column name 'value1'");
            Assert.AreEqual("value2", result.Columns[1].ColumnName, "result didn't contain column name 'value2'");
            Assert.AreEqual("value3", result.Columns[2].ColumnName, "result didn't contain column name 'value3'");
            Assert.AreEqual(typeof(decimal), result.Columns["value1"].DataType);
            Assert.AreEqual(typeof(bool), result.Columns["value2"].DataType);
            Assert.AreEqual(typeof(string), result.Columns["value3"].DataType);
            Assert.AreEqual(2, result.Rows.Count, "result didn't have 2 rows");
            Assert.AreEqual(1m, result.Rows[0]["value1"], "result didn't have value 1 in row 1");
            Assert.AreEqual(true, result.Rows[0]["value2"], "result didn't have value true in row 1");
            Assert.IsTrue(result.Rows[0].IsNull("value3"), "result didn't have null value in row 1");
            Assert.AreEqual(3m, result.Rows[1]["value1"], "result didn't have value 3 in row 2");
            Assert.IsTrue(result.Rows[1].IsNull("value2"), "result didn't have null value in row 1");
            Assert.AreEqual("foo", result.Rows[1]["value3"], "result didn't have value \"foo\" in row 2");
            Assert.IsFalse(result.IsSingleRow(), "result is a single row");
        }

        [TestMethod()]
        public void ArrayWithNestedArray1Test()
        {
            var json = "[ [] ]";

            var result = BluePrismConverter.JsonToCollection(json);

            Assert.IsNotNull(result, "result is null");
            Assert.AreEqual(1, result.Columns.Count, "result didn't contain 1 column");
            Assert.AreEqual(SpecialValues.JsonUnnamedValue, result.Columns[0].ColumnName, "result didn't contains column name " + SpecialValues.JsonUnnamedValue);
            Assert.AreEqual(1, result.Rows.Count, "result didn't have 1 rows");
            Assert.IsFalse(result.IsSingleRow(), "result is a single row");

            var secondTable = result.Rows[0][SpecialValues.JsonUnnamedValue] as DataTable;

            Assert.IsNotNull(secondTable, "row 0 col 0 isn't a DataTable");
            Assert.AreEqual(0, secondTable.Columns.Count, "secondTable contains columns");
            Assert.AreEqual(0, secondTable.Rows.Count, "secondTable didn't have 0 rows");
            Assert.IsFalse(secondTable.IsSingleRow(), "secondTable is a single row");
        }

        [TestMethod()]
        public void ArrayWithNestedArray2Test()
        {
            var json = "[ [ { \"value\": 1 },{ \"value\": 2 } ] ]";

            var result = BluePrismConverter.JsonToCollection(json);

            Assert.IsNotNull(result, "result is null");
            Assert.AreEqual(1, result.Columns.Count, "result didn't contain 1 column");
            Assert.AreEqual(SpecialValues.JsonUnnamedValue, result.Columns[0].ColumnName, "result didn't contains column name " + SpecialValues.JsonUnnamedValue);
            Assert.AreEqual(1, result.Rows.Count, "result didn't have 1 rows");
            Assert.IsFalse(result.IsSingleRow(), "result is a single row");

            var secondTable = result.Rows[0][SpecialValues.JsonUnnamedValue] as DataTable;

            Assert.IsNotNull(secondTable, "row 0 col 0 isn't a DataTable");
            Assert.AreEqual(1, secondTable.Columns.Count, "secondTable didn't contain 1 column");
            Assert.AreEqual("value", secondTable.Columns[0].ColumnName, "secondTable didn't contains column name 'value'");
            Assert.AreEqual(2, secondTable.Rows.Count, "secondTable didn't have 2 rows");
            Assert.AreEqual(1m, secondTable.Rows[0]["value"], "secondTable didn't have value 1 in row 1");
            Assert.AreEqual(2m, secondTable.Rows[1]["value"], "secondTable didn't have value 2 in row 2");
            Assert.IsFalse(secondTable.IsSingleRow(), "secondTable is a single row");
        }

        [TestMethod()]
        public void DtCombined2Test()
        {
            var json = "[ [1,2,3], [{\"x\":1}, {\"x\":2}, {\"x\":3}] ]";

            var result = BluePrismConverter.JsonToCollection(json);

            Assert.IsNotNull(result, "result is null");
            Assert.IsFalse(result.IsSingleRow(), "result is a single row");
            Assert.AreEqual(1, result.Columns.Count, "result no exactly 2 columns");
            Assert.AreEqual(2, result.Rows.Count, "result no exactly 1 row");

            var subTable1 = result.Rows[0][SpecialValues.JsonUnnamedValue] as DataTable;
            Assert.IsInstanceOfType(subTable1, typeof(DataTable), "subTable1 is not a DataTable");
            Assert.AreEqual(3, subTable1.Rows.Count);
            Assert.AreEqual(1m, subTable1.Rows[0][SpecialValues.JsonUnnamedValue]);
            Assert.AreEqual(2m, subTable1.Rows[1][SpecialValues.JsonUnnamedValue]);
            Assert.AreEqual(3m, subTable1.Rows[2][SpecialValues.JsonUnnamedValue]);

            var subTable2 = result.Rows[1][SpecialValues.JsonUnnamedValue] as DataTable;
            Assert.IsInstanceOfType(subTable2, typeof(DataTable), "subTable2 is not a DataTable");
            Assert.AreEqual(3, subTable2.Rows.Count);
            Assert.AreEqual(1m, subTable2.Rows[0]["x"]);
            Assert.AreEqual(2m, subTable2.Rows[1]["x"]);
            Assert.AreEqual(3m, subTable2.Rows[2]["x"]);
        }

        [TestMethod()]
        public void DateTimeArray()
        {
            var json = "[\"1970-12-26T08:15:00Z\",\"2019-10-02T09:38:24Z\"]";

            var result = BluePrismConverter.JsonToCollection(json);

            Assert.AreEqual(DataSetDateTime.Utc, result.Columns[SpecialValues.JsonUnnamedValue].DateTimeMode);
            Assert.AreEqual(new DateTime(1970, 12, 26, 8, 15, 0), result.Rows[0][SpecialValues.JsonUnnamedValue]);
            Assert.AreEqual(new DateTime(2019, 10, 2, 9, 38, 24), result.Rows[1][SpecialValues.JsonUnnamedValue]);
        }

        [TestMethod()]
        public void LocalDateTimeWithNullArray()
        {
            var now = DateTime.Now;
            Assert.AreEqual(DateTimeKind.Local, now.Kind);
            var json = $"[{JsonConvert.SerializeObject(now)}, null]";

            var result = BluePrismConverter.JsonToCollection(json);

            Assert.AreEqual(DataSetDateTime.Utc, result.Columns[SpecialValues.JsonUnnamedValue].DateTimeMode);
            Assert.AreEqual(2, result.Rows.Count);
            Assert.AreEqual(now, result.Rows[0][SpecialValues.JsonUnnamedValue]);
            Assert.AreEqual(DBNull.Value, result.Rows[1][SpecialValues.JsonUnnamedValue]);
        }

        [TestMethod()]
        public void PyramidTest()
        {
            var json = "[{\"value\":\"1970-12-26T08:15:00Z\"},{\"value\":\"1970-12-26T08:15:00Z\",\"numvalue\":4711}]";

            var result = BluePrismConverter.JsonToCollection(json);

            Assert.AreEqual(2, result.Columns.Count);
            Assert.AreEqual(2, result.Rows.Count);
            Assert.AreEqual(DBNull.Value, result.Rows[0]["numvalue"]);
        }

        [TestMethod()]
        public void PyramidReverseTest()
        {
            var json = "[{\"value\":\"1970-12-26T08:15:00Z\"},{\"numvalue\":4711,\"value\":\"1970-12-26T08:15:00Z\"}]";

            var result = BluePrismConverter.JsonToCollection(json);

            Assert.AreEqual(2, result.Columns.Count);
            Assert.AreEqual(2, result.Rows.Count);
            Assert.AreEqual(DBNull.Value, result.Rows[0]["numvalue"]);
        }

        [TestMethod()]
        public void PyramidWithNull1Test()
        {
            var json = "[null,{\"value\":\"1970-12-26T08:15:00Z\",\"numvalue\":4711}]";

            var result = BluePrismConverter.JsonToCollection(json);

            Assert.AreEqual(2, result.Columns.Count);
            Assert.AreEqual(2, result.Rows.Count);
            Assert.AreEqual(DBNull.Value, result.Rows[0]["value"]);
            Assert.AreEqual(DBNull.Value, result.Rows[0]["numvalue"]);
        }

        [TestMethod()]
        public void PyramidWithNull2Test()
        {
            var json = "[{\"value\":\"1970-12-26T08:15:00Z\",\"numvalue\":4711},null]";

            var result = BluePrismConverter.JsonToCollection(json);

            Assert.AreEqual(2, result.Columns.Count);
            Assert.AreEqual(2, result.Rows.Count);
            Assert.AreEqual(DBNull.Value, result.Rows[1]["value"]);
            Assert.AreEqual(DBNull.Value, result.Rows[1]["numvalue"]);
        }

        [TestMethod()]
        public void ExtensiveDateTimeTest()
        {
            var json = "[{\"Datum\":\"2020-10-01T08:12:34Z\"},{\"Datum\":\"0001-01-01T00:00:00Z\"},{\"Datum\":\"2020-10-12T10:12:34Z\"}]";

            var result = BluePrismConverter.JsonToCollection(json);

            Assert.AreEqual(1, result.Columns.Count);
            Assert.AreEqual(3, result.Rows.Count);
            Assert.AreEqual(DateTime.Parse("2020-10-01T08:12:34Z", null, System.Globalization.DateTimeStyles.RoundtripKind), result.Rows[0]["Datum"]);
            Assert.AreEqual(DateTimeKind.Utc, ((DateTime)result.Rows[0]["Datum"]).Kind);
            Assert.AreEqual(DateTime.Parse("0001-01-01T00:00:00Z", null, System.Globalization.DateTimeStyles.RoundtripKind), result.Rows[1]["Datum"]);
            Assert.AreEqual(DateTimeKind.Utc, ((DateTime)result.Rows[1]["Datum"]).Kind);
            Assert.AreEqual(DateTime.Parse("2020-10-12T10:12:34Z", null, System.Globalization.DateTimeStyles.RoundtripKind), result.Rows[2]["Datum"]);
            Assert.AreEqual(DateTimeKind.Utc, ((DateTime)result.Rows[2]["Datum"]).Kind);
        }

        [TestMethod()]
        public void LateTypingTest()
        {
            var json = "[{\"Test\":\"Zeile 1\",\"Nr\": null, \"Datum\":null, \"NullColumn\":null },{\"Test\":\"Zeile 2\",\"Nr\": 1.0, \"Datum\":null, \"NullColumn\":null },{\"Test\":\"Zeile 3\",\"Nr\": 2.0,\"Datum\":\"2020-10-01T08:12:34Z\", \"NullColumn\":null }]";

            var result = BluePrismConverter.JsonToCollection(json);

            Assert.AreEqual(4, result.Columns.Count);
            Assert.AreEqual(3, result.Rows.Count);
            Assert.AreEqual(typeof(decimal), result.Columns["Nr"].DataType);
            Assert.AreEqual(typeof(DateTime), result.Columns["Datum"].DataType);
            Assert.AreEqual(typeof(string), result.Columns["Test"].DataType);
            Assert.AreEqual(typeof(string), result.Columns["NullColumn"].DataType);
        }

        [TestMethod()]
        public void LateTypingLastRowWithNullTest()
        {
            var json = "[{\"Test\":\"Zeile 1\",\"Nr\": null, \"Datum\":null, \"NullColumn\":null },{\"Test\":\"Zeile 2\",\"Nr\": 1.0, \"Datum\":null, \"NullColumn\":null },{\"Test\":\"Zeile 3\",\"Nr\": null,\"Datum\":\"2020-10-01T08:12:34Z\", \"NullColumn\":null }]";

            var result = BluePrismConverter.JsonToCollection(json);

            Assert.AreEqual(4, result.Columns.Count);
            Assert.AreEqual(3, result.Rows.Count);
            Assert.AreEqual(typeof(decimal), result.Columns["Nr"].DataType);
            Assert.AreEqual(typeof(DateTime), result.Columns["Datum"].DataType);
            Assert.AreEqual(typeof(string), result.Columns["Test"].DataType);
            Assert.AreEqual(typeof(string), result.Columns["NullColumn"].DataType);
        }

        //[TestMethod()]
        //public void MixedArrayTest()
        //{
        //    var json = "[1,\"1\",true,{\"x\":\"Hello\"}]";

        //    var result = BluePrismConverter.JsonToCollection(json);

        //    Assert.AreEqual(4, result.Columns.Count);
        //    Assert.AreEqual(4, result.Rows.Count);
        //    Assert.IsTrue(result.Columns.OfType<DataColumn>().All(c => c.ColumnName.StartsWith(SpecialValues.JsonColumnPrefix)));
        //    Assert.AreEqual(1m, result.Rows[0][SpecialValues.JsonColumnPrefix]);
        //    Assert.AreEqual("1", result.Rows[1][SpecialValues.JsonColumnPrefix]);
        //    Assert.AreEqual(true, result.Rows[2][SpecialValues.JsonColumnPrefix]);
        //    Assert.AreEqual(typeof(DataTable), result.Rows[3][SpecialValues.JsonColumnPrefix]);
        //}
    }
}
