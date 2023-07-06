
using XFrame.Core;
using XFrame.Modules.Conditions;
using XFrame.Modules.Diagnotics;

namespace XFrameTest.Condition
{
    [TestClass]
    public class ConditionTest
    {
        [TestMethod]
        public void Test1()
        {
            EntryTest.Exec(() =>
            {
                string name1 = "cond1";
                ArrayParser<PairParser<IntParser, UniversalParser>> cond1 = new ArrayParser<PairParser<IntParser, UniversalParser>>();
                cond1.Parse("1|100,2|1.2,3|9;8");

                Pair<IntParser, UniversalParser> pair = cond1.Get(2);
                PairParser<IntParser, IntParser> valuePairParser = new PairParser<IntParser, IntParser>(';');
                Pair<IntParser, IntParser> valuePair = pair.Value.AddParser(valuePairParser);
                Log.Debug(valuePair.Key + " " + valuePair.Value);
                ConditionModule.Inst.SetHelper(new ConditionHelper());
                ConditionModule.Inst.Register(name1, cond1).OnComplete((handle) =>
                {
                    Log.Debug($"{handle.Name} complete");
                });
            });
        }
    }
}
