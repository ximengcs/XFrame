
namespace XFrame.Modules.Plots
{
    public interface IStory
    {
        string Name { get; }
        bool IsFinish { get; }
        void OnInit(string name, PlotDataBinder data);
        void OnStart();
        void OnUpdate();
    }
}
