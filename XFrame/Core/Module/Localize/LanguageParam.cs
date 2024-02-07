
namespace XFrame.Modules.Local
{
    public struct LanguageParam
    {
        public int Id;
        public object[] Params;

        public static LanguageParam Create(int id, params object[] args)
        {
            LanguageParam param = new LanguageParam();
            param.Id = id;
            param.Params = args;
            return param;
        }
    }

    public struct LanguageIdParam
    {
        public int Id;
        public int[] Params;

        public static LanguageIdParam Create(int id, params int[] args)
        {
            LanguageIdParam param = new LanguageIdParam();
            param.Id = id;
            param.Params = args;
            return param;
        }
    }
}
