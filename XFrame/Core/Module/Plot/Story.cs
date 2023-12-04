using System;
using XFrame.Core;
using System.Collections.Generic;

namespace XFrame.Modules.Plots
{
    internal partial class Story : IStory
    {
        private IStoryHelper m_Helper;
        private IDirector m_Director;
        private IPlotDataProvider m_Data;
        private int m_Index;
        private SectionInfo m_Current;
        private List<SectionInfo> m_SectionTypes;

        public string Name { get; private set; }

        public int Count => m_SectionTypes.Count;

        public bool IsFinish
        {
            get => m_Data.Finish;
            set => m_Data.Finish.Value = value;
        }

        public IDirector Director => m_Director;

        public IStoryHelper Helper => m_Helper;

        public IEnumerable<ISection> Sections
        {
            get
            {
                List<ISection> sections = new List<ISection>();
                foreach (SectionInfo info in sections)
                {
                    sections.Add(info.Section);
                }
                return sections;
            }
        }

        ISection IStory.AddSection(Type type)
        {
            ISection section = (ISection)XModule.Type.CreateInstance(type);
            int index = m_SectionTypes.Count;
            SectionInfo info = new SectionInfo(index, section, SectionState.WaitInit, m_Data);
            info.Section.OnCreate(this, new SectionDataProvider(index, m_Data));
            m_SectionTypes.Add(info);
            return section;
        }

        public Story(IDirector director, IStoryHelper helper, string name)
        {
            m_Helper = helper;
            m_Director = director;
            if (string.IsNullOrEmpty(name))
                name = $"story_{XModule.Rand.RandPath()}";
            Name = name;
            m_SectionTypes = new List<SectionInfo>();
            m_Data = director.CreateDataProvider(this);
        }

        void IStory.OnInit()
        {

        }

        void IStory.OnStart()
        {
            InnerCreateNext();
        }

        void IStory.OnUpdate()
        {
            if (IsFinish)
                return;

            switch (m_Current.State)
            {
                case SectionState.WaitInit:
                    m_Current.State = SectionState.WaitStart;
                    m_Current.Section.OnInit();
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
                        m_Current.SetFinishData();
                        XModule.Plot.Event.TriggerNow(PlotSectionFinishEvent.Create(m_Current.Section));
                        m_Current = null;
                        InnerCreateNext();
                    }
                    break;
            }
        }

        private void InnerCreateNext()
        {
            if (m_Index < m_SectionTypes.Count)
            {
                m_Current = m_SectionTypes[m_Index++];
                if (m_Current.CheckFinishData())
                {
                    InnerCreateNext();
                }
            }
            else
            {
                m_Data.Finish.Value = true;
            }
        }

        void IStory.OnFinish()
        {

        }

        void IStory.OnDestroy()
        {
            m_Data.ClearData();
        }

        public bool HasData<T>()
        {
            return m_Data.HasData<T>();
        }

        public bool HasData<T>(string name)
        {
            return m_Data.HasData<T>(name);
        }

        public void SetData<T>(T value)
        {
            m_Data.SetData<T>(value);
        }

        public T GetData<T>()
        {
            return m_Data.GetData<T>();
        }

        public void SetData<T>(string name, T value)
        {
            m_Data.SetData<T>(name, value);
        }

        public T GetData<T>(string name)
        {
            return m_Data.GetData<T>(name);
        }

        public void ClearData()
        {
            m_Data.ClearData();
        }
    }
}
