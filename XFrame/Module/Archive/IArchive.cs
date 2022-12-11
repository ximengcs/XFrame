
namespace XFrame.Modules
{
    public interface IArchive
    {
        void Init(string path);
        object Read();
        void Write(object data);
        void Save();
        void Delete();
    }
}
