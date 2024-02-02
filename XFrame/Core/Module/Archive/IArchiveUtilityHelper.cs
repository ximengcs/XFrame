
namespace XFrame.Modules.Archives
{
    public interface IArchiveUtilityHelper
    {
        byte[] ReadAllBytes(string path);
        void WriteAllBytes(string path, byte[] data);
        string ReadAllText(string path);
        void WriteAllText(string path, string data);
    }
}
