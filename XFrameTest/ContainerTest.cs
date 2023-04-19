
using XFrame.Modules.Containers;
using XFrame.Modules.Diagnotics;
using XFrame.Modules.Tasks;

namespace XFrameTest
{
    [TestClass]
    public class ContainerTest
    {
        private class EntityA
        {
            private IContainer m_Coms;

            public EntityA()
            {
                m_Coms = ContainerModule.Inst.New();
                m_Coms.AddCom<Com1>();
                m_Coms.AddCom<Com2>((com) => com.SetData(98259));
                m_Coms.AddCom<Com3>();
                m_Coms.AddCom<Com1>();
            }

            public void Destroy()
            {
                ContainerModule.Inst.Remove(m_Coms);
                m_Coms = default;
            }
        }

        private class Com1 : Com
        {
            protected override void OnInit()
            {
                base.OnInit();
                Log.Debug($"Com1 OnInit");
            }

            protected override void OnUpdate(float elpseTime)
            {
                base.OnUpdate(elpseTime);
                Log.Debug($"Com1 OnUpdate");
            }

            protected override void OnDestroy()
            {
                base.OnDestroy();
                Log.Debug($"Com1 OnDestroy");
            }
        }

        private class Com2 : Com
        {
            protected override void OnInit()
            {
                base.OnInit();
                Log.Debug($"Com2 OnInit");
            }

            protected override void OnUpdate(float elpseTime)
            {
                base.OnUpdate(elpseTime);
                Log.Debug($"Com2 OnUpdate");
            }

            protected override void OnDestroy()
            {
                base.OnDestroy();
                Log.Debug($"Com2 OnDestroy");
            }
        }

        private class Com3 : Com
        {
            protected override void OnInit()
            {
                base.OnInit();
                Log.Debug($"Com3 OnInit");
            }

            protected override void OnUpdate(float elpseTime)
            {
                base.OnUpdate(elpseTime);
                Log.Debug($"Com3 OnUpdate");
            }

            protected override void OnDestroy()
            {
                base.OnDestroy();
                Log.Debug($"Com3 OnDestroy");
            }
        }

        private class Com4 : Com
        {
            protected override void OnInit()
            {
                base.OnInit();
                Log.Debug($"Com4 OnInit");
            }

            protected override void OnUpdate(float elpseTime)
            {
                base.OnUpdate(elpseTime);
                Log.Debug($"Com4 OnUpdate");
            }

            protected override void OnDestroy()
            {
                base.OnDestroy();
                Log.Debug($"Com4 OnDestroy");
            }
        }

        [TestMethod]
        public void Test1()
        {
            EntryTest.Exec(() =>
            {
                EntityA a = new EntityA();
                TaskModule.Inst.GetOrNew<DelayTask>().Add(1.0f, a.Destroy).Start();
            });
        }
    }
}
