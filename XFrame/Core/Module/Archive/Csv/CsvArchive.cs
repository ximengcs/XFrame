using System.IO;
using XFrame.Collections;

namespace XFrame.Modules.Archives
{
    [Archive("csv")]
    public class CsvArchive : IArchive
    {
        private string m_Path;
        private Csv<string> m_Csv;

        public Csv<string> Data => m_Csv;

        void IArchive.OnInit(string path)
        {
            m_Path = path;
        }

        public void Delete()
        {
            if (File.Exists(m_Path))
                File.Delete(m_Path);
        }

        public void Save()
        {
            File.WriteAllText(m_Path, m_Csv.ToString());
        }
    }
}
