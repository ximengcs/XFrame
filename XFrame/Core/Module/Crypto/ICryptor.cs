﻿using System;
using System.IO;

namespace XFrame.Modules.Crypto
{
    /// <summary>
    /// 加密器
    /// </summary>
    public interface ICryptor : IDisposable
    {
        /// <summary>
        /// 加密数据写入流
        /// </summary>
        StreamWriter Writer { get; }

        /// <summary>
        /// 解密数据读取流
        /// </summary>
        StreamReader Reader { get; }

        /// <summary>
        /// 标记开始加密
        /// </summary>
        void BeginEncrypt();

        /// <summary>
        /// 标记加密结束
        /// </summary>
        /// <returns>加密好的数据</returns>
        byte[] EndEncrypt();

        /// <summary>
        /// 标记开始解密
        /// </summary>
        /// <param name="buffer">需要解密的数据</param>
        void BeginDecrypty(byte[] buffer);

        /// <summary>
        /// 标记解密结束
        /// </summary>
        byte[] EndDecrypty();
    }
}
