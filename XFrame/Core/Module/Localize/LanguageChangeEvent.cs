using XFrame.Modules.Event;
using XFrame.Modules.Pools;

namespace XFrame.Modules.Local
{
    /// <summary>
    /// 语言改变事件
    /// </summary>
    public class LanguageChangeEvent : XEvent
    {
        private static int s_EventId;

        /// <summary>
        /// 时间Id
        /// </summary>
        public static int EventId
        {
            get
            {
                if (s_EventId == default)
                    s_EventId = typeof(LanguageChangeEvent).GetHashCode();
                return s_EventId;
            }
        }

        /// <summary>
        /// 旧语言
        /// </summary>
        public Language Old { get; private set; }

        /// <summary>
        /// 新语言
        /// </summary>
        public Language New { get; private set; }

        /// <summary>
        /// 创建事件
        /// </summary>
        /// <param name="oldLang">旧语言</param>
        /// <param name="newLang">新语言</param>
        /// <returns>事件实例</returns>
        public static LanguageChangeEvent Create(Language oldLang, Language newLang)
        {
            LanguageChangeEvent evt = References.Require<LanguageChangeEvent>();
            evt.Id = EventId;
            evt.Old = oldLang;
            evt.New = newLang;
            return evt;
        }
    }
}
