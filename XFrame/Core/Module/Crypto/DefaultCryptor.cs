using System.IO;
using System.Text;
using System.Security.Cryptography;
using System;

namespace XFrame.Modules.Crypto
{
    internal class DefaultCryptor : ICryptor
    {
        private string m_KeyStr;
        private string m_IvStr;

        private IDisposable m_Provider;
        private ICryptoTransform m_CryptorTf;
        private MemoryStream m_Stream;
        private CryptoStream m_CryptStream;
        private StreamWriter m_Wirter;
        private StreamReader m_Reader;

        public StreamWriter Writer => m_Wirter;
        public StreamReader Reader => m_Reader;

        void ICryptor.OnInit(string keyStr, string ivStr)
        {
            m_KeyStr = keyStr;
            m_IvStr = ivStr;
        }

        public void BeginEncrypt()
        {
            InnerGetKeyIV(m_KeyStr, out byte[] key, m_IvStr, out byte[] iv);
            DESCryptoServiceProvider provider = new DESCryptoServiceProvider();
            m_Provider = provider;
            m_CryptorTf = provider.CreateEncryptor(key, iv);
            m_Stream = new MemoryStream();
            m_CryptStream = new CryptoStream(m_Stream, m_CryptorTf, CryptoStreamMode.Write);
            m_Wirter = new StreamWriter(m_CryptStream);
        }

        public byte[] EndEncrypt()
        {
            m_Wirter.Flush();
            m_CryptStream.FlushFinalBlock();
            return m_Stream.ToArray();
        }

        public void BeginDecrypty(byte[] buffer)
        {
            InnerGetKeyIV(m_KeyStr, out byte[] key, m_IvStr, out byte[] iv);
            DESCryptoServiceProvider provider = new DESCryptoServiceProvider();
            m_Provider = provider;
            m_CryptorTf = provider.CreateDecryptor(key, iv);
            m_Stream = new MemoryStream(buffer);
            m_CryptStream = new CryptoStream(m_Stream, m_CryptorTf, CryptoStreamMode.Read);
            m_Reader = new StreamReader(m_CryptStream);
        }

        public void EndDecrypty()
        {
            
        }

        public void Dispose()
        {
            m_Wirter?.Close();
            m_Reader?.Close();
            m_CryptStream.Close();
            m_Stream.Close();
            m_Provider.Dispose();
        }

        private void InnerGetKeyIV(string keyStr, out byte[] key, string ivStr, out byte[] iv)
        {
            key = Encoding.UTF8.GetBytes(keyStr);
            iv = Encoding.UTF8.GetBytes(ivStr);
        }
    }
}
