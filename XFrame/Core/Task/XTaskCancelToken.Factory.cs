using System.Collections.Concurrent;

namespace XFrame.Tasks
{
    public partial class XTaskCancelToken
    {
        private const int MAX_CACHE = 1024;

        private static ConcurrentQueue<XTaskCancelToken>
            s_CacheQueue = new ConcurrentQueue<XTaskCancelToken>();

        /// <summary>
        /// 请求一个取消绑定器
        /// </summary>
        /// <returns>取消绑定器</returns>
        public static XTaskCancelToken Require()
        {
            if (!s_CacheQueue.TryDequeue(out XTaskCancelToken token))
            {
                token = new XTaskCancelToken();
            }

            token.m_Canceled = false;
            token.m_Disposed = false;
            return token;
        }

        /// <summary>
        /// 请求一个取消绑定器
        /// </summary>
        /// <param name="token">取消绑定器</param>
        public static void Release(XTaskCancelToken token)
        {
            if (token == null)
                return;
            if (s_CacheQueue.Count > MAX_CACHE)
                return;
            token.Dispose();
            s_CacheQueue.Enqueue(token);
        }
    }
}