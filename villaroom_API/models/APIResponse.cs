using Microsoft.AspNetCore.JsonPatch.Internal;
using System.Net;

namespace villaroom_API.models
{
    public class APIResponse
    {

        // Codigo de estado HTTP de la respuesta
        public HttpStatusCode statusCode { get; set; }

        public bool IsSuccessful { get; set; }


        // Lista de mensajes de error para los endpoints
        public List<string> ErrorMessages { get; set; }



        // Puede contener cualquier tipo de resultado lista u objeto
        public object Result { get; set; }
    }
}
