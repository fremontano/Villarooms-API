using System.Linq.Expressions;

namespace villaroom_API.Repositories.IRepositories
{
    public interface IRepository<T> where T : class
    {


        //Repositorio de tipo generico 
        Task Create(T entity);
        Task<List<T>> GetAll(Expression<Func<T, bool>>? filter = null);
        Task<T> Get(Expression<Func<T, bool>> filter = null, bool tracked = true);
        Task Delete(T entity);
        Task Save();

    }
}
