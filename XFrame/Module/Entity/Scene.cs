
namespace XFrame.Modules
{
    /// <summary>
    /// 场景
    /// </summary>
    [EntityProp]
    public class Scene : Com
    {
        #region Interface
        /// <summary>
        /// 场景是否处于运行状态
        /// </summary>
        public bool IsRunning { get; private set; }

        /// <summary>
        /// 运行场景
        /// </summary>
        public virtual void Run()
        {
            IsRunning = true;
        }

        /// <summary>
        /// 暂停场景
        /// </summary>
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
        #endregion

        #region Pool Life Fun
        public override void OnRelease()
        {
            base.OnRelease();
            IsRunning = false;
        }
        #endregion

        #region Entity Life Fun
        protected override void OnInit(EntityData data)
        {

        }

        protected override void OnUpdate(float elapseTime)
        {

        }

        protected override void OnDestroy()
        {

        }

        protected override void OnDelete()
        {

        }
        #endregion
    }
}
