using XFrame.Core;

namespace XFrame.Modules.Plots
{
    /// <summary>
    /// 情节基类
    /// </summary>
    public abstract class SectionBase : ISection
    {
        private IDataProvider m_Data;
        private IStory m_Story;

        /// <inheritdoc/>
        public IStory Story => m_Story;

        /// <inheritdoc/>
        public IDataProvider Data => m_Data;

        /// <inheritdoc/>
        public bool IsDone { get; protected set; }

        /// <inheritdoc/>
        public virtual bool CanStart()
        {
            return true;
        }

        /// <inheritdoc/>
        public virtual bool OnFinish()
        {
            return true;
        }

        void ISection.OnCreate(IStory story, IDataProvider data)
        {
            m_Story = story;
            m_Data = data;
        }

        /// <inheritdoc/>
        public virtual void OnStart()
        {

        }

        /// <inheritdoc/>
        public virtual void OnUpdate()
        {

        }

        /// <inheritdoc/>
        public virtual void OnInit()
        {

        }
    }
}
