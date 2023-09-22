
using System;
using XFrame.Core;
using XFrame.Modules.Event;

namespace XFrame.Modules.Local
{
    public interface ILocalizeModule : IModule
    {
        IEventSystem Event { get; }

        Language Lang { get; set; }

        ArrayParser<EnumParser<Language>> ExistLangs { get; }

        bool HasLanguage(Language language);

        void Parse(string content);

        void SetFormater(ICustomFormatter formatter);

        string[] GetLine(int key);

        string GetValue(Language language, int key, params object[] values);

        string GetValue(int key, params object[] values);

        string GetValue(Language language, LanguageParam param);

        string GetValue(LanguageParam param);

        string[] GetValues(int[] idList);

        string[] GetValues(Language language, int[] idList);

        string GetValueParam(int key, params int[] args);

        string GetValueParam(Language language, int key, params int[] args);

        string GetValueParam(LanguageIdParam param);

        string GetValueParam(Language language, LanguageIdParam param);
    }
}
