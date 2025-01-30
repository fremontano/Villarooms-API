using AutoMapper;
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
        private readonly IMapper _mapper;



        public VillaController(ILogger<VillaController> logger, ApplicationDbContext dbContext, IMapper mapper)
        { 
           _logger = logger;
           _dbContext = dbContext;
           _mapper = mapper;
        }



        //Obtener villas 
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<VillaDto>>> GetVilla()
        {
            _logger.LogInformation("Obteniendos todas las villas");
            IEnumerable<Villa> VillaList = await _dbContext.Villas.ToListAsync();
            return Ok(_mapper.Map<IEnumerable<VillaDto>>(VillaList));
        }





        //Obtener villa por id 
        [HttpGet("id:int", Name = "GetVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<VillaDto>>GetVilla(int id)
        {

            if (id == 0)
            {
                _logger.LogError("Error al traer Villa con el Id " + id);
                return BadRequest();
            }

            //var villa = VillaStore.GetVillaList.FirstOrDefault(v => v.Id == id);
            var villa =await _dbContext.Villas.FirstOrDefaultAsync(x => x.Id == id);  

            if (villa == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<VillaDto>(villa));
        }


        //Crear villa
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<VillaDto>> CreateVilla([FromBody]VillaCreateDto createDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            if (await _dbContext.Villas.FirstOrDefaultAsync(v => v.Name.ToLower() == createDto.Name.ToLower()) != null)
            {
                ModelState.AddModelError("NombreExiste", "Ya existe una villa con ese nombre.");
                return BadRequest(ModelState);
            }


            if (createDto == null)
            {
                return BadRequest(createDto);
            }

            //Crear un nuevo modelo en base a la villa, con mapper
            Villa modelo = _mapper.Map<Villa>(createDto);

            //Crear un nuevo modelo en base a la villa mappeo propieda por peopieda, sin mapper
            //Villa modelo = new()
            //{

            //    Name = createDto.Name,
            //    Detalle = createDto.Detalle,
            //    ImagenUrl = createDto.ImagenUrl,
            //    Ocupantes = createDto.Ocupantes,
            //    Amenidad = createDto.Amenidad,
            //    Tarifa = createDto.Tarifa,
            //    MetrosCuadrado = createDto.MetrosCuadrado
            //};

            await _dbContext.Villas.AddAsync(modelo);
           await _dbContext.SaveChangesAsync();

            return  CreatedAtRoute("GetVilla", new {id = modelo.Id}, modelo);
        }



        //Eliminar villa 
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteVilla(int id)
        {

            if (id == 0)
            {
                return BadRequest();
            }

            var villa = await _dbContext.Villas.FirstOrDefaultAsync(v => v.Id == id);

            if (villa  ==  null)
            {
                return NotFound();
            }

           _dbContext.Villas.Remove(villa);//no tiene metodo async
            await _dbContext.SaveChangesAsync();

            return NoContent();
        }


        //Actualizar villa
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateVilla(int id, [FromBody] VillaUpdateDto updateDto)
        {


            if (updateDto == null || updateDto.Id!= id)
            {
                return BadRequest();
            }

            //var villa = VillaStore.GetVillaList.FirstOrDefault( v => v.Id == id);
            //villa.Name = villaDto.Name;
            //villa.MetrosCuadrado = villaDto.MetrosCuadrado;
            //villa.Ocupantes = villaDto.Ocupantes;

            Villa modelo = _mapper.Map<Villa>(updateDto);

            //Villa modelo = new()
            //{
            //    Id = villaDto.Id,
            //    Name = villaDto.Name,
            //    Detalle = villaDto.Detalle,
            //    ImagenUrl = villaDto.ImagenUrl,
            //    MetrosCuadrado = villaDto.MetrosCuadrado,
            //    Ocupantes = villaDto.Ocupantes,
            //    Amenidad = villaDto.Amenidad

            //};

            _dbContext.Villas.Update(modelo);//no es un metodo async
            await _dbContext.SaveChangesAsync();

            return NoContent();
        }




        //Actualizar villa con pacth
        [HttpPatch("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdatePacthVilla(int id, JsonPatchDocument<VillaUpdateDto> patchDto)
        {

            if (patchDto == null || id == 0)
            {
                return BadRequest();
            }

            //var villa = VillaStore.GetVillaList.FirstOrDefault(v => v.Id == id);
            var villa = await _dbContext.Villas.AsNoTracking().FirstOrDefaultAsync(v => v.Id == id);


            VillaUpdateDto villadDto = _mapper.Map<VillaUpdateDto>(villa);

            if (villa == null) return BadRequest();

            patchDto.ApplyTo(villadDto, ModelState);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Villa modelo = _mapper.Map<Villa>(villadDto);

            _dbContext.Update(modelo);
            await _dbContext.SaveChangesAsync();
            return NoContent();
        }
    }
}
