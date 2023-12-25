
namespace XFrame.Modules.Archives
{
    /// <summary>
    /// 存档
    /// </summary>
    public interface IArchive
    {
        string Name { get; }

        /// <summary>
        /// 保存存档
        /// </summary>
        void Save();

        /// <summary>
        /// 删除存档
        /// </summary>
        void Delete();
    }
}
