﻿
namespace XFrame.Modules
{
    [EntityProp]
    public class Scene : Com
    {
        #region Interface
        public bool IsRunning { get; private set; }

        public virtual void Run()
        {
            IsRunning = true;
        }

        public virtual void Pause()
        {
            IsRunning = false;
        }
        #endregion

        #region Life Fun
        internal override void OnInternalUpdate(float elapseTime)
        {
            if (IsRunning)
                base.OnInternalUpdate(elapseTime);
        }

        public override void OnRelease()
        {
            base.OnRelease();
            IsRunning = false;
        }

        protected override void OnInit(EntityData data)
        {

        }

        protected override void OnUpdate(float elapseTime)
        {

        }

        protected override void OnDestroy()
        {

        }
        #endregion
    }
}
