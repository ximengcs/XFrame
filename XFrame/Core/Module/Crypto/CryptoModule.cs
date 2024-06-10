using System;
using XFrame.Core;
using XFrame.Collections;
using XFrame.Modules.Config;

namespace XFrame.Modules.Crypto
{
    /// <inheritdoc/>
    [CoreModule]
    [XType(typeof(ICryptoModule))]
    public class CryptoModule : ModuleBase, ICryptoModule
    {
        private const string DEFAULT_KEY = "x1df2eop";
        private const string DEFAULT_IV = "3sfd2ds4";

        #region Life Fun
        private Type m_Type;

        /// <inheritdoc/>
        protected override void OnInit(object data)
        {
            base.OnInit(data);

            if (!string.IsNullOrEmpty(XConfig.DefaultCryptor))
                m_Type = Domain.TypeModule.GetType(XConfig.DefaultCryptor);
            if (m_Type == null)
                m_Type = typeof(DefaultCryptor);
        }
        #endregion

        #region Interface
        /// <inheritdoc/>
        public ICryptor New(string keyStr, string ivStr)
        {
            ICryptor cryptor = (ICryptor)Domain.TypeModule.CreateInstance(m_Type);
            cryptor.OnInit(keyStr, ivStr);
            return cryptor;
        }

        /// <inheritdoc/>
        public ICryptor New()
        {
            ICryptor cryptor = (ICryptor)Domain.TypeModule.CreateInstance(m_Type);
            cryptor.OnInit(DEFAULT_KEY, DEFAULT_IV);
            return cryptor;
        }
        #endregion
    }
}
