using System.IO;
using XFrame.Core;
using XFrame.Modules.Config;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace XFrame.Modules.Local
{
    /// <summary>
    /// 本地化模块
    /// </summary>
    [CoreModule]
    public class LocalizeModule : SingletonModule<LocalizeModule>
    {
        private const int REQUIRE = 2;
        private const string CSV_PATTERN = "(?:^|,)(?=[^\"]|(\")?)\"?((?(1)[^\"]*|[^,\"]*))\"?(?=,|$)";

        private Language m_Language;
        private Dictionary<int, string> m_Content;

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
        /// <summary>
        /// 获取本地化值
        /// </summary>
        /// <param name="key">Id</param>
        /// <param name="values">参数</param>
        /// <returns>值</returns>
        public string GetValue(int key, params object[] values)
        {
            if (m_Content.ContainsKey(key))
                return string.Format(m_Content[key], values);
            else
                return string.Empty;
        }

        /// <summary>
        /// 获取本地化值
        /// </summary>
        /// <param name="key">Id</param>
        /// <param name="args">参数Id</param>
        /// <returns>值</returns>
        public string GetValueParam(int key, params int[] args)
        {
            if (m_Content.ContainsKey(key))
            {
                string result = m_Content[key];
                foreach (int id in args)
                    result = string.Format(result, GetValue(id));
                return result;
            }
            else
                return default;
        }
        #endregion

        private void InnerInit(string csv, Language language)
        {
            m_Language = language;
            m_Content = new Dictionary<int, string>();
            InnerAnalyze(csv);
        }

        private void InnerAnalyze(string csv)
        {
            string[] lines = csv.Split('\n');
            MatchCollection languages = Regex.Matches(lines[0], CSV_PATTERN);
            int langIndex = 1;
            for (int i = langIndex; i < languages.Count; i++)
            {
                string value = languages[i].Groups[REQUIRE].Value;
                if (value.StartsWith(m_Language.ToString()))
                {
                    langIndex = i;
                    break;
                }
            }

            if (langIndex > 0)
            {
                for (int i = 1; i < lines.Length; i++)
                {
                    if (lines[i].Length < 2)
                        continue;
                    MatchCollection matchs = Regex.Matches(lines[i], CSV_PATTERN);
                    if (matchs.Count < 2)
                        continue;

                    string numStr = matchs[0].Groups[REQUIRE].Value;
                    string lanStr = matchs[langIndex].Groups[REQUIRE].Value;
                    m_Content.Add(IntParser.Parse(numStr), lanStr);
                }
            }
        }
    }
}
