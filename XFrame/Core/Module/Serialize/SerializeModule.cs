using System;
using XFrame.Core;
using XFrame.Collections;
using XFrame.Modules.Reflection;
using System.Collections.Generic;

namespace XFrame.Modules.Serialize
{
    /// <summary>
    /// 序列化模块
    /// </summary>
    [CoreModule]
    [XType(typeof(ISerializeModule))]
    public class SerializeModule : ModuleBase, ISerializeModule
    {
        private Dictionary<int, ISerializeHelper> m_Helpers;

        protected override void OnInit(object data)
        {
            base.OnInit(data);

            m_Helpers = new Dictionary<int, ISerializeHelper>();
            TypeSystem typeSys = ModuleUtility.Type.GetOrNew<ISerializeHelper>();
            foreach (Type type in typeSys)
            {
                InnerInit(type);
            }
        }

        #region Interface
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="text">text本文</param>
        /// <param name="type">目标类型</param>
        /// <returns>序列化到的对象</returns>
        public object DeserializeToObject(string text, Type type)
        {
            return DeserializeToObject(text, default, type);
        }

        public object DeserializeToObject(string text, int textType, Type type)
        {
            if (m_Helpers.TryGetValue(textType, out ISerializeHelper helper))
                return helper.Deserialize(text, type);
            return default;
        }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="T">目标类型</typeparam>
        /// <param name="text">text本文</param>
        /// <returns>序列化到的对象</returns>
        public T DeserializeToObject<T>(string text)
        {
            return DeserializeToObject<T>(text, default);
        }

        public T DeserializeToObject<T>(string text, int textType)
        {
            if (m_Helpers.TryGetValue(textType, out ISerializeHelper helper))
                return helper.Deserialize<T>(text);
            return default;
        }

        /// <summary>
        /// 序列化 
        /// </summary>
        /// <param name="obj">需要序列化的对象</param>
        /// <returns>json本文</returns>
        public string SerializeObjectToRaw(object obj)
        {
            return SerializeObjectToRaw(obj, default);
        }

        public string SerializeObjectToRaw(object obj, int textType)
        {
            if (m_Helpers.TryGetValue(textType, out ISerializeHelper helper))
                return helper.Serialize(obj);
            return default;
        }
        #endregion

        private void InnerInit(Type type)
        {
            ISerializeHelper helper = ModuleUtility.Type.CreateInstance(type) as ISerializeHelper;
            m_Helpers.Add(helper.HandleType, helper);
        }
    }
}
