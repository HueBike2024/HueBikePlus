using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Core.Interfaces.Repositories
{
    public interface IBaseRepository<T> where T : class
    {
        /// <summary>
        /// Find an entity with expression
        /// </summary>
        /// <param name="expression"></param>
        /// <returns>Entity</returns>
        T Find(Expression<Func<T, bool>> expression);
        T Find(Expression<Func<T, bool>> expression, Expression<Func<T, object>>[] includeExpressions);

        /// <summary>
        /// Find an entity with expression
        /// </summary>
        /// <param name="expression"></param>
        /// <returns>Entity</returns>
        Task<T> FindAsync(Expression<Func<T, bool>> expression);
        Task<T> FindAsync(Expression<Func<T, bool>> expression, Expression<Func<T, object>>[] includeExpressions);

        /// <summary>
        /// Find all entities in a table
        /// </summary>
        /// <param name="expression"></param>
        /// <returns>List of entities</returns>
        IQueryable<T> All();
        IQueryable<T> All(Expression<Func<T, object>>[] includeExpressions);

        /// <summary>
        /// Find entities with expression
        /// </summary>
        /// <param name="expression"></param>
        /// <returns>List of entities</returns>
        IQueryable<T> List(Expression<Func<T, bool>> expression);
        IQueryable<T> List(Expression<Func<T, bool>> expression, Expression<Func<T, object>>[] includeExpressions);
        /// <summary>
        /// Update an Entity
        /// </summary>
        /// <param name="entity">Entity need to update</param>
        void Update(T entity);

        /// <summary>
        /// Update a list of Entities
        /// </summary>
        /// <param name="entities">Entities need to update</param>
        void UpdateRange(IEnumerable<T> entities);

        /// <summary>
        /// Delete an Entity
        /// </summary>
        /// <param name="entity">Entity need to Delete</param>
        void Delete(T entity);

        /// <summary>
        /// Delete a list of Entities
        /// </summary>
        /// <param name="entities">List entities need to Delete</param>
        void DeleteRange(IEnumerable<T> entities);

        /// <summary>
        /// Delete an Entity
        /// </summary>
        /// <param name="entity">Entity need to Delete</param>
        void DeleteReal(T entity);

        /// <summary>
        /// Delete a list of Entities
        /// </summary>
        /// <param name="entities">List entities need to Delete</param>
        void DeleteRealRange(IEnumerable<T> entities);

        /// <summary>
        /// Add new Entity
        /// </summary>
        /// <param name="entity">New Entity</param>
        void Insert(T entity);

        /// <summary>
        /// Add a list of Entities
        /// </summary>
        /// <param name="entities">New Entities</param>
        void InsertRange(IEnumerable<T> entities);

        /// <summary>
        /// Execute SqlCommand
        /// </summary>
        /// <param name="sql">SqlCommand</param>
        /// <param name="parameters">Sql parameters</param>
        /// <returns></returns>
        int ExecuteCommand(string sql, params object[] parameters);

        /// <summary>
        /// Execute SqlCommand
        /// </summary>
        /// <param name="sql">SqlCommand</param>
        /// <param name="parameters">Sql parameters</param>
        /// <returns></returns>
        Task<int> ExecuteCommandAsync(string sql, params object[] parameters);

        /// <summary>
        /// Execute Sql Qeury
        /// </summary>
        /// <typeparam name="TResult">Type of result returned</typeparam>
        /// <param name="sql">SqlQuery</param>
        /// <param name="commandType">SP or Text</param>
        /// <param name="parameters">Sql parameters</param>
        /// <returns></returns>
        IEnumerable<TResult> ExecuteQuery<TResult>(string sql
            , CommandType commandType = CommandType.Text
            , params object[] parameters
            ) where TResult : new();

        /// <summary>
        /// Execute Sql Query
        /// </summary>
        /// <typeparam name="TResult">Type of result returned</typeparam>
        /// <param name="sql">SqlQuery</param>
        /// <param name="commandType">SP or Text</param>
        /// <param name="parameters">Sql parameters</param>
        /// <returns></returns>
        Task<IEnumerable<TResult>> ExecuteQueryAsync<TResult>(string sql
            , CommandType commandType = CommandType.Text
            , params object[] parameters
            ) where TResult : new();
        DataSet ExecuteQueryReturnDataSet(string queryString, CommandType cmdType = CommandType.Text,
            params SqlParameter[] parameters);

        // Get the lastest value from db
        void Reload(T entity);
    }
}
