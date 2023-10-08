
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
using System.Diagnostics;
using XFrame.Modules.Datas;
using YamlDotNet.Serialization;

namespace XFrameTest
{
    [TestClass]
    public class YamlTest
    {
        public class YamlText
        {
            public string T1;
            public List<int> T2;
        }

        public class Prop
        {
            public string itemid;
            public string itemicon;
            public int itemorder;
            public string itemunlock;
            public string itemtype;
            public bool isdraw;
            public string itemcolor;
            public int itemprice;
            public int itemlevel;
            public bool multipleoptions;
        }

        [TestMethod]
        public void Test1()
        {
            string text = File.ReadAllText("E:\\Proj\\dressii\\DressIIUnity\\Assets\\ResAll\\config\\ABTest_E\\common_prop_list.json"); 
            Stopwatch sw = Stopwatch.StartNew();
            List<Prop> prop = JsonConvert.DeserializeObject<List<Prop>>(text);
            sw.Stop();
            Console.WriteLine(sw.ElapsedMilliseconds);

            sw.Restart();
            Serializer serializer = new Serializer();
            string yaml = serializer.Serialize(prop, typeof(List<Prop>));
            sw.Stop();  
            Console.WriteLine(sw.ElapsedMilliseconds);

            sw.Restart();
            Deserializer deserializer = new Deserializer();
            deserializer.Deserialize<List<Prop>>(yaml);
            sw.Stop();
            Console.WriteLine(sw.ElapsedMilliseconds);

            sw.Restart();
            JsonConvert.SerializeObject(prop);
            sw.Stop();
            Console.WriteLine(sw.ElapsedMilliseconds);
        }

        [TestMethod]
        public void Test2()
        {

        }
    }
}
