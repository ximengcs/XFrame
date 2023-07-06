
namespace XFrame.Modules.Conditions
{
    public interface IConditionCompare
    {
        int Target { get; }
        bool CheckFinish(ConditionHandle info);
        bool Check(ConditionHandle info, object param);
    }
}
