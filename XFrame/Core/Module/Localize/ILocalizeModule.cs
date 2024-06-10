using System;
using XFrame.Core;
using XFrame.Modules.Event;

namespace XFrame.Modules.Local
{
    /// <summary>
    /// 本地化模块
    /// </summary>
    public interface ILocalizeModule : IModule
    {
        /// <summary>
        /// 事件系统
        /// </summary>
        IEventSystem Event { get; }

        /// <summary>
        /// 当前语言
        /// </summary>
        Language Lang { get; set; }

        /// <summary>
        /// 存在的语言
        /// </summary>
        Language[] ExistLangs { get; }

        /// <summary>
        /// 是否存在语言
        /// </summary>
        /// <param name="language">语言</param>
        /// <returns>true为存在</returns>
        bool HasLanguage(Language language);

        /// <summary>
        /// 配置
        /// </summary>
        /// <param name="content">文本内容</param>
        void Parse(string content);

        /// <summary>
        /// 设置文本格式化器
        /// </summary>
        /// <param name="formatter">自定义格式化</param>
        void SetFormater(ICustomFormatter formatter);

        /// <summary>
        /// 获取一整行
        /// </summary>
        /// <param name="key">Id</param>
        /// <returns>行</returns>
        string[] GetLine(int key);

        /// <summary>
        /// 获取本地化值
        /// </summary>
        /// <param name="language">指定语言</param>
        /// <param name="key">Id</param>
        /// <param name="values">参数</param>
        /// <returns>值</returns>
        string GetValue(Language language, int key, params object[] values);

        /// <summary>
        /// 获取本地化值
        /// </summary>
        /// <param name="key">Id</param>
        /// <param name="values">参数</param>
        /// <returns>值</returns>
        string GetValue(int key, params object[] values);

        /// <summary>
        /// 获取本地化值
        /// </summary>
        /// <param name="language">语言</param>
        /// <param name="param">参数</param>
        /// <returns>值</returns>
        string GetValue(Language language, LanguageParam param);

        /// <summary>
        /// 获取本地化值
        /// </summary>
        /// <param name="param">参数</param>
        /// <returns>值</returns>
        string GetValue(LanguageParam param);

        /// <summary>
        /// 获取本地化值
        /// </summary>
        /// <param name="idList">语言Id列表</param>
        /// <returns>值列表</returns>
        string[] GetValues(int[] idList);

        /// <summary>
        /// 获取本地化值
        /// </summary>
        /// <param name="language">语言</param>
        /// <param name="idList">语言Id列表</param>
        /// <returns>值列表</returns>
        string[] GetValues(Language language, int[] idList);

        /// <summary>
        /// 获取本地化值
        /// </summary>
        /// <param name="key">Id</param>
        /// <param name="args">参数Id</param>
        /// <returns>值</returns>
        string GetValueParam(int key, params int[] args);

        /// <summary>
        /// 获取本地化值
        /// </summary>
        /// <param name="language">指定语言</param>
        /// <param name="key">Id</param>
        /// <param name="args">参数Id</param> 
        /// <returns>值</returns>
        string GetValueParam(Language language, int key, params int[] args);

        /// <summary>
        /// 获取本地化值
        /// </summary>
        /// <param name="param">参数</param>
        /// <returns>值</returns>
        string GetValueParam(LanguageIdParam param);

        /// <summary>
        /// 获取本地化值
        /// </summary>
        /// <param name="language">语言</param>
        /// <param name="param">参数</param>
        /// <returns>值</returns>
        string GetValueParam(Language language, LanguageIdParam param);
    }
}
