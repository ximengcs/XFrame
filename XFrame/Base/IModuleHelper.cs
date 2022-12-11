
namespace XFrame.Core
{
    public interface IModuleHelper
    {
        void OnModuleCreate(IModule module);
        void OnModuleUpdate();
        void OnModuleDestroy();
    }
}
