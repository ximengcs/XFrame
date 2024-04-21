﻿using System;
using System.ComponentModel;
using XFrame.Modules.Diagnotics;
using XFrame.Modules.Pools;

namespace XFrame.Core
{
    public class EnumParser<T> : IParser<T>, ICanConfigLog where T : Enum
    {
        private IPoolModule m_Module;
        private T m_Value;
        public T Value => m_Value;
        public LogLevel LogLv { get; set; }

        object IParser.Value => m_Value;

        int IPoolObject.PoolKey => default;
        public string MarkName { get; set; }
        IPool IPoolObject.InPool { get; set; }

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

        public void Release()
        {
            References.Release(this);
        }

        public override string ToString()
        {
            return m_Value.ToString();
        }

        public override int GetHashCode()
        {
            return m_Value.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            IParser parser = obj as IParser;
            return parser != null ? m_Value.Equals(parser.Value) : m_Value.Equals(obj);
        }

        void IPoolObject.OnCreate(IPoolModule module)
        {
            m_Module = module;
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

        public static implicit operator EnumParser<T>(T value)
        {
            EnumParser<T> parser = References.Require<EnumParser<T>>();
            parser.m_Value = value;
            return parser;
        }

        public static implicit operator T(EnumParser<T> value)
        {
            return value != null ? value.m_Value : default;
        }
    }
}
