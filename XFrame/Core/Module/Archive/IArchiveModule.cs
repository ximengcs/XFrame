using XFrame.Core;

namespace XFrame.Modules.Archives
{
    public interface IArchiveModule : IModule, IUpdater, ISaveable
    {
        T GetOrNew<T>(string name, object param = null) where T : IArchive;
        void Delete(string name);
        void Delete(IArchive archive);
        void DeleteAll();
    }
}
