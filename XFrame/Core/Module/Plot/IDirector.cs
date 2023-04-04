namespace XFrame.Modules.Plots
{
    public interface IDirector
    {
        void Add(IStory[] stories);
        void Add(IStory story);
        void Remove(IStory story);
        void Remove(string storyName);

        internal void OnInit();
        internal void OnUpdate();
        internal void OnDestory();
    }
}
