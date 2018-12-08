using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using StoreHelper.Dal.Core.Interfaces.Entity;

namespace StoreHelper.Dal.Core.Interfaces
{
    /// <summary>
    /// Defines a members for access to app's database
    /// </summary>
    public interface IEntitiesDbContext : ISaveable, IDisposable
    {
        
        Database Database { get; }
        /// <summary>
        /// Returns an IDbSet of <typeparamref name="TEntity"/>
        /// </summary>
        /// <typeparam name="TEntity">Type of entity, for which DbSet is required</typeparam>
        /// <returns></returns>
        IDbSet<TEntity> TryGetSet<TEntity, TKey>() where TEntity : class, IKeyable<TKey>;

        /// <summary>
        /// Set entity as created in DbContext
        /// </summary>
        /// <typeparam name="TEntity">Type of entity that is needed to add to db</typeparam>
        /// <param name="entity">Entity that is needed to add to db</param>
        void AddEntity<TEntity, TKey>(TEntity entity) where TEntity : class, IKeyable<TKey>;

        /// <summary>
        /// Set entity as modified in DbContext
        /// </summary>
        /// <typeparam name="TEntity">Type of entity that is needed to update</typeparam>
        /// <param name="entity">Entity that is needed to update</param>
        void UpdateEntity<TEntity, TKey>(TEntity entity) where TEntity : class, IKeyable<TKey>;
        
        /// <summary>
        /// Set entity as deleted in DbContext
        /// </summary>
        /// <typeparam name="TEntity">Type of entity that is needed to delete</typeparam>
        /// <param name="entity">Entity that is needed to delete</param>
        void DeleteEntity<TEntity, TKey>(TEntity entity) where TEntity : class, IKeyable<TKey>;
        /// <summary>
        /// Gets a System.Data.Entity.Infrastructure.DbEntityEntry`1 object for the given
        //     entity providing access to information about the entity and the ability to perform
        //     actions on the entity.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        DbEntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;
    }
}
