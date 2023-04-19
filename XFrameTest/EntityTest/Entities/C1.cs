using XFrame.Modules.Entities;
using XFrame.Modules.Diagnotics;

namespace XFrameTest
{
    internal class C1 : EntityCom
    {
        protected override void OnInit()
        {
            base.OnInit();

            Log.Debug("C1 OnInit");
        }

        protected override void OnUpdate(float elapseTime)
        {
            base.OnUpdate(elapseTime);
            Log.Debug("C1 OnUpdate");
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            Log.Debug("C1 OnDestroy");
        }
    }
}
