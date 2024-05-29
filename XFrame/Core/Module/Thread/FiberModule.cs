﻿using System.Threading;
using System.Collections.Generic;
using XFrame.Modules.Diagnotics;

namespace XFrame.Core.Threads
{
    [BaseModule]
    public class FiberModule : ModuleBase, IUpdater, ITaskModule
    {
        private Fiber m_MainFiber;
        private Dictionary<int, Fiber> m_Fibers;

        public Fiber MainFiber
        {
            get
            {
                if (m_MainFiber == null)
                {
                    m_MainFiber = new Fiber(0, Thread.CurrentThread.ManagedThreadId);
                    Log.Debug(Log.Fiber, $"create main fiber {m_MainFiber.Type} {m_MainFiber.Thread}");
                }
                return m_MainFiber;
            }
        }

        public bool CurrentIsMain
        {
            get { return Thread.CurrentThread.ManagedThreadId == m_MainFiber.Thread; }
        }

        protected override void OnInit(object data)
        {
            base.OnInit(data);
            m_Fibers = new Dictionary<int, Fiber>();
        }

        public Fiber FindFiber(SynchronizationContext context)
        {
            if (MainFiber.CheckContent(context))
                return MainFiber;
            foreach (Fiber fiber in m_Fibers.Values)
            {
                if (fiber.CheckContent(context))
                    return fiber;
            }
            return null;
        }

        void IUpdater.OnUpdate(double escapeTime)
        {
            m_MainFiber.Update(escapeTime);
        }

        public Fiber Get(int type)
        {
            if (m_Fibers.TryGetValue(type, out Fiber fiber))
                return fiber;
            return default;
        }

        public Fiber GetOrNew(int type)
        {
            if (!m_Fibers.TryGetValue(type, out Fiber fiber))
            {
                fiber = new Fiber(type);
                m_Fibers.Add(type, fiber);
            }
            return fiber;
        }

        public void Destroy(Fiber fiber)
        {
            if (m_Fibers.ContainsKey(fiber.Type))
            {
                fiber.Dispose();
                m_Fibers.Remove(fiber.Type);
            }
        }

        public void Update(int type, double escapeTime)
        {
            if (m_Fibers.TryGetValue(type, out Fiber fiber))
                fiber.Update(escapeTime);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            foreach (Fiber fiber in m_Fibers.Values)
                fiber.Dispose();
            m_Fibers = null;
        }

        public void Register(IUpdater task)
        {
            Fiber currentFiber = FindFiber(SynchronizationContext.Current);
            currentFiber.RegisterUpdater(task);
        }

        public void UnRegister(IUpdater task)
        {
            Fiber currentFiber = FindFiber(SynchronizationContext.Current);
            currentFiber.UnRegisterUpdater(task);
        }
    }
}
