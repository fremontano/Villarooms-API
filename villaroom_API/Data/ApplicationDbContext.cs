using Microsoft.EntityFrameworkCore;
using villaroom_API.models;

namespace villaroom_API.Data
{
    public class ApplicationDbContext :DbContext
    {


        //Constructor
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) 
        {
            
        }



        public DbSet<Villa> Villas { get; set; }



        //Crear registro en la base de datos
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Villa>().HasData(

        new Villa
        {
            Id = 1,
            Name = "Villa Las Mercedes",
            Tarifa = 200.0,
            Ocupantes = 4,
            Detalle = "Dettale Mercedes",
            MetrosCuadrado = 120,
            ImagenUrl = "image_url",
            Amenidad = "Piscina",
            FechaCreacion = new DateTime(2023, 1, 1),
            FechaActualizacion = new DateTime(2023, 1, 1)
        },
        new Villa
        {
            Id = 2,
            Name = "Villa El Oasis",
            Tarifa = 250.0,
            Ocupantes = 6,
            Detalle = "Dettale Oasis",
            MetrosCuadrado = 150,
            ImagenUrl = "image_url",
            Amenidad = "Gimnasio",
            FechaCreacion = new DateTime(2023, 1, 1),
            FechaActualizacion = new DateTime(2023, 1, 1)
        }
         
            );
        }

    }
}
