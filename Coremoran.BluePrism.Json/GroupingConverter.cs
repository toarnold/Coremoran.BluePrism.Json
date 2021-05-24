using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Coremoran.BluePrism.Json
{
    internal class GroupingConverter : JsonConverter
    {
        public override bool CanRead => false;

        public override bool CanConvert(Type objectType) => objectType.GetInterfaces().Any(a => a.IsGenericType && a.GetGenericTypeDefinition() == typeof(IGrouping<,>));

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) => throw new NotImplementedException();

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            // Get key object with reflection
            var keyInfo = value.GetType().GetProperty("Key");
            var key = JToken.FromObject(keyInfo.GetValue(value));

            var data = key is JObject obj ? obj : new JObject { { SpecialValues.GroupingSingleKeyColumnName, key } };
            data.Add(SpecialValues.GroupingGroupColumnName, JArray.FromObject(((IEnumerable<object>)value).Select(x => x), serializer));

            serializer.Serialize(writer, data);
        }
    }
}
