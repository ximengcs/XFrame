using XFrame.Modules.Event;

namespace XFrame.Modules.Local
{
    public class LanguageChangeEvent : XEvent
    {
        public static int EventId => typeof(LanguageChangeEvent).GetHashCode();

        public Language Old { get; }
        public Language New { get; }

        internal LanguageChangeEvent(Language oldLang, Language newLang) : base(EventId)
        {
            Old = oldLang;
            New = newLang;
        }
    }
}
