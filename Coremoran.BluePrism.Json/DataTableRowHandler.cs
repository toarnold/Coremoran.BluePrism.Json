using Newtonsoft.Json;
using System;
using System.Data;

namespace Coremoran.BluePrism.Json
{
    internal static class DataTableRowHandler
    {
        private static void AppendArrayItem(DataTable table, Type colType, string colName, object value)
        {
            if (!table.Columns.Contains(colName))
            {
                var newcolumn = new DataColumn(colName, colType); 
                if (colType == typeof(DateTime))
                {
                    newcolumn.DateTimeMode = DataSetDateTime.Utc;
                }
                table.Columns.Add(newcolumn);
            }
            if (table.IsSingleRow())
            {
                table.Rows[0][colName] = value;
            }
            else
            {
                var newRow = table.NewRow();
                newRow[colName] = value;
                table.Rows.Add(newRow);
            }
        }

        internal static void HandleJsonData(DataTable table, string currentColumnName, JsonSerializer serializer, JsonReader reader)
        {
            switch (reader.TokenType)
            {
                case JsonToken.String:
                case JsonToken.Null:
                    AppendArrayItem(table, typeof(string), currentColumnName ?? SpecialValues.JsonUnnamedValue, reader.Value);
                    break;
                case JsonToken.Integer:
                case JsonToken.Float:
                    AppendArrayItem(table, typeof(decimal), currentColumnName ?? SpecialValues.JsonUnnamedValue, reader.Value);
                    break;
                case JsonToken.Boolean:
                    AppendArrayItem(table, typeof(bool), currentColumnName ?? SpecialValues.JsonUnnamedValue, reader.Value);
                    break;
                case JsonToken.Date:
                    AppendArrayItem(table, typeof(DateTime), currentColumnName ?? SpecialValues.JsonUnnamedValue, reader.Value);
                    break;
                case JsonToken.StartObject:
                    AppendArrayItem(table, typeof(DataTable), currentColumnName ?? SpecialValues.JsonUnnamedValue, serializer.Deserialize<DataTable>(reader));
                    break;
                case JsonToken.StartArray:
                    AppendArrayItem(table, typeof(DataTable), currentColumnName ?? SpecialValues.JsonUnnamedValue, serializer.Deserialize<DataTable>(reader));
                    break;
                default:
                    throw new Exception("Unsupported JsonToken " + reader.TokenType.ToString());
            }
        }
    }
}
