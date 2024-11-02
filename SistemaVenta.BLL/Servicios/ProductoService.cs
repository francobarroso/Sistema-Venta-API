using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SistemaVenta.BLL.Servicios.Contrato;
using SistemaVenta.DAL.Repositorios.Contrato;
using SistemaVenta.DTO;
using SistemaVenta.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaVenta.BLL.Servicios
{
    public class ProductoService : IProductoService
    {
        private readonly IGenericRepository<Producto> _productoRepository;
        private readonly IMapper _mapper;

        public ProductoService(IGenericRepository<Producto> productoRepository, IMapper mapper)
        {
            _productoRepository = productoRepository;
            _mapper = mapper;
        }

        public async Task<List<ProductoDTO>> Lista()
        {
            try
            {
                var listaProductos = await _productoRepository.Consultar();
                return _mapper.Map<List<ProductoDTO>>(listaProductos.ToList());
            }
            catch
            {
                throw;
            }
        }

        public async Task<ProductoDTO> Crear(ProductoDTO productoDto)
        {
            try
            {
                var producto = await _productoRepository.Crear(_mapper.Map<Producto>(productoDto));

                if(producto.IdProducto == 0) throw new TaskCanceledException("No se pudo crear el producto");

                return _mapper.Map<ProductoDTO>(producto);
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> Editar(ProductoDTO productoDto)
        {
            try
            {
                var producto = _mapper.Map<Producto>(productoDto);
                var productoEncontrado = await _productoRepository.Obtener(p => p.IdProducto == producto.IdProducto);

                if(productoEncontrado == null) throw new TaskCanceledException("No se pudo encontrar el producto");

                productoEncontrado.Nombre = producto.Nombre;
                productoEncontrado.IdCategoria = producto.IdCategoria;
                productoEncontrado.Stock = producto.Stock;
                productoEncontrado.Precio = producto.Precio;
                productoEncontrado.EsActivo = producto.EsActivo;

                bool respuesta = await _productoRepository.Editar(productoEncontrado);

                if(!respuesta) throw new TaskCanceledException("No se pudo editar el producto");

                return respuesta;
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> Eliminar(int idProducto)
        {
            try
            {
                var productoEncontrado = await _productoRepository.Obtener(p => p.IdProducto == idProducto);

                bool respuesta = await _productoRepository.Delete(productoEncontrado);

                if(!respuesta) throw new TaskCanceledException("No se pudo eliminar el producto");

                return respuesta;
            }
            catch
            {
                throw;
            }
        }
    }
}
