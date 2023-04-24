
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
        public static IUpdater Default
        {
            get
            {
                if (m_Default == null)
                    m_Default = new DefaultTimer();
                return m_Default;
            }
        }

        public class DefaultTimer : IUpdater
        {
            float IUpdater.Time => TimeModule.Inst.Time;
        }
        #endregion
    }
}
