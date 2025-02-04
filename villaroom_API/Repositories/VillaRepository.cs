using villaroom_API.Data;
using villaroom_API.models;
using villaroom_API.Repositories.IRepositories;

namespace villaroom_API.Repositories
{

    // lo ponemos que herede del repositorio generico y el repositorio de actualizar
    public class VillaRepository : Repository<Villa>, IVillaRepository
    {
        private readonly ApplicationDbContext _context;

        public VillaRepository(ApplicationDbContext context) :base(context) 
        {
            _context = context;
        }



        public async Task<Villa>  Update(Villa entity)
        {
           entity.FechaCreacion = DateTime.Now;
            _context.Update(entity);

            await _context.SaveChangesAsync();  
            return entity;
        }

    }
}
