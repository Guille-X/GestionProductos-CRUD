using GestionProductos.Frontend.Models;

namespace GestionProductos.Frontend.Services
{
    public interface IProductoService
    {
        Task<List<Producto>> GetProductos(string? codigo = null, string? nombre = null, bool? activo = null);
        Task<Producto?> GetProducto(int id);
        Task<Producto> CreateProducto(Producto producto);
        Task UpdateProducto(int id, Producto producto);
        Task DeleteProducto(int id);
    }
}