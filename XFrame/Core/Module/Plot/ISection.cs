using XFrame.Core;

namespace XFrame.Modules.Plots
{
    public interface ISection
    {
        bool IsDone { get; }
        void OnInit(IDataProvider data);
        bool CanStart();
        void OnStart();
        void OnUpdate();
        bool OnFinish();
    }
}
