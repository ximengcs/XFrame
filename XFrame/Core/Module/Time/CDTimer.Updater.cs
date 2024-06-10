
namespace XFrame.Modules.Times
{
    public partial class CDTimer
    {
        /// <summary>
        /// CD计时器的更新器
        /// </summary>
        public interface IUpdater
        {
            /// <summary>
            /// 时间
            /// </summary>
            double Time { get; }
        }

        #region Default
        private static IUpdater m_Default;

        /// <summary>
        /// 获取默认更新器
        /// </summary>
        /// <param name="module"></param>
        /// <returns></returns>
        public static IUpdater GetDeaultUpdater(ITimeModule module)
        {
            if (m_Default == null)
                m_Default = new DefaultTimer(module);
            return m_Default;
        }

        /// <summary>
        /// 默认更新器
        /// </summary>
        public class DefaultTimer : IUpdater
        {
            private ITimeModule m_Module;

            double IUpdater.Time => m_Module.Time;

            /// <summary>
            /// 构造器
            /// </summary>
            /// <param name="module">所属模块</param>
            public DefaultTimer(ITimeModule module)
            {
                m_Module = module;
            }
        }
        #endregion
    }
}
