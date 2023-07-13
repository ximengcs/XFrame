using XFrame.Modules.Event;
using XFrame.Modules.Pools;

namespace XFrame.Modules.Local
{
    public class LanguageChangeEvent : XEvent
    {
        private static int s_EventId;
        public static int EventId
        {
            get
            {
                if (s_EventId == default)
                    s_EventId = typeof(LanguageChangeEvent).GetHashCode();
                return s_EventId;
            }
        }

        public Language Old { get; private set; }
        public Language New { get; private set; }

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
