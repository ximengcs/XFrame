
namespace XFrame.Modules
{
    public static class Log
    {
        public static void Debug(params object[] content)
        {
            LogModule.Inst.Debug(content);
        }
        public static void Warning(params object[] content)
        {
            LogModule.Inst.Warning(content);
        }
        public static void Error(params object[] content)
        {
            LogModule.Inst.Error(content);
        }
        public static void Fatal(params object[] content)
        {
            LogModule.Inst.Fatal(content);
        }
    }
}
