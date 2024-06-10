using System;
using XFrame.Core;
using XFrame.Modules.ID;
using XFrame.Modules.Pools;
using XFrame.Modules.Config;
using XFrame.Collections;
using XFrame.Core.Threads;

namespace XFrame.Modules.Download
{
    /// <inheritdoc/>
    [BaseModule]
    [RequireModule(typeof(PoolModule))]
    [RequireModule(typeof(IdModule))]
    [RequireModule(typeof(FiberModule))]
    [XType(typeof(IDownloadModule))]
    public partial class DownloadModule : ModuleBase, IDownloadModule
    {
        #region Inner Fileds
        private Type m_Helper;
        #endregion

        #region Life Fun
        /// <inheritdoc/>
        protected override void OnInit(object data)
        {
            base.OnInit(data);

            if (!string.IsNullOrEmpty(XConfig.DefaultDownloadHelper))
            {
                Type type = Domain.TypeModule.GetType(XConfig.DefaultDownloadHelper);
                InnerSetHelperType(type);
            }
        }
        #endregion

        #region Interface
        /// <inheritdoc/>
        public void SetHelper<T>() where T : IDownloadHelper
        {
            InnerSetHelperType(typeof(T));
        }

        /// <inheritdoc/>
        public DownTask Down(string url, params string[] reserveUrls)
        {
            IDownloadHelper helper = (IDownloadHelper)Domain.TypeModule.CreateInstance(m_Helper);
            helper.Url = url;
            helper.ReserveUrl = reserveUrls;
            helper.OnInit();
            return new DownTask(helper);
        }
        #endregion

        #region Inner Implement
        private void InnerSetHelperType(Type type)
        {
            m_Helper = type;
        }
        #endregion
    }
}
