﻿using XFrame.Core;
using XFrame.Modules.ID;
using XFrame.Modules.Pools;
using XFrame.Modules.XType;

namespace XFrameTest
{
    [TestClass]
    public class BaseModuleInit
    {
        [TestMethod]
        public void Test1()
        {
            XCore core = XCore.Create(typeof(TypeModule), typeof(IdModule), typeof(PoolModule));
        }
    }
}
