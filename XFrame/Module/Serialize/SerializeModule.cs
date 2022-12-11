using System;
using XFrame.Core;

namespace XFrame.Modules
{
    public class SerializeModule : SingleModule<SerializeModule>
    {
        private IJsonSerializeHelper m_JsonHelper;

        public void Register<T>() where T : IJsonSerializeHelper
        {
            m_JsonHelper = Activator.CreateInstance<T>();
        }

        public T DeserializeJsonToObject<T>(string json)
        {
            return m_JsonHelper.Deserialize<T>(json);
        }

        public string SerializeObjectToJson(object obj)
        {
            return m_JsonHelper.Serialize(obj);
        }
    }
}
