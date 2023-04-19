using XFrame.Core;
using XFrame.Modules.Diagnotics;
using XFrame.Modules.Entities;

namespace XFrameTest
{
    internal class E1 : Entity
    {
        protected override void OnInit()
        {
            base.OnInit();
            Log.Debug("E1 OnInit " + GetData<Pair<string, string>>());
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

        protected override void OnCreateFromPool()
        {
            base.OnCreateFromPool();
            Log.Debug("E1 OnCreateFromPool");
        }

        protected override void OnDestroyFromPool()
        {
            base.OnDestroyFromPool();
            Log.Debug("E1 OnDestroyFromPool");
        }

        protected override void OnReleaseFromPool()
        {
            base.OnReleaseFromPool();
            Log.Debug("E1 OnReleaseFromPool");
        }
    }
}
