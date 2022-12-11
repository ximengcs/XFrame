﻿using System;
using XFrame.Modules;
using System.Collections.Generic;

namespace XFrame.Core
{
    public class GenericFsm<T> : DataProvider, IGenericFsm<T>
    {
        private T m_Owner;
        private string m_Name;
        private Dictionary<Type, FsmState<T>> m_States;
        private FsmState<T> m_Current;

        public T Owner => m_Owner;
        public string Name => m_Name;
        public FsmState<T> Current => m_Current;

        public GenericFsm(string name, List<FsmState<T>> states)
        {
            m_Name = name;
            m_Current = null;
            m_States = new Dictionary<Type, FsmState<T>>();
            foreach (FsmState<T> state in states)
                m_States[state.GetType()] = state;
        }

        public TState GetState<TState>() where TState : FsmState<T>
        {
            if (m_States.TryGetValue(typeof(TState), out FsmState<T> state))
                return (TState)state;
            else
                return default;
        }

        public bool HasState<TState>() where TState : FsmState<T>
        {
            return m_States.ContainsKey(typeof(TState));
        }

        public void Start<TState>() where TState : FsmState<T>
        {
            if (HasState<TState>())
            {
                ChangeState<TState>();
            }
            else
            {
                Log.Error("XFrame", $"Fsm start error");
            }
        }

        internal void ChangeState<TState>() where TState : FsmState<T>
        {
            m_Current?.OnLeave();
            if (m_States.TryGetValue(typeof(TState), out FsmState<T> state))
            {
                m_Current = state;
                m_Current.OnEnter();
            }
            else
            {
                Log.Error("XFrame", $"Fsm Change State Error");
            }
        }

        public void OnInit(T owner)
        {
            m_Owner = owner;
            foreach (FsmState<T> state in m_States.Values)
                state.OnInit(this);
        }

        public void OnUpdate()
        {
            m_Current?.OnUpdate();
        }

        public void OnDestroy()
        {
            foreach (FsmState<T> state in m_States.Values)
                state.OnDestroy();
            m_States = null;
            m_Current = null;
        }
    }
}
