
using XFrame.Modules.Diagnotics;
using XFrame.Modules.Entities;

namespace XFrameTest
{
    internal class C2 : EntityShareCom
    {
        private bool m_Update;

        protected override void OnInit()
        {
            base.OnInit();
            Log.Debug(GetHashCode() + " " + "C2 OnInit " + GetData<string>());
            m_Update = true;
        }

        protected override void OnUpdate(float elapseTime)
        {
            base.OnUpdate(elapseTime);
            if (m_Update)
            {
                m_Update = false;
                Log.Debug(GetHashCode() + " " + "C2 OnUpdate");
            }
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            Log.Debug(GetHashCode() + " " + "C2 OnDestroy");
        }
    }
}
