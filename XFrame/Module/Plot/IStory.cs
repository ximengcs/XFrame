
namespace XFrame.Modules
{
    public interface IStory
    {
        string Name { get; }
        bool IsFinish { get; }
        void OnInit(PlotDataBinder data);
        void OnStart();
        void OnUpdate();
    }
}
