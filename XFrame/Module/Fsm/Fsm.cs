using System;
using XFrame.Core;
using System.Collections.Generic;

namespace XFrame.Modules
{
    public class Fsm : DataProvider, IFsm
    {
        private string m_Name;
        private Dictionary<Type, FsmState> m_States;
        private FsmState m_Current;

        public string Name => m_Name;
        public FsmState Current => m_Current;

        public Fsm(string name, List<FsmState> states)
        {
            m_Name = name;
            m_Current = null;
            m_States = new Dictionary<Type, FsmState>();
            foreach (FsmState state in states)
                m_States[state.GetType()] = state;
        }

        public TState GetState<TState>() where TState : FsmState
        {
            if (m_States.TryGetValue(typeof(TState), out FsmState state))
                return (TState)state;
            else
                return default;
        }

        public bool HasState<TState>() where TState : FsmState
        {
            return HasState(typeof(TState));
        }

        public bool HasState(Type type)
        {
            return m_States.ContainsKey(type);
        }

        public void Start<TState>() where TState : FsmState
        {
            if (HasState<TState>())
            {
                ChangeState(typeof(TState));
            }
            else
            {
                Log.Error("XFrame", $"Fsm start error");
            }
        }

        public void Start(Type type)
        {
            if (HasState(type))
            {
                ChangeState(type);
            }
            else
            {
                Log.Error("XFrame", $"Fsm start error");
            }
        }

        internal void ChangeState(Type type)
        {
            m_Current?.OnLeave();
            if (m_States.TryGetValue(type, out FsmState state))
            {
                m_Current = state;
                m_Current.OnEnter();
            }
            else
            {
                Log.Error($"Fsm Change State Error");
            }
        }

        public void OnInit()
        {
            foreach (FsmState state in m_States.Values)
                state.OnInit(this);
        }

        public void OnUpdate()
        {
            m_Current?.OnUpdate();
        }

        public void OnDestroy()
        {
            foreach (FsmState state in m_States.Values)
                state.OnDestroy();
            m_States = null;
            m_Current = null;
        }
    }
}
