
namespace XFrame.Modules.Local
{
    /// <summary>
    /// 语言参数
    /// </summary>
    public struct LanguageParam
    {
        /// <summary>
        /// 语言Id
        /// </summary>
        public int Id;

        /// <summary>
        /// 通配参数
        /// </summary>
        public object[] Params;

        /// <summary>
        /// 构造参数
        /// </summary>
        /// <param name="id">语言Id</param>
        /// <param name="args">通配参数</param>
        /// <returns>参数</returns>
        public static LanguageParam Create(int id, params object[] args)
        {
            LanguageParam param = new LanguageParam();
            param.Id = id;
            param.Params = args;
            return param;
        }
    }

    /// <summary>
    /// 语言参数，通配Id
    /// </summary>
    public struct LanguageIdParam
    {
        /// <summary>
        /// 语言Id
        /// </summary>
        public int Id;

        /// <summary>
        /// 通配符对应语言Id
        /// </summary>
        public int[] Params;

        /// <summary>
        /// 创建参数
        /// </summary>
        /// <param name="id">语言Id</param>
        /// <param name="args">通配符对应语言Id</param>
        /// <returns>参数</returns>
        public static LanguageIdParam Create(int id, params int[] args)
        {
            LanguageIdParam param = new LanguageIdParam();
            param.Id = id;
            param.Params = args;
            return param;
        }
    }
}
