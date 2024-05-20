
using XFrame.Core;
using XFrame.Modules.Diagnotics;
using XFrame.Modules.Entities;

namespace XFrameTest
{
    class EBase : Entity
    {

    }

    [EntityProp(3)]
    internal class E3 : EBase
    {
        protected override void OnInit()
        {
            base.OnInit();
            Log.Debug("E3 OnInit " + GetData<Pair<string, string>>());
        }

        protected override void OnUpdate(double elapseTime)
        {
            base.OnUpdate(elapseTime);
            Log.Debug("E3 OnUpdate");
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            Log.Debug("E3 OnDestroy");
        }

    }
}
