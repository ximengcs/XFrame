using System;
using XFrame.Modules.Diagnotics;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace XFrame.Collections
{
    public partial class Csv : IXEnumerable<Csv.Line>
    {
        private int m_Row;
        private int m_Column;

        private XLinkList<Line> m_Lines;
        private Dictionary<int, XLinkNode<Line>> m_LinesWithIndex;

        private const int REQUIRE = 2;
        private const string CSV_PATTERN = "(?:^|,)(?=[^\"]|(\")?)\"?((?(1)[^\"]*|[^,\"]*))\"?(?=,|$)";

        public int Row => m_Row;
        public int Column => m_Column;

        public Csv()
        {
            m_Lines = new XLinkList<Line>();
            m_LinesWithIndex = new Dictionary<int, XLinkNode<Line>>();
        }

        public Csv(string content)
        {
            m_Lines = new XLinkList<Line>();
            m_LinesWithIndex = new Dictionary<int, XLinkNode<Line>>();
            InnerInit(content);
        }

        public Line Add()
        {
            m_Row++;
            Line line = new Line(m_Column);
            XLinkNode<Line> node = m_Lines.AddLast(line);
            m_LinesWithIndex.Add(m_Row, node);
            return line;
        }

        public Line Insert(int row)
        {
            return Insert(row, new Line(m_Column));
        }

        public Line Insert(int row, Line line)
        {
            if (line.Count != m_Column)
            {
                Log.Error("XFrame", $"csv inset column error.");
                return line;
            }

            row = Math.Clamp(row, 1, m_Row);
            if (m_Lines.Empty)
            {
                m_Lines.AddLast(line);
            }
            else
            {
                XLinkNode<Line> node = m_LinesWithIndex[row];
                node.AddBefore(line);
            }

            InnerRefreshMap();
            return line;
        }

        public void Delete(int row)
        {
            if (m_LinesWithIndex.TryGetValue(row, out XLinkNode<Line> node))
            {
                node.Delete();
                InnerRefreshMap();
            }
        }

        public string Get(int row, int column)
        {
            if (row < 0 || row >= m_Row)
                Log.Debug("CSV", $"get csv data error.row out of bounds. cur {row} max {m_Row}");
            Line rowContent = m_LinesWithIndex[row].Value;
            if (column < 0 | column >= rowContent.Count)
                Log.Debug("CSV", $"get csv data error.column out of bounds. cur {column} max {rowContent.Count}");
            return rowContent[column];
        }

        private void InnerRefreshMap()
        {
            int column = 1;
            m_LinesWithIndex.Clear();
            foreach (XLinkNode<Line> node in m_Lines)
                m_LinesWithIndex.Add(column++, node);
        }

        private void InnerInit(string raw)
        {
            string[] lines = raw.Split('\n');
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

            for (int i = 0; i < rows.Count; i++)
            {
                Line line = rows[i];
                XLinkNode<Line> node = m_Lines.AddLast(line);
                m_LinesWithIndex.Add(i + 1, node);
            }
        }

        public IEnumerator<Line> GetEnumerator()
        {
            return new Enumerator(m_Lines);
        }

        public void SetIt(XItType type)
        {
            m_Lines.SetIt(type);
        }
    }
}
