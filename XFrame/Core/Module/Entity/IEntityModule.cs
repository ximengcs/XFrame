using System;
using XFrame.Core;
using XFrame.Modules.Containers;

namespace XFrame.Modules.Entities
{
    /// <summary>
    /// 实体模块
    /// 只有根实体才会接受实体模块的更新生命周期
    /// </summary>
    public interface IEntityModule : IModule
    {
        /// <summary>
        /// 注册实体，创建实体前需要注册实体
        /// </summary>
        /// <typeparam name="T">实体基类或实体类</typeparam>
        void RegisterEntity<T>() where T : class, IEntity;

        /// <summary>
        /// 根据Id获取实体
        /// </summary>
        /// <param name="entityId">实体Id</param>
        /// <returns>实体</returns>
        IEntity Get(int entityId);

        /// <summary>
        /// 创建实体
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="onReady">数据提供委托</param>
        /// <returns>创建的实体</returns>
        T Create<T>(OnDataProviderReady onReady = null) where T : class, IEntity;

        /// <summary>
        /// 创建实体
        /// </summary>
        /// <param name="type">实体类型</param>
        /// <param name="onReady">数据提供委托</param>
        /// <returns>创建的实体</returns>
        IEntity Create(Type type, OnDataProviderReady onReady = null);

        /// <summary>
        /// 创建实体
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="parent">父实体</param>
        /// <param name="onReady">数据提供委托</param>
        /// <returns>创建的实体</returns>
        T Create<T>(IEntity parent, OnDataProviderReady onReady = null) where T : class, IEntity;

        /// <summary>
        /// 创建实体
        /// </summary>
        /// <param name="type">实体类型</param>
        /// <param name="parent">父实体</param>
        /// <param name="onReady">数据提供委托</param>
        /// <returns>创建的实体</returns>
        IEntity Create(Type type, IEntity parent, OnDataProviderReady onReady = null);

        /// <summary>
        /// 创建实体
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="typeId">类型Id</param>
        /// <param name="onReady">数据提供委托</param>
        /// <returns>创建的实体</returns>
        T Create<T>(int typeId, OnDataProviderReady onReady = null) where T : class, IEntity;

        /// <summary>
        /// 创建实体
        /// </summary>
        /// <param name="baseType">实体类型</param>
        /// <param name="typeId">类型Id</param>
        /// <param name="onReady">数据提供委托</param>
        /// <returns>创建的实体</returns>
        IEntity Create(Type baseType, int typeId, OnDataProviderReady onReady = null);

        /// <summary>
        /// 创建实体
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="parent">父实体</param>
        /// <param name="typeId">类型Id</param>
        /// <param name="onReady">数据提供委托</param>
        /// <returns>创建的实体</returns>
        T Create<T>(IEntity parent, int typeId, OnDataProviderReady onReady = null) where T : class, IEntity;

        /// <summary>
        /// 创建实体
        /// </summary>
        /// <param name="baseType">实体类型</param>
        /// <param name="parent">父实体</param>
        /// <param name="typeId">类型Id</param>
        /// <param name="onReady">数据提供委托</param>
        /// <returns>创建的实体</returns>
        IEntity Create(Type baseType, IEntity parent, int typeId, OnDataProviderReady onReady = null);

        /// <summary>
        /// 销毁一个实体
        /// </summary>
        /// <param name="entity">需要销毁的实体</param>
        void Destroy(IEntity entity);
    }
}
