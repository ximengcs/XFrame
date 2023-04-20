using System;
using XFrame.Core;
using XFrame.Collections;
using XFrame.Modules.Pools;

namespace XFrame.Modules.Containers
{
    /// <summary>
    /// 容器
    /// </summary>
    public interface IContainer : IXItem, IDataProvider, IPoolObject, IXEnumerable<ICom>
    {
        /// <summary>
        /// 容器拥有者
        /// </summary>
        object Master { get; }

        /// <summary>
        /// 初始化生命周期
        /// </summary>
        /// <param name="id">容器Id</param>
        /// <param name="master">容器拥有者</param>
        /// <param name="onReady">容器就绪事件</param>
        void OnInit(int id, object master, OnContainerReady onReady);

        /// <summary>
        /// 更新生命周期
        /// </summary>
        /// <param name="elapseTime">逃逸时间</param>
        void OnUpdate(float elapseTime);

        /// <summary>
        /// 销毁生命周期
        /// </summary>
        void OnDestroy();

        /// <summary>
        /// 获取一个组件(Id为默认Id)
        /// </summary>
        /// <typeparam name="T">组件类型</typeparam>
        /// <param name="id">组件Id</param>
        /// <returns>组件实例</returns>
        T GetCom<T>(int id = default) where T : ICom;

        /// <summary>
        /// 获取一个组件
        /// </summary>
        /// <param name="type">组件类型</param>
        /// <param name="id">组件Id</param>
        /// <returns>组件实例</returns>
        ICom GetCom(Type type, int id = default);

        /// <summary>
        /// 添加一个组件(Id为默认Id)
        /// </summary>
        /// <typeparam name="T">组件类型</typeparam>
        /// <param name="onReady">初始化完成事件</param>
        /// <returns>添加的组件</returns>
        T AddCom<T>(OnComReady onReady = null) where T : ICom;

        /// <summary>
        /// 添加一个组件
        /// </summary>
        /// <param name="com">组件实例</param>
        /// <returns>组件实例</returns>
        ICom AddCom(ICom com, int id = default, OnComReady onReady = null);

        /// <summary>
        /// 添加一个组件
        /// </summary>
        /// <typeparam name="T">组件类型</typeparam>
        /// <param name="id">组件Id</param>
        /// <param name="onReady">初始化完成事件</param>
        /// <returns>添加的组件</returns>
        T AddCom<T>(int id, OnComReady onReady = null) where T : ICom;

        /// <summary>
        /// 添加一个组件(Id为默认Id)
        /// </summary>
        /// <param name="type">组件类型</param>
        /// <param name="onReady">初始化完成事件</param>
        /// <returns>添加的组件</returns>
        ICom AddCom(Type type, OnComReady onReady = null);

        /// <summary>
        /// 添加一个组件
        /// </summary>
        /// <param name="type"></param>
        /// <param name="id"></param>
        /// <param name="onReady">初始化完成事件</param>
        /// <returns>添加的组件</returns>
        ICom AddCom(Type type, int id, OnComReady onReady = null);

        /// <summary>
        /// 获取或添加一个组件(Id为默认Id)
        /// </summary>
        /// <typeparam name="T">组件类型</typeparam>
        /// <param name="onReady">初始化完成事件</param>
        /// <returns>添加的组件</returns>
        T GetOrAddCom<T>(OnComReady onReady = null) where T : ICom;

        /// <summary>
        /// 获取或添加一个组件
        /// </summary>
        /// <typeparam name="T">组件类型</typeparam>
        /// <param name="id">组件Id</param>
        /// <param name="onReady">初始化完成事件</param>
        /// <returns>添加的组件</returns>
        T GetOrAddCom<T>(int id, OnComReady onReady = null) where T : ICom;

        /// <summary>
        /// 获取或添加一个组件(Id为默认Id)
        /// </summary>
        /// <param name="type">组件类型</param>
        /// <param name="onReady">初始化完成事件</param>
        /// <returns>添加的组件</returns>
        ICom GetOrAddCom(Type type, OnComReady onReady = null);

        /// <summary>
        /// 获取或添加一个组件
        /// </summary>
        /// <param name="type">组件类型</param>
        /// <param name="id">组件Id</param>
        /// <param name="onReady">初始化完成事件</param>
        /// <returns>添加的组件</returns>
        ICom GetOrAddCom(Type type, int id, OnComReady onReady = null);

        /// <summary>
        /// 移除组件
        /// </summary>
        /// <typeparam name="T">组件类型</typeparam>
        /// <param name="id">组件Id</param>
        void RemoveCom<T>(int id = default) where T : ICom;

        /// <summary>
        /// 移除组件
        /// </summary>
        /// <param name="type">组件类型</param>
        /// <param name="id">组件Id</param>
        void RemoveCom(Type type, int id = default);

        /// <summary>
        /// 移除所有组件
        /// </summary>
        void ClearCom();

        /// <summary>
        /// 分发所有组件事件
        /// </summary>
        /// <param name="handle">处理委托</param>
        void DispatchCom(OnComReady handle);
    }
}
