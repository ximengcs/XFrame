using Newtonsoft.Json;
using XFrame.Modules;
using XFrame.SimpleJSON;

namespace XFrameTest
{
    public class Json1
    {
        public int Test1 = 9;
        public string Test2 = "982598";
        public bool Test3 = true;
        public List<int> Test4 = new List<int>() { 1, 34, 434 };
    }

    [TestClass]
    public class ArchiveTest
    {
        [TestMethod]
        public void TestJson()
        {
            JsonArchive archive = new JsonArchive();
            archive.OnInit("C:\\Users\\XM\\Desktop\\data\\test.json");
            archive.SetFloat("f1", 1.0f);
            archive.SetInt("i1", 2);
            archive.SetBool("b1", true);
            JSONArray array = archive.GetOrNewArray("a1");
            for (int i = 0; i < 10; i++)
            {
                array[i] = i;
            }

            archive.Save();
        }

        [TestMethod]
        public void TestJson2()
        {
            JsonArchive archive = new JsonArchive();
            archive.OnInit("C:\\Users\\XM\\Desktop\\data\\test.json");
            Console.WriteLine(archive.GetFloat("f1"));
            Console.WriteLine(archive.GetInt("i1"));
            Console.WriteLine(archive.GetBool("b1"));
            JSONArray array = archive.GetOrNewArray("a1");
            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine(array[i].AsInt);
            }
        }

        [TestMethod]
        public void TestJson3()
        {
            object obj = new Json1();
            string json = JsonConvert.SerializeObject(obj);
            Console.WriteLine(json);
            JSONNode node = JSONNode.Parse(json);
            Console.WriteLine(node["Test1"].AsInt);
            Console.WriteLine(node["Test2"].Value);
            Console.WriteLine(node["Test3"].AsBool);

            JSONArray array = node["Test4"].AsArray;
            foreach (JSONNode n in array)
            {
                Console.WriteLine(n.AsInt);
            }
        }

        [TestMethod]
        public void TestJson4()
        {
            JsonArchive archive = new JsonArchive();
            archive.OnInit("C:\\Users\\XM\\Desktop\\data\\test.json");

            Json1 json = new Json1();
            archive.Set("json1", json);
            archive.Save();
        }

        [TestMethod]
        public void TestJson5()
        {
            JsonArchive archive = new JsonArchive();
            archive.OnInit("C:\\Users\\XM\\Desktop\\data\\test.json");
            Json1 json = archive.Get<Json1>("json1");
            Console.WriteLine(json.Test1);
            Console.WriteLine(json.Test2);
            Console.WriteLine(json.Test3);
            Console.WriteLine(archive.GetInt("1111"));
        }
    }
}
