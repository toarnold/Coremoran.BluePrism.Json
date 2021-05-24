using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Data;

namespace Coremoran.BluePrism.Json
{
    public static class BluePrismConverter
    {
        public static DataTable ObjectToCollection(object o)
        {
            return JsonToCollection(JsonConvert.SerializeObject(o,
                    new DataTableConverter(),
                    new GroupingConverter(),
                    new DataRowConverter()
            ));
        }

        public static DataTable JsonToCollection(string json)
        {
            return (DataTable)JsonConvert.DeserializeObject(json, typeof(DataTable), new JsonSerializerSettings
            {
                Converters = new JsonConverter[]
                {
                    new DataTableConverter(),
                    new DataRowConverter()
                },
                DateTimeZoneHandling = DateTimeZoneHandling.Unspecified
            });
        }

        public static string CollectionToJson(DataTable table)
        {
            return JsonConvert.SerializeObject(table,
                    new DataTableConverter(),
                    new DataRowConverter());
        }

        public static JArray CollectionToJArray(DataTable table)
        {
            var serializer = JsonSerializer.Create(new JsonSerializerSettings()
            {
                Converters = new JsonConverter[] {
                    new DataTableConverter(),
                    new DataRowConverter() }
            });
            return JArray.FromObject(table, serializer);
        }
    }
}
