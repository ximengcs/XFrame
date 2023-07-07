﻿
using System.Reflection;
using XFrame.Modules.Diagnotics;
using XFrame.Modules.XType;

namespace XFrameTest
{
    [TestClass]
    public class ActivatorTest
    {
        private class C1
        {
            protected C1()
            {
                Log.Debug("protected");
            }

            public C1(int i)
            {
                Log.Debug("public " + i);
            }

            private C1(float i)
            {
                Log.Debug("private " + i);
            }

            internal C1(bool i)
            {
                Log.Debug("internal " + i);
            }

            private C1(int i, int i2)
            {
                Log.Debug("private " + i + " " + i2);
            }
        }

        [TestMethod]
        public void Test1()
        {
            EntryTest.Exec(() =>
            {
                Log.Debug(TypeModule.Inst.CreateInstance<C1>() == null);
                Log.Debug(TypeModule.Inst.CreateInstance<C1>(9) == null);
                Log.Debug(TypeModule.Inst.CreateInstance<C1>(9.9f) == null);
                Log.Debug(TypeModule.Inst.CreateInstance<C1>(true) == null);
                Log.Debug(TypeModule.Inst.CreateInstance(typeof(C1).FullName, 1, 2) == null);
            });
        }

        [TestMethod]
        public void Test2()
        {
            Type t1 = typeof(int);
            Type t2 = typeof(float);
            Console.WriteLine(t1.IsAssignableFrom(t2));
            Console.WriteLine(t2.IsAssignableFrom(t1));
            Console.WriteLine(t1 == t2);
        }
    }
}
