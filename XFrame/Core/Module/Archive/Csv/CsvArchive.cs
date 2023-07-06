using System.IO;
using XFrame.Core;
using XFrame.Collections;
using System.Xml.Linq;

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

        public string Name { get; private set; }
        #endregion

        #region Archive Interface
        void IArchive.OnInit(string path, string name, object param)
        {
            Name = name;
            m_Path = path;
            int column = 0;
            if (param != null)
                column = (int)param;
            if (File.Exists(m_Path))
            {
                string text = ArchiveUtility.ReadText(m_Path);
                m_Csv = new Csv<string>(text, ParserModule.Inst.STRING);
            }
            else
            {
                if (column > 0)
                    m_Csv = new Csv<string>(column);
                else
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
            ArchiveUtility.WriteText(m_Path, m_Csv.ToString());
        }
        #endregion
    }
}
