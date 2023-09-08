using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XFrameTest
{
    [TestClass]
    public class JsonTest
    {
        [Serializable]
        public class A
        {
            public int[] test = new int[10] {1,2,3,4,5,6,7,8,9,10};
        }

        [TestMethod]
        public void Test1()
        {
            string json = "{\"test\":[1,2,3,4,5]}";
            A a = JsonConvert.DeserializeObject<A>(json);
            Console.WriteLine(a.test.Length);
        }
    }
}
