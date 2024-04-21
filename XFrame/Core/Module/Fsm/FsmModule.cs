using System;
using XFrame.Core;
using System.Collections.Generic;
using XFrame.Modules.Rand;
using XFrame.Collections;
using XFrame.Modules.Event;

namespace XFrame.Modules.StateMachine
{
    /// <summary>
    /// 有限状态机模块
    /// </summary>
    [CoreModule]
    [RequireModule(typeof(RandModule))]
    [XType(typeof(IFsmModule))]
    public class FsmModule : ModuleBase, IFsmModule
    {
        #region Inner Field
        private Dictionary<string, IFsmBase> m_Fsms;
        private List<IFsmBase> m_FsmList;
        #endregion

        #region Module Life Fun
        protected override void OnInit(object data)
        {
            base.OnInit(data);

            m_FsmList = new List<IFsmBase>();
            m_Fsms = new Dictionary<string, IFsmBase>();
        }

        public void OnUpdate(float escapeTime)
        {
            foreach (IFsmBase fsm in m_FsmList)
                fsm.OnUpdate();
        }

        protected override void OnDestroy()
        {
            foreach (IFsmBase fsm in m_FsmList)
                fsm.OnDestroy();
        }
        #endregion

        #region Interface
        /// <summary>
        /// 获取(不存在时创建)有限状态机
        /// </summary>
        /// <param name="name">状态机名</param>
        /// <param name="states">状态机状态集合</param>
        /// <returns>获取到的状态机</returns>
        public IFsm GetOrNew(string name, params Type[] states)
        {
            return InnerCreateFsm(name, states);
        }

        /// <summary>
        /// 获取(不存在时创建)有限状态机
        /// </summary>
        /// <param name="states">状态机状态集合</param>
        /// <returns>获取到的状态机</returns>
        public IFsm GetOrNew(params Type[] states)
        {
            return GetOrNew(Domain.GetModule<IRandModule>().RandString(), states);
        }

        /// <summary>
        /// 获取(不存在时创建)有限状态机
        /// </summary>
        /// <typeparam name="T">状态机拥有者类型</typeparam>
        /// <param name="name">状态机名</param>
        /// <param name="owner">状态机拥有者</param>
        /// <param name="states">状态机状态集合</param>
        /// <returns>获取到的状态机</returns>
        public IFsm<T> GetOrNew<T>(string name, T owner, params Type[] states)
        {
            return InnerCreateFsm(name, owner, states);
        }

        /// <summary>
        /// 获取(不存在时创建)有限状态机
        /// </summary>
        /// <typeparam name="T">状态机拥有者类型</typeparam>
        /// <param name="owner">状态机拥有者</param>
        /// <param name="states">状态机状态集合</param>
        /// <returns>获取到的状态机</returns>
        public IFsm<T> GetOrNew<T>(T owner, params Type[] states)
        {
            return GetOrNew(Domain.GetModule<IRandModule>().RandString(), owner, states);
        }

        /// <summary>
        /// 移除有限状态机
        /// </summary>
        /// <param name="name">需要移除的状态机</param>
        public void Remove(string name)
        {
            InnerRemoveFsm(name);
        }

        /// <summary>
        /// 移除状态机
        /// </summary>
        /// <param name="fsm">需要移除的状态机</param>
        public void Remove(IFsmBase fsm)
        {
            Remove(fsm.Name);
        }
        #endregion

        #region Inner Implement
        private IFsm<T> InnerCreateFsm<T>(string name, T owner, Type[] types)
        {
            List<FsmState<T>> states = new List<FsmState<T>>(types.Length);
            foreach (Type type in types)
            {
                FsmState<T> state = (FsmState<T>)Domain.TypeModule.CreateInstance(type);
                states.Add(state);
            }

            IFsm<T> fsm = new Fsm<T>(name, states, owner);
            fsm.OnInit();
            m_Fsms[name] = fsm;
            m_FsmList.Add(fsm);
            return fsm;
        }

        private IFsm InnerCreateFsm(string name, Type[] types)
        {
            List<FsmState> states = new List<FsmState>(types.Length);
            foreach (Type type in types)
            {
                FsmState state = (FsmState)Domain.TypeModule.CreateInstance(type);
                states.Add(state);
            }

            IFsm fsm = new Fsm(name, states);
            fsm.OnInit();
            m_Fsms[name] = fsm;
            m_FsmList.Add(fsm);
            return fsm;
        }

        private void InnerRemoveFsm(string name)
        {
            if (m_Fsms.TryGetValue(name, out IFsmBase fsm))
            {
                m_Fsms.Remove(name);
                m_FsmList.Remove(fsm);
                fsm.OnDestroy();
            }
        }
        #endregion
    } 
} 
