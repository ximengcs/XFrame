using XFrame.Core;
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

        public Csv(string content)
        {
            m_Raw = content;
            InnerInit();
        }

        private void InnerInit()
        {
            string[] lines = m_Raw.Split('\n');
            m_Row = lines.Length;
            m_Data = new string[m_Row][];

            for (int i = 0; i < m_Row; i++)
            {
                MatchCollection matchs = Regex.Matches(lines[i], CSV_PATTERN);
                int columnCount = matchs.Count;
                if (m_Column < columnCount)
                    m_Column = columnCount;

                string[] columns = new string[columnCount];
                for (int j = 0; j < columnCount; j++)
                    columns[j] = matchs[j].Groups[REQUIRE].Value;
                m_Data[i] = columns;
            }
        }
    }
}
