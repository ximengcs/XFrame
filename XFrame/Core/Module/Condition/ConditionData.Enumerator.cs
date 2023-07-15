﻿
using System.Collections;
using System.Collections.Generic;
using XFrame.Collections;
using XFrame.Core;

namespace XFrame.Modules.Conditions
{
    public partial struct ConditionData
    {
        private struct Enumerator : IEnumerator<PairParser<IntParser, UniversalParser>>
        {
            private IEnumerator<XLinkNode<PairParser<IntParser, UniversalParser>>> m_It;

            public PairParser<IntParser, UniversalParser> Current => m_It.Current.Value;

            object IEnumerator.Current => Current;

            public Enumerator(ConditionData data)
            {
                m_It = data.Parser.Value.GetEnumerator();
            }

            public bool MoveNext()
            {
                return m_It.MoveNext();
            }

            public void Reset()
            {
                m_It.Reset();
            }

            public void Dispose()
            {
                m_It.Dispose();
            }

        }
    }
}
