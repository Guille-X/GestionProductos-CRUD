using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using GestionProductos.Frontend;
using GestionProductos.Frontend.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");


// IMPORTANTE: Usar la URL de tu API (la que está corriendo)
builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri("https://localhost:7220/") // Puerto HTTP de la API
});

// Registrar servicios
builder.Services.AddScoped<IProductoService, ProductoService>();

await builder.Build().RunAsync();