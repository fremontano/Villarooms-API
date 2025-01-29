using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using villaroom_API.Data;
using villaroom_API.models;
using villaroom_API.models.Dto;

namespace villaroom_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VillaController : ControllerBase
    {

        //inyeccion de dependencias
        private readonly ILogger<VillaController> _logger;
        private readonly ApplicationDbContext _dbContext;





        public VillaController(ILogger<VillaController> logger, ApplicationDbContext dbContext)
        { 
           _logger = logger;
            _dbContext = dbContext;
        }



        //Obtener villas 
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<VillaDto>> GetVilla()
        {
            _logger.LogInformation("Obteniendos todas las villas");
            return Ok(_dbContext.Villas.ToList());
        }





        //Obtener villa por id 
        [HttpGet("id:int", Name = "GetVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<VillaDto> GetVilla(int id)
        {

            if (id == 0)
            {
                _logger.LogError("Error al traer Villa con el Id " + id);
                return BadRequest();
            }

            //var villa = VillaStore.GetVillaList.FirstOrDefault(v => v.Id == id);
            var villa = _dbContext.Villas.FirstOrDefault(x => x.Id == id);  

            if (villa == null)
            {
                return NotFound();
            }

            return Ok(villa);
        }


        //Crear villa
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<VillaDto> CreateVilla([FromBody]VillaDto villaDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            if (_dbContext.Villas.FirstOrDefault(v => v.Name.ToLower() == villaDto.Name.ToLower()) != null)
            {
                ModelState.AddModelError("NombreExiste", "Ya existe una villa con ese nombre.");
                return BadRequest(ModelState);
            }



            if (villaDto == null)
            {
                return BadRequest(villaDto);
            }
            if (villaDto.Id > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            //Crear un nuevo modelo en base a la villa 
            Villa modelo = new()
            {

                Name = villaDto.Name,
                Detalle = villaDto.Detalle,
                ImagenUrl = villaDto.ImagenUrl,
                Ocupantes = villaDto.Ocupantes,
                Amenidad = villaDto.Amenidad,
                Tarifa = villaDto.Tarifa,
                MetrosCuadrado = villaDto.MetrosCuadrado
            };

            _dbContext.Villas.Add(modelo);
            _dbContext.SaveChanges();

            return  CreatedAtRoute("GetVilla", new {id = villaDto.Id}, villaDto);
        }



        //Eliminar villa 
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public  IActionResult DeleteVilla(int id)
        {

            if (id == 0)
            {
                return BadRequest();
            }

            var villa = _dbContext.Villas.FirstOrDefault(v => v.Id == id);

            if (villa  ==  null)
            {
                return NotFound();
            }

           _dbContext.Villas.Remove(villa);
            _dbContext.SaveChanges();

            return NoContent();
        }


        //Actualizar villa
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult UpdateVilla(int id, [FromBody] VillaDto villaDto)
        {


            if (villaDto == null || villaDto.Id!= id)
            {
                return BadRequest();
            }

            //var villa = VillaStore.GetVillaList.FirstOrDefault( v => v.Id == id);
            //villa.Name = villaDto.Name;
            //villa.MetrosCuadrado = villaDto.MetrosCuadrado;
            //villa.Ocupantes = villaDto.Ocupantes;

            Villa modelo = new()
            {
                Id = villaDto.Id,
                Name = villaDto.Name,
                Detalle = villaDto.Detalle,
                ImagenUrl = villaDto.ImagenUrl,
                MetrosCuadrado = villaDto.MetrosCuadrado,
                Ocupantes = villaDto.Ocupantes,
                Amenidad = villaDto.Amenidad

            };

            _dbContext.Villas.Update(modelo);
            _dbContext.SaveChanges();

            return NoContent();
        }




        //Actualizar villa con pacth
        [HttpPatch("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult UpdatePacthVilla(int id, JsonPatchDocument<VillaDto> patchDto)
        {


            if (patchDto == null || id == 0)
            {
                return BadRequest();
            }

            //var villa = VillaStore.GetVillaList.FirstOrDefault(v => v.Id == id);
            var villa = _dbContext.Villas.AsNoTracking().FirstOrDefault(v => v.Id == id);

            VillaDto villaDto = new()
            {
                Id = villa.Id,
                Name = villa.Name,
                Detalle = villa.Detalle,
                MetrosCuadrado = villa.MetrosCuadrado,
                ImagenUrl = villa.ImagenUrl,
                Tarifa = villa.Tarifa,
                Ocupantes = villa.Ocupantes,
                Amenidad = villa.Amenidad
            };

            if (villa == null) return BadRequest();

            patchDto.ApplyTo(villaDto, ModelState);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Villa modelo = new()
            {
                Id = villaDto.Id,
                Name = villaDto.Name,
                Detalle = villaDto.Detalle,
                MetrosCuadrado = villaDto.MetrosCuadrado,
                ImagenUrl = villaDto.ImagenUrl,
                Tarifa= villaDto.Tarifa,
                Ocupantes = villaDto.Ocupantes,
                Amenidad = villaDto.Amenidad
            };


            _dbContext.Update(modelo);
            _dbContext.SaveChanges();

            return NoContent();
        }


    }
}
