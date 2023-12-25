﻿using System;
using XFrame.Core;

namespace XFrame.Modules.Archives
{
    internal class DefaultSaver : IModuleHandler
    {
        public Type Target => typeof(ISaveable);

        public void Handle(IModule module, object data)
        {
            ISaveable saver = module as ISaveable;
            saver.OnSave();
        }
    }
}
