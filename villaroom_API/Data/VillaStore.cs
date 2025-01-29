using villaroom_API.models.Dto;

namespace villaroom_API.Data
{
    public class VillaStore
    {
        public static List<VillaDto> GetVillaList = new List<VillaDto>
        {
            new VillaDto { Id = 1, Name = "Villa Las Mercedes", Ocupantes = 4, MetrosCuadrado = 120 },
            new VillaDto { Id = 2, Name = "Villa El Oasis", Ocupantes = 6, MetrosCuadrado = 150 },
            new VillaDto { Id = 3, Name = "Villa La Paz", Ocupantes = 5, MetrosCuadrado = 110 },
            new VillaDto { Id = 4, Name = "Villa Jardín del Mar", Ocupantes = 8, MetrosCuadrado = 200 },
            new VillaDto { Id = 5, Name = "Villa Los Pinos", Ocupantes = 3, MetrosCuadrado = 100 },
            new VillaDto { Id = 6, Name = "Villa El Sol", Ocupantes = 4, MetrosCuadrado = 130 },
            new VillaDto { Id = 7, Name = "Villa Los Álamos", Ocupantes = 7, MetrosCuadrado = 160 }
        };

    };
};
