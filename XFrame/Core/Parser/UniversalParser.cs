﻿using System;
using System.Collections.Generic;
using XFrame.Modules.Diagnotics;

namespace XFrame.Core
{
    public class UniversalParser : IParser<string>
    {
        private string m_Value;
        private int m_IntValue;
        private float m_FloatValue;
        private bool m_BoolValue;
        private Dictionary<Type, IParser> m_Parsers;

        public string Value => m_Value;
        public int IntValue => m_IntValue;
        public float FloatValue => m_FloatValue;
        public bool BoolValue => m_BoolValue;

        object IParser.Value => m_Value;

        public T GetOrAddParser<T>() where T : IParser
        {
            T parser = GetParser<T>();
            if (parser == null)
                parser = AddParser<T>();
            return parser;
        }

        public T GetParser<T>() where T : IParser
        {
            if (m_Parsers.TryGetValue(typeof(T), out IParser parser))
            {
                parser.Parse(m_Value);
                return (T)parser;
            }
            return default;
        }

        public T AddParser<T>(T parser) where T : IParser
        {
            if (parser != null)
            {
                Type type = typeof(T);
                parser.Parse(m_Value);
                if (m_Parsers.TryGetValue(type, out IParser oldParser))
                {
                    Log.Warning("XFrame", $"Universal parser already has parser {type.Name}.");
                    m_Parsers[type] = parser;
                }
                else
                {
                    m_Parsers.Add(type, parser);
                }
            }
            return parser;
        }

        public T AddParser<T>() where T : IParser
        {
            Type type = typeof(T);
            if (!m_Parsers.TryGetValue(typeof(T), out IParser parser))
            {
                parser = (IParser)Activator.CreateInstance(type);
                parser.Parse(m_Value);
                m_Parsers.Add(type, parser);
            }
            return (T)parser;
        }

        public UniversalParser()
        {
            m_Parsers = new Dictionary<Type, IParser>();
        }

        public UniversalParser(int value)
        {
            m_Parsers = new Dictionary<Type, IParser>();
            m_IntValue = value;
            m_Value = value.ToString();
            m_FloatValue = value;
            m_BoolValue = value == 0 ? false : true;
        }

        public UniversalParser(float value)
        {
            m_Parsers = new Dictionary<Type, IParser>();
            m_IntValue = (int)value;
            m_Value = value.ToString();
            m_FloatValue = value;
            m_BoolValue = value == 0 ? false : true;
        }

        public UniversalParser(bool value)
        {
            m_Parsers = new Dictionary<Type, IParser>();
            m_IntValue = value ? 1 : 0;
            m_Value = value.ToString();
            m_FloatValue = m_IntValue;
            m_BoolValue = value;
        }

        public UniversalParser(string value)
        {
            m_Parsers = new Dictionary<Type, IParser>();
            Parse(value);
        }

        public string Parse(string pattern)
        {
            m_Value = pattern;
            m_IntValue = ParserModule.Inst.INT.Parse(m_Value);
            m_FloatValue = ParserModule.Inst.FLOAT.Parse(m_Value);
            m_BoolValue = ParserModule.Inst.BOOL.Parse(m_Value);
            return m_Value;
        }

        object IParser.Parse(string pattern)
        {
            return Parse(pattern);
        }

        public override string ToString()
        {
            return Value.ToString();
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            IParser parser = obj as IParser;
            return parser != null ? Value.Equals(parser.Value) : Value.Equals(obj);
        }

        public static bool operator ==(UniversalParser src, object tar)
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

        public static bool operator !=(UniversalParser src, object tar)
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

        public static implicit operator string(UniversalParser parser)
        {
            return parser.Value;
        }

        public static implicit operator UniversalParser(string value)
        {
            UniversalParser parser = new UniversalParser();
            parser.Parse(value);
            return parser;
        }
    }
}
