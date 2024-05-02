
using NUnit.Framework.Internal;
using XFrame.Core;
using XFrame.Modules.Containers;
using XFrame.Modules.Diagnotics;
using XFrame.Modules.Tasks;

namespace XFrameTest
{
    [TestClass]
    public class ContainerTest2
    {
        #region Container1
        public class Container1 : Container
        {
            protected override void OnInit()
            {
                base.OnInit();
                AddCom<Com1>();
                AddCom<ShareCom1>();
                update = false;

                string master = "null";
                if (Master != null) master = Master.GetHashCode().ToString();
                Log.Debug("Container1 Master " + master);

                Log.Debug("Container1 OnInit " + GetHashCode());
            }

            bool update;
            protected override void OnUpdate(float elapseTime)
            {
                base.OnUpdate(elapseTime);
                if (update) return;
                update = true;
                Log.Debug("Container1 OnUpdate " + GetHashCode());
            }

            protected override void OnDestroy()
            {
                base.OnDestroy();
                Log.Debug("Container1 OnDestroy " + GetHashCode());
            }
        }
        #endregion

        #region Com1
        public class Com1 : Com
        {
            protected override void OnInit()
            {
                base.OnInit();
                update = false;
                AddCom<Com2>();

                string master = "null";
                if (Master != null) master = Master.GetHashCode().ToString();
                string owner = "null";
                if (Parent != null) owner = Parent.GetHashCode().ToString();
                Log.Debug("Com1 Master " + master);
                Log.Debug("Com1 Owner " + owner);

                Log.Debug("Com1 OnInit " + GetHashCode());
            }

            bool update;
            protected override void OnUpdate(float elapseTime)
            {
                base.OnUpdate(elapseTime);
                if (update) return;
                update = true;
                Log.Debug("Com1 OnUpdate " + GetHashCode());
            }

            protected override void OnDestroy()
            {
                base.OnDestroy();
                Log.Debug("Com1 OnDestroy " + GetHashCode());
            }
        }
        #endregion

        #region Com2
        private class Com2 : Com
        {
            protected override void OnInit()
            {
                base.OnInit();
                update = false;
                AddCom<ShareCom1>();

                string master = "null";
                if (Master != null) master = Master.GetHashCode().ToString();
                string owner = "null";
                if (Parent != null) owner = Parent.GetHashCode().ToString();
                Log.Debug("Com2 Master " + master);
                Log.Debug("Com2 Owner " + owner);

                Log.Debug("Com2 OnInit " + GetHashCode());
            }

            bool update;
            protected override void OnUpdate(float elapseTime)
            {
                base.OnUpdate(elapseTime);
                if (update) return;
                update = true;
                Log.Debug("Com2 OnUpdate " + GetHashCode());
            }

            protected override void OnDestroy()
            {
                base.OnDestroy();
                Log.Debug("Com2 OnDestroy " + GetHashCode());
            }
        }
        #endregion

        #region 
        public class ShareCom1 : ShareCom
        {
            protected override void OnInit()
            {
                base.OnInit();
                update = false;

                AddCom<Com3>();

                string master = "null";
                if (Master != null) master = Master.GetHashCode().ToString();
                string owner = "null";
                if (Owner != null) owner = Owner.GetHashCode().ToString();
                Log.Debug("ShareCom1 Master " + master);
                Log.Debug("ShareCom1 Owner " + owner);

                Log.Debug("ShareCom1 OnInit " + GetHashCode());
            }

            bool update;
            protected override void OnUpdate(float elapseTime)
            {
                base.OnUpdate(elapseTime);
                if (update) return;
                update = true;
                Log.Debug("ShareCom1 OnUpdate " + GetHashCode());
            }

            protected override void OnDestroy()
            {
                base.OnDestroy();
                Log.Debug("ShareCom1 OnDestroy " + GetHashCode());
            }
        }
        #endregion

        public class Com3 : Com
        {

        }

        [TestMethod]
        public void Test1()
        {
            EntryTest.Exec(() =>
            {
                Log.Debug("Start");
                IContainer c1 = Entry.GetModule<IContainerModule>().New<Container1>();

                //XModule.Task.GetOrNew<ActionTask>().Add(1, () =>
                //{
                //    XModule.Container.Remove(c1);
                //    Log.Debug("Complete");
                //    IContainer c2 = XModule.Container.New<Container1>();
                //}).Start();
            });
        }
    }
}
