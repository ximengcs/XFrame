using XFrame.Core;
using XFrame.SimpleJSON;
using XFrame.Core.Binder;
using XFrame.Modules.Archives;

namespace XFrame.Modules.Plots
{
    public class PlotDataBinder : IDataProvider
    {
        private JsonArchive m_Persist;
        private JSONObject m_SectionData;
        private JSONObject m_Sections;

        public ValueBinder<bool> Finish { get; set; }

        public PlotDataBinder(JsonArchive data)
        {
            m_Persist = data;
            m_Sections = m_Persist.GetOrNewObject(nameof(m_Persist));
            m_SectionData = m_Persist.GetOrNewObject(nameof(m_SectionData));
            Finish = new ValueBinder<bool>(
                () => m_Persist.GetBool(nameof(Finish)),
                (v) => m_Persist.SetBool(nameof(Finish), v));
        }

        public bool CheckSectionFinish(int index)
        {
            string key = $"section{index}";
            if (m_Sections.HasKey(key))
                return m_Sections[key];
            else
                return false;
        }

        public void SetSectionFinish(int index, bool finish)
        {
            string key = $"section{index}";
            m_Sections[key] = finish;
        }

        public void Dispose()
        {

        }

        public T GetData<T>() where T : class
        {
            return GetData<T>("main_value");
        }

        public T GetData<T>(string name) where T : class
        {
            string key = $"{name}_{typeof(T).Name}";
            if (m_Sections.HasKey(key))
                return (T)(object)m_Sections[key].AsObject;
            return default;
        }

        public void SetData<T>(T value) where T : class
        {
            SetData<T>("main_value", value);
        }

        public void SetData<T>(string name, T value) where T : class
        {
            string key = $"{name}_{typeof(T).Name}";
        }
    }
}
