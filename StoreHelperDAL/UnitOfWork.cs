using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoreHelper.Dal.Core;
using StoreHelper.Dal.Core.Interfaces;

namespace StoreHelperDAL
{
    public class UnitOfWork : BaseUnitOfWork
    {
        private readonly DbContext m_DbContext;

        public UnitOfWork(DbContext dbContext)
            : base(dbContext)
        {
            m_DbContext = dbContext;
        }

        public override void BulkInsert<T>(IEnumerable<T> entities, int? batchSize = null)
        {
            throw new NotImplementedException();
        }

        protected override IRepository<TEntity, TKey> CreateSpecificRepository<TEntity, TKey>()
        {
            //if (typeof(TEntity) == typeof(Users))
            //    return new UsersRepository(m_DbContext) as IRepository<TEntity>;


            return base.CreateSpecificRepository<TEntity, TKey>();
        }



    }
}
