using System;
using XFrame.Core;
using XFrame.Collections;
using XFrame.Modules.Config;

namespace XFrame.Modules.Crypto
{
    /// <summary>
    /// 数据加密模块
    /// </summary>
    [CoreModule]
    [XType(typeof(ICryptoModule))]
    public class CryptoModule : ModuleBase, ICryptoModule
    {
        private const string DEFAULT_KEY = "x1df2eop";
        private const string DEFAULT_IV = "3sfd2ds4";

        #region Life Fun
        private Type m_Type;

        protected override void OnInit(object data)
        {
            base.OnInit(data);

            if (!string.IsNullOrEmpty(XConfig.DefaultCryptor))
                m_Type = XModule.Type.GetType(XConfig.DefaultCryptor);
            if (m_Type == null)
                m_Type = typeof(DefaultCryptor);
        }
        #endregion

        #region Interface
        /// <summary>
        /// 创建加密器
        /// </summary>
        /// <param name="keyStr">密钥</param>
        /// <param name="ivStr">密钥</param>
        /// <returns>加密器</returns>
        public ICryptor New(string keyStr, string ivStr)
        {
            ICryptor cryptor = (ICryptor)XModule.Type.CreateInstance(m_Type);
            ICanInitialize initializer = cryptor as ICanInitialize;
            if (initializer != null)
                initializer.OnInit(keyStr, ivStr);
            return cryptor;
        }

        /// <summary>
        /// 使用默认密钥创建加密器
        /// </summary>
        /// <returns>加密器</returns>
        public ICryptor New()
        {
            ICryptor cryptor = (ICryptor)XModule.Type.CreateInstance(m_Type);
            ICanInitialize initializer = cryptor as ICanInitialize;
            if (initializer != null)
                initializer.OnInit(DEFAULT_KEY, DEFAULT_IV);
            return cryptor;
        }
        #endregion
    }
}
