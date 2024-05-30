
using System.Collections.Generic;
using XFrame.Core;
using XFrame.Core.Threads;
using XFrame.Modules.Containers;
using XFrame.Modules.ID;

namespace XFrame.Modules.Entities
{
    [CommonModule]
    public class SceneModule : ModuleBase
    {
        private Dictionary<int, IScene> m_Scenes;

        protected override void OnInit(object data)
        {
            base.OnInit(data);
            m_Scenes = new Dictionary<int, IScene>();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            List<IScene> list = new List<IScene>(m_Scenes.Values);
            m_Scenes = null;
            foreach (IScene scene in list)
                scene.OnDestroy();
        }

        public IScene Get(int id)
        {
            if (m_Scenes.TryGetValue(id, out IScene scene))
                return scene;
            return null;
        }

        public IScene Create(int id, Fiber fiber = null)
        {
            if (m_Scenes.ContainsKey(id))
                return m_Scenes[id];
            if (fiber == null)
                fiber = Domain.GetModule<FiberModule>().MainFiber;
            Scene scene = Entry.AddModule<Scene>(id, fiber);
            ContainerModule containerModule = Entry.AddModule<ContainerModule>(id);
            scene.RegisterUseModule(typeof(IContainerModule), id);
            containerModule.RegisterUseModule(typeof(IEntityModule), id);
            m_Scenes.Add(id, scene);
            return scene;
        }

        public IScene Create(Fiber fiber = null)
        {
            int id = Domain.GetModule<IIdModule>().Next();
            return Create(id, fiber);
        }

        public void Destroy(IScene scene)
        {
            int key = scene.Id;
            if (m_Scenes.ContainsKey(key))
            {
                m_Scenes.Remove(key);
                Entry.RemoveModule<Scene>(key);
                Entry.RemoveModule<ContainerModule>(key);
            }
        }
    }
}
