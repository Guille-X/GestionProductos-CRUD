using System.Text;
using System.Text.Json;
using GestionProductos.Frontend.Models;

namespace GestionProductos.Frontend.Services
{
    public class ProductoService : IProductoService
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonOptions;

        public ProductoService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
        }

        public async Task<List<Producto>> GetProductos(string? codigo = null, string? nombre = null, bool? activo = null)
        {
            try
            {
                var queryString = BuildQueryString(codigo, nombre, activo);
                var response = await _httpClient.GetAsync($"api/productos{queryString}");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<List<Producto>>(content, _jsonOptions) ?? new List<Producto>();
                }

                return new List<Producto>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error obteniendo productos: {ex.Message}");
                return new List<Producto>();
            }
        }

        public async Task<Producto?> GetProducto(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/productos/{id}");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<Producto>(content, _jsonOptions);
                }

                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error obteniendo producto: {ex.Message}");
                return null;
            }
        }

        public async Task<Producto> CreateProducto(Producto producto)
        {
            try
            {
                var json = JsonSerializer.Serialize(producto);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("api/productos", content);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<Producto>(responseContent, _jsonOptions) ?? producto;
                }

                var errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error al crear producto: {response.StatusCode} - {errorContent}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creando producto: {ex.Message}");
                throw;
            }
        }

        public async Task UpdateProducto(int id, Producto producto)
        {
            try
            {
                var json = JsonSerializer.Serialize(producto);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PutAsync($"api/productos/{id}", content);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Error al actualizar producto: {response.StatusCode} - {errorContent}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error actualizando producto: {ex.Message}");
                throw;
            }
        }

        public async Task DeleteProducto(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"api/productos/{id}");

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Error al eliminar producto: {response.StatusCode} - {errorContent}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error eliminando producto: {ex.Message}");
                throw;
            }
        }

        private string BuildQueryString(string? codigo, string? nombre, bool? activo)
        {
            var queryParams = new List<string>();

            if (!string.IsNullOrWhiteSpace(codigo))
                queryParams.Add($"codigo={Uri.EscapeDataString(codigo)}");

            if (!string.IsNullOrWhiteSpace(nombre))
                queryParams.Add($"nombre={Uri.EscapeDataString(nombre)}");

            if (activo.HasValue)
                queryParams.Add($"activo={activo.Value.ToString().ToLower()}");

            return queryParams.Any() ? "?" + string.Join("&", queryParams) : string.Empty;
        }
    }
}
