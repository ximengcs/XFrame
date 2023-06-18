﻿using Newtonsoft.Json;
using XFrame.Modules.Serialize;

namespace XFrameTest
{
    public class TestJsonSerializer : ISerializeHelper
    {
        public object Deserialize(string json, Type dataType)
        {
            return JsonConvert.DeserializeObject(json, dataType);
        }

        public T Deserialize<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }

        public string Serialize<T>(T obj)
        {
            return JsonConvert.SerializeObject(obj);
        }
    }
}
