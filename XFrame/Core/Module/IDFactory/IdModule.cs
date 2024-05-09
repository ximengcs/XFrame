using System;
using XFrame.Collections;
using XFrame.Core;
using XFrame.Modules.Config;
using XFrame.Modules.Diagnotics;

namespace XFrame.Modules.ID
{
    /// <inheritdoc/>
    [BaseModule]
    [XType(typeof(IIdModule))]
    public class IdModule : ModuleBase, IIdModule
    {
        private IIDNumberHelper m_Helper;

        private int m_Time;
        private int m_Count;

        /// <inheritdoc/>
        protected override void OnInit(object data)
        {
            base.OnInit(data);
            if (string.IsNullOrEmpty(XConfig.DefaultIDHelper))
            {
                m_Helper = new DefaultIDNumberHelper();
            }
            else
            {
                Type type = Domain.TypeModule.GetType(XConfig.DefaultIDHelper);
                m_Helper = (IIDNumberHelper)Domain.TypeModule.CreateInstance(type);
            }
        }

        /// <inheritdoc/>
        public int Next()
        {
            int result = m_Time + m_Count++;
            if (result == 0)
                result = Next();
            return result;
        }
    }
}