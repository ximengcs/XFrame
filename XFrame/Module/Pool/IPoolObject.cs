namespace XFrame.Modules
{
    public interface IPoolObject
    {
        void OnCreate(IPool from);
        void OnRelease(IPool from);
        void OnDestroy(IPool from);
    }
}
