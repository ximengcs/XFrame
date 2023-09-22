using XFrame.Core;
using XFrame.Modules.Datas;

namespace XFrameTest
{
    [TestClass]
    public class DataTableTest
    {
        class Data1 : IDataRaw
        {
            public int Id;
            public string Name;

            public override string ToString()
            {
                return $"{Id},{Name}";
            }
        }

        [Data(TableType.Object)]
        class Data2 : IDataRaw
        {
            public string Name;
            public string Icon;

            public override string ToString()
            {
                return $"{Name},{Icon}";
            }
        }

        [TestMethod]
        public void Test2()
        {
            EntryTest.Exec(() =>
            {
                string content = "{\"Name\":\"Simon\", \"Icon\":\"icon_simon\"}";
                IDataTable<Data2> table = ModuleUtility.Data.Add<Data2>(content);
                Console.WriteLine("Add");
                Console.WriteLine(table);

                table = ModuleUtility.Data.Get<Data2>();
                Console.WriteLine("Get");
                Console.WriteLine(table);

                table = ModuleUtility.Data.Get<Data2>(0);
                Console.WriteLine("Get2");
                Console.WriteLine(table);

                Data2 data1 = ModuleUtility.Data.GetOne<Data2>();
                Console.WriteLine("Get One");
                Console.WriteLine(data1);

                data1 = ModuleUtility.Data.GetItem<Data2>(2);
                Console.WriteLine("GetItem");
                Console.WriteLine(data1);
            });
        }

        [TestMethod]
        public void Test1()
        {
            EntryTest.Exec(() =>
            {
                string content = "[" +
                "{\"Id\":1, \"Name\":\"Simon\"}," +
                "{\"Id\":2, \"Name\":\"Simon2\"}," +
                "{\"Id\":3, \"Name\":\"Simon2\"}" +
                "]";
                IDataTable<Data1> table = ModuleUtility.Data.Add<Data1>(content);
                Console.WriteLine("Add");
                Console.WriteLine(table);

                table = ModuleUtility.Data.Get<Data1>();
                Console.WriteLine("Get");
                Console.WriteLine(table);

                table = ModuleUtility.Data.Get<Data1>(0);
                Console.WriteLine("Get2");
                Console.WriteLine(table);

                Data1 data1 = ModuleUtility.Data.GetOne<Data1>();
                Console.WriteLine("Get One");
                Console.WriteLine(data1);

                data1 = ModuleUtility.Data.GetItem<Data1>(2);
                Console.WriteLine("GetItem");
                Console.WriteLine(data1);

                List<Data1> list = new List<Data1>();
                int count = table.Select("Name", "Simon2", list);
                Console.WriteLine("Count " + count);
                foreach (Data1 data in list)
                    Console.WriteLine(data);

            });
        }
    }
}
