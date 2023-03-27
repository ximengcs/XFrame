using System.IO;
using XFrame.Core;
using XFrame.Collections;
using XFrame.Modules.Config;
using System.Collections.Generic;
using XFrame.Modules.Diagnotics;

namespace XFrame.Modules.Local
{
    /// <summary>
    /// 本地化模块
    /// </summary>
    [CoreModule]
    public class LocalizeModule : SingletonModule<LocalizeModule>
    {
        private int m_Index;
        private Csv<string> m_Data;
        private Language m_Language;
        private Dictionary<Language, int> m_LanguageIndex;

        protected override void OnInit(object data)
        {
            base.OnInit(data);

            if (!string.IsNullOrEmpty(XConfig.LocalizeFile))
            {
                if (File.Exists(XConfig.LocalizeFile))
                {
                    string file = File.ReadAllText(XConfig.LocalizeFile);
                    InnerInit(file, XConfig.Lang);
                }
            }
        }

        #region Interface
        public Language Lang
        {
            get { return m_Language; }
            set
            {
                if (m_Language != value)
                {
                    m_Language = value;
                    InnerRefreshLang();
                }
            }
        }

        /// <summary>
        /// 获取本地化值
        /// </summary>
        /// <param name="key">Id</param>
        /// <param name="values">参数</param>
        /// <returns>值</returns>
        public string GetValue(int key, params object[] values)
        {
            Csv<string>.Line line = m_Data.Get(key);
            string content = line[m_Index];
            return string.Format(content, values);
        }

        /// <summary>
        /// 获取本地化值
        /// </summary>
        /// <param name="key">Id</param>
        /// <param name="args">参数Id</param>
        /// <returns>值</returns>
        public string GetValueParam(int key, params int[] args)
        {
            Csv<string>.Line line = m_Data.Get(key);
            string content = line[m_Index];
            foreach (int id in args)
                content = string.Format(content, GetValue(id));
            return content;
        }
        #endregion

        private void InnerInit(string csvText, Language language)
        {
            m_LanguageIndex = new Dictionary<Language, int>();
            m_Data = new Csv<string>(csvText, ParserModule.Inst.STRING);
            Csv<string>.Line line = m_Data.Get(0);
            EnumParser<Language> parser = new EnumParser<Language>();
            for (int i = 0; i < line.Count; i++)
            {
                Language lang = parser.Parse(line[i]);
                m_LanguageIndex[lang] = i;
            }
            Lang = language;
        }

        private void InnerRefreshLang()
        {
            if (!m_LanguageIndex.TryGetValue(m_Language, out m_Index))
                Log.Debug("XFrame", "language map error.");
        }
    }
}
