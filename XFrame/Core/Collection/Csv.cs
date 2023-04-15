using System;
using System.Text;
using XFrame.Core;
using XFrame.Modules.Diagnotics;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace XFrame.Collections
{
    /// <summary>
    /// CSV
    /// </summary>
    /// <typeparam name="T">数据类型</typeparam>
    public partial class Csv<T> : IXEnumerable<Csv<T>.Line>
    {
        #region Inner Fields
        private int m_Row;
        private int m_Column;

        private XLinkList<Line> m_Lines;
        private Dictionary<int, XLinkNode<Line>> m_LinesWithIndex;
        #endregion

        #region Const Fields
        private const int DEFAULT_COLUMN = 8;
        private const int REQUIRE = 2;
        private const string CSV_PATTERN = "(?:^|,)(?=[^\"]|(\")?)\"?((?(1)[^\"]*|[^,\"]*))\"?(?=,|$)";
        #endregion

        #region Constructor
        /// <summary>
        /// 构造一个 <paramref name="column"/> 列的Csv
        /// </summary>
        /// <param name="column"></param>
        public Csv(int column = DEFAULT_COLUMN)
        {
            m_Column = column;
            m_Lines = new XLinkList<Line>();
            m_LinesWithIndex = new Dictionary<int, XLinkNode<Line>>();
        }

        /// <summary>
        /// 通过 <paramref name="content"/> 构造Csv
        /// </summary>
        /// <param name="content">Csv文本内容</param>
        /// <param name="parser">解析器</param>
        public Csv(string content, IParser<T> parser)
        {
            m_Lines = new XLinkList<Line>();
            m_LinesWithIndex = new Dictionary<int, XLinkNode<Line>>();
            InnerInit(content, parser);
        }
        #endregion

        #region Interface
        /// <summary>
        /// 行
        /// </summary>
        public int Row => m_Row;

        /// <summary>
        /// 列
        /// </summary>
        public int Column => m_Column;

        /// <summary>
        /// 在尾部添加一行
        /// </summary>
        /// <returns>行数据</returns>
        public Line Add()
        {
            m_Row++;
            Line line = new Line(m_Column);
            XLinkNode<Line> node = m_Lines.AddLast(line);
            m_LinesWithIndex.Add(m_Row, node);
            return line;
        }

        /// <summary>
        /// 在第 <paramref name="row"/> 行之前插入一行
        /// </summary>
        /// <param name="row">行</param>
        /// <returns>行数据</returns>
        public Line Insert(int row)
        {
            return Insert(row, new Line(m_Column));
        }

        /// <summary>
        /// 在第 <paramref name="row"/> 行之前插入 <paramref name="line"/> 数据行
        /// </summary>
        /// <param name="row">行</param>
        /// <param name="line">行数据</param>
        /// <returns>行数据</returns>
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

        /// <summary>
        /// 删除第 <paramref name="row"/> 行数据
        /// </summary>
        /// <param name="row">行</param>
        public void Delete(int row)
        {
            if (m_LinesWithIndex.TryGetValue(row, out XLinkNode<Line> node))
            {
                node.Delete();
                InnerRefreshMap();
            }
        }

        /// <summary>
        /// 获取第 <paramref name="row"/> 行数据
        /// </summary>
        /// <param name="row">行</param>
        /// <returns>行数据</returns>
        public Line Get(int row)
        {
            if (row < 0)
                Log.Debug("CSV", $"get csv data error.row out of bounds. cur {row} max {m_Row}");
            return m_LinesWithIndex[row].Value;
        }

        /// <summary>
        /// 获取第 <paramref name="row"/> 行第 <paramref name="column"/> 列的数据
        /// </summary>
        /// <param name="row">行</param>
        /// <param name="column">列</param>
        /// <returns>数据</returns>
        public T Get(int row, int column)
        {
            if (row < 0 || row >= m_Row)
                Log.Debug("CSV", $"get csv data error.row out of bounds. cur {row} max {m_Row}");
            Line rowContent = m_LinesWithIndex[row].Value;
            if (column < 0 | column >= rowContent.Count)
                Log.Debug("CSV", $"get csv data error.column out of bounds. cur {column} max {rowContent.Count}");
            return rowContent[column];
        }
        #endregion


        #region IXEnumerable Interface
        public IEnumerator<Line> GetEnumerator()
        {
            return new Enumerator(m_Lines);
        }

        public void SetIt(XItType type)
        {
            m_Lines.SetIt(type);
        }
        #endregion

        #region Inner Implement
        private void InnerRefreshMap()
        {
            int column = 1;
            m_LinesWithIndex.Clear();
            foreach (XLinkNode<Line> node in m_Lines)
                m_LinesWithIndex.Add(column++, node);
        }

        private void InnerInit(string raw, IParser<T> parser)
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
                    line[j] = parser.Parse(value);
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
        #endregion

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (XLinkNode<Line> node in m_Lines)
            {
                sb.Append(node.Value.ToString());
                if (node.Next != null)
                    sb.Append('\n');
            }
            return sb.ToString();
        }

        public static implicit operator string(Csv<T> csv)
        {
            return csv.ToString();
        }
    }
}
