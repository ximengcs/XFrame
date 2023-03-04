﻿using System;
using XFrame.Core;
using XFrame.Modules.XType;
using XFrame.Modules.Diagnotics;
using XFrame.Modules.StateMachine;
using XFrame.Modules.Config;
using XFrame.Utility;

namespace XFrame.Modules.Procedure
{
    /// <summary>
    /// 流程模块
    /// </summary>
    public class ProcedureModule : SingletonModule<ProcedureModule>
    {
        private TypeModule.System m_Procedures;

        protected override void OnInit(object data)
        {
            base.OnInit(data);
            m_Procedures = TypeModule.Inst.GetOrNew<ProcedureBase>();
        }

        protected override void OnStart()
        {
            base.OnStart();
            string entrance = XConfig.Entrance;
            if (!string.IsNullOrEmpty(entrance) && m_Procedures.TryGetByName(entrance, out Type type))
            {
                Log.Debug("XFrame", $"Enter {TypeUtility.GetSimpleName(entrance)} Procedure");
                FsmModule.Inst.GetOrNew(m_Procedures.ToArray()).Start(type);
            }
            else
            {
                Log.Error("XFrame", $"{TypeUtility.GetSimpleName(entrance)} Procedure do not define");
            }
        }
    }
}
