using Coremoran.BluePrism.Json;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Data;

namespace BluePrismTests
{
    [TestClass()]
    public class Table2JsonTests
    {
        [TestMethod()]
        public void EmptyDtMultiRowTest()
        {
            var testData = new DataTable();
            testData.SetSingleRow(false);

            var json = BluePrismConverter.CollectionToJson(testData);

            Assert.AreEqual("[]", json, "not an empty object");
        }

        [TestMethod()]
        public void DtSimpleArrayTest()
        {
            var testData = new DataTable();
            testData.SetSingleRow(false);
            testData.Columns.Add(SpecialValues.JsonUnnamedValue, typeof(decimal));
            testData.Rows.Add(1);

            var json = BluePrismConverter.CollectionToJson(testData);

            Assert.AreEqual("[1.0]", json, "not as expected");
        }

        [TestMethod()]
        public void DtSimple2ArrayTest()
        {
            var testData = new DataTable();
            testData.SetSingleRow(false);
            testData.Columns.Add(SpecialValues.JsonUnnamedValue, typeof(decimal));
            testData.Rows.Add(1);
            testData.Rows.Add(2.45);

            var json = BluePrismConverter.CollectionToJson(testData);

            Assert.AreEqual("[1.0,2.45]", json, "not as expected");
        }

        [TestMethod()]
        public void DtObjectArrayTest()
        {
            var testData = new DataTable();
            testData.SetSingleRow(false);
            testData.Columns.Add("value", typeof(decimal));
            testData.Columns.Add("text", typeof(string));
            testData.Rows.Add(1, "text1");
            testData.Rows.Add(2, "text2");

            var json = BluePrismConverter.CollectionToJson(testData);

            Assert.AreEqual("[{\"value\":1.0,\"text\":\"text1\"},{\"value\":2.0,\"text\":\"text2\"}]", json, "not as expected");
        }

        [TestMethod()]
        public void DtNestedArrayInArrayTest()
        {
            var testData = new DataTable();
            testData.SetSingleRow(false);
            testData.Columns.Add(SpecialValues.JsonUnnamedValue, typeof(DataTable));
            var nestedTable = new DataTable();
            nestedTable.SetSingleRow(false);
            testData.Rows.Add(nestedTable);

            var json = BluePrismConverter.CollectionToJson(testData);

            Assert.AreEqual("[[]]", json, "not as expected");
        }

        [TestMethod()]
        public void DtCombined2Test()
        {
            var testData = new DataTable();
            testData.SetSingleRow(false);
            testData.Columns.Add(SpecialValues.JsonUnnamedValue, typeof(DataTable));
            var nestedTable = new DataTable();
            nestedTable.Columns.Add(SpecialValues.JsonUnnamedValue, typeof(decimal));
            nestedTable.SetSingleRow(false);
            nestedTable.Rows.Add(1);
            nestedTable.Rows.Add(2);
            nestedTable.Rows.Add(3);
            var nestedTable2 = new DataTable();
            nestedTable2.Columns.Add("x", typeof(decimal));
            nestedTable2.SetSingleRow(false);
            nestedTable2.Rows.Add(1);
            nestedTable2.Rows.Add(2);
            nestedTable2.Rows.Add(3.14159265m);
            testData.Rows.Add(nestedTable);
            testData.Rows.Add(nestedTable2);

            var json = BluePrismConverter.CollectionToJson(testData);

            Assert.AreEqual("[[1.0,2.0,3.0],[{\"x\":1.0},{\"x\":2.0},{\"x\":3.14159265}]]", json, "not as expected");
        }

        [TestMethod()]
        public void DateTimeArray()
        {
            var testData = new DataTable();
            testData.SetSingleRow(false);
            var column = testData.Columns.Add(SpecialValues.JsonUnnamedValue, typeof(DateTime));
            column.DateTimeMode = DataSetDateTime.Utc;
            testData.Rows.Add(new DateTime(1970, 12, 26, 8, 15, 0));
            testData.Rows.Add(new DateTime(2019, 10, 2, 9, 38, 24));

            var json = BluePrismConverter.CollectionToJson(testData);

            Assert.AreEqual("[\"1970-12-26T08:15:00Z\",\"2019-10-02T09:38:24Z\"]", json, "not as expected");
        }

    }
}
