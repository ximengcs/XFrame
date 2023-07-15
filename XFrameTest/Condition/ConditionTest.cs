
using System.Reflection;
using XFrame.Core;
using XFrame.Modules.Conditions;
using XFrame.Modules.Diagnotics;
using XFrame.Modules.Tasks;

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
                cond1.Parse("1|100,2|1.2,3|9;8,3|7;4");

                Pair<IntParser, UniversalParser> pair = cond1.Get(2);
                PairParser<IntParser, IntParser> valuePairParser = new PairParser<IntParser, IntParser>(';');
                Pair<IntParser, IntParser> valuePair = pair.Value.AddParser(valuePairParser);
                Log.Debug(valuePair.Key + " " + valuePair.Value);
                //ConditionModule.Inst.Register(name1, cond1).OnComplete((handle) =>
                //{
                //    Log.Debug($"{handle.Name} complete");
                //});

                TaskModule.Inst.GetOrNew<DelayTask>().Add(100, () =>
                {
                    Log.Debug("Exec");
                    CondConst.Coin.Value += 200;
                    CondConst.Gem.Value += 200;
                }).Start();

                TaskModule.Inst.GetOrNew<DelayTask>().Add(1000, () =>
                {
                    Log.Debug("Exec2");
                    ConditionModule.Inst.Event.Trigger(ConditionEvent.Create(CondConst.TEST, "9;9"));
                }).Start();
            });
        }

        [TestMethod]
        public void Test2()
        {
            CompareInfo info = new CompareInfo(new Con1Compare());
            info.Check(null, 9);
            info.CheckFinish(null);
            info.OnEventTrigger(8);
        }

        public class CompareInfo
        {
            private IConditionCompare m_Inst;
            private Delegate m_CheckFun;
            private Delegate m_OnEventTriggerFun;

            public int Target => m_Inst.Target;

            public bool CheckFinish(IConditionHandle info)
            {
                return m_Inst.CheckFinish(info);
            }

            public bool Check(IConditionHandle info, object param)
            {
                return (bool)m_CheckFun.DynamicInvoke(info, param);
            }

            public void OnEventTrigger(object param)
            {
                m_OnEventTriggerFun.DynamicInvoke(param);
            }

            public CompareInfo(IConditionCompare compare)
            {
                m_Inst = compare;
                Type type = compare.GetType();
                Type interfaceType = type.GetInterface(typeof(IConditionCompare<>).FullName);
                Console.WriteLine(type.Name);
                Type geneType = interfaceType.GetGenericArguments()[0];
                Console.WriteLine(geneType.FullName);
                MethodInfo methodInfo = interfaceType.GetMethod(nameof(Check));
                Type deleType = typeof(Func<,,>);
                deleType = deleType.MakeGenericType(typeof(IConditionHandle), geneType, typeof(bool));
                m_CheckFun = methodInfo.CreateDelegate(deleType, m_Inst);

                methodInfo = interfaceType.GetMethod(nameof(OnEventTrigger));
                deleType = typeof(Action<>);
                deleType = deleType.MakeGenericType(geneType);
                m_OnEventTriggerFun = methodInfo.CreateDelegate(deleType, m_Inst);
            }
        }
    }
}
