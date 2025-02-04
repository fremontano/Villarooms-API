using villaroom_API;
using villaroom_API.Data;
using Microsoft.EntityFrameworkCore;
using villaroom_API.Repositories.IRepositories;
using villaroom_API.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddNewtonsoftJson();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


//Configuarcion del servicio dbContext 
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});



// Configuración de AutoMapper
builder.Services.AddAutoMapper(typeof(MappingConfig));

//Agregar Servicio para las interfaces
builder.Services.AddScoped<IVillaRepository, VillaRepository>();




var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
