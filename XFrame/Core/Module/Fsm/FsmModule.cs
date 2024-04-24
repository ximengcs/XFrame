using System;
using XFrame.Core;
using System.Collections.Generic;
using XFrame.Modules.Rand;
using XFrame.Collections;

namespace XFrame.Modules.StateMachine
{
    /// <inheritdoc/>
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
        /// <inheritdoc/>
        protected override void OnInit(object data)
        {
            base.OnInit(data);

            m_FsmList = new List<IFsmBase>();
            m_Fsms = new Dictionary<string, IFsmBase>();
        }

        /// <inheritdoc/>
        public void OnUpdate(float escapeTime)
        {
            foreach (IFsmBase fsm in m_FsmList)
                fsm.OnUpdate();
        }

        /// <inheritdoc/>
        protected override void OnDestroy()
        {
            foreach (IFsmBase fsm in m_FsmList)
                fsm.OnDestroy();
        }
        #endregion

        #region Interface
        /// <inheritdoc/>
        public IFsm GetOrNew(string name, params Type[] states)
        {
            return InnerCreateFsm(name, states);
        }

        /// <inheritdoc/>
        public IFsm GetOrNew(params Type[] states)
        {
            return GetOrNew(Domain.GetModule<IRandModule>().RandString(), states);
        }

        /// <inheritdoc/>
        public IFsm<T> GetOrNew<T>(string name, T owner, params Type[] states)
        {
            return InnerCreateFsm(name, owner, states);
        }

        /// <inheritdoc/>
        public IFsm<T> GetOrNew<T>(T owner, params Type[] states)
        {
            return GetOrNew(Domain.GetModule<IRandModule>().RandString(), owner, states);
        }

        /// <inheritdoc/>
        public void Remove(string name)
        {
            InnerRemoveFsm(name);
        }

        /// <inheritdoc/>
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
