using XFrame.Core;
using XFrame.Modules.Diagnotics;
using XFrame.Modules.Entities;

namespace XFrameTest
{
    internal class E1 : Entity
    {
        private bool m_Update;

        protected override void OnInit()
        {
            base.OnInit();
            m_Update = true;
            Log.Debug(GetHashCode() + " " + "E1 OnInit " + GetData<Pair<string, string>>());

            GetOrAddCom<C2>((db) => db.SetData(GetData<string>()));
            GetOrAddCom<C1>((db) => db.SetData(GetData<int>()));
        }

        protected override void OnUpdate(double elapseTime)
        {
            base.OnUpdate(elapseTime);
            if (m_Update)
            {
                m_Update = false;
                Log.Debug(GetHashCode() + " " + "E1 OnUpdate");
            }
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            Log.Debug(GetHashCode() + " " + "E1 OnDestroy");
        }
    }
}
