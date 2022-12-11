
namespace XFrame.Modules
{
    public interface IFsmBase
    {
        string Name { get; }
        void OnUpdate();
        void OnDestroy();
    }
}
