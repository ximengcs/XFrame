using System;

namespace XFrame.Modules.Resource
{
    /// <summary>
    /// 资源加载辅助器
    /// </summary>
    public interface IResourceHelper
    {
        /// <summary>
        /// 资源加载初始化生命周期
        /// </summary>
        /// <param name="rootPath">资源根路径</param>
        void OnInit(string rootPath);

        /// <summary>
        /// 设置资源重定向辅助器
        /// </summary>
        /// <param name="helper">辅助器</param>
        void SetResDirectHelper(IResRedirectHelper helper);

        /// <summary>
        /// 加载资源
        /// </summary>
        /// <param name="resPath">资源路径</param>
        /// <param name="type">资源类型</param>
        /// <returns>加载到的资源</returns>
        object Load(string resPath, Type type);

        /// <summary>
        /// 加载资源
        /// </summary>
        /// <typeparam name="T">资源类型</typeparam>
        /// <param name="resPath">资源路径</param>
        /// <returns>加载到的资源</returns>
        T Load<T>(string resPath);

        /// <summary>
        /// 异步加载资源
        /// </summary>
        /// <param name="resPath">资源路径</param>
        /// <param name="type">资源类型</param>
        /// <returns>加载到的资源</returns>
        ResLoadTask LoadAsync(string resPath, Type type);

        /// <summary>
        /// 异步加载资源
        /// </summary>
        /// <typeparam name="T">资源类型</typeparam>
        /// <param name="resPath">资源路径</param>
        /// <returns>加载到的资源</returns>
        ResLoadTask<T> LoadAsync<T>(string resPath);

        /// <summary>
        /// 卸载资源
        /// </summary>
        /// <param name="target">卸载目标</param>
        void Unload(object target);

        /// <summary>
        /// 卸载所有资源
        /// </summary>
        void UnloadAll();
    }
}
