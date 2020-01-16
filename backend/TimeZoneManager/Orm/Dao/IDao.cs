using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace TimeZoneManager.Orm.Dao
{
    /// <summary>
    /// Generic Data Access Object (ORM independent)
    /// </summary>
    /// <typeparam name="TEntity">Entity<typeparamref name="string"/></typeparam>
    public interface IDao<TEntity>
        where TEntity : Entity
    {
        /// <summary>
        /// Creates entry in the database
        /// </summary>
        /// <param name="entity">Entity to create</param>
        /// <returns></returns>
        TEntity Create(TEntity entity);

        /// <summary>
        /// Creates the list of entities in the database
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        ICollection<TEntity> Create(ICollection<TEntity> entities);

        /// <summary>
        /// Updates entity in the database
        /// </summary>
        /// <param name="entity">Entity to update</param>
        TEntity Update(TEntity entity);

        /// <summary>
        /// Deletes entity from the Database
        /// </summary>
        /// <param name="entity">entity to delete</param>
        void Delete(TEntity entity);

        /// <summary>
        /// Deletes entity by Key
        /// </summary>
        /// <param name="key">Entity Key</param>
        void Delete(string key);

        /// <summary>
        /// Delete list of entities by AppId
        /// </summary>
        /// <param name="keyList"></param>
        void Delete(ICollection<string> keyList);

        /// <summary>
        /// Loads by Key. Returns null if it does not exist
        /// </summary>
        /// <param name="key">Entity Key</param>
        /// <returns></returns>
        TEntity Load(string key);

        /// <summary>
        /// Load multiple entities by key
        /// </summary>
        /// <param name="keyList"></param>
        /// <returns></returns>
        ICollection<TEntity> Load(ICollection<string> keyList);

        /// <summary>
        /// Load all entities
        /// </summary>
        /// <returns></returns>
        ICollection<TEntity> LoadAll();

        /// <summary>
        /// Load all entities which fullfil a given predicate
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        ICollection<TEntity> FindWhere(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Returns if there is any entity that satisfies the condition of the predicate
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        bool Any(Expression<Func<TEntity, bool>> predicate);
    }
}
