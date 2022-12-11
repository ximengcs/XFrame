using System;
using XFrame.Core;
using System.Collections.Generic;

namespace XFrame.Modules
{
    public class FsmModule : SingleModule<FsmModule>
    {
        private Dictionary<string, IFsmBase> m_Fsms;
        private List<IFsmBase> m_FsmList;

        public override void OnInit(object data)
        {
            base.OnInit(data);

            m_FsmList = new List<IFsmBase>();
            m_Fsms = new Dictionary<string, IFsmBase>();
        }

        public override void OnUpdate(float escapeTime)
        {
            foreach (IFsmBase fsm in m_FsmList)
                fsm.OnUpdate();
        }

        public override void OnDestroy()
        {
            foreach (IFsmBase fsm in m_FsmList)
                fsm.OnDestroy();
        }

        public IFsm Create(string name, List<Type> states)
        {
            return InnerCreateFsm(name, states);
        }

        public IFsm Create(params Type[] states)
        {
            return Create(string.Empty, new List<Type>(states));
        }

        public IGenericFsm<T> Create<T>(string name, T owner, List<Type> states)
        {
            return InnerCreateFsm<T>(name, owner, states);
        }

        public IGenericFsm<T> Create<T>(string name, T owner, params Type[] states)
        {
            return Create<T>(name, owner, new List<Type>(states));
        }

        public void Remove(string name)
        {
            InnerRemoveFsm(name);
        }

        public void Remove(IFsmBase fsm)
        {
            Remove(fsm.Name);
        }

        private IGenericFsm<T> InnerCreateFsm<T>(string name, T owner, List<Type> types)
        {
            List<FsmState<T>> states = new List<FsmState<T>>(types.Count);
            foreach (Type type in types)
            {
                FsmState<T> state = (FsmState<T>)Activator.CreateInstance(type);
                states.Add(state);
            }

            GenericFsm<T> fsm = new GenericFsm<T>(name, states);
            fsm.OnInit(owner);
            m_Fsms[name] = fsm;
            m_FsmList.Add(fsm);
            return fsm;
        }

        private IFsm InnerCreateFsm(string name, List<Type> types)
        {
            List<FsmState> states = new List<FsmState>(types.Count);
            foreach (Type type in types)
            {
                FsmState state = (FsmState)Activator.CreateInstance(type);
                states.Add(state);
            }

            Fsm fsm = new Fsm(name, states);
            fsm.OnInit();
            m_Fsms[name] = fsm;
            m_FsmList.Add(fsm);
            return fsm;
        }

        private void InnerRemoveFsm(string name)
        {
            if (m_Fsms.ContainsKey(name))
            {
                m_Fsms.Remove(name);
                foreach (IFsmBase willDel in m_FsmList)
                {
                    if (willDel.Name == name)
                    {
                        m_FsmList.Remove(willDel);
                        break;
                    }
                }
            }
        }
    }
}
