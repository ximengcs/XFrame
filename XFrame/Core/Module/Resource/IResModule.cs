using System;
using XFrame.Modules.Tasks;

namespace XFrame.Modules.Resource
{
    public interface IResModule
    {
        /// <summary>
        /// 资源预加载(在预加载完成之后，可调用同步方法直接获取资源，在一些不支持同步方法的接口可预加载以便必要时同步加载资源)
        /// </summary>
        /// <param name="resPaths">需要加载的资源列表</param>
        /// <param name="types">资源类型列表</param>
        /// <returns>加载任务</returns>
        ITask Preload(string[] resPaths, Type type);

        ITask Preload<T>(string[] resPaths);

        /// <summary>
        /// 加载资源(同步)
        /// </summary>
        /// <param name="resPath">资源路径</param>
        /// <param name="type">资源类型</param>
        /// <returns>加载到的资源</returns>
        object Load(string resPath, Type type);

        /// <summary>
        /// 加载资源(同步)
        /// </summary>
        /// <typeparam name="T">资源类型</typeparam>
        /// <param name="resPath">资源路径</param>
        /// <returns>加载到的资源</returns>
        T Load<T>(string resPath);

        /// <summary>
        /// 加载资源(异步)
        /// </summary>
        /// <param name="resPath">资源路径</param>
        /// <param name="type">资源类型</param>
        /// <returns>加载任务</returns>
        ResLoadTask LoadAsync(string resPath, Type type);

        /// <summary>
        /// 加载资源(异步)
        /// </summary>
        /// <typeparam name="T">资源类型</typeparam>
        /// <param name="resPath">资源路径</param>
        /// <returns>加载任务</returns>
        ResLoadTask<T> LoadAsync<T>(string resPath);

        /// <summary>
        /// 卸载资源
        /// </summary>
        /// <param name="target">要卸载的目标</param>
        void Unload(object target);

        /// <summary>
        /// 卸载预加载的资源
        /// </summary>
        /// <param name="resPath">资源路径</param>
        void UnloadPre(string resPath);

        /// <summary>
        /// 卸载所有已经加载的资源
        /// </summary>
        void UnloadAll();

        /// <summary>
        /// 卸载所有预加载的资源
        /// </summary>
        void UnloadAllPre();
    }
}
