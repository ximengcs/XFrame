using XFrame.Modules.Diagnotics;
using XFrame.Modules.Entities;

namespace XFrameTest
{
    internal class E1 : Entity
    {
        protected override void OnInit()
        {
            base.OnInit();
            Log.Debug("E1 OnInit");
        }

        protected override void OnUpdate(float elapseTime)
        {
            base.OnUpdate(elapseTime);
            Log.Debug("E1 OnUpdate");
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            Log.Debug("E1 OnDestroy");
        }

        protected override void OnDestroyFromPool()
        {
            base.OnDestroyFromPool();
            Log.Debug("E1 OnDestroyFromPool");
        }
    }
}
