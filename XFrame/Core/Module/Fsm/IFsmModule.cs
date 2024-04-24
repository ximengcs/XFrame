
using System;
using XFrame.Core;

namespace XFrame.Modules.StateMachine
{
    /// <summary>
    /// 有限状态机模块
    /// </summary>
    public interface IFsmModule : IModule
    {
        /// <summary>
        /// 获取(不存在时创建)有限状态机
        /// </summary>
        /// <param name="name">状态机名</param>
        /// <param name="states">状态机状态集合</param>
        /// <returns>获取到的状态机</returns>
        IFsm GetOrNew(string name, params Type[] states);

        /// <summary>
        /// 获取(不存在时创建)有限状态机
        /// </summary>
        /// <param name="states">状态机状态集合</param>
        /// <returns>获取到的状态机</returns>
        IFsm GetOrNew(params Type[] states);

        /// <summary>
        /// 获取(不存在时创建)有限状态机
        /// </summary>
        /// <typeparam name="T">状态机拥有者类型</typeparam>
        /// <param name="name">状态机名</param>
        /// <param name="owner">状态机拥有者</param>
        /// <param name="states">状态机状态集合</param>
        /// <returns>获取到的状态机</returns>
        IFsm<T> GetOrNew<T>(string name, T owner, params Type[] states);

        /// <summary>
        /// 获取(不存在时创建)有限状态机
        /// </summary>
        /// <typeparam name="T">状态机拥有者类型</typeparam>
        /// <param name="owner">状态机拥有者</param>
        /// <param name="states">状态机状态集合</param>
        /// <returns>获取到的状态机</returns>
        IFsm<T> GetOrNew<T>(T owner, params Type[] states);

        /// <summary>
        /// 移除有限状态机
        /// </summary>
        /// <param name="name">需要移除的状态机</param>
        void Remove(string name);

        /// <summary>
        /// 移除状态机
        /// </summary>
        /// <param name="fsm">需要移除的状态机</param>
        void Remove(IFsmBase fsm);
    }
}
