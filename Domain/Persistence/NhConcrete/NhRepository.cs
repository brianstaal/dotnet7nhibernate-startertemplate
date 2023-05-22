using Domain.Persistence.Abstract;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Persistence.NhConcrete
{
    public class NhRepository : IRepository
    {
        protected readonly ISession Session;
        protected readonly ISessionFactory SessionFactory;
        protected ITransaction Transaction;

        public NhRepository(ISession session, ISessionFactory sessionFactory)
        {
            Session = session;
            SessionFactory = sessionFactory;
        }

        public void BeginTransaction()
        {
            Transaction = Session.BeginTransaction();
        }

        public void Commit()
        {
            try
            {
                Session.GetCurrentTransaction().Commit();
                Session.Flush();
            }
            catch (Exception)
            {
                Session.GetCurrentTransaction().Rollback();
                throw;
            }
        }

        public void ResetSession()
        {
            Session.Clear();
        }

        public void Rollback()
        {
            Transaction.Rollback();
        }

        public void CloseTransaction()
        {
            if (Transaction != null)
            {
                Transaction.Dispose();
                Transaction = null;
            }
        }

        #region Helper Methods
        protected void SaveItem(object toSave)
        {
            try
            {
                Session.BeginTransaction();
                Session.Save(toSave);
            }
            catch (Exception)
            {
                Session.GetCurrentTransaction().Rollback();
                throw;
            }
        }

        protected async Task SaveItemsAsync(ICollection<object> objectItems)
        {
            using var statelessSession = SessionFactory.OpenStatelessSession();
            using var transaction = statelessSession.BeginTransaction();

            foreach (var o in objectItems)
            {
                await statelessSession.InsertAsync(o);
            }

            await transaction.CommitAsync();
            transaction.Dispose();
            statelessSession.Dispose();

        }

        protected void UpdateItem(object toUpdate)
        {
            try
            {
                Session.BeginTransaction();
                Session.Update(toUpdate);
            }
            catch (Exception)
            {
                Session.GetCurrentTransaction().Rollback();
                throw;
            }
        }

        protected void DeleteItem(object toDelete)
        {
            try
            {
                Session.BeginTransaction();
                Session.Delete(toDelete);
            }
            catch (Exception)
            {
                Session.GetCurrentTransaction().Rollback();
                throw;
            }
        }

        #endregion
    }
}
