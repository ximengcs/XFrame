using System.IO;

namespace XFrame.Modules
{
    public partial class DataArchive : IArchive
    {
        #region InnerField
        private const int FILE_CODE = default;

        private string m_Path;
        private Node m_Root;
        private BytesBuilder m_Builder;
        #endregion

        public void Init(string path)
        {
            m_Path = path;
            m_Builder = new BytesBuilder(FILE_CODE);

            if (File.Exists(m_Path))
            {
                m_Root = m_Builder.From(File.ReadAllBytes(m_Path));
            }
            else
            {
                m_Root = new Node(FileType.Directory, string.Empty);
            }
        }

        public void Write(string path, byte[] data)
        {
            m_Root.Write(path, data);
        }

        public byte[] Read(string path)
        {
            return m_Root.Read(path);
        }

        public byte[] ToBytes()
        {
            return m_Builder.To(m_Root);
        }

        public void Save()
        {
            string dir = Path.GetDirectoryName(m_Path);
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            File.WriteAllBytes(m_Path, ToBytes());
        }

        public object Read()
        {
            return ToBytes();
        }

        public void Write(object data)
        {
            Write("_", (byte[])data);
        }

        public void Delete()
        {
            m_Root.Delete("_");
        }

        public void ExportDisk(string toPath)
        {
            m_Root.Export(toPath);
        }

        public void ImportDisk(string fromPath)
        {
            m_Root.Import(fromPath);
        }
    }
}
