using Condition = XFrame.Core.ArrayParser<XFrame.Core.PairParser<XFrame.Core.IntParser, XFrame.Core.UniversalParser>>;

namespace XFrame.Modules.Conditions
{
    public struct ConditionSetting
    {
        public string Name;
        public int UseHelper;
        public bool AutoRemove;
        public Condition Condition;

        public ConditionSetting(string name, Condition condition)
        {
            Name = name;
            Condition = condition;
            AutoRemove = true;
            UseHelper = 0;
        }

        public ConditionSetting(string name, Condition condition, int useHelper)
        {
            Name = name;
            Condition = condition;
            AutoRemove = true;
            UseHelper = useHelper;
        }

        public ConditionSetting(string name, Condition condition, bool autoRemove)
        {
            Name = name;
            Condition = condition;
            AutoRemove = autoRemove;
            UseHelper = 0;
        }

        public ConditionSetting(string name, Condition condition, bool autoRemove, int useHelper)
        {
            Name = name;
            Condition = condition;
            AutoRemove = autoRemove;
            UseHelper = useHelper;
        }
    }
}
