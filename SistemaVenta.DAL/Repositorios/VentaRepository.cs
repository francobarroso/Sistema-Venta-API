using SistemaVenta.DAL.Repositorios.Contrato;
using SistemaVenta.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaVenta.DAL.Repositorios
{
    public class VentaRepository : GenericRepository<Venta>, IVentaRepository
    {
        private readonly DbventaContext _dbventaContext;

        public VentaRepository(DbventaContext dbventaContext) : base(dbventaContext) 
        {
            _dbventaContext = dbventaContext;
        }

        public async Task<Venta> Registrar(Venta venta)
        {
            Venta ventaGenerada = new Venta();

            using (var transaction = _dbventaContext.Database.BeginTransaction())
            {
                try
                {
                    foreach(DetalleVenta detalle in venta.DetalleVenta)
                    {
                        Producto producto = _dbventaContext.Productos.Where(p => p.IdProducto == detalle.IdProducto).First();

                        producto.Stock = producto.Stock - detalle.Cantidad;
                        _dbventaContext.Productos.Update(producto);
                    }

                    await _dbventaContext.SaveChangesAsync();

                    NumeroDocumento correlativo = _dbventaContext.NumeroDocumentos.First();
                    correlativo.UltimoNumero = correlativo.UltimoNumero + 1;
                    correlativo.FechaRegistro = DateTime.Now;

                    _dbventaContext.NumeroDocumentos.Update(correlativo);
                    await _dbventaContext.SaveChangesAsync();

                    int cantDigitos = 4;
                    string ceros = string.Concat(Enumerable.Repeat("0", cantDigitos));
                    string nroVenta = ceros + correlativo.ToString();

                    nroVenta = nroVenta.Substring(nroVenta.Length - cantDigitos, cantDigitos);

                    venta.NumeroDocumento = nroVenta;

                    await _dbventaContext.Venta.AddAsync(venta);
                    await _dbventaContext.SaveChangesAsync();

                    ventaGenerada = venta;

                    transaction.Commit();

                }catch
                {
                    transaction.Rollback();
                    throw;
                }

                return ventaGenerada;
            }
        }
    }
}