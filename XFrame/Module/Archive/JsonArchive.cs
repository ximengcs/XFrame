using System;
using System.IO;
using XFrame.SimpleJSON;
using XFrame.Modules.Serialize;

namespace XFrame.Modules.Archives
{
    /// <summary>
    /// Json存档
    /// </summary>
    [Archive("json")]
    public class JsonArchive : IArchive
    {
        private string m_Path;
        private JSONNode m_Root;

        #region Value Interface
        /// <summary>
        /// 设置int值
        /// </summary>
        /// <param name="key">键, 如果键已经存在，则会覆盖原始数据</param>
        /// <param name="v">值</param>
        public void SetInt(string key, int v)
        {
            m_Root[key] = v;
        }

        /// <summary>
        /// 设置long值
        /// </summary>
        /// <param name="key">键, 如果键已经存在，则会覆盖原始数据</param>
        /// <param name="v">值</param>
        public void SetLong(string key, long v)
        {
            m_Root[key] = v;
        }

        /// <summary>
        /// 获取int值
        /// </summary>
        /// <param name="key">键</param>
        /// <returns>如果未设置过此键，则会返回0</returns>
        public int GetInt(string key)
        {
            return m_Root[key];
        }

        /// <summary>
        /// 获取long值
        /// </summary>
        /// <param name="key">键</param>
        /// <returns>如果未设置过此键，则会返回0</returns>
        public long GetLong(string key)
        {
            return m_Root[key];
        }

        /// <summary>
        /// 设置float值
        /// </summary>
        /// <param name="key">键, 如果键已经存在，则会覆盖原始数据</param>
        /// <param name="v">值</param>
        public void SetFloat(string key, float v)
        {
            m_Root[key] = v;
        }

        /// <summary>
        /// 设置double值
        /// </summary>
        /// <param name="key">键, 如果键已经存在，则会覆盖原始数据</param>
        /// <param name="v">值</param>
        public void SetDouble(string key, double v)
        {
            m_Root[key] = v;
        }

        /// <summary>
        /// 获取float值
        /// </summary>
        /// <param name="key">键</param>
        /// <returns>如果未设置过此键，则会返回0</returns>
        public float GetFloat(string key)
        {
            return m_Root[key];
        }

        /// <summary>
        /// 获取double值
        /// </summary>
        /// <param name="key">键</param>
        /// <returns>如果未设置过此键，则会返回0</returns>
        public float GetDouble(string key)
        {
            return m_Root[key];
        }

        /// <summary>
        /// 设置bool值
        /// </summary>
        /// <param name="key">键, 如果键已经存在，则会覆盖原始数据</param>
        /// <param name="v">值</param>
        public void SetBool(string key, bool v)
        {
            m_Root[key] = v;
        }

        /// <summary>
        /// 获取bool值
        /// </summary>
        /// <param name="key">键</param>
        /// <returns>如果未设置过此键，则会返回false</returns>
        public bool GetBool(string key)
        {
            return m_Root[key];
        }

        /// <summary>
        /// 设置数据，推荐调用Get方法获取设置的值
        /// </summary>
        /// <param name="key">键, 如果键已经存在，则会覆盖原始数据</param>
        /// <param name="v">此值将会被序列化并保存</param>
        public void Set(string key, object v)
        {
            m_Root[key] = JSONNode.Parse(SerializeModule.Inst.SerializeObjectToJson(v));
        }

        /// <summary>
        /// 获取数据，推荐调用Set此类数据
        /// </summary>
        /// <typeparam name="T">数据类型，如果与Set设置的数据类型不匹配，可能导致出错</typeparam>
        /// <param name="key">键</param>
        /// <returns>获取到的数据</returns>
        public T Get<T>(string key)
        {
            string objStr = m_Root[key].ToString();
            return SerializeModule.Inst.DeserializeJsonToObject<T>(objStr);
        }

        /// <summary>
        /// 获取(不存在时创建)一个JsonObject对象，即Json对象{}
        /// 可以使用该对象直接设置键值
        /// </summary>
        /// <param name="key">键, 如果键已经存在，则会覆盖原始数据</param>
        /// <returns></returns>
        public JSONObject GetOrNewObject(string key)
        {
            JSONObject node;
            if (m_Root.HasKey(key))
            {
                node = m_Root[key] as JSONObject;
                if (node != null && node.GetType() == typeof(JSONObject))
                    return node;
            }
            node = new JSONObject();
            m_Root[key] = node;
            return node;
        }

        /// <summary>
        /// 获取(不存在时创建)一个JsonArray对象，即Json数组对象[]
        /// 可以使用该对象直接设置数组数据
        /// </summary>
        /// <param name="key">键, 如果键已经存在，则会覆盖原始数据</param>
        /// <returns>获取到的JsonArray对象</returns>
        public JSONArray GetOrNewArray(string key)
        {
            JSONArray node;
            if (m_Root.HasKey(key))
            {
                node = m_Root[key] as JSONArray;
                if (node != null && node.GetType() == typeof(JSONArray))
                    return node;
            }
            node = new JSONArray();
            m_Root[key] = node;
            return node;
        }
        #endregion

        #region Archive Interface
        public void Delete()
        {
            if (File.Exists(m_Path))
                File.Delete(m_Path);
        }

        void IArchive.OnInit(string path)
        {
            m_Path = path;
            if (File.Exists(m_Path))
                m_Root = JSONNode.Parse(File.ReadAllText(m_Path));
            if (m_Root == null)
                m_Root = new JSONObject();
        }

        public void Save()
        {
            Console.WriteLine(m_Root.ToString());
            File.WriteAllText(m_Path, m_Root.ToString(4));
        }
        #endregion
    }
}
