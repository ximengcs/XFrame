
namespace XFrame.Modules.Archives
{
    public abstract class ArchiveBase : IArchive
    {
        public string Name { get; protected set; }

        public abstract void Delete();
        public abstract void Save();

        /// <summary>
        /// 初始化生命周期
        /// </summary>
        /// <param name="path">存储路径</param>
        /// <param name="data">用户参数</param>
        protected internal abstract void OnInit(string path, string name, object data);
    }
}
