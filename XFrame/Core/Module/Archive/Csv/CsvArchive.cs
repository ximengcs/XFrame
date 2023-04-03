using System.IO;
using XFrame.Collections;
using XFrame.Core;

namespace XFrame.Modules.Archives
{
    [Archive("csv")]
    public class CsvArchive : IArchive
    {
        #region Inner Fields
        private string m_Path;
        private Csv<string> m_Csv;
        #endregion

        #region Interface
        public Csv<string> Data => m_Csv;
        #endregion

        #region Archive Interface
        bool IArchive.Encrypt { get; set; }

        void IArchive.OnInit(string path)
        {
            m_Path = path;

            if (File.Exists(m_Path))
            {
                string text = File.ReadAllText(m_Path);
                m_Csv = new Csv<string>(text, ParserModule.Inst.STRING);
            }
            else
            {
                m_Csv = new Csv<string>();
            }
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
        #endregion
    }
}
