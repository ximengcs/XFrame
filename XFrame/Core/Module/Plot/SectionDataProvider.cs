
using XFrame.Core;

namespace XFrame.Modules.Plots
{
    public class SectionDataProvider : IDataProvider
    {
        private IDataProvider m_Data;
        private string m_Prefix;

        public SectionDataProvider(int index, IDataProvider dataProvider)
        {
            m_Prefix = $"section_{index}";
            m_Data = dataProvider;
        }

        public void ClearData()
        {

        }

        public T GetData<T>()
        {
            return GetData<T>($"{m_Prefix}_main_value");
        }

        public T GetData<T>(string name)
        {
            return m_Data.GetData<T>($"{m_Prefix}_{name}");
        }

        public bool HasData<T>()
        {
            return HasData<T>($"{m_Prefix}_main_value");
        }

        public bool HasData<T>(string name)
        {
            return m_Data.HasData<T>($"{m_Prefix}_{name}");
        }

        public void SetData<T>(T value)
        {
            SetData($"{m_Prefix}_main_value", value);
        }

        public void SetData<T>(string name, T value)
        {
            m_Data.SetData<T>($"{m_Prefix}_{name}", value);
        }
    }
}
