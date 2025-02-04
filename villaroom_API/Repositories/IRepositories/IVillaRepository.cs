using villaroom_API.models;

namespace villaroom_API.Repositories.IRepositories
{

    //Hereda de la Interfaz Repository generic
    //para nuestro modelo actualizar 
    public interface IVillaRepository :IRepository<Villa> 
    {
        Task<Villa> Update(Villa villa);    
    }
}
