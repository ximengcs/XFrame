
namespace XFrame.Modules.Containers
{
    internal interface ICanInitialize
    {
        void OnInit(int id, IContainer master, OnDataProviderReady onReady);
    }
}
