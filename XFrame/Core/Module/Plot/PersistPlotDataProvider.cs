using XFrame.Core;
using XFrame.SimpleJSON;
using XFrame.Core.Binder;
using XFrame.Modules.Archives;
using XFrame.Modules.Serialize;

namespace XFrame.Modules.Plots
{
    /// <summary>
    /// 故事数据提供器
    /// </summary>
    internal class PersistPlotDataProvider : IPlotDataProvider
    {
        private JsonArchive m_Persist;
        private JSONObject m_Sections;
        private IPlotModule m_Module;

        public ValueBinder<bool> Finish { get; set; }

        public PersistPlotDataProvider(IPlotModule module, JsonArchive data)
        {
            m_Persist = data;
            m_Module = module;
            m_Sections = m_Persist.GetOrNewObject(nameof(m_Persist));
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

        public void ClearData()
        {
            m_Module.Domain.GetModule<IArchiveModule>().Delete(m_Persist);
            m_Persist = null;
            m_Sections = null;
        }

        public bool HasData<T>()
        {
            return HasData<T>("main_value");
        }

        public bool HasData<T>(string name)
        {
            string key = $"{name}_{typeof(T).Name}";
            return m_Sections.HasKey(key);
        }

        public T GetData<T>()
        {
            return GetData<T>("main_value");
        }

        public T GetData<T>(string name)
        {
            string key = $"{name}_{typeof(T).Name}";
            if (m_Sections.HasKey(key))
            {
                string content = m_Sections[key];
                return m_Module.Domain.GetModule<ISerializeModule>().DeserializeToObject<T>(content);
            }

            return default;
        }

        public void SetData<T>(T value)
        {
            SetData("main_value", value);
        }

        public void SetData<T>(string name, T value)
        {
            string key = $"{name}_{typeof(T).Name}";
            string content = m_Module.Domain.GetModule<ISerializeModule>().SerializeObjectToRaw(value);
            m_Sections[key] = content;
        }
    }
}
