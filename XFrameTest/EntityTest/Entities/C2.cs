
using XFrame.Modules.Diagnotics;
using XFrame.Modules.Entities;

namespace XFrameTest
{
    internal class C2 : EntityCom
    {
        protected override void OnInit()
        {
            base.OnInit();
            C1 c1 = GetCom<C1>();
            Log.Debug("C2 OnInit");
        }

        protected override void OnUpdate(float elapseTime)
        {
            base.OnUpdate(elapseTime);
            Log.Debug("C2 OnUpdate");
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            Log.Debug("C2 OnDestroy");
        }
    }
}
