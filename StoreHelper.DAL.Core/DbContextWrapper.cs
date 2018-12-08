using System;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;
using StoreHelper.Dal.Core.Interfaces;
using StoreHelper.Dal.Core.Interfaces.Entity;

namespace StoreHelper.Dal.Core
{
    public sealed class DbContextWrapper : IEntitiesDbContext
    {
        private DbContext m_DbContext;
        private static readonly object m_Lock = new object();

        public DbContextWrapper(DbContext dbContext)
        {
            this.DbContext = dbContext;
        }

        public DbContext DbContext
        {
            get
            {
                return m_DbContext;
            }
            set
            {
                if (value != null)
                {
                    m_DbContext = value;
                }
                else throw new ArgumentNullException(nameof(DbContext));
            }
        }

        private bool IsSetExist<TEntity, TKey>() where TEntity : class, IKeyable<TKey>
        {
            string entityName = typeof(TEntity).Name;
            var objectContext = ((IObjectContextAdapter)DbContext).ObjectContext;

            return objectContext.MetadataWorkspace
                .GetItems<EntityType>(DataSpace.CSpace)
                .Any(p => p.Name.Equals(entityName));
        }

        public IDbSet<TEntity> TryGetSet<TEntity, TKey>() where TEntity : class, IKeyable<TKey>
        {
            return IsSetExist<TEntity, TKey>()
                ? DbContext.Set<TEntity>()
                : null;
        }


        public void AddEntity<TEntity, TKey>(TEntity entity) where TEntity : class, IKeyable<TKey>
        {
            lock (m_Lock)
            {
                DbContext.Set<TEntity>().Add(entity);
            }
        }

        public void UpdateEntity<TEntity, TKey>(TEntity entity) where TEntity : class, IKeyable<TKey>
        {
            lock (m_Lock)
            {
                IDbSet<TEntity> set = TryGetSet<TEntity, TKey>();
                var result = set.Find(entity.Id);//.FirstOrDefault(p => p.Id.Equals(entity.Id));

                if (result != null)
                {
                    DbContext.Entry(result).CurrentValues.SetValues(entity);
                    DbContext.Entry(result).State = EntityState.Modified;
                }

            }
        }

        public void DeleteEntity<TEntity, TKey>(TEntity entity) where TEntity : class, IKeyable<TKey>
        {
            lock (m_Lock)
            {
                TryGetSet<TEntity, TKey>().Remove(entity);
            }
        }
        public void Dispose() => DbContext.Dispose();

        public int SaveChanges() => DbContext.SaveChanges();
        public Task<int> SaveChangesAsync() => DbContext.SaveChangesAsync();

        public DbEntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class
        {
            return DbContext.Entry(entity);
        }
        public Database Database { get { return m_DbContext.Database; } }
    }
}
