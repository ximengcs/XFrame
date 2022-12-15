using XFrame.Modules.ID;
using XFrame.Modules.Pools;
using XFrame.Modules.XType;
using XFrame.Modules.Entities;
using NUnit.Framework.Internal;

namespace XFrameTest
{
    public class Entity1 : Entity
    {
        protected override void OnInit(EntityData data)
        {
            Console.WriteLine("OnInit");
        }

        protected override void OnUpdate(float elapseTime)
        {
            Console.WriteLine("OnUpdate");
        }

        protected override void OnDestroy()
        {
            Console.WriteLine("OnDestroy");
        }

        protected override void OnDelete()
        {
            Console.WriteLine("OnDelete");
        }
    }

    public class Entity2 : Entity
    {
        protected override void OnDelete()
        {
            Console.WriteLine("OnDelete 2");
        }

        protected override void OnDestroy()
        {
            Console.WriteLine("OnDestroy 2");
        }

        protected override void OnInit(EntityData data)
        {
            Console.WriteLine("OnInit 2");
        }

        protected override void OnUpdate(float elapseTime)
        {
            Console.WriteLine("OnUpdate 2");
        }
    }

    [TestClass]
    public class EntityTest
    {
        [TestMethod]
        public void Test1()
        {
            new IdModule().OnInit(default);
            new PoolModule().OnInit(default);
            new TypeModule().OnInit(default);
            EntityModule module = new EntityModule();
            module.OnInit(default);
            module.RegisterEntity<Entity>();

            Entity1 entity = module.Create<Entity1>(null);
            entity.Add<Entity2>();
            module.OnUpdate(0.1f);
            module.Destroy(entity);
            module.OnDestroy();
        }
    }
}
