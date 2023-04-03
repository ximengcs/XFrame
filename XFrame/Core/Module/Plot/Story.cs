using System;
using System.Collections.Generic;

namespace XFrame.Modules.Plots
{
    public partial class Story : IStory
    {
        private int m_Index;
        private PlotDataBinder m_Data;
        private SectionInfo m_Current;
        private Queue<Type> m_SectionTypes;

        public string Name { get; private set; }

        public bool IsFinish => m_Data.Finish;

        public Story AddSection(Type type)
        {
            m_SectionTypes.Enqueue(type);
            return this;
        }

        public void OnInit(string name, PlotDataBinder data)
        {
            Name = name;
            m_Data = data;
            m_SectionTypes = new Queue<Type>();
        }

        public void OnStart()
        {
            InnerCreateNext();
        }

        public void OnUpdate()
        {
            if (IsFinish)
                return;

            switch (m_Current.State)
            {
                case SectionState.WaitInit:
                    m_Current.Section.OnInit(m_Data);
                    m_Current.State = SectionState.WaitStart;
                    break;

                case SectionState.WaitStart:
                    if (m_Current.Section.CanStart())
                    {
                        m_Current.Section.OnStart();
                        m_Current.State = SectionState.Running;
                    }
                    break;

                case SectionState.Running:
                    m_Current.Section.OnUpdate();
                    if (m_Current.Section.IsDone)
                        m_Current.State = SectionState.Finish;
                    break;

                case SectionState.Finish:
                    if (m_Current.Section.OnFinish())
                    {
                        m_Data.SetSectionFinish(m_Index, true);
                        m_Current = null;
                        m_Index++;
                    }
                    InnerCreateNext();
                    break;
            }
        }

        private void InnerCreateNext()
        {
            if (m_SectionTypes.Count > 0)
            {
                Type type = m_SectionTypes.Dequeue();
                if (m_Data.CheckSectionFinish(m_Index))
                {
                    m_Index++;
                    InnerCreateNext();
                }
                else
                {
                    ISection section = (ISection)Activator.CreateInstance(type);
                    m_Current = new SectionInfo(section, SectionState.WaitInit);
                }
            }
            else
            {
                m_Data.Finish.Value = true;
            }
        }
    }
}
