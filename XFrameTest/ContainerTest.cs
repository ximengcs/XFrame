
using XFrame.Core;
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

            public IContainer Container => m_Coms;

            public EntityA()
            {
                m_Coms = XModule.Container.New();
                Log.Debug($"{m_Coms.GetHashCode()} + {m_Coms.GetCom<Com1>() == null}");

                m_Coms.GetOrAddCom<Com1>();
                m_Coms.GetOrAddCom<Com2>((com) =>
                {
                    com.SetData(98259);
                });
                m_Coms.GetOrAddCom<Com3>();
                m_Coms.GetOrAddCom<Com1>();
            }

            public void Destroy()
            {
                XModule.Container.Remove(m_Coms);
                m_Coms = default;
            }
        }

        private interface IT : ICom
        {

        }

        private class Com1 : Com, IT
        {
            protected override void OnInit()
            {
                base.OnInit();
                GetOrAddCom<Com4>();
                Log.Debug($"Com1 OnInit " + (Master == null));
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

        private class Com3 : ShareCom
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

        private class Com4 : ShareCom
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
                IT it = a.Container.GetCom<IT>();
                a.Destroy();

                a = new EntityA();
            });
        }

        [TestMethod]
        public void Test2()
        {
            EntryTest.Exec(() =>
            {
                Console.WriteLine("New");
                Container container = XModule.Container.New();
                XModule.Task.GetOrNew<ActionTask>()
                .Add(() =>
                {
                    XModule.Container.Remove(container);
                }, true).Start();
            });
        }
    }
}
