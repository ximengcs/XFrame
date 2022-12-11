using System;
using System.IO;
using System.Collections.Generic;
using XFrame.Core;

namespace XFrame.Modules
{
    public class ArchiveModule : SingleModule<ArchiveModule>
    {
        #region Inner Field
        private const string JSON_SUFFIX = ".jsonacv";
        private const string BIN_SUFFIX = ".binacv";
        private const int SAVE_KEY = 0;
        private const int SAVE_GAP = 60;

        private string m_RootPath;
        private CDTimer m_Timer;
        private Dictionary<string, IArchive> m_Archives;
        #endregion

        #region Life Fun
        public override void OnInit(object data)
        {
            base.OnInit(data);
            m_Timer = new CDTimer();
            m_Timer.Record(SAVE_KEY, SAVE_GAP);
            m_Archives = new Dictionary<string, IArchive>();
        }

        public override void OnUpdate(float escapeTime)
        {
            base.OnUpdate(escapeTime);
            if (m_Timer.Check(SAVE_KEY, true))
                InnerSaveAll();
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            InnerSaveAll();
        }

        private void OnApplicationFocus(bool focus)
        {
            if (!focus)
                InnerSaveAll();
        }
        #endregion

        #region Interface
        public void SetPath(string rootPath)
        {
            m_RootPath = rootPath;
            foreach (string file in Directory.EnumerateFiles(m_RootPath))
            {
                string suffix = Path.GetExtension(file).ToLower();
                string fileName = Path.GetFileNameWithoutExtension(file);
                if (suffix == JSON_SUFFIX)
                    GetOrNew<JsonArchive>(fileName);
                else if (suffix == BIN_SUFFIX)
                    GetOrNew<DataArchive>(fileName);
            }
        }

        public T GetOrNew<T>(string name) where T : IArchive
        {
            if (m_Archives.TryGetValue(name, out IArchive archieve))
            {
                return (T)archieve;
            }
            else
            {
                Type type = typeof(T);
                T source = (T)Activator.CreateInstance(type);
                source.Init(InnerGetPath(type, name));
                m_Archives.Add(name, source);
                return source;
            }
        }

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
            string fileName = default;
            if (type == typeof(JsonArchive))
                fileName = $"{name}{JSON_SUFFIX}";
            else if (type == typeof(DataArchive))
                fileName = $"{name}{BIN_SUFFIX}";
            return Path.Combine(m_RootPath, fileName);
        }

        private void InnerSaveAll()
        {
            foreach (IArchive archive in m_Archives.Values)
                archive.Save();
        }
        #endregion
    }
}
