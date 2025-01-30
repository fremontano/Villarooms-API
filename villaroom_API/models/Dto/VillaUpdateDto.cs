using System.ComponentModel.DataAnnotations;

namespace villaroom_API.models.Dto
{
    public class VillaUpdateDto
    {


        [Required]  
        public int Id { get; set; }


        [Required]
        [MaxLength(45)]
        public string Name { get; set; }

        public String Detalle { get; set; }

        [Required]
        public double Tarifa { get; set; }

        [Required]
        public int Ocupantes { get; set; }

        [Required]
        public int MetrosCuadrado { get; set; }

        [Required]
        public String ImagenUrl { get; set; }

        public String Amenidad { get; set; }
    }
}
