using Newtonsoft.Json;
using XFrame.Core;
using XFrame.Modules.Diagnotics;
using XFrame.Modules.Local;
using XFrame.Modules.Pools;
using XFrame.Utility;

namespace XFrameTest
{
    [TestClass]
    public class ParserTest
    {
        enum Enum1
        {
            A, B, C, D,
        }

        [TestMethod]
        public void NullTest()
        {
            IntParser parser = null;
            int value = parser;
        }

        [TestMethod]
        public void EnumTest()
        {
            var test = new EnumParser<Enum1>();
            test.Parse("D");
            Console.WriteLine(test.Value.GetHashCode());

            Enum1 e = test;
            Console.WriteLine(e.GetHashCode());

            e = (Enum1)test;
            Console.WriteLine(e.GetHashCode());

            var test2 = new EnumParser<Enum1>();
            test2.Parse("D");
            Console.WriteLine(test.Equals(test2));
            Console.WriteLine(test == test2);
            Console.WriteLine(e == test);
            Console.WriteLine(test.Equals(Enum1.D));
            Console.WriteLine(test == Enum1.D);
        }

        [TestMethod]
        public void IntTest()
        {
            var test = new IntParser();
            test.Parse("2");

            int num = test;
            Console.WriteLine(num);

            num = (int)test;
            Console.WriteLine(num);

            var test2 = new IntParser();
            test2.Parse("2");
            Console.WriteLine(test.Equals(test2));
            Console.WriteLine(test == test2);
            Console.WriteLine(test.Equals(2));
            Console.WriteLine(test == 2);
            Console.WriteLine(2 == test);
            Console.WriteLine(test == 1);
        }

        [TestMethod]
        public void FloatTest()
        {
            var test = new FloatParser();
            test.Parse("2");

            float num = test;
            Console.WriteLine(num);

            num = (float)test;
            Console.WriteLine(num);

            var test2 = new FloatParser();
            test2.Parse("2");
            Console.WriteLine(test.Equals(test2));
            Console.WriteLine(test == test2);
            Console.WriteLine(test.Equals(2));
            Console.WriteLine(test == 2);
            Console.WriteLine(2 == test);
            Console.WriteLine(2f == test);
            Console.WriteLine(test == 1);
        }

        [TestMethod]
        public void PairTest()
        {
            var t1 = new Pair<int, string>(1, "2");
            var t2 = new Pair<int, string>(1, "2");
            Console.WriteLine(t1.Equals(t2));
            Console.WriteLine(t1 == t2);
            Console.WriteLine(t1 != t2);

            var t3 = new PairParser<IntParser, StringParser>();
            t3.Parse("1|2");
            Console.WriteLine(t3.Value.Key + " " + t3.Value.Value);
            Console.WriteLine(t3 == t1);
            Console.WriteLine(t3 == t2);
            Console.WriteLine(t3.Equals(t1));
            Console.WriteLine(t3.Equals(t2));
        }

        [TestMethod]
        public void ArrayTest()
        {
            var t1 = new ArrayParser<FloatParser>();
            t1.Parse("1,2.0,9.8");
            Console.WriteLine(t1);
            Console.WriteLine(t1.Has(1));
            Console.WriteLine(t1.Has(2));
            Console.WriteLine(t1.Has(2f));
            Console.WriteLine(t1.Has(9.8f));
            Console.WriteLine(t1.Has(9f));
            Console.WriteLine(t1.Has(9f, (a, b) => (int)(float)a == (int)(float)b));
        }

        [TestMethod]
        public void ArrayTest2()
        {
            var t1 = new ArrayParser<PairParser<EnumParser<Enum1>, IntParser>>();
            t1.Parse("B|1,A|2,D|4,C|3,A|5,D|6");
            Console.WriteLine(t1);
            Console.WriteLine(t1.Has(Pair.Create(Enum1.A, 2)));
            Console.WriteLine(t1.Has(Pair.Create(Enum1.A, 3)));
            Console.WriteLine(t1.Has(Pair.Create(Enum1.A, 5)));

            var t2 = new PairParser<EnumParser<Enum1>, IntParser>();
            t2.Parse("A|5");
            Console.WriteLine(t1.Has(t2));
        }

        [TestMethod]
        public void Test2()
        {
            var test = new PairParser<EnumParser<Enum1>, IntParser>();
            test.Parse("D|2");
            var value = test.Value;
            Console.WriteLine("key " + value.Key);
            Console.WriteLine("value " + value.Value);

            PairParser<EnumParser<Enum1>, IntParser> kv1 = new Pair<EnumParser<Enum1>, IntParser>(Enum1.D, 2);
            Console.WriteLine(test.Equals(kv1));
            Console.WriteLine(test.Equals(new Pair<EnumParser<Enum1>, IntParser>(Enum1.D, 2)));
            Console.WriteLine(test.Equals(Pair.Create(Enum1.D, 2)));
        }

