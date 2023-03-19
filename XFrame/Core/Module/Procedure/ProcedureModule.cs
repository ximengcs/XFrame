using System;
using XFrame.Core;
using XFrame.Utility;
using XFrame.Modules.XType;
using XFrame.Modules.Config;
using XFrame.Modules.Diagnotics;
using XFrame.Modules.StateMachine;

namespace XFrame.Modules.Procedure
{
    /// <summary>
    /// 流程模块
    /// </summary>
    [XModule]
    public class ProcedureModule : SingletonModule<ProcedureModule>
    {
        private TypeSystem m_Procedures;
        private Fsm m_Fsm;

        protected override void OnInit(object data)
        {
            base.OnInit(data);
            InnerRefreshHandler();
        }

        protected override void OnStart()
        {
            base.OnStart();
            string entrance = XConfig.Entrance;
            if (!string.IsNullOrEmpty(entrance) && m_Procedures.TryGetByName(entrance, out Type type))
            {
                Log.Debug("XFrame", $"Enter {TypeUtility.GetSimpleName(entrance)} Procedure");
                m_Fsm = (Fsm)FsmModule.Inst.GetOrNew(m_Procedures.ToArray());
                m_Fsm.Start(type);
            }
            else
            {
                Log.Error("XFrame", $"{TypeUtility.GetSimpleName(entrance)} Procedure do not define");
            }
        }

        public void Redirect(string name)
        {
            InnerRefreshHandler();
            if (!string.IsNullOrEmpty(name) && m_Procedures.TryGetByName(name, out Type type))
            {
                Log.Debug("XFrame", $"Enter {TypeUtility.GetSimpleName(name)} Procedure");
                m_Fsm.ChangeState(type);
            }
            else
            {
                Log.Error("XFrame", $"{TypeUtility.GetSimpleName(name)} Procedure do not define");
            }
        }

        public void Redirect(Type type)
        {
            if (m_Fsm != null && m_Fsm.HasState(type))
                m_Fsm.ChangeState(type);
        }

        public void Add(Type type)
        {
            InnerAdd(type);
        }

        public void Add<T>() where T : ProcedureBase
        {
            InnerAdd(typeof(T));
        }

        private void InnerAdd(Type type)
        {
            if (!m_Fsm.HasState(type))
            {
                ProcedureBase proc = (ProcedureBase)Activator.CreateInstance(type);
                m_Fsm.InnerAddState(proc);
            }
            else
            {
                Log.Debug("XFrame", $"Already has exist proc {type.Name}.");
            }
        }

        private void InnerRefreshHandler()
        {
            m_Procedures = TypeModule.Inst.GetOrNew<ProcedureBase>();
        }
    }
}
