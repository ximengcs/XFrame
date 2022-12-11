namespace XFrame.Modules
{
    public interface IDirector
    {
        void Add(IStory story);
        void OnUpdate();
        void OnDestory();
    }
}
