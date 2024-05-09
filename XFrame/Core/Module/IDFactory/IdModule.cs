using System;
using XFrame.Collections;
using XFrame.Core;
using XFrame.Modules.Diagnotics;

namespace XFrame.Modules.ID
{
    /// <inheritdoc/>
    [BaseModule]
    [XType(typeof(IIdModule))]
    public class IdModule : ModuleBase, IIdModule
    {
        private int m_Time;
        private int m_Count;

        /// <inheritdoc/>
        protected override void OnInit(object data)
        {
            base.OnInit(data);
            m_Time =(int)(DateTime.Now.Ticks / 1000);
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