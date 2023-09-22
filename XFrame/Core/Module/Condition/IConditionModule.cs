using XFrame.Core;
using XFrame.Modules.Event;

namespace XFrame.Modules.Conditions
{
    public interface IConditionModule : IModule
    {
        IEventSystem Event { get; }
        IConditionGroupHandle Get(string name);
        IConditionGroupHandle Register(ConditionSetting setting);
        void UnRegister(string name);
        void UnRegister(IConditionGroupHandle handle);
        CompareInfo GetOrNewCompare(int target, int instance = ConditionHelperSetting.DEFAULT_INSTANCE);
        IConditionHelper GetOrNewHelper(int type, int instance = ConditionHelperSetting.DEFAULT_INSTANCE);
    }
}
