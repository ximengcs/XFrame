using System.IO;

namespace XFrame.Modules.Archives
{
    /// <summary>
    /// 二进制存档
    /// </summary>
    [Archive("data")]
    public partial class DataArchive : IArchive
    {
        #region InnerField
        private const int FILE_CODE = default;

        private string m_Path;
        private Node m_Root;
        private BytesBuilder m_Builder;
        #endregion

        #region Archive Interface
        void IArchive.OnInit(string path, object param)
        {
            m_Path = path;
            m_Builder = new BytesBuilder(FILE_CODE);

            if (File.Exists(m_Path))
            {
                byte[] buffer = ArchiveUtility.ReadBytes(m_Path);
                m_Root = m_Builder.From(buffer);
            }
            else
            {
                m_Root = new Node(FileType.Directory, string.Empty);
            }
        }

        public void Save()
        {
            string dir = Path.GetDirectoryName(m_Path);
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            ArchiveUtility.WriteBytes(m_Path, ToBytes());
        }

        public void Delete()
        {
            m_Root.Delete("_");
        }
        #endregion

        #region Interface
        /// <summary>
        /// 向存档写入字节数据
        /// </summary>
        /// <param name="path">处于存档中的路径</param>
        /// <param name="data">需要写入的数据</param>
        public void Write(string path, byte[] data)
        {
            m_Root.Write(path, data);
        }

        /// <summary>
        /// 读取字节数据
        /// </summary>
        /// <param name="path">处于存档中的路径</param>
        /// <returns>读取到的数据</returns>
        public byte[] Read(string path)
        {
            return m_Root.Read(path);
        }

        /// <summary>
        /// 获取存档二进制字节数据
        /// </summary>
        /// <returns></returns>
        public byte[] ToBytes()
        {
            return m_Builder.To(m_Root);
        }

        /// <summary>
        /// 将存档中的所有数据导出到指定路径
        /// </summary>
        /// <param name="toPath"></param>
        public void ExportDisk(string toPath)
        {
            m_Root.Export(toPath);
        }

        /// <summary>
        /// 将指定路径的文件导入到存档中
        /// </summary>
        /// <param name="fromPath"></param>
        public void ImportDisk(string fromPath)
        {
            m_Root.Import(fromPath);
        }
        #endregion
    }
}
