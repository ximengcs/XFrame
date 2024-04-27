using System;
using XFrame.Core;
using XFrame.Collections;
using XFrame.Modules.Reflection;
using System.Collections.Generic;

namespace XFrame.Modules.Serialize
{
    /// <inheritdoc/>
    [CoreModule]
    [XType(typeof(ISerializeModule))]
    public class SerializeModule : ModuleBase, ISerializeModule
    {
        private Dictionary<int, ISerializeHelper> m_Helpers;

        /// <inheritdoc/>
        protected override void OnInit(object data)
        {
            base.OnInit(data);

            m_Helpers = new Dictionary<int, ISerializeHelper>();
            TypeSystem typeSys = Domain.TypeModule.GetOrNew<ISerializeHelper>();
            foreach (Type type in typeSys)
            {
                InnerInit(type);
            }
        }

        #region Interface
        /// <inheritdoc/>
        public object DeserializeToObject(string text, Type type)
        {
            return DeserializeToObject(text, default, type);
        }

        /// <inheritdoc/>
        public object DeserializeToObject(string text, int textType, Type type)
        {
            if (m_Helpers.TryGetValue(textType, out ISerializeHelper helper))
                return helper.Deserialize(text, type);
            return default;
        }

        /// <inheritdoc/>
        public T DeserializeToObject<T>(string text)
        {
            return DeserializeToObject<T>(text, default);
        }

        /// <inheritdoc/>
        public T DeserializeToObject<T>(string text, int textType)
        {
            if (m_Helpers.TryGetValue(textType, out ISerializeHelper helper))
                return helper.Deserialize<T>(text);
            return default;
        }

        /// <inheritdoc/>
        public string SerializeObjectToRaw(object obj)
        {
            return SerializeObjectToRaw(obj, default);
        }

        /// <inheritdoc/>
        public string SerializeObjectToRaw(object obj, int textType)
        {
            if (m_Helpers.TryGetValue(textType, out ISerializeHelper helper))
                return helper.Serialize(obj);
            return default;
        }
        #endregion

        private void InnerInit(Type type)
        {
            ISerializeHelper helper = Domain.TypeModule.CreateInstance(type) as ISerializeHelper;
            helper.OnInit(this);
            m_Helpers.Add(helper.HandleType, helper);
        }
    }
}
