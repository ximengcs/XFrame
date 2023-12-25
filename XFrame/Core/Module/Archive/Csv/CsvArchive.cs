using System.IO;
using XFrame.Core;
using XFrame.Collections;
using System.Xml.Linq;
using XFrame.Modules.Pools;

namespace XFrame.Modules.Archives
{
    [Archive("csv")]
    public class CsvArchive : ArchiveBase, IArchive
    {
        #region Inner Fields
        private string m_Path;
        private Csv<string> m_Csv;
        #endregion

        #region Interface
        public Csv<string> Data => m_Csv;
        #endregion

        #region Archive Interface
        protected internal override void OnInit(string path, string name, object param)
        {
            Name = name;
            m_Path = path;
            int column = 0;
            if (param != null)
                column = (int)param;
            if (File.Exists(m_Path))
            {
                string text = ArchiveUtility.ReadText(m_Path);
                m_Csv = new Csv<string>(text, References.Require<StringParser>());
            }
            else
            {
                if (column > 0)
                    m_Csv = new Csv<string>(column);
                else
                    m_Csv = new Csv<string>();
            }
        }

        public override void Delete()
        {
            if (File.Exists(m_Path))
                File.Delete(m_Path);
        }

        public override void Save()
        {
            ArchiveUtility.WriteText(m_Path, m_Csv.ToString());
        }
        #endregion
    }
}
