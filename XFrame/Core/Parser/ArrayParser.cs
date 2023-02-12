using System;
using System.Collections;
using System.Collections.Generic;

namespace XFrame.Core
{
    public class ArrayParser<T> : IEnumerable<T> where T : IParser
    {
        private const char SPLIT = ',';
        private T[] m_Patterns;

        public ArrayParser(string patterns)
        {
            string[] pArray = patterns.Split(SPLIT);
            m_Patterns = new T[pArray.Length];
            for (int i = 0; i < pArray.Length; i++)
            {
                T parser = Activator.CreateInstance<T>();
                parser.Init(pArray[i]);
                m_Patterns[i] = parser;
            }
        }

        public T this[int index] { get { return m_Patterns[index]; } }

        public int Count { get { return m_Patterns.Length; } }

        public bool Empty { get { return m_Patterns.Length == 0; } }

        public T Main { get { return m_Patterns[0]; } }

        public T Get(int index)
        {
            return m_Patterns[index];
        }

        public IEnumerator<T> GetEnumerator()
        {
            return (IEnumerator<T>)m_Patterns.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return m_Patterns.GetEnumerator();
        }
    }
}
