using System;

namespace XFrame.Modules.Resource
{
    /// <summary>
    /// 资源重定位辅助器
    /// </summary>
    public interface IResRedirectHelper : IResourceHelper
    {
        /// <summary>
        /// 检查是否可以重定向路径
        /// </summary>
        /// <param name="assetPath">资源路径</param>
        /// <param name="assetType">资源类型</param>
        /// <returns>true为可重定向路径</returns>
        bool CanRedirect(string assetPath, Type assetType);

        /// <summary>
        /// 重定向资源路径
        /// </summary>
        /// <param name="assetPath">资源路径</param>
        /// <param name="assetType">资源类型</param>
        /// <param name="newAssetPath">重定向到的目标路径</param>
        /// <returns>true表示重定向成功</returns>
        bool Redirect(string assetPath, Type assetType, out string newAssetPath);

        /// <summary>
        /// 重定向资源路径
        /// </summary>
        /// <param name="assetPath">资源路径</param>
        /// <param name="assetType">资源类型</param>
        /// <returns>如果可以重定向则返回重定向的路径，否则返回原始路径</returns>
        string Redirect(string assetPath, Type assetType);
    }
}
