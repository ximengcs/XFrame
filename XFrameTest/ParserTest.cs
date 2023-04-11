﻿using XFrame.Core;

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
            Console.WriteLine(t1.Has(new Pair<Enum1, int>(Enum1.A, 2)));
            Console.WriteLine(t1.Has(new Pair<Enum1, int>(Enum1.A, 3)));
            Console.WriteLine(t1.Has(new Pair<Enum1, int>(Enum1.A, 5)));

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
            Console.WriteLine(test.Equals(new Pair<Enum1, int>(Enum1.D, 2)));
        }

        [TestMethod]
        public void Test3()
        {
            var parser = new ArrayParser<PairParser<EnumParser<Enum1>, IntParser>>();
            parser.Parse("A|2,D|3,C|1,B|5,D|10");
            Console.WriteLine(parser);

            KeyValuePair<Enum1, int> kv1 = new KeyValuePair<Enum1, int>(Enum1.B, 2);
            KeyValuePair<Enum1, int> kv2 = new KeyValuePair<Enum1, int>(Enum1.B, 2);
            Console.WriteLine(kv1.Equals(kv2));

            PairParser<EnumParser<Enum1>, IntParser> kv3 = new Pair<EnumParser<Enum1>, IntParser>(Enum1.B, 5);
            Console.WriteLine(parser.Has(kv1));
            Console.WriteLine(parser.Has(kv2));
            Console.WriteLine(parser.Has(kv3));
        }
    }
}