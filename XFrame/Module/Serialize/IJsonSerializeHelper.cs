
namespace XFrame.Modules
{
    public interface IJsonSerializeHelper
    {
        T Deserialize<T>(string json);
        string Serialize<T>(T obj);
    }
}
