using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Coremoran.BluePrism.Json
{
    internal class DataTableConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType) => objectType == typeof(DataTable);

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var dt = (DataTable)value;
            if (dt.IsSingleRow())
            {
                serializer.Serialize(writer, dt.Rows[0]);
            }
            else
            {
                serializer.Serialize(writer, dt.Select());
            }
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var dt = new DataTable();
            switch (reader.TokenType)
            {
                case JsonToken.StartObject:
                    dt = serializer.Deserialize<DataRow>(reader).Table;
                    break;
                case JsonToken.StartArray:
                    dt.SetSingleRow(false);
                    var nullColumns = new HashSet<string>();
                    while (reader.Read() && reader.TokenType != JsonToken.EndArray)
                    {
                        switch (reader.TokenType)
                        {
                            case JsonToken.Null:
                                dt.Rows.Add();
                                break;
                            case JsonToken.StartObject:
                                var tempTable = serializer.Deserialize<DataRow>(reader).Table;
                                var nulls = tempTable.Columns.Cast<DataColumn>().Where(w => tempTable.Rows[0].IsNull(w.ColumnName)).Select(s => s.ColumnName).ToList();
                                nulls.ForEach(n => tempTable.Columns.Remove(n));
                                nullColumns.UnionWith(nulls);
                                dt.Merge(tempTable, true, MissingSchemaAction.Add);
                                break;
                            default:
                                DataTableRowHandler.HandleJsonData(dt, null, serializer, reader);
                                break;
                        }
                    }
                    // Add not set Null-Columns
                    dt.Columns.AddRange(nullColumns.Where(w => !dt.Columns.Contains(w)).Select(s => new DataColumn(s)).ToArray());
                    break;
                default: /* Try to append a single value */
                    if (reader.TokenType != JsonToken.Null)
                    {
                        dt.SetSingleRow(true);
                        dt.Rows.Add();
                        DataTableRowHandler.HandleJsonData(dt, null, serializer, reader);
                    }
                    break;
            }
            return dt;
        }

   }
}