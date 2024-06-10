using System.IO;
using XFrame.Core;
using XFrame.Collections;
using XFrame.Modules.Pools;

namespace XFrame.Modules.Archives
{
    /// <summary>
    /// csv存档
    /// </summary>
    [Archive("csv")]
    public class CsvArchive : IArchive
    {
        #region Inner Fields
        private string m_Path;
        private IArchiveModule m_Module;
        private Csv<string> m_Csv;
        #endregion

        #region Interface
        /// <summary>
        /// CSV数据
        /// </summary>
        public Csv<string> Data => m_Csv;

        /// <inheritdoc/>
        public string Name { get; private set; }
        #endregion

        #region Archive Interface
        void IArchive.OnInit(IArchiveModule module, string path, string name, object param)
        {
            Name = name;
            m_Path = path;
            m_Module = module;
            int column = 0;
            if (param != null)
                column = (int)param;
            if (File.Exists(m_Path))
            {
                string text = ArchiveUtility.ReadText(m_Module,m_Path);
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

        /// <inheritdoc/>
        public void Delete()
        {
            if (File.Exists(m_Path))
                File.Delete(m_Path);
        }

        /// <inheritdoc/>
        public void Save()
        {
            ArchiveUtility.WriteText(m_Module, m_Path, m_Csv.ToString());
        }
        #endregion
    }
}
