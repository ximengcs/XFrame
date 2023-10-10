using System.IO;
using XFrame.Core;
using XFrame.SimpleJSON;

namespace XFrame.Modules.Archives
{
    /// <summary>
    /// Json存档
    /// </summary>
    [Archive("json")]
    public class JsonArchive : JsonArchiveBase, IJsonArchive, IArchive
    {
        #region Inner Fields
        private string m_Path;
        #endregion

        #region Archive Interface
        void IArchive.OnInit(string path, string name, object param)
        {
            Name = name;
            m_Path = path;
            if (File.Exists(m_Path))
            {
                m_Root = JSONNode.Parse(ArchiveUtility.ReadText(m_Path));
            }
            if (m_Root == null)
                m_Root = new JSONObject();
        }

        public void Save()
        {
            ArchiveUtility.WriteText(m_Path, m_Root.ToString(4));
        }

        public void Delete()
        {
            if (File.Exists(m_Path))
                File.Delete(m_Path);
        }
        #endregion

        public override void ClearData()
        {
            XModule.Archive.Delete(this);
        }
    }
}
