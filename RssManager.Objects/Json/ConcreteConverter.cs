using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RssManager.Interfaces.DTO;
using RssManager.Objects.BO;
using System;

namespace RssManager.Objects.Json
{
    public abstract class JsonCreationConverter<T> : JsonConverter
    {
        protected abstract T Create(Type objectType, JObject jsonObject);

        public override bool CanConvert(Type objectType)
        {
            return typeof(T).IsAssignableFrom(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject jsonObject = null;
            jsonObject = JObject.Load(reader);
            var target = Create(objectType, jsonObject);
            serializer.Populate(jsonObject.CreateReader(), target);
            return target;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value);
        }
    }

    public class JsonSpecsConverter : JsonCreationConverter<RssItem>
    {
        protected override RssItem Create(Type objectType, JObject jsonObject)
        {
            return new RssItem();
        }
    }

    public class JsonToTokenConverter : JsonCreationConverter<TokenDTO>
    {
        protected override TokenDTO Create(Type objectType, JObject jsonObject)
        {
            return new TokenDTO();
        }
    }
}
