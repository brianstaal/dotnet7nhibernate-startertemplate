namespace Domain.Persistence.Abstract
{
    public interface IRepository
    {
        void BeginTransaction();

        void Commit();

        void ResetSession();

        void Rollback();

        void CloseTransaction();
    }
}
