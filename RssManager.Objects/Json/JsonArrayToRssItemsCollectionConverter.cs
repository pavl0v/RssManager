using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RssManager.Interfaces.DTO;
using RssManager.Objects.BO;
using System;
using System.Collections.Generic;

namespace RssManager.Objects.Json
{
    public class JsonArrayToRssItemsCollectionConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return typeof(RssItem).IsAssignableFrom(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JArray jsonArray = null;

            jsonArray = JArray.Load(reader);
            List<RssItem> tmp = new List<RssItem>();
            serializer.Populate(jsonArray.CreateReader(), tmp);
            List<IRssItemDTO> target = new List<IRssItemDTO>(tmp);

            return target;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value);
        }
    }
}
