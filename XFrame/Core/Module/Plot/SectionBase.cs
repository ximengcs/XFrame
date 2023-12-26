using XFrame.Core;

namespace XFrame.Modules.Plots
{
    public abstract class SectionBase : ISection
    {
        private IDataProvider m_Data;
        private IStory m_Story;

        public IStory Story => m_Story;
        public IDataProvider Data => m_Data;

        public bool IsDone { get; protected set; }

        /// <summary>
        /// 初始化生命周期
        /// </summary>
        /// <param name="data">数据提供器</param>
        protected internal void OnCreate(IStory story, IDataProvider data)
        {
            m_Story = story;
            m_Data = data;
        }

        protected internal virtual void OnInit()
        {

        }

        /// <summary>
        /// 是否可以开始播放
        /// </summary>
        /// <returns>true表示此情节可以开始播放</returns>
        protected internal virtual bool CanStart()
        {
            return true;
        }

        /// <summary>
        /// 开始生命周期
        /// </summary>
        protected internal virtual void OnStart()
        {

        }

        /// <summary>
        /// 更新生命周期
        /// </summary>
        protected internal virtual void OnUpdate()
        {

        }

        /// <summary>
        /// 完成生命周期
        /// </summary>
        /// <returns>返回true表示已处理完完成后的清理工作</returns>
        protected internal virtual bool OnFinish()
        {
            return true;
        }

    }
}
