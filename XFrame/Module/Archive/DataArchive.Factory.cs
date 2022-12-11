
namespace XFrame.Modules
{
    public partial class DataArchive
    {
        public static DataArchive FromBytes(byte[] data)
        {
            DataArchive archive = new DataArchive();
            archive.m_Path = string.Empty;
            archive.m_Builder = new BytesBuilder(FILE_CODE);
            archive.m_Root = archive.m_Builder.From(data);
            return archive;
        }

        public static DataArchive LoadPath(string fromPath, string toPath)
        {
            DataArchive archive = new DataArchive();
            archive.m_Path = toPath;
            archive.m_Builder = new BytesBuilder(FILE_CODE);
            archive.m_Root = new Node(FileType.Directory, string.Empty);
            archive.ImportDisk(fromPath);
            return archive;
        }
    }
}
