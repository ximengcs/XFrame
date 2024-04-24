
using System;
using XFrame.Core;

namespace XFrame.Modules.Containers
{
    /// <summary>
    /// 容器类模块
    /// </summary>
    public interface IContainerModule : IModule, IUpdater
    {
        /// <summary>
        /// 请求一个新的容器
        /// </summary>
        /// <typeparam name="T">容器类型</typeparam>
        /// <param name="updateTrusteeship">是否需要模块处理更新</param>
        /// <param name="master">容器拥有者</param>
        /// <param name="onReady">数据提供委托</param>
        /// <returns>容器实例</returns>
        T New<T>(bool updateTrusteeship = true, IContainer master = null, OnDataProviderReady onReady = null) where T : IContainer;

        /// <summary>
        /// 请求一个新的容器
        /// </summary>
        /// <param name="updateTrusteeship">是否需要模块处理更新</param>
        /// <param name="master">容器拥有者</param>
        /// <param name="onReady">数据提供委托</param>
        /// <returns>容器实例</returns>
        Container New(bool updateTrusteeship = true, IContainer master = null, OnDataProviderReady onReady = null);

        /// <summary>
        /// 请求一个新的容器
        /// </summary>
        /// <param name="type">容器类型</param>
        /// <param name="updateTrusteeship">是否需要模块处理更新</param>
        /// <param name="master">容器拥有者</param>
        /// <param name="onReady">数据提供委托</param>
        /// <returns>容器实例</returns>
        IContainer New(Type type, bool updateTrusteeship = true, IContainer master = null, OnDataProviderReady onReady = null);

        /// <summary>
        /// 请求一个新的容器
        /// </summary>
        /// <param name="type">容器类型</param>
        /// <param name="id">容器Id</param>
        /// <param name="updateTrusteeship">是否需要模块处理更新</param>
        /// <param name="master">容器拥有者</param>
        /// <param name="onReady">数据提供委托</param>
        /// <returns>容器实例</returns>
        IContainer New(Type type, int id, bool updateTrusteeship = true, IContainer master = null, OnDataProviderReady onReady = null);

        /// <summary>
        /// 移除一个容器
        /// </summary>
        /// <param name="container">容器实例</param>
        void Remove(IContainer container);
    }
}
