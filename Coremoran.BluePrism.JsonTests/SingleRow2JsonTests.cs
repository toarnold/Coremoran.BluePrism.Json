using Coremoran.BluePrism.Json;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Data;

namespace BluePrismTests
{
    [TestClass()]
    public class SingleRow2JsonTests
    {
        [TestMethod()]
        public void EmptyDtSingleRowTest()
        {
            var testData = new DataTable();
            testData.SetSingleRow(true);
            testData.Rows.Add(testData.NewRow());

            var json = BluePrismConverter.CollectionToJson(testData);

            Assert.AreEqual("{}", json, "not an empty object");
        }

        [TestMethod()]
        public void DtObjectNotSingleRowTest()
        {
            var testData = new DataTable();
            testData.Columns.Add("value", typeof(decimal));
            testData.Rows.Add(1m);

            var json = BluePrismConverter.CollectionToJson(testData);

            Assert.AreEqual("[{\"value\":1.0}]", json, "not as expected");
        }


        [TestMethod()]
        public void DtUnnamedSingleRowValueTest()
        {
            var testData = new DataTable();
            testData.SetSingleRow(true);
            testData.Columns.Add(SpecialValues.JsonUnnamedValue, typeof(decimal));
            testData.Rows.Add(1.1m);

            var json = BluePrismConverter.CollectionToJson(testData);

            Assert.AreEqual("1.1", json, "not as expected");
        }

        [TestMethod()]
        public void DtUnnamedNestedTest()
        {
            var parentTable = new DataTable();
            parentTable.Columns.Add("Child", typeof(DataTable));
            var testData = new DataTable();
            testData.SetSingleRow(false);
            testData.Columns.Add(SpecialValues.JsonUnnamedValue, typeof(decimal));
            testData.Rows.Add(1.1m);
            parentTable.Rows.Add(testData);

            var json = BluePrismConverter.CollectionToJson(parentTable);

            Assert.AreEqual("[{\"Child\":[1.1]}]", json, "not as expected");
        }

        [TestMethod()]
        public void DtNestedSingleRowTest()
        {
            var parentTable = new DataTable();
            parentTable.Columns.Add("Child", typeof(DataTable));
            var testData = new DataTable();
            testData.SetSingleRow(true);
            testData.Columns.Add(SpecialValues.JsonUnnamedValue, typeof(decimal));
            testData.Rows.Add(1.1m);
            parentTable.Rows.Add(testData);

            var json = BluePrismConverter.CollectionToJson(parentTable);

            Assert.AreEqual("[{\"Child\":{\"" + SpecialValues.JsonUnnamedValue + "\":1.1}}]", json, "not as expected");
        }

        [TestMethod()]
        public void DtSingleRowNestedSingleRowTest()
        {
            var parentTable = new DataTable();
            parentTable.Columns.Add("Child", typeof(DataTable));
            parentTable.SetSingleRow(true);
            var testData = new DataTable();
            testData.SetSingleRow(true);
            testData.Columns.Add(SpecialValues.JsonUnnamedValue, typeof(decimal));
            testData.Rows.Add(1.1m);
            parentTable.Rows.Add(testData);

            var json = BluePrismConverter.CollectionToJson(parentTable);

            Assert.AreEqual("{\"Child\":{\"" + SpecialValues.JsonUnnamedValue + "\":1.1}}", json, "not as expected");
        }

        [TestMethod()]
        public void DtSingleRowNestedTest()
        {
            var parentTable = new DataTable();
            parentTable.Columns.Add("Child", typeof(DataTable));
            parentTable.SetSingleRow(true);
            var testData = new DataTable();
            testData.Columns.Add(SpecialValues.JsonUnnamedValue, typeof(decimal));
            testData.Rows.Add(1.1m);
            parentTable.Rows.Add(testData);

            var json = BluePrismConverter.CollectionToJson(parentTable);

            Assert.AreEqual("{\"Child\":[1.1]}", json, "not as expected");
        }

        [TestMethod()]
        public void DtNestedObjectRowTest()
        {
            var testData = new DataTable();
            testData.SetSingleRow(true);
            testData.Columns.Add("value", typeof(decimal));
            testData.Columns.Add("ref", typeof(DataTable));
            var nestedTable = new DataTable();
            nestedTable.SetSingleRow(true);
            nestedTable.Columns.Add("nestedValue", typeof(string));
            nestedTable.Rows.Add("testString");
            testData.Rows.Add(4711, nestedTable);

            var json = BluePrismConverter.CollectionToJson(testData);

            Assert.AreEqual("{\"value\":4711.0,\"ref\":{\"nestedValue\":\"testString\"}}", json, "not as expected");
        }

        [TestMethod()]
        public void DtCombinedTest()
        {
            var testData = new DataTable();
            testData.SetSingleRow(true);
            testData.Columns.Add("value", typeof(decimal));
            testData.Columns.Add("list", typeof(DataTable));
            var nestedTable = new DataTable();
            nestedTable.Columns.Add(SpecialValues.JsonUnnamedValue, typeof(decimal));
            nestedTable.SetSingleRow(false);
            nestedTable.Rows.Add(1);
            nestedTable.Rows.Add(2.234567m);
            nestedTable.Rows.Add(3);
            testData.Rows.Add(4711, nestedTable);

            var json = BluePrismConverter.CollectionToJson(testData);

            Assert.AreEqual("{\"value\":4711.0,\"list\":[1.0,2.234567,3.0]}", json, "not as expected");
        }

        [TestMethod()]
        public void DtCombined2Test()
        {
            var testData = new DataTable();
            testData.SetSingleRow(false);
            testData.Columns.Add("value", typeof(decimal));
            testData.Columns.Add("list", typeof(DataTable));
            var nestedTable = new DataTable();
            nestedTable.Columns.Add("text");
            nestedTable.SetSingleRow(true);
            nestedTable.Rows.Add("Hello");
            testData.Rows.Add(4711, nestedTable);

            var json = BluePrismConverter.CollectionToJson(testData);

            Assert.AreEqual("[{\"value\":4711.0,\"list\":{\"text\":\"Hello\"}}]", json, "not as expected");
        }

        [TestMethod()]
        public void DateTimeValue()
        {
            var testData = new DataTable();
            testData.SetSingleRow(true);
            var column = testData.Columns.Add("value", typeof(DateTime));
            column.DateTimeMode = DataSetDateTime.Utc;
            testData.Rows.Add(new DateTime(1970, 12, 26, 8, 15, 0));

            var json = BluePrismConverter.CollectionToJson(testData);

            Assert.AreEqual("{\"value\":\"1970-12-26T08:15:00Z\"}", json, "not as expected");
        }
    }
}
