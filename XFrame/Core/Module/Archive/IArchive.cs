
namespace XFrame.Modules.Archives
{
    /// <summary>
    /// 存档
    /// </summary>
    public interface IArchive
    {
        /// <summary>
        /// 初始化生命周期
        /// </summary>
        /// <param name="path">存储路径</param>
        protected internal void OnInit(string path);

        /// <summary>
        /// 是否加密
        /// </summary>
        bool Encrypt { get; set; }

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
