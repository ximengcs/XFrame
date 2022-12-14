using XFrame.Core;
using XFrame.Modules;

namespace XFrameTest
{
    public class Entity1 : Entity
    {
        protected override void OnInit(EntityData data)
        {

        }

        protected override void OnUpdate(float elapseTime)
        {

        }

        protected override void OnDestroy()
        {

        }
    }

    [TestClass]
    public class EntityTest
    {
        [TestMethod]
        public void Test1()
        {
            new PoolModule().OnInit(default);
            new TypeModule().OnInit(default);
            EntityModule module = new EntityModule();
            module.OnInit(default);
            module.RegisterEntity<Entity>();

            Entity1 entity = module.Create<Entity1>(null);
        }
    }
}
