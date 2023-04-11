using XFrame.Collections;

namespace XFrame.Modules.Containers
{
    public interface ICom : IXItem, IContainer
    {
        bool Active { get; set; }
        internal void OnInit(int id, object owner, object userData);
        internal void OnUpdate();
        internal void OnDestroy();
    }
}
