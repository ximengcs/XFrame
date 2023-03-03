using XFrame.Modules.Diagnotics;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace XFrame.Collections
{
    public partial class Csv : IXEnumerable<Csv.Line>
    {
        private string m_Raw;
        private int m_Row;
        private int m_Column;
        private Line[] m_Data;
        private XItType m_ItType;

        private const int REQUIRE = 2;
        private const string CSV_PATTERN = "(?:^|,)(?=[^\"]|(\")?)\"?((?(1)[^\"]*|[^,\"]*))\"?(?=,|$)";

        public int Row => m_Row;
        public int Column => m_Column;

        public Csv(string content)
        {
            m_Raw = content;
            InnerInit();
        }

        public string Get(int row, int column)
        {
            if (row < 0 || row >= m_Row)
                Log.Debug("CSV", $"get csv data error.row out of bounds. cur {row} max {m_Row}");
            Line rowContent = m_Data[row];
            if (column < 0 | column >= rowContent.Count)
                Log.Debug("CSV", $"get csv data error.column out of bounds. cur {column} max {rowContent.Count}");
            return rowContent[column];
        }

        private void InnerInit()
        {
            string[] lines = m_Raw.Split('\n');
            m_Row = lines.Length;

            List<Line> rows = new List<Line>(m_Row);
            for (int i = 0; i < m_Row; i++)
            {
                string content = lines[i];
                if (string.IsNullOrEmpty(content))
                {
                    m_Row--;
                    continue;
                }
                MatchCollection matchs = Regex.Matches(content, CSV_PATTERN);
                int columnCount = matchs.Count;
                if (m_Column < columnCount)
                    m_Column = columnCount;

                Line line = new Line(columnCount);
                for (int j = 0; j < columnCount; j++)
                {
                    string value = matchs[j].Groups[REQUIRE].Value;
                    value = value.Trim(' ', '\n', '\t', '\r');
                    line[j] = value;
                }
                rows.Add(line);
            }
            m_Data = rows.ToArray();
        }

        public IEnumerator<Line> GetEnumerator()
        {
            switch (m_ItType)
            {
                case XItType.Forward: return new ForwardIt(m_Data);
                case XItType.Backward: return new BackwardIt(m_Data);
                default: return default;
            }
        }

        public void SetIt(XItType type)
        {
            m_ItType = type;
        }
    }
}
