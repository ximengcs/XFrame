using XFrame.Collections;

namespace XFrame.Core
{
    public interface IModule : IXItem
    {
        void OnInit(object data);
        void OnUpdate(float escapeTime);
        void OnDestroy();
    }
}
