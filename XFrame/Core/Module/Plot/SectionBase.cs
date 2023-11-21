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

        public virtual bool CanStart()
        {
            return true;
        }

        public virtual bool OnFinish()
        {
            return true;
        }

        void ISection.OnCreate(IStory story, IDataProvider data)
        {
            m_Story = story;
            m_Data = data;
        }

        public virtual void OnStart()
        {

        }

        public virtual void OnUpdate()
        {

        }

        public virtual void OnInit()
        {

        }
    }
}
