
namespace XFrame.Modules.Archives
{
    /// <summary>
    /// 存档
    /// </summary>
    public interface IArchive
    {
        string Name { get; }

        /// <summary>
        /// 初始化生命周期
        /// </summary>
        /// <param name="path">存储路径</param>
        /// <param name="data">用户参数</param>
        protected internal void OnInit(string path, string name, object data);

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