        [TestMethod]
        public void Test3()
        {
            var parser = new ArrayParser<PairParser<EnumParser<Enum1>, IntParser>>();
            parser.Parse("A|2,D|3,C|1,B|5,D|10");
            Console.WriteLine(parser);

            Pair<Enum1, int> kv1 = Pair.Create(Enum1.B, 2);
            Pair<Enum1, int> kv2 = Pair.Create(Enum1.B, 2);
            Console.WriteLine(kv1.Equals(kv2));

            PairParser<EnumParser<Enum1>, IntParser> kv3 = new Pair<EnumParser<Enum1>, IntParser>(Enum1.B, 5);
            Console.WriteLine(parser.Has(kv1));
            Console.WriteLine(parser.Has(kv2));
            Console.WriteLine(parser.Has(kv3));
        }

        [TestMethod]
        public void Test10()
        {
            float value = JsonConvert.DeserializeObject<float>("0.001");
            Console.WriteLine(value);
        }

        class Test4Data
        {
            public string A;
            public int B;

            public Test4Data(string a, int b)
            {
                A = a;
                B = b;
            }

            public override string ToString()
            {
                return $"A:{A} B:{B}";
            }

            public override bool Equals(object obj)
            {
                Test4Data other = obj as Test4Data;
                if (other != null)
                {
                    return A == other.A && B == other.B;
                }
                return base.Equals(obj);
            }
        }

        [TestMethod]
        public void Test4()
        {
            EntryTest.Exec(() =>
            {
                TupleParser<Test4Data> test = new TupleParser<Test4Data>();
                TupleParser<Test4Data> test2 = new TupleParser<Test4Data>();
                test.Parse("{{\"A\":\"T9\",\"B\":98259}}");
                test2.Parse("{{\"A\":\"T9\",\"B\":98259}}");
                Log.Debug(test == ValueTuple.Create(new Test4Data("T9", 98299)));
                Log.Debug(test == ValueTuple.Create(new Test4Data("T9", 98259)));

                TupleParser<Test4Data> test3 = new Test4Data("T9", 98259);
                Test4Data test4 = test3;
                Log.Debug(test == test3);
                Log.Debug(test);
                Log.Debug(test4);
                Log.Debug(test == test4);
            });
        }

        [TestMethod]
        public void Test5()
        {
            EntryTest.Exec(() =>
            {
                TupleParser<Test4Data, int> test = new TupleParser<Test4Data, int>();
                test.Parse("{{\"A\":\"T9\",\"B\":98259};999}");
                Log.Debug(test.Value.Item1);
                Log.Debug(test.Value.Item2);
            });
        }

        [TestMethod]
        public void MapTest()
        {
            EntryTest.Exec(() =>
            {
                Log.ToQueue = false;
                //MapParser<IntParser, StringParser> parser = new MapParser<IntParser, StringParser>();
                //parser.Parse("1|name1,2|name2");
                //Log.Debug(parser.Get(1));
                //Log.Debug(parser.Get(2));
                //Log.Debug(parser.Get(3));
            });
        }

        [TestMethod]
        public void UtilityTest()
        {
            Console.WriteLine(TypeUtility.GetSimpleName(typeof(EnumParser<Language>)));
            Console.WriteLine(TypeUtility.GetSimpleName(typeof(Action<Language, XCore>)));
        }

        [TestMethod]
        public void NumAreaTest1()
        {
            EntryTest.Exec(() =>
            {
                Log.ToQueue = false;
                AreaParser parser = References.Require<AreaParser>();
                parser.Parse("add#1-10@remove#3@add#190-192");
            });
        }

        [TestMethod]
        public void NumAreaTest2()
        {
            EntryTest.Exec(() =>
            {
                Log.ToQueue = false;
                Names names = References.Require<Names>();
                names.Parse("yanying_series#add~1-10@remove~3_layer#add~2-4_dir#add~l@add~r");
                foreach (string name in names)
                {
                    Console.WriteLine(name);
                }
                Console.WriteLine(names.Has("yanying_series#1_layer#2_dir#l"));
                Console.WriteLine(names.Has("yanying_series#1_layer#2_dir#a"));
                Console.WriteLine(names.Has(Name.Create("yanying_series#1_layer#2_dir#l")));
            });
        }

        [TestMethod]
        public void NameAreaTest3()
        {
            EntryTest.Exec(() =>
            {
                Log.ToQueue = false;
                Names names = References.Require<Names>();
                names.Parse("shen_chushi^layer#~shen_chushi^series#add!5-10@add!18-32");
                foreach (string name in names)
                {
                    Console.WriteLine(name);
                }
            });
        }
    }
}
