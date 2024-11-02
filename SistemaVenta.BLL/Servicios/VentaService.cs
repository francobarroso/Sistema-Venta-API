using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SistemaVenta.BLL.Servicios.Contrato;
using SistemaVenta.DAL.Repositorios.Contrato;
using SistemaVenta.DTO;
using SistemaVenta.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaVenta.BLL.Servicios
{
    public class VentaService : IVentaService
    {
        private readonly IVentaRepository _ventaRepository;
        private readonly IGenericRepository<DetalleVenta> _detalleVentaRepository;
        private readonly IMapper _mapper;

        public VentaService(IVentaRepository ventaRepository, IGenericRepository<DetalleVenta> detalleVentaRepository, IMapper mapper)
        {
            _ventaRepository = ventaRepository;
            _detalleVentaRepository = detalleVentaRepository;
            _mapper = mapper;
        }

        public async Task<VentaDTO> Registrar(VentaDTO ventaDto)
        {
            try
            {
                var ventaGenerada = await _ventaRepository.Registrar(_mapper.Map<Venta>(ventaDto));

                if(ventaGenerada.IdVenta == 0) throw new TaskCanceledException("No se pudo crear la venta");

                return _mapper.Map<VentaDTO>(ventaGenerada);
            }
            catch
            {
                throw;
            }
        }

        public async Task<List<VentaDTO>> Historial(string buscarPor, string nroVenta, string fechaInicio, string fechaFin)
        {
            IQueryable<Venta> query = await _ventaRepository.Consultar();
            var listaVenta = new List<Venta>();

            try
            {
                if(buscarPor == "fecha")
                {
                    DateTime fechaStart = DateTime.ParseExact(fechaInicio, "dd/MM/yyyy", new CultureInfo("es-AR"));
                    DateTime fechaEnd = DateTime.ParseExact(fechaFin, "dd/MM/yyyy", new CultureInfo("es-AR"));

                    listaVenta = await query.Where(v =>
                    v.FechaRegistro.HasValue &&
                    v.FechaRegistro.Value.Date >= fechaStart.Date &&
                    v.FechaRegistro.Value.Date <= fechaEnd.Date
                    ).Include(detalle => detalle.DetalleVenta)
                    .ThenInclude(p => p.IdProductoNavigation)
                    .ToListAsync();
                }
                else
                {
                    listaVenta = await query.Where(v => v.NumeroDocumento == nroVenta)
                    .Include(detalle => detalle.DetalleVenta)
                    .ThenInclude(p => p.IdProductoNavigation)
                    .ToListAsync();
                }
            }
            catch
            {
                throw;
            }

            return _mapper.Map<List<VentaDTO>>(listaVenta);
        }

        public async Task<List<ReporteDTO>> Reporte(string fechaInicio, string fechaFin)
        {
            IQueryable<DetalleVenta> query = await _detalleVentaRepository.Consultar();
            var listaDetalleVenta = new List<DetalleVenta>();
            try
            {
                DateTime fechaStart = DateTime.ParseExact(fechaInicio, "dd/MM/yyyy", new CultureInfo("es-AR"));
                DateTime fechaEnd = DateTime.ParseExact(fechaFin, "dd/MM/yyyy", new CultureInfo("es-AR"));

                listaDetalleVenta = await query
                    .Include(p => p.IdProductoNavigation)
                    .Include(v => v.IdVentaNavigation)
                    .Where(detalle =>
                    detalle.IdVentaNavigation.FechaRegistro.HasValue &&
                    detalle.IdVentaNavigation.FechaRegistro.Value.Date >= fechaStart.Date &&
                    detalle.IdVentaNavigation.FechaRegistro.Value.Date <= fechaEnd.Date).ToListAsync();
            }
            catch
            {
                throw;
            }

            return _mapper.Map<List<ReporteDTO>>(listaDetalleVenta);
        }
    }
}
