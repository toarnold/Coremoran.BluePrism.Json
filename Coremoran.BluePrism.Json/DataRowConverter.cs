using Newtonsoft.Json;
using System;
using System.Data;
using System.Linq;

namespace Coremoran.BluePrism.Json
{
    internal class DataRowConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType) => objectType == typeof(DataRow);

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType != JsonToken.StartObject)
            {
                throw new Exception("Unsupported JsonToken " + reader.TokenType.ToString());
            }

            var dt = new DataTable();
            dt.SetSingleRow(true);
            dt.Rows.Add();
            string currentColumnName = null;
            while (reader.Read() && reader.TokenType != JsonToken.EndObject)
            {
                switch (reader.TokenType)
                {
                    case JsonToken.PropertyName:
                        currentColumnName = reader.Value.ToString();
                        break;
                    default:
                        DataTableRowHandler.HandleJsonData(dt, currentColumnName, serializer, reader);
                        break;
                }
            }
            return dt.Rows[0];
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var dataRow = (DataRow)value;
            var dataTable = dataRow.Table;
            if (dataTable.Columns.Count == 1
                && dataTable.Columns[0].ColumnName == SpecialValues.JsonUnnamedValue 
                && (!dataTable.IsSingleRow() || writer.Path == string.Empty))
            {
                // First (and single!) element
                serializer.Serialize(writer, dataRow[0]); 
            }
            else
            {
                serializer.Serialize(writer, dataTable.Columns.Cast<DataColumn>().ToDictionary(k => k.ColumnName, v => dataRow[v]));
            }
        }
    }
}
