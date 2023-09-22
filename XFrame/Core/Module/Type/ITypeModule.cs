
using System;
using XFrame.Core;

namespace XFrame.Modules.XType
{
    public interface ITypeModule : IModule
    {
        T CreateInstance<T>(params object[] args);
        object CreateInstance(Type type, params object[] args);
        object CreateInstance(string typeName, params object[] args);
        void LoadAssembly(byte[] data);
        void OnTypeChange(Action handler);
        Type GetType(string name);
        TypeSystem GetOrNewWithAttr<T>() where T : Attribute;
        TypeSystem GetOrNewWithAttr(Type pType);
        bool HasAttribute<T>(Type classType) where T : Attribute;
        bool HasAttribute(Type classType, Type pType);
        T GetAttribute<T>(Type classType) where T : Attribute;
        Attribute GetAttribute(Type classType, Type pType);
        TypeSystem GetOrNew<T>() where T : class;
        TypeSystem GetOrNew(Type baseType);
        Type[] GetAllType();
    }
}
