using XFrame.Core;
using XFrame.SimpleJSON;

namespace XFrame.Modules.Archives
{
    public interface IJsonArchive : IDataProvider
    {
        string Name { get; }
        void SetInt(string key, int v);
        void SetLong(string key, long v);
        int GetInt(string key, int defaultValue = default);
        long GetLong(string key, long defaultValue = default);
        void SetFloat(string key, float v);
        void SetDouble(string key, double v);
        float GetFloat(string key, float defaultValue = default);
        float GetDouble(string key, float defaultValue = default);
        void SetBool(string key, bool v);
        bool GetBool(string key, bool defaultValue = default);
        void Set(string key, object v);
        T Get<T>(string key, T defaultValue = default);
        JSONObject GetOrNewObject(string key);
        JSONArray GetOrNewArray(string key);
        void Remove(string key);
        IJsonArchive SpwanDataProvider(string name);
        IJsonArchive SpwanDataProvider();
    }
}
