using XFrame.Modules.Diagnotics;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace XFrame.Collections
{
    public class Csv
    {
        private string m_Raw;
        private int m_Row;
        private int m_Column;
        private string[][] m_Data;

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
            string[] rowContent = m_Data[row];
            if (column < 0 | column >= rowContent.Length)
                Log.Debug("CSV", $"get csv data error.column out of bounds. cur {column} max {rowContent.Length}");
            return rowContent[column];
        }

        private void InnerInit()
        {
            string[] lines = m_Raw.Split('\n');
            m_Row = lines.Length;

            List<string[]> rows = new List<string[]>(m_Row);
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

                string[] columns = new string[columnCount];
                for (int j = 0; j < columnCount; j++)
                {
                    string value = matchs[j].Groups[REQUIRE].Value;
                    value = value.Trim(' ', '\n', '\t', '\r');
                    columns[j] = value;
                }
                rows.Add(columns);
            }
            m_Data = rows.ToArray();
        }
    }
}
