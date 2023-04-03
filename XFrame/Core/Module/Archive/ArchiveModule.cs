using System;
using System.IO;
using XFrame.Core;
using XFrame.Utility;
using XFrame.Modules.XType;
using XFrame.Modules.Times;
using XFrame.Modules.Config;
using XFrame.Modules.Crypto;
using System.Collections.Generic;

namespace XFrame.Modules.Archives
{
    /// <summary>
    /// 存档模块
    /// </summary>
    [CoreModule]
    [RequireModule(typeof(CryptoModule))]
    public class ArchiveModule : SingletonModule<ArchiveModule>
    {
        #region Inner Field
        private const int SAVE_KEY = 0;
        private const int SAVE_GAP = 60;

        private string m_RootPath;
        private CDTimer m_Timer;
        private Dictionary<string, IArchive> m_Archives;
        private Dictionary<string, Type> m_ArchiveTypes;
        #endregion

        #region Life Fun
        protected override void OnInit(object data)
        {
            base.OnInit(data);
            m_Timer = new CDTimer();
            m_Timer.Record(SAVE_KEY, SAVE_GAP);
            m_Archives = new Dictionary<string, IArchive>();
            m_ArchiveTypes = new Dictionary<string, Type>();

            InnerInit();
        }

        private void InnerInit()
        {
            TypeSystem system = TypeModule.Inst.GetOrNewWithAttr<ArchiveAttribute>();
            foreach (Type type in system)
                InnerAddType(type);
            InnerRefreshFiles();
        }

        private void InnerAddType(Type type)
        {
            ArchiveAttribute attri = TypeUtility.GetAttribute<ArchiveAttribute>(type);
            if (attri != null)
            {
                if (!m_ArchiveTypes.ContainsKey(attri.Suffix))
                    m_ArchiveTypes.Add(attri.Suffix, type);
            }
        }

        private void InnerRefreshFiles()
        {
            m_RootPath = XConfig.ArchivePath;
            if (!string.IsNullOrEmpty(m_RootPath))
            {
                if (Directory.Exists(m_RootPath))
                    InnerInitRootPath();
                else
                    Directory.CreateDirectory(m_RootPath);
            }
        }

        protected override void OnUpdate(float escapeTime)
        {
            base.OnUpdate(escapeTime);
            if (m_Timer.Check(SAVE_KEY, true))
                InnerSaveAll();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            InnerSaveAll();
        }
        #endregion

        #region Interface
        /// <summary>
        /// 获取或创建一个存档实例
        /// </summary>
        /// <typeparam name="T">存档类型</typeparam>
        /// <param name="name">存档名</param>
        /// <returns>存档实例</returns>
        public T GetOrNew<T>(string name) where T : IArchive
        {
            return (T)InnerGetOrNew(name, typeof(T));
        }

        /// <summary>
        /// 保存
        /// </summary>
        public void Save()
        {
            InnerSaveAll();
        }

        /// <summary>
        /// 删除一份存档
        /// </summary>
        /// <param name="name"></param>
        public void Delete(string name)
        {
            if (m_Archives.TryGetValue(name, out IArchive source))
            {
                source.Delete();
                m_Archives.Remove(name);
            }
        }
        #endregion

        #region Inner Implement
        private string InnerGetPath(Type type, string name)
        {
            ArchiveAttribute attri = TypeUtility.GetAttribute<ArchiveAttribute>(type);
            return Path.Combine(m_RootPath, $"{name}{attri.Suffix}");
        }

        private void InnerSaveAll()
        {
            foreach (IArchive archive in m_Archives.Values)
                archive.Save();
        }

        private void InnerInitRootPath()
        {
            foreach (string file in Directory.EnumerateFiles(m_RootPath))
            {
                string suffix = Path.GetExtension(file).ToLower();
                string fileName = Path.GetFileNameWithoutExtension(file);
                if (m_ArchiveTypes.TryGetValue(suffix, out Type archiveType))
                    InnerGetOrNew(fileName, archiveType);
            }
        }

        private IArchive InnerGetOrNew(string name, Type archiveType)
        {
            if (m_Archives.TryGetValue(name, out IArchive archieve))
            {
                return archieve;
            }
            else
            {
                IArchive source = (IArchive)Activator.CreateInstance(archiveType);
                source.OnInit(InnerGetPath(archiveType, name));
                m_Archives.Add(name, source);
                return source;
            }
        }
        #endregion
    }
}
