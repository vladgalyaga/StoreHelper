using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;
using StoreHelper.Dal.Core.Interfaces;
using StoreHelper.Dal.Core.Interfaces.Entity;

namespace StoreHelper.Dal.Core
{
    public delegate void DisposeMe(string typeName);
    public abstract class BaseUnitOfWork : IUnitOfWork
    {
        private readonly Hashtable m_Repositories;
        private IEntitiesDbContext m_DbContext;
        private bool m_IsDisposed;

        public BaseUnitOfWork(DbContext dbContext)
        {
            m_DbContext = new DbContextWrapper(dbContext);
            m_Repositories = new Hashtable();
        }

        public IEntitiesDbContext DbContext
        {
            get { return m_DbContext; }
        }

        public IRepository<TEntity, TKey> GetRepository<TEntity, TKey>(bool createSpecificRepositiry = true) where TEntity : class, IKeyable<TKey>
        {
            string typeName = typeof(TEntity).Name;
            if (createSpecificRepositiry)
            {

                if (m_Repositories.ContainsKey(typeName))
                {
                    return (IRepository<TEntity, TKey>)m_Repositories[typeName];
                }
            }
            IRepository<TEntity, TKey> repository = CreateRepository<TEntity, TKey>(createSpecificRepositiry);
            repository.DisposeMe += DisposeRepository;
            if (createSpecificRepositiry)
            {
                m_Repositories.Add(typeName, repository);
            }
            return repository;
        }

        protected virtual IRepository<TEntity, TKey> CreateSpecificRepository<TEntity, TKey>() where TEntity : class, IKeyable<TKey>
        {
            return null;
        }

        private IRepository<TEntity, TKey> CreateRepository<TEntity, TKey>(bool createSpecificRepositiry) where TEntity : class, IKeyable<TKey>
        {
            IRepository<TEntity, TKey> repository = null;
            if (createSpecificRepositiry)
            {
                repository = CreateSpecificRepository<TEntity, TKey>();
            }
         
            if (repository != null)
            {
                return repository;
            }
            Type repositoryType = typeof(EntityRepository<,>);

            repository = (IRepository<TEntity, TKey>)Activator.CreateInstance(
               repositoryType.MakeGenericType(typeof(TEntity), typeof(TKey)), m_DbContext);


            return repository;
        }
        public void DisposeRepository(string typeName)
        {
            m_Repositories.Remove(typeName);
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!m_IsDisposed && disposing)
            {
                m_DbContext.Dispose();

                List<IDisposable> repositories = new List<IDisposable>();
                foreach (IDisposable repository in m_Repositories.Values)
                {
                    //dispose all repositries
                    //  repository.Dispose();
                    repositories.Add(repository);
                }
                foreach (IDisposable repository in repositories)
                {
                    repository.Dispose();
                }
            }

            m_IsDisposed = true;
        }

        public int SaveChanges() => m_DbContext.SaveChanges();
        public Task<int> SaveChangesAsync() => m_DbContext.SaveChangesAsync();

        public int ExecuteSqlCommand(string sql, params Object[] parameters)
        {
            return m_DbContext.Database.ExecuteSqlCommand(sql, parameters);
        }
        public int ExecuteSqlCommand(TransactionalBehavior transactionakBehavior, String sql, params Object[] parameters)
        {
            return m_DbContext.Database.ExecuteSqlCommand(transactionakBehavior, sql, parameters);
        }
        public abstract void BulkInsert<T>(IEnumerable<T> entities, int? batchSize = default(int?));

        public DbEntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class
        {
            return m_DbContext.Entry(entity);

        }
        public int CallStoredProcedure(string nameOfProcedure, params int[] parameters)
        {
            return m_DbContext.Database.SqlQuery<int>($@"DECLARE @tmp INT;
                    EXECUTE @tmp = [Data].[{nameOfProcedure}]
                    {string.Join(",", parameters)};
                    Select @tmp; ").Single();
        }

        public List<T> CallFunction<T>(string nameOfFunction, params string[] parameters)
        {
            var sql = $@"SELECT * FROM {nameOfFunction} ({string.Join(",", parameters.Select(m => $"'{m}'"))})";
            return m_DbContext.Database.SqlQuery<T>(sql).ToList();

        }
    }
}
