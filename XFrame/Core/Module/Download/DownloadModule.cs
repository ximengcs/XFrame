using System;
using XFrame.Core;
using XFrame.Modules.ID;
using XFrame.Modules.Pools;
using XFrame.Modules.Tasks;
using XFrame.Modules.Config;
using XFrame.Collections;
using XFrame.Modules.Datas;

namespace XFrame.Modules.Download
{
    /// <summary>
    /// 下载器模块
    /// </summary>
    [BaseModule]
    [RequireModule(typeof(PoolModule))]
    [RequireModule(typeof(IdModule))]
    [RequireModule(typeof(TaskModule))]
    [XType(typeof(IDownloadModule))]
    public partial class DownloadModule : ModuleBase, IDownloadModule
    {
        #region Inner Fileds
        private Type m_Helper;
        #endregion

        #region Life Fun
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
        /// <summary>
        /// 设置下载辅助器
        /// </summary>
        /// <typeparam name="T">辅助器类型</typeparam>
        public void SetHelper<T>() where T : IDownloadHelper
        {
            InnerSetHelperType(typeof(T));
        }

        /// <summary>
        /// 下载文件或数据
        /// </summary>
        /// <param name="url">url</param>
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
