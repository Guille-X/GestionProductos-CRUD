using GestionProductos.API.Data;
using GestionProductos.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GestionProductos.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProductosController(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Obtiene todos los productos con filtros opcionales
        /// </summary>
        /// <param name="codigo">Filtrar por código (contiene)</param>
        /// <param name="nombre">Filtrar por nombre (contiene)</param>
        /// <param name="activo">Filtrar por estado activo (true/false). Si no se envía, trae todos</param>
        /// <returns>Lista de productos</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Producto>), 200)]
        public async Task<ActionResult<IEnumerable<Producto>>> GetProductos(
            [FromQuery] string? codigo,
            [FromQuery] string? nombre,
            [FromQuery] bool? activo)
        {
            var query = _context.Productos.AsQueryable();

            // Aplicar filtros
            if (!string.IsNullOrWhiteSpace(codigo))
                query = query.Where(p => p.Codigo.Contains(codigo));

            if (!string.IsNullOrWhiteSpace(nombre))
                query = query.Where(p => p.Nombre.Contains(nombre));

            if (activo.HasValue)
                query = query.Where(p => p.Activo == activo.Value);

            return await query.OrderByDescending(p => p.CreatedAt).ToListAsync();
        }

        /// <summary>
        /// Obtiene un producto por su ID
        /// </summary>
        /// <param name="id">ID del producto</param>
        /// <returns>Producto encontrado o 404</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Producto), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<Producto>> GetProducto(int id)
        {
            var producto = await _context.Productos.FindAsync(id);

            if (producto == null)
            {
                return NotFound(new { message = $"No se encontró el producto con ID {id}" });
            }

            return producto;
        }

        /// <summary>
        /// Crea un nuevo producto
        /// </summary>
        /// <param name="producto">Datos del producto</param>
        /// <returns>Producto creado</returns>
        [HttpPost]
        [ProducesResponseType(typeof(Producto), 201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<Producto>> PostProducto(Producto producto)
        {
            // Validar código único
            if (await _context.Productos.AnyAsync(p => p.Codigo == producto.Codigo))
            {
                return BadRequest(new { message = $"Ya existe un producto con el código {producto.Codigo}" });
            }

            // Establecer fechas
            producto.CreatedAt = DateTime.Now;
            producto.Activo = true;

            _context.Productos.Add(producto);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProducto), new { id = producto.Id }, producto);
        }

        /// <summary>
        /// Actualiza un producto existente
        /// </summary>
        /// <param name="id">ID del producto a actualizar</param>
        /// <param name="producto">Datos actualizados</param>
        /// <returns>204 No Content o error</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> PutProducto(int id, Producto producto)
        {
            if (id != producto.Id)
            {
                return BadRequest(new { message = "El ID de la URL no coincide con el ID del producto" });
            }

            var productoExistente = await _context.Productos.FindAsync(id);
            if (productoExistente == null)
            {
                return NotFound(new { message = $"No se encontró el producto con ID {id}" });
            }

            // Validar código único (excluyendo el producto actual)
            if (await _context.Productos.AnyAsync(p => p.Codigo == producto.Codigo && p.Id != id))
            {
                return BadRequest(new { message = $"Ya existe otro producto con el código {producto.Codigo}" });
            }

            // Actualizar propiedades
            productoExistente.Codigo = producto.Codigo;
            productoExistente.Nombre = producto.Nombre;
            productoExistente.Precio = producto.Precio;
            productoExistente.Stock = producto.Stock;
            productoExistente.Activo = producto.Activo;
            productoExistente.UpdatedAt = DateTime.Now;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await ProductoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        /// <summary>
        /// Elimina un producto (soft delete - lo marca como inactivo)
        /// </summary>
        /// <param name="id">ID del producto a eliminar</param>
        /// <returns>204 No Content</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteProducto(int id)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto == null)
            {
                return NotFound(new { message = $"No se encontró el producto con ID {id}" });
            }

            // Soft delete: marcar como inactivo
            producto.Activo = false;
            producto.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        private async Task<bool> ProductoExists(int id)
        {
            return await _context.Productos.AnyAsync(e => e.Id == id);
        }
    }
}