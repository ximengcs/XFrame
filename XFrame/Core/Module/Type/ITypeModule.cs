
using System;
using XFrame.Core;

namespace XFrame.Modules.Reflection
{
    /// <summary>
    /// 类型模块
    /// </summary>
    public interface ITypeModule : IModule
    {
        /// <summary>
        /// 创建对象实例
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="args">构造参数</param>
        /// <returns>对象实例</returns>
        T CreateInstance<T>(params object[] args);

        /// <summary>
        /// 创建对象实例
        /// </summary>
        /// <param name="type">对象类型</param>
        /// <param name="args">构造参数</param>
        /// <returns>对象实例</returns>
        object CreateInstance(Type type, params object[] args);

        /// <summary>
        /// 创建对象实例
        /// </summary>
        /// <param name="typeName">对象类型名</param>
        /// <param name="args">构造参数</param>
        /// <returns>对象实例</returns>
        object CreateInstance(string typeName, params object[] args);

        /// <summary>
        /// 加载程序集
        /// </summary>
        /// <param name="data">程序集</param>
        void LoadAssembly(byte[] data);

        /// <summary>
        /// 注册类型改变事件
        /// </summary>
        /// <param name="handler">处理函数</param>
        void OnTypeChange(Action handler);

        /// <summary>
        /// 通过名称获取类型
        /// </summary>
        /// <param name="name">类型名</param>
        /// <returns>类型</returns>
        Type GetType(string name);

        /// <summary>
        /// 获取(不存在时创建)一个类型系统
        /// 类型都具有所给定的属性类
        /// </summary>
        /// <typeparam name="T">Attribute属性类</typeparam>
        /// <returns>获取到的类型系统</returns>
        TypeSystem GetOrNewWithAttr<T>() where T : Attribute;

        /// <summary>
        /// 获取(不存在时创建)一个类型系统
        /// 类型都具有所给定的属性类
        /// </summary>
        /// <param name="pType">Attribute属性类</param>
        /// <returns>获取到的类型系统</returns>
        TypeSystem GetOrNewWithAttr(Type pType);

        /// <summary>
        /// 检查类型是否存在特性
        /// </summary>
        /// <typeparam name="T">特性类型</typeparam>
        /// <param name="classType">目标类型</param>
        /// <returns>true为存在特性</returns>
        bool HasAttribute<T>(Type classType) where T : Attribute;

        /// <summary>
        /// 检查类型是否存在特性
        /// </summary>
        /// <param name="classType">目标类型</param>
        /// <param name="pType">特性类型</param>
        /// <returns>true为存在特性</returns>
        bool HasAttribute(Type classType, Type pType);

        /// <summary>
        /// 获取类型的所有特性
        /// </summary>
        /// <typeparam name="T">特性类型</typeparam>
        /// <param name="classType">目标类型</param>
        /// <returns>特性数组</returns>
        T[] GetAttributes<T>(Type classType) where T : Attribute;

        /// <summary>
        /// 获取类型的特性
        /// </summary>
        /// <typeparam name="T">特性类型</typeparam>
        /// <param name="classType">目标类型</param>
        /// <returns>特性</returns>
        T GetAttribute<T>(Type classType) where T : Attribute;

        /// <summary>
        /// 获取类型的所有特性
        /// </summary>
        /// <param name="classType">目标类型</param>
        /// <param name="pType">特性类型</param>
        /// <returns>特性数组</returns>
        Attribute[] GetAttributes(Type classType, Type pType);

        /// <summary>
        /// 获取类型的特性
        /// </summary>
        /// <param name="classType">目标类型</param>
        /// <param name="pType">特性类型</param>
        /// <returns>特性</returns>
        Attribute GetAttribute(Type classType, Type pType);

        /// <summary>
        /// 获取(不存在时创建)一个类型系统
        /// 类型都是所给定的类型或子类
        /// </summary>
        /// <typeparam name="T">基类</typeparam>
        /// <returns>获取到的类型系统</returns>
        TypeSystem GetOrNew<T>() where T : class;

        /// <summary>
        /// 获取(不存在时创建)一个类型系统
        /// 类型都是所给定的类型或子类
        /// </summary>
        /// <param name="baseType">基类</param>
        /// <returns>获取到的类型系统</returns>
        TypeSystem GetOrNew(Type baseType);

        /// <summary>
        /// 获取所有类型
        /// </summary>
        /// <returns>类型列表</returns>
        Type[] GetAllType();
    }
}
