using XFrame.Core;
using System.Collections.Generic;

namespace XFrame.Modules
{
    public class PlotDataBinder : IDataProvider
    {
        private JsonArchive m_Persist;

        public PlotDataBinder(JsonArchive data)
        {
            m_Persist = data;
            Steps = m_Persist.GetArray<bool>(nameof(Steps));
            Finish = new ValueBinder<bool>(
                () => m_Persist.Get<bool>(nameof(Finish)), 
                (v) => m_Persist.Set(nameof(Finish), v));
        }

        public ValueBinder<bool> Finish { get; set; }
        public List<bool> Steps { get; set; }

        public void Dispose()
        {
            throw new System.NotImplementedException();
        }

        public void EnsureStepData(int index)
        {
            if (Steps.Count <= index)
                Steps.Add(false);
        }

        public T GetData<T>() where T : class
        {
            return default;
        }

        public T GetData<T>(string name) where T : class
        {
            return default;
        }

        public void SetData<T>(T value) where T : class
        {

        }

        public void SetData<T>(string name, T value) where T : class
        {

        }
    }
}
