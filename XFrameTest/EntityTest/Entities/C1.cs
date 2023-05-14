using XFrame.Modules.Entities;
using XFrame.Modules.Diagnotics;

namespace XFrameTest
{
    internal class C1 : EntityCom
    {
        private bool m_Update;

        protected override void OnInit()
        {
            base.OnInit();
            Log.Debug(GetHashCode() + " " + "C1 OnInit " + GetData<int>());
            m_Update = true;
        }

        protected override void OnUpdate(float elapseTime)
        {
            base.OnUpdate(elapseTime);
            if (m_Update)
            {
                m_Update = false;
                Log.Debug(GetHashCode() + " " + "C1 OnUpdate");
            }
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            Log.Debug(GetHashCode() + " " + "C1 OnDestroy");
        }

        protected override void OnCreateFromPool()
        {
            base.OnCreateFromPool();
            Log.Debug(GetHashCode() + " " + "C1 OnCreateFromPool");
        }

        protected override void OnRequestFromPool()
        {
            base.OnRequestFromPool();
            Log.Debug(GetHashCode() + " " + "C1 OnRequestFromPool");
        }

        protected override void OnDestroyFromPool()
        {
            base.OnDestroyFromPool();
            Log.Debug(GetHashCode() + " " + "C1 OnDestroyFromPool");
        }

        protected override void OnReleaseFromPool()
        {
            base.OnReleaseFromPool();
            Log.Debug(GetHashCode() + " " + "C1 OnReleaseFromPool");
        }
    }
}
