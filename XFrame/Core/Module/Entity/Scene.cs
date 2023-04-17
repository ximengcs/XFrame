
namespace XFrame.Modules.Entities
{
    /// <summary>
    /// 场景
    /// </summary>
    [EntityProp]
    public class Scene : EntityCom, IScene
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

        #region Pool Life Fun
        protected override void OnRelease()
        {
            IsRunning = false;
        }

        protected override void OnCreate()
        {

        }

        protected override void OnDestroyFromPool()
        {

        }
        #endregion

        #region Entity Life Fun
        protected override void OnInit(EntityData data)
        {
            base.OnInit(data);
        }

        protected override void OnUpdate(float elapseTime)
        {
            if (IsRunning)
                base.OnUpdate(elapseTime);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
        }
        #endregion
    }
}
