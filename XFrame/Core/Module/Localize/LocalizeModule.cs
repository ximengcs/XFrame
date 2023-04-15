﻿using System;
using System.IO;
using XFrame.Core;
using XFrame.Collections;
using XFrame.Modules.Config;
using XFrame.Modules.Diagnotics;

namespace XFrame.Modules.Local
{
    /// <summary>
    /// 本地化模块
    /// </summary>
    [CoreModule]
    public partial class LocalizeModule : SingletonModule<LocalizeModule>
    {
        #region Inner Fileds
        private int m_Index;
        private Csv<string> m_Data;
        private Language m_Language;
        private FormatterProvider m_Formatter;
        private ArrayParser<EnumParser<Language>> m_Title;
        #endregion

        #region Life Fun
        protected override void OnInit(object data)
        {
            base.OnInit(data);

            m_Formatter = new FormatterProvider();
            if (!string.IsNullOrEmpty(XConfig.LocalizeFile))
            {
                if (File.Exists(XConfig.LocalizeFile))
                {
                    string file = File.ReadAllText(XConfig.LocalizeFile);
                    InnerInit(file, XConfig.Lang);
                }
            }
        }
        #endregion

        #region Interface
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
                    m_Language = value;
                    InnerRefreshLang();
                }
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
        /// 获取本地化值
        /// </summary>
        /// <param name="key">Id</param>
        /// <param name="values">参数</param>
        /// <returns>值</returns>
        public string GetValue(int key, params object[] values)
        {
            Csv<string>.Line line = m_Data.Get(key);
            string content = line[m_Index];
            return string.Format(m_Formatter, content, values);
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
            string[] param = new string[args.Length];
            for (int i = 0; i < args.Length; i++)
                param[i] = GetValue(args[i]);

            return string.Format(m_Formatter, content, param);
        }
        #endregion

        #region Inner Imeplement
        private void InnerInit(string csvText, Language language)
        {
            m_Data = new Csv<string>(csvText, ParserModule.Inst.STRING);
            m_Title = new ArrayParser<EnumParser<Language>>();
            m_Title.Parse(m_Data.Get(1));
            Lang = language;
        }

        private void InnerRefreshLang()
        {
            m_Index = m_Title.IndexOf(m_Language);
            if (m_Index == -1)
                Log.Debug("XFrame", "language map error.");
        }
        #endregion
    }
}
