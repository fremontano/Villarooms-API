using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using villaroom_API.Data;
using villaroom_API.Repositories.IRepositories;


namespace villaroom_API.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {


        // El patron de repositorio es  útil para separar la logica de acceso a datos del controladors
        // evitando exponer directamente el DbContext en los controladores.

        //variables privadas 
        private readonly ApplicationDbContext _context;
        internal DbSet<T> dbSet;


        //inyectar el DbContext 
        public Repository(ApplicationDbContext context)
        {
            _context = context;
            this.dbSet = _context.Set<T>();
        }






        public async Task Create(T entity)
        {
            await dbSet.AddAsync(entity);
            await Save();
        }

       

        public async Task<T> Get(Expression<Func<T, bool>> filter = null, bool tracked = true)
        {
            IQueryable<T> query = dbSet;

            if (!tracked)
            {
                query = query.AsNoTracking(); 
            }

            if (filter != null)
            {
                query = query.Where(filter);
            }

            return await query.FirstOrDefaultAsync();
        }

        public async Task<List<T>> GetAll(Expression<Func<T, bool>>? filter = null)
        {
            IQueryable<T> query = dbSet;
            if (filter != null)
            {
                query = query.Where(filter);
            }
            return await query.ToListAsync();

        }


        public async Task Delete(T entity)
        {
            dbSet.Remove(entity);
            await Save();
        }


        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }
    }
}
