using System.IO;
using System.Collections.Generic;
using JsonType = System.Collections.Generic.Dictionary<string, object>;

namespace XFrame.Modules
{
    public class JsonArchive : IArchive
    {
        private string m_Path;
        private JsonType m_Cache;

        public void Delete()
        {
            if (File.Exists(m_Path))
                File.Delete(m_Path);
        }

        public void Init(string path)
        {
            m_Path = path;
            if (File.Exists(m_Path))
                m_Cache = SerializeModule.Inst.DeserializeJsonToObject<JsonType>(File.ReadAllText(m_Path));
            if (m_Cache == null)
                m_Cache = new JsonType();
        }

        public void Set(string key, object v)
        {
            m_Cache[key] = v;
        }

        public void SetArray<T>(string key, List<T> values)
        {
            m_Cache[key] = values;
        }

        public T Get<T>(string key)
        {
            if (m_Cache.TryGetValue(key, out object value))
                return (T)value;
            else
                return default(T);
        }

        public List<T> GetArray<T>(string key)
        {
            if (m_Cache.TryGetValue(key, out object value))
                return value as List<T>;
            else
                return default;
        }

        public object Read()
        {
            return m_Cache;
        }

        public void Write(object data)
        {
            m_Cache = (JsonType)data;
            string json = SerializeModule.Inst.SerializeObjectToJson(data);
            File.WriteAllText(m_Path, json);
        }

        public void Save()
        {
            Write(m_Cache);
        }
    }
}
