
namespace XFrame.Modules.Archives
{
    internal class SubJsonArchive : JsonArchiveBase, IJsonArchive
    {
        public IJsonArchive Parent { get; }

        internal SubJsonArchive(IJsonArchive archive, string name)
        {
            Parent = archive;
        }

        public override void ClearData()
        {
            Parent.Remove(Name);
        }

        public override void Delete()
        {
            ClearData();
        }

        public override void Save()
        {
            
        }

        protected internal override void OnInit(string path, string name, object data)
        {
            Name = $"{Parent.Name}_{nameof(SubJsonArchive)}_{name}";
            m_Root = Parent.GetOrNewObject(Name);
        }
    }
}
