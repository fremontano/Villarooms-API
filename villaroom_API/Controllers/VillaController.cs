using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using villaroom_API.Data;
using villaroom_API.models;
using villaroom_API.models.Dto;
using villaroom_API.Repositories.IRepositories;

namespace villaroom_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VillaController : ControllerBase
    {

        //inyeccion de dependencias
        private readonly ILogger<VillaController> _logger;
        private readonly IVillaRepository _villarepo;
        private readonly IMapper _mapper;
        
        //no hay necesida de inyectarlo, lo inicializamos
        protected APIResponse _response;



        public VillaController(ILogger<VillaController> logger, IVillaRepository villaRepo, IMapper mapper)
        { 
           _logger = logger;
           _villarepo = villaRepo;
           _mapper = mapper;
           _response = new ();
        }



        //Obtener villas 
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetVilla()
        {

            try {
                _logger.LogInformation("Obteniendos todas las villas");

                IEnumerable<Villa> VillaList = await _villarepo.GetAll();
                _response.Result = _mapper.Map<IEnumerable<VillaDto>>(VillaList);

                _response.statusCode = HttpStatusCode.OK;
                _response.IsSuccessful = true;

                return Ok(_response);
            }
            catch (Exception ex)
            {

                _response.IsSuccessful = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };

            }

            return _response;
         
           
        }





        //Obtener villa por id 
        [HttpGet("id:int", Name = "GetVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>>GetVilla(int id)
        {

            try
            {
                if (id == 0)
                {
                    _logger.LogError("Error al traer Villa con el Id " + id);
                    _response.statusCode =HttpStatusCode.BadRequest;
                    _response.IsSuccessful = false;
                    return BadRequest(_response);
                }

                //var villa = VillaStore.GetVillaList.FirstOrDefault(v => v.Id == id);
                var villa = await _villarepo.Get(x => x.Id == id);

                if (villa == null)
                {
                    _response.statusCode = HttpStatusCode.NotFound;
                    _response.IsSuccessful = false;
                    return NotFound();
                }

                _response.Result = _mapper.Map<VillaDto>(villa);
                _response.statusCode = HttpStatusCode.OK;
                _response.IsSuccessful = true;
                return Ok(_response);

            }
            catch (Exception ex)
            {
                _response.IsSuccessful = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }

            return _response;
        }


        //Crear villa
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> CreateVilla([FromBody]VillaCreateDto createDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                if (await _villarepo.Get(v => v.Name.ToLower() == createDto.Name.ToLower()) != null)
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

                //antes de guardar, grabamos los nuevos registros
                modelo.FechaCreacion = DateTime.Now;
                modelo.FechaActualizacion = DateTime.Now;

                await _villarepo.Create(modelo);

                _response.Result = modelo;
                _response.statusCode = HttpStatusCode.Created;

                return CreatedAtRoute("GetVilla", new { id = modelo.Id }, _response);

            }
            catch(Exception ex)
            {
                _response.IsSuccessful = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }

            return _response;
        }



        //Eliminar villa 
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteVilla(int id)
        {

            try
            {

                if (id == 0)
                {
                    _response.IsSuccessful = false;
                    _response.statusCode=HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                var villa = await _villarepo.Get(v => v.Id == id);

                if (villa == null)
                {
                    _response.IsSuccessful = false;
                    _response.statusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                await _villarepo.Delete(villa);

                _response.statusCode = HttpStatusCode.NoContent;
                _response.IsSuccessful = true;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccessful = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }

            return BadRequest(_response);
        }




        //Actualizar villa
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateVilla(int id, [FromBody] VillaUpdateDto updateDto)
        {


            if (updateDto == null || updateDto.Id!= id)
            {
                _response.IsSuccessful = false;
                _response.statusCode=HttpStatusCode.BadRequest; 
                return BadRequest(_response);
            }


           Villa modelo = _mapper.Map<Villa>(updateDto);

          await  _villarepo.Update(modelo);
            _response.statusCode = HttpStatusCode.NoContent;

            return Ok(_response);
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
            var villa = await _villarepo.Get(v => v.Id == id, tracked:false);
            VillaUpdateDto villadDto = _mapper.Map<VillaUpdateDto>(villa);

            if (villa == null) return BadRequest();

            patchDto.ApplyTo(villadDto, ModelState);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Villa modelo = _mapper.Map<Villa>(villadDto);

          await  _villarepo.Update(modelo);
            _response.statusCode = HttpStatusCode.NoContent;
            return Ok(_response);
        }
    }
}
