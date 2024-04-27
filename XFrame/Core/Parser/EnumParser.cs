﻿using System;
using System.ComponentModel;
using XFrame.Modules.Diagnotics;
using XFrame.Modules.Pools;

namespace XFrame.Core
{
    /// <summary>
    /// 枚举解析器
    /// </summary>
    /// <typeparam name="T">枚举类型</typeparam>
    public class EnumParser<T> : IParser<T>, ICanConfigLog where T : Enum
    {
        private IPoolModule m_Module;
        private T m_Value;

        /// <summary>
        /// 转换的值
        /// </summary>
        public T Value => m_Value;

        /// <summary>
        /// Log等级
        /// </summary>
        public LogLevel LogLv { get; set; }

        object IParser.Value => m_Value;

        int IPoolObject.PoolKey => default;

        /// <inheritdoc/>
        public string MarkName { get; set; }
        IPool IPoolObject.InPool { get; set; }

        /// <summary>
        /// 解析值
        /// </summary>
        /// <param name="pattern">文本</param>
        /// <returns>解析到的值</returns>
        public T Parse(string pattern)
        {
            if (!string.IsNullOrEmpty(pattern) && Enum.TryParse(typeof(T), pattern, out object value))
            {
                m_Value = (T)value;
            }
            else
            {
                InnerSetDefault();
                Log.Print(LogLv, "XFrame", $"EnumParser {typeof(T).Name} parse failure. {pattern}");
            }

            return m_Value;
        }

        private void InnerSetDefault()
        {
            DefaultValueAttribute attr = m_Module.Domain.TypeModule.GetAttribute<DefaultValueAttribute>(typeof(T));
            if (attr != null)
                m_Value = (T)attr.Value;
            else
                m_Value = default;
        }

        object IParser.Parse(string pattern)
        {
            return Parse(pattern);
        }

        /// <summary>
        /// 释放到池中s
        /// </summary>
        public void Release()
        {
            References.Release(this);
        }

        /// <summary>
        /// 返回字符串形式
        /// </summary>
        /// <returns>枚举字符串</returns>
        public override string ToString()
        {
            return m_Value.ToString();
        }

        /// <summary>
        /// 获取枚举哈希值
        /// </summary>
        /// <returns>哈希值</returns>
        public override int GetHashCode()
        {
            return m_Value.GetHashCode();
        }

        /// <summary>
        /// 检查两个值是否相等
        /// </summary>
        /// <param name="obj">对比值</param>
        /// <returns>true表示相等</returns>
        public override bool Equals(object obj)
        {
            IParser parser = obj as IParser;
            return parser != null ? m_Value.Equals(parser.Value) : m_Value.Equals(obj);
        }

        void IPoolObject.OnCreate()
        {
            IPoolObject poolObj = this;
            m_Module = poolObj.InPool.Module;
        }

        void IPoolObject.OnRequest()
        {
            LogLv = LogLevel.Warning;
            InnerSetDefault();
        }

        void IPoolObject.OnRelease()
        {

        }

        void IPoolObject.OnDelete()
        {

        }

        /// <summary>
        /// 检查两个值是否相等
        /// </summary>
        /// <param name="src">枚举解析器</param>
        /// <param name="tar">对比值</param>
        /// <returns>true表示相等</returns>
        public static bool operator ==(EnumParser<T> src, object tar)
        {
            if (ReferenceEquals(src, null))
            {
                return ReferenceEquals(tar, null);
            }
            else
            {
                return src.Equals(tar);
            }
        }

        /// <summary>
        /// 检查两个值是否不相等
        /// </summary>
        /// <param name="src"></param>
        /// <param name="tar"></param>
        /// <returns>true表示不相等</returns>
        public static bool operator !=(EnumParser<T> src, object tar)
        {
            if (ReferenceEquals(src, null))
            {
                return !ReferenceEquals(tar, null);
            }
            else
            {
                return !src.Equals(tar);
            }
        }

        /// <summary>
        /// 将枚举转换为解析器
        /// </summary>
        /// <param name="value">枚举值</param>
        public static implicit operator EnumParser<T>(T value)
        {
            EnumParser<T> parser = References.Require<EnumParser<T>>();
            parser.m_Value = value;
            return parser;
        }

        /// <summary>
        /// 返回枚举解析器的值
        /// </summary>
        /// <param name="value">枚举值</param>
        public static implicit operator T(EnumParser<T> value)
        {
            return value != null ? value.m_Value : default;
        }
    }
}
