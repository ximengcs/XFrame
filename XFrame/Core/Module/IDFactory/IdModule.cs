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

        /// <inheritdoc/>
        protected override void OnInit(object data)
        {
            base.OnInit(data);
            if (!string.IsNullOrEmpty(XConfig.DefaultIDHelper))
            {
                Type type = Domain.TypeModule.GetType(XConfig.DefaultIDHelper);
                if (type != null)
                {
                    m_Helper = (IIDNumberHelper)Domain.TypeModule.CreateInstance(type);
                }
            }

            if (m_Helper == null)
                m_Helper = new DefaultIDNumberHelper();
        }

        /// <inheritdoc/>
        public int Next()
        {
            return m_Helper.Next();
        }
    }
}