using System;

namespace XFrame.Modules.Entities
{
    /// <summary>
    /// 实体属性
    /// 有数据的实体必须生命此属性并提供正确的类型
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class EntityPropAttribute : Attribute
    {
        /// <summary>
        /// 类型Id
        /// </summary>
        public int Type { get; }

        /// <summary>
        /// 标记实体
        /// 类型为0
        /// </summary>
        public EntityPropAttribute()
        {
            Type = 0;
        }

        /// <summary>
        /// 标记实体
        /// </summary>
        /// <param name="type">实体类型Id</param>
        public EntityPropAttribute(int type)
        {
            Type = type;
        }
    }
}
