
namespace XFrame.Modules.Containers
{
    public delegate void OnComReady(ICom com);

    public delegate void OnComReady<T>(T com) where T : ICom;
}
