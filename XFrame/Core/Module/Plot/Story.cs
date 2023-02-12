using System;
using System.Collections.Generic;

namespace XFrame.Modules
{
    public partial class Story : IStory
    {
        private PlotDataBinder m_Data;
        private List<Type> m_SectionOrg;
        private SectionInfo m_Current;
        private int m_CurIndex;

        public string Name { get; }
        public bool IsFinish { get => m_Data.Finish; }

        public Story(string name, string openCond)
        {
            Name = name;
            m_SectionOrg = new List<Type>();
        }

        public Story AddSection(Type type)
        {
            m_SectionOrg.Add(type);
            return this;
        }

        public void OnInit(PlotDataBinder data)
        {
            m_Data = data;
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
                        m_Data.Steps[m_CurIndex - 1] = true;
                    InnerCreateNext();
                    break;
            }
        }

        private void InnerCreateNext()
        {
            if (m_CurIndex < m_SectionOrg.Count)
            {
                m_Data.EnsureStepData(m_CurIndex);
                if (!m_Data.Steps[m_CurIndex])
                {
                    m_Current = new SectionInfo((ISection)Activator.CreateInstance(m_SectionOrg[m_CurIndex]), SectionState.WaitInit);
                    m_CurIndex++;
                }
                else
                {
                    m_CurIndex++;
                    InnerCreateNext();
                }
            }
            else
            {
                m_Current = null;
                m_Data.Finish.Value = true;
            }
        }
    }
}
