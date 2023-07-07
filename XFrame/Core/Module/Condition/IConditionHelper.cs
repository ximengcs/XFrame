
namespace XFrame.Modules.Conditions
{
    public interface IConditionHelper
    {
        int Type { get; }
        void MarkFinish(string groupName);
        bool CheckFinish(string groupName);
    }
}
