namespace Lab_07_Roman_Qquelcca.Repository.Unit;

public interface IUnitOfWork
{
    IGenericRepository<TEntity> Repository<TEntity>() where TEntity : class;
    Task<int> Complete();
}