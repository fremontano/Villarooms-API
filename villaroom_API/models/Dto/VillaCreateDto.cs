using System.ComponentModel.DataAnnotations;

namespace villaroom_API.models.Dto
{
    public class VillaCreateDto
    {


        [Required]
        [MaxLength(45)]
        public string Name { get; set; }

        public String Detalle { get; set; }

        [Required]
        public double Tarifa { get; set; }

        public int Ocupantes { get; set; }

        public int MetrosCuadrado { get; set; }

        public String ImagenUrl { get; set; }

        public String Amenidad { get; set; }
    }
}
