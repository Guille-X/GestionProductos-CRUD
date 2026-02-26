using GestionProductos.API.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();

// Configurar DbContext con SQL Server
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// IMPORTANTE: Configure CORS - VERSIÓN PERMISIVA SOLO PARA DESARROLLO
builder.Services.AddCors(options =>
{
    options.AddPolicy("PermitirTodo",
        policy =>
        {
            // Permite cualquier origen (útil para desarrollo)
            policy.AllowAnyOrigin()      // ⬅️ Esto permite cualquier puerto
                  .AllowAnyMethod()      // Permite GET, POST, PUT, DELETE
                  .AllowAnyHeader();     // Permite cualquier header
        });

    
    options.AddPolicy("PermitirFrontend",
        policy =>
        {
            policy.WithOrigins(
                "http://localhost:5000",
                "http://localhost:5001",
                "https://localhost:5001",
                "http://localhost:5020",
                "https://localhost:7220",
                "http://localhost:5159",  
                "http://localhost:5182",  
                "https://localhost:7001",
                "https://localhost:7002"
                )
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials();
        });
});


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Productos API",
        Version = "v1",
        Description = "API para gestionar productos. CRUD completo con soft delete.",
        Contact = new OpenApiContact
        {
            Name = "Guillermo Martin",
            Email = "guillermoajsivinac@gmail.com"
        }
    });
});

var app = builder.Build();

// HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Productos API V1");
    });
}

app.UseCors("PermitirTodo");

app.UseAuthorization();

app.MapControllers();

app.Run();