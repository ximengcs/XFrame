using System;
using XFrame.Core;
using XFrame.Collections;
using XFrame.Modules.Event;
using XFrame.Modules.Diagnotics;
using System.Collections.Generic;
using System.Reflection;
using XFrame.Modules.Pools;

namespace XFrame.Modules.Local
{
    /// <summary>
    /// 本地化模块
    /// </summary>
    [CoreModule]
    [RequireModule(typeof(EventModule))]
    public partial class LocalizeModule : SingletonModule<LocalizeModule>
    {
        #region Inner Fileds
        private int m_Index;
        private Csv<string> m_Data;
        private Language m_Language;
        private IEventSystem m_Event;
        private Dictionary<int, int> m_IdMap;
        private FormatterProvider m_Formatter;
        private ArrayParser<EnumParser<Language>> m_Title;
        #endregion

        #region Life Fun
        protected override void OnInit(object data)
        {
            base.OnInit(data);

            m_Language = Language.None;
            m_Event = EventModule.Inst.NewSys();
            m_Formatter = new FormatterProvider();
        }
        #endregion

        #region Interface
        /// <summary>
        /// 事件系统
        /// </summary>
        public IEventSystem Event => m_Event;

        /// <summary>
        /// 当前语言
        /// </summary>
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

        public ArrayParser<EnumParser<Language>> ExistLangs => m_Title;

        /// <summary>
        /// 是否存在语言
        /// </summary>
        /// <param name="language">语言</param>
        /// <returns>true为存在</returns>
        public bool HasLanguage(Language language)
        {
            return InnerGetLangIndex(language) != -1;
        }

        /// <summary>
        /// 配置
        /// </summary>
        /// <param name="content">文本内容</param>
        public void Parse(string content)
        {
            if (!string.IsNullOrEmpty(content))
            {
                string file = content;
                InnerInit(file);
                m_Index = InnerGetLangIndex(m_Language);
            }
        }

        /// <summary>
        /// 设置文本格式化器
        /// </summary>
        /// <param name="formatter">自定义格式化</param>
        public void SetFormater(ICustomFormatter formatter)
        {
            m_Formatter.SetFormatter(formatter);
        }

        /// <summary>
        /// 获取一整行
        /// </summary>
        /// <param name="key">Id</param>
        /// <returns>行</returns>
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

        /// <summary>
        /// 获取本地化值
        /// </summary>
        /// <param name="language">指定语言</param>
        /// <param name="key">Id</param>
        /// <param name="values">参数</param>
        /// <returns>值</returns>
        public string GetValue(Language language, int key, params object[] values)
        {
            int index = InnerGetLangIndex(language);
            return InnerGetValue(index, key, values);
        }

        /// <summary>
        /// 获取本地化值
        /// </summary>
        /// <param name="key">Id</param>
        /// <param name="values">参数</param>
        /// <returns>值</returns>
        public string GetValue(int key, params object[] values)
        {
            return InnerGetValue(m_Index, key, values);
        }

        /// <summary>
        /// 获取本地化值
        /// </summary>
        /// <param name="key">Id</param>
        /// <param name="args">参数Id</param>
        /// <returns>值</returns>
        public string GetValueParam(int key, params int[] args)
        {
            return InnerGetValueParam(m_Index, key, args);
        }

        /// <summary>
        /// 获取本地化值
        /// </summary>
        /// <param name="language">指定语言</param>
        /// <param name="key">Id</param>
        /// <param name="args">参数Id</param> 
        /// <returns>值</returns>
        public string GetValueParam(Language language, int key, params int[] args)
        {
            int index = InnerGetLangIndex(language);
            return InnerGetValueParam(index, key, args);
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
            m_Title = new ArrayParser<EnumParser<Language>>();
            m_Title.Parse(m_Data.Get(1));

            for (int i = 1; i <= m_Data.Row; i++)
            {
                string idStr = m_Data.Get(i)[0];
                if (!string.IsNullOrEmpty(idStr) && int.TryParse(idStr, out int id))
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
            int index = m_Title.IndexOf(language);
            if (index == -1)
                Log.Debug("XFrame", $"language map error. {language}");
            return index;
        }
        #endregion
    }
}
