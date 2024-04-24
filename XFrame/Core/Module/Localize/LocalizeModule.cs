using System;
using XFrame.Core;
using XFrame.Collections;
using XFrame.Modules.Event;
using XFrame.Modules.Diagnotics;
using System.Collections.Generic;
using XFrame.Modules.Pools;

namespace XFrame.Modules.Local
{
    /// <inheritdoc/>
    [CoreModule]
    [RequireModule(typeof(EventModule))]
    [XType(typeof(ILocalizeModule))]
    public partial class LocalizeModule : ModuleBase, ILocalizeModule
    {
        #region Inner Fileds
        private int m_Index;
        private Csv<string> m_Data;
        private Language m_Language;
        private IEventSystem m_Event;
        private Dictionary<int, int> m_IdMap;
        private FormatterProvider m_Formatter;
        private Language[] m_Title;
        #endregion

        #region Life Fun
        /// <inheritdoc/>
        protected override void OnInit(object data)
        {
            base.OnInit(data);

            m_Language = Language.None;
            m_Event = Domain.GetModule<IEventModule>().NewSys();
            m_Formatter = new FormatterProvider();
        }
        #endregion

        #region Interface
        /// <inheritdoc/>
        public IEventSystem Event => m_Event;

        /// <inheritdoc/>
        public Language Lang
        {
            get { return m_Language; }
            set
            {
                if (m_Language != value)
                {
                    Language oldLang = m_Language;
                    m_Language = value;
                    m_Index = InnerGetLangIndex(m_Language);
                    m_Event.Trigger(LanguageChangeEvent.Create(oldLang, m_Language));
                }
            }
        }

        /// <inheritdoc/>
        public Language[] ExistLangs => m_Title;

        /// <inheritdoc/>
        public bool HasLanguage(Language language)
        {
            return InnerGetLangIndex(language) != -1;
        }

        /// <inheritdoc/>
        public void Parse(string content)
        {
            if (!string.IsNullOrEmpty(content))
            {
                string file = content;
                InnerInit(file);
                m_Index = InnerGetLangIndex(m_Language);
            }
        }

        /// <inheritdoc/>
        public void SetFormater(ICustomFormatter formatter)
        {
            m_Formatter.SetFormatter(formatter);
        }

        /// <inheritdoc/>
        public string[] GetLine(int key)
        {
            if (m_Data == null)
            {
                Log.Error("XFrame", $"data can not init finish. key {key}");
                return default;
            }

            if (InnerGetContentIndex(key, out int contentIndex))
            {
                Csv<string>.Line line = m_Data.Get(contentIndex);
                string[] lineContent = new string[line.Count - 1];
                for (int i = 1; i < line.Count; i++)
                    lineContent[i - 1] = line[i];
                return lineContent;
            }
            else
            {
                return default;
            }
        }

        /// <inheritdoc/>
        public string GetValue(Language language, int key, params object[] values)
        {
            int index = InnerGetLangIndex(language);
            return InnerGetValue(index, key, values);
        }

        /// <inheritdoc/>
        public string GetValue(int key, params object[] values)
        {
            return InnerGetValue(m_Index, key, values);
        }

        /// <inheritdoc/>
        public string GetValue(Language language, LanguageParam param)
        {
            int index = InnerGetLangIndex(language);
            return InnerGetValue(index, param.Id, param.Params);
        }

        /// <inheritdoc/>
        public string GetValue(LanguageParam param)
        {
            return InnerGetValue(m_Index, param.Id, param.Params);
        }

        /// <inheritdoc/>
        public string[] GetValues(int[] idList)
        {
            string[] result = new string[idList.Length];
            for (int i = 0; i < idList.Length; i++)
                result[i] = GetValue(idList[i]);
            return result;
        }

        /// <inheritdoc/>
        public string[] GetValues(Language language, int[] idList)
        {
            string[] result = new string[idList.Length];
            for (int i = 0; i < idList.Length; i++)
                result[i] = GetValue(language, idList[i]);
            return result;
        }

        /// <inheritdoc/>
        public string GetValueParam(int key, params int[] args)
        {
            return InnerGetValueParam(m_Index, key, args);
        }

        /// <inheritdoc/>
        public string GetValueParam(Language language, int key, params int[] args)
        {
            int index = InnerGetLangIndex(language);
            return InnerGetValueParam(index, key, args);
        }

        /// <inheritdoc/>
        public string GetValueParam(LanguageIdParam param)
        {
            return InnerGetValueParam(m_Index, param.Id, param.Params);
        }

        /// <inheritdoc/>
        public string GetValueParam(Language language, LanguageIdParam param)
        {
            int index = InnerGetLangIndex(language);
            return InnerGetValueParam(index, param.Id, param.Params);
        }
        #endregion

        #region Inner Imeplement
        private string InnerGetValue(int index, int key, params object[] values)
        {
            if (m_Data == null)
            {
                Log.Error("XFrame", $"data can not init finish. index {index}, key {key}");
                return default;
            }

            if (InnerGetContentIndex(key, out int contentIndex))
            {
                Csv<string>.Line line = m_Data.Get(contentIndex);
                string content = line[index];
                if (values == null || values.Length == 0)
                    return content;
                return string.Format(m_Formatter, content, values);
            }
            else
            {
                return default;
            }
        }

        private string InnerGetValueParam(int index, int key, params int[] args)
        {
            if (m_Data == null)
            {
                Log.Error("XFrame", $"data can not init finish. index {index}, key {key}");
                return default;
            }

            if (InnerGetContentIndex(key, out int contentIndex))
            {
                Csv<string>.Line line = m_Data.Get(contentIndex);
                string content = line[index];
                string[] param = new string[args.Length];
                for (int i = 0; i < args.Length; i++)
                    param[i] = GetValue(args[i]);

                if (args == null || args.Length == 0)
                    return content;
                return string.Format(m_Formatter, content, param);
            }
            else
            {
                return default;
            }
        }

        private void InnerInit(string csvText)
        {
            m_IdMap = new Dictionary<int, int>();
            m_Data = new Csv<string>(csvText, References.Require<StringParser>());
            ArrayParser<EnumParser<Language>> titleParser = References.Require<ArrayParser<EnumParser<Language>>>();
            titleParser.Parse(m_Data.Get(1));
            m_Title = new Language[titleParser.Count];
            int index = 0;
            foreach (var item in titleParser.Value)
                m_Title[index++] = item.Value;
            titleParser.Release();

            for (int i = 1; i <= m_Data.Row; i++)
            {
                string idStr = m_Data.Get(i)[0];
                if (!string.IsNullOrEmpty(idStr) && IntParser.TryParse(idStr, out int id))
                    m_IdMap.Add(id, i);
            }
        }

        private bool InnerGetContentIndex(int id, out int index)
        {
            if (m_IdMap.TryGetValue(id, out index))
                return true;
            else
            {
                index = default;
                Log.Error("XFrame", $"id {id} not index");
                return false;
            }
        }

        private int InnerGetLangIndex(Language language)
        {
            if (m_Title == null)
                return -1;
            int index = Array.IndexOf(m_Title, language);
            if (index == -1)
                Log.Debug("XFrame", $"language map error. {language}");
            return index;
        }
        #endregion
    }
}
