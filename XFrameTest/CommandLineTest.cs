
using CommandLine;

namespace XFrameTest
{
    public class P
    {
        [Value(0)]
        public string Cmd { get; set; }

        [Value(1)]
        public string Param1 { get; set; }

        [Option('c')]
        public string Option1 { get; set; }

        public override string ToString()
        {
            return $"{Cmd}_{Param1}_{Option1}";
        }
    }

    [Verb("test2", HelpText = "test2 help text")]
    public class V1
    {
        [Value(0)]
        public string Param1 { get; set; }

        public override string ToString()
        {
            return $"V1_{Param1}";
        }
    }

    [Verb("test3", HelpText = "test2 help text")]
    public class V2
    {
        [Value(0)]
        public string P1 { get; set; }

        public override string ToString()
        {
            return $"V2_{P1}";
        }
    }


    [TestClass]
    public class CommandLineTest
    {
        [TestMethod]
        public void Test1()
        {
            ParserResult<P> p = Parser.Default.ParseArguments<P>(new string[] { "test", "-c", "111", "param1" });
            Console.WriteLine(p.Value);
        }

        [TestMethod]
        public void Test2()
        {
            var result = Parser.Default.ParseArguments<V1, V2>(new string[] { "test2", "t" });
            Console.WriteLine(result.Value);
        }
    }
}
