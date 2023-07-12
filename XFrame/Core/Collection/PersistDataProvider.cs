using XFrame.Modules.Archives;

namespace XFrame.Core
{
    public class PersistDataProvider : IDataProvider
    {
        private JsonArchive m_Persist;

        public PersistDataProvider(string name)
        {
            m_Persist = ArchiveModule.Inst.GetOrNew<JsonArchive>(name);
        }

        public void ClearData()
        {
            ArchiveModule.Inst.Delete(m_Persist);
            m_Persist = null;
        }

        public T GetData<T>()
        {
            return GetData<T>("main_value");
        }

        public T GetData<T>(string name)
        {
            string key = $"{name}_{typeof(T).Name}";
            return m_Persist.Get<T>(key);
        }

        public void SetData<T>(T value)
        {
            SetData("main_value", value);
        }

        public void SetData<T>(string name, T value)
        {
            string key = $"{name}_{typeof(T).Name}";
            m_Persist.Set(key, value);
        }
    }
}
