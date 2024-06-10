using System;
using XFrame.Core;
using XFrame.Tasks;
using System.Collections.Generic;

namespace XFrame.Modules.Resource
{
    /// <summary>
    /// 资源模块
    /// </summary>
    public interface IResModule : IModule
    {
        /// <summary>
        /// 资源辅助器
        /// </summary>
        IResourceHelper Helper { get; }

        /// <summary>
        /// 预加载资源
        /// </summary>
        /// <param name="resPaths">资源路径列表</param>
        /// <param name="type">资源类型</param>
        /// <returns>异步加载任务</returns>
        XTask Preload(IEnumerable<string> resPaths, Type type);

        /// <summary>
        /// 预加载资源
        /// </summary>
        /// <param name="resPath">资源路径</param>
        /// <param name="type">资源类型</param>
        /// <returns>异步加载任务</returns>
        XTask Preload(string resPath, Type type);

        /// <summary>
        /// 是否已经预加载资源
        /// </summary>
        /// <param name="resPath">资源路径</param>
        /// <returns>true表示已预加载</returns>
        bool HasPreload(string resPath);

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
        /// <param name="target">资源对象</param>
        void Unload(object target);

        /// <summary>
        /// 卸载预加载的资源
        /// </summary>
        /// <param name="resPath">资源路径</param>
        void UnloadPre(string resPath);

        /// <summary>
        /// 卸载所有资源
        /// </summary>
        void UnloadAll();

        /// <summary>
        /// 卸载所有预加载的资源
        /// </summary>
        void UnloadAllPre();
    }
}
