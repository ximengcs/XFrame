using System;
using XFrame.Core;

namespace XFrame.Modules.Containers
{
    /// <summary>
    /// 容器
    /// </summary>
    public interface IContainer : IDataProvider
    {
        /// <summary>
        /// 获取一个组件(Id为默认Id)
        /// </summary>
        /// <typeparam name="T">组件类型</typeparam>
        /// <param name="id">组件Id</param>
        /// <returns>组件实例</returns>
        T Get<T>(int id = default) where T : ICom;

        /// <summary>
        /// 获取一个组件
        /// </summary>
        /// <param name="type">组件类型</param>
        /// <param name="id">组件Id</param>
        /// <returns>组件实例</returns>
        ICom Get(Type type, int id = default);

        /// <summary>
        /// 添加一个组件(Id为默认Id)
        /// </summary>
        /// <typeparam name="T">组件类型</typeparam>
        /// <param name="comInitComplete">初始化完成事件</param>
        /// <returns>添加的组件</returns>
        T Add<T>(Action<ICom> comInitComplete = null) where T : ICom;

        /// <summary>
        /// 添加一个组件
        /// </summary>
        /// <typeparam name="T">组件类型</typeparam>
        /// <param name="id">组件Id</param>
        /// <param name="comInitComplete">初始化完成事件</param>
        /// <returns>添加的组件</returns>
        T Add<T>(int id, Action<ICom> comInitComplete = null) where T : ICom;

        /// <summary>
        /// 添加一个组件(Id为默认Id)
        /// </summary>
        /// <param name="type">组件类型</param>
        /// <param name="comInitComplete">初始化完成事件</param>
        /// <returns>添加的组件</returns>
        ICom Add(Type type, Action<ICom> comInitComplete = null);

        /// <summary>
        /// 添加一个组件
        /// </summary>
        /// <param name="type"></param>
        /// <param name="id"></param>
        /// <param name="comInitComplete">初始化完成事件</param>
        /// <returns>添加的组件</returns>
        ICom Add(Type type, int id = default, Action<ICom> comInitComplete = null);

        /// <summary>
        /// 获取或添加一个组件(Id为默认Id)
        /// </summary>
        /// <typeparam name="T">组件类型</typeparam>
        /// <param name="comInitComplete">初始化完成事件</param>
        /// <returns>添加的组件</returns>
        T GetOrAdd<T>(Action<ICom> comInitComplete = null) where T : ICom;

        /// <summary>
        /// 获取或添加一个组件
        /// </summary>
        /// <typeparam name="T">组件类型</typeparam>
        /// <param name="id">组件Id</param>
        /// <param name="comInitComplete">初始化完成事件</param>
        /// <returns>添加的组件</returns>
        T GetOrAdd<T>(int id = default, Action<ICom> comInitComplete = null) where T : ICom;

        /// <summary>
        /// 获取或添加一个组件(Id为默认Id)
        /// </summary>
        /// <param name="type">组件类型</param>
        /// <param name="comInitComplete">初始化完成事件</param>
        /// <returns>添加的组件</returns>
        ICom GetOrAdd(Type type, Action<ICom> comInitComplete = null);

        /// <summary>
        /// 获取或添加一个组件
        /// </summary>
        /// <param name="type">组件类型</param>
        /// <param name="id">组件Id</param>
        /// <param name="comInitComplete">初始化完成事件</param>
        /// <returns>添加的组件</returns>
        ICom GetOrAdd(Type type, int id = default, Action<ICom> comInitComplete = null);

        /// <summary>
        /// 移除组件
        /// </summary>
        /// <typeparam name="T">组件类型</typeparam>
        /// <param name="id">组件Id</param>
        void Remove<T>(int id = default) where T : ICom;

        /// <summary>
        /// 移除组件
        /// </summary>
        /// <param name="type">组件类型</param>
        /// <param name="id">组件Id</param>
        void Remove(Type type, int id = default);

        /// <summary>
        /// 移除所有组件
        /// </summary>
        void Clear();

        /// <summary>
        /// 分发所有组件事件
        /// </summary>
        /// <param name="handle">处理委托</param>
        void Dispatch(Action<ICom> handle);
    }
}
