using System;
using System.IO;
using XFrame.Core;
using XFrame.Modules.Reflection;
using XFrame.Modules.Times;
using XFrame.Modules.Config;
using XFrame.Modules.Crypto;
using System.Collections.Generic;
using XFrame.Collections;

namespace XFrame.Modules.Archives
{
    /// <summary>
    /// 存档模块
    /// </summary>
    [CoreModule]
    [RequireModule(typeof(CryptoModule))]
    [XType(typeof(IArchiveModule))]
    public class ArchiveModule : ModuleBase, IArchiveModule
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
            m_Timer = CDTimer.Create();
            m_Timer.MarkName = nameof(ArchiveModule);
            m_Timer.Record(SAVE_KEY, SAVE_GAP);
            m_Archives = new Dictionary<string, IArchive>();
            m_ArchiveTypes = new Dictionary<string, Type>();

            if (!string.IsNullOrEmpty(XConfig.ArchiveUtilityHelper))
            {
                Type type = XModule.Type.GetType(XConfig.ArchiveUtilityHelper);
                IArchiveUtilityHelper helper = (IArchiveUtilityHelper)XModule.Type.CreateInstance(type);
                ArchiveUtility.Helper = helper;
            }
            InnerInit();
        }

        private void InnerInit()
        {
            TypeSystem system = XModule.Type.GetOrNewWithAttr<ArchiveAttribute>();
            foreach (Type type in system)
                InnerAddType(type);
            InnerRefreshFiles();
        }

        private void InnerAddType(Type type)
        {
            ArchiveAttribute attri = XModule.Type.GetAttribute<ArchiveAttribute>(type);
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

        public void OnUpdate(float escapeTime)
        {
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
        public T GetOrNew<T>(string name, object param = null) where T : IArchive
        {
            return (T)InnerGetOrNew(name, typeof(T), param);
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

        public void Delete(IArchive archive)
        {
            if (m_Archives.ContainsKey(archive.Name))
            {
                archive.Delete();
                m_Archives.Remove(archive.Name);
            }
        }

        public void DeleteAll()
        {
            foreach (IArchive archive in m_Archives.Values)
            {
                archive.Delete();
            }
            m_Archives.Clear();
        }
        #endregion

        #region Inner Implement
        private string InnerGetPath(Type type, string name)
        {
            ArchiveAttribute attri = XModule.Type.GetAttribute<ArchiveAttribute>(type);
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
                    InnerGetOrNew(fileName, archiveType, null);
            }
        }

        private IArchive InnerGetOrNew(string name, Type archiveType, object param)
        {
            if (m_Archives.TryGetValue(name, out IArchive archieve))
            {
                return archieve;
            }
            else
            {
                IArchive source = (IArchive)XModule.Type.CreateInstance(archiveType);
                source.OnInit(InnerGetPath(archiveType, name), name, param);
                m_Archives.Add(name, source);
                return source;
            }
        }
        #endregion
    }
}
