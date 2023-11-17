using System;

namespace XFrame.Modules.Resource
{
    /// <summary>
    /// 资源重定位辅助器
    /// </summary>
    public interface IResRedirectHelper : IResourceHelper
    {
        bool CanRedirect(string assetPath, Type assetType);
        bool Redirect(string assetPath, Type assetType, out string newAssetPath);
        string Redirect(string assetPath, Type assetType);
    }
}
