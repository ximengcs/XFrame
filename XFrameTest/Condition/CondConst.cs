
using XFrame.Core;
using XFrame.Core.Binder;
using XFrame.Modules.Conditions;

namespace XFrameTest.Condition
{
    public class CondConst
    {
        public const int COIN = 1;
        public const int GEM = 2;
        public const int TEST = 3;

        private static int s_Coin;
        public static ValueBinder<int> Coin = new ValueBinder<int>(
            () => s_Coin,
            (v) =>
            {
                if (v != s_Coin)
                {
                    s_Coin = v;
                    XModule.Condition.Event.Trigger(ConditionEvent.Create(COIN, s_Coin));
                }
            });

        private static int s_Gem;
        public static ValueBinder<int> Gem = new ValueBinder<int>(
            () => s_Gem,
            (v) =>
            {
                if (v != s_Gem)
                {
                    s_Gem = v;
                    XModule.Condition.Event.Trigger(ConditionEvent.Create(GEM, s_Gem));
                }
            });
    }
}
