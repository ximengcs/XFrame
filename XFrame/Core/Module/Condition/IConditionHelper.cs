
namespace XFrame.Modules.Conditions
{
    public interface IConditionHelper
    {
        void MarkFinish(string groupName);
        bool CheckFinish(string groupName);
    }
}
