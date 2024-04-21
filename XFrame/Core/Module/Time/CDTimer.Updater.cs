
using XFrame.Core;

namespace XFrame.Modules.Times
{
    public partial class CDTimer
    {
        public interface IUpdater
        {
            float Time { get; }
        }

        #region Default
        private static IUpdater m_Default;
        public static IUpdater GetDeaultUpdater(ITimeModule module)
        {
            if (m_Default == null)
                m_Default = new DefaultTimer(module);
            return m_Default;
        }

        public class DefaultTimer : IUpdater
        {
            private ITimeModule m_Module;

            float IUpdater.Time => m_Module.Time;

            public DefaultTimer(ITimeModule module)
            {
                m_Module = module;
            }
        }
        #endregion
    }
}
