using XFrame.Core;
using XFrame.Modules.Conditions;
using XFrame.Modules.Diagnotics;

namespace XFrameTest.Condition
{
    public class Con1Compare : IConditionCompare
    {
        int IConditionCompare.Target => CondConst.COIN;

        public void OnEventTrigger(object param)
        {

        }

        bool IConditionCompare.Check(ConditionHandle info, object param)
        {
            Log.Debug("Check Coin " + info.Param.IntValue + " " + CondConst.Coin);
            return CondConst.Coin >= info.Param.IntValue;
        }

        bool IConditionCompare.CheckFinish(ConditionHandle info)
        {
            Log.Debug("CheckFinish Coin " + info.Param.IntValue + " " + CondConst.Coin);
            return CondConst.Coin >= info.Param.IntValue;
        }
    }

    public class Con2Compare : IConditionCompare
    {
        int IConditionCompare.Target => CondConst.GEM;

        public void OnEventTrigger(object param)
        {

        }

        bool IConditionCompare.Check(ConditionHandle info, object param)
        {
            Log.Debug("Check Gem " + info.Param.FloatValue + " " + CondConst.Gem);
            return CondConst.Gem >= info.Param.IntValue;
        }

        bool IConditionCompare.CheckFinish(ConditionHandle info)
        {
            Log.Debug("CheckFinish Gem " + info.Param.FloatValue + " " + CondConst.Gem);
            return CondConst.Gem >= info.Param.IntValue;
        }
    }

    public class Con3Compare : IConditionCompare
    {
        int IConditionCompare.Target => CondConst.TEST;

        public void OnEventTrigger(object param)
        {

        }

        bool IConditionCompare.Check(ConditionHandle info, object param)
        {
            int times = info.Data.GetData<int>("times");
            PairParser<IntParser, IntParser> parser = info.Param.GetParser<PairParser<IntParser, IntParser>>();
            if (parser == null)
            {
                parser = new PairParser<IntParser, IntParser>(';');
                info.Param.AddParser(parser);
            }
            Pair<IntParser, IntParser> pair = parser;

            int target = pair.Value;
            bool finish = times >= target;
            if (!finish)
            {
                parser.Parse((string)param);
                times += parser.Value.Value;
                info.Data.SetData("times", times);
            }
            Log.Debug("Check Item3 " + times + " " + target);
            return times >= target;
        }

        bool IConditionCompare.CheckFinish(ConditionHandle info)
        {
            int times = info.Data.GetData<int>("times");
            PairParser<IntParser, IntParser> parser = new PairParser<IntParser, IntParser>(';');
            Pair<IntParser, IntParser> pair = info.Param.AddParser(parser);
            Log.Debug("CheckFinish Item3 " + times + " " + pair.Value);
            return times >= pair.Value;
        }
    }
}
