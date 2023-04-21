
namespace XFrame.Modules.Containers
{
    public delegate void OnContainerReady(IContainer container);

    public delegate void OnContainerReady<T>(T container) where T : IContainer;
}
