
using System.Collections.Generic;
using XFrame.Core;
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

        public IScene Create(int id)
        {
            if (m_Scenes.ContainsKey(id))
                return m_Scenes[id];
            return Entry.AddModule<Scene>(id);
        }

        public IScene Create()
        {
            int id = Domain.GetModule<IIdModule>().Next();
            return Create(id);
        }

        public void Destroy(IScene scene)
        {
            if (m_Scenes.ContainsKey(scene.Id))
            {
                m_Scenes.Remove(scene.Id);
                Entry.RemoveModule<Scene>(scene.Id);
            }
        }
    }
}
