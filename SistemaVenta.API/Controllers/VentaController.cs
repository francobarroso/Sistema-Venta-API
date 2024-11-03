using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SistemaVenta.API.Utilidad;
using SistemaVenta.BLL.Servicios;
using SistemaVenta.BLL.Servicios.Contrato;
using SistemaVenta.DTO;

namespace SistemaVenta.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VentaController : ControllerBase
    {
        private readonly IVentaService _ventaService;

        public VentaController(IVentaService ventaService)
        {
            _ventaService = ventaService;
        }

        [HttpPost]
        public async Task<IActionResult> Registrar([FromBody] VentaDTO ventaDto)
        {
            var response = new Response<VentaDTO>();

            try
            {
                response.status = true;
                response.value = await _ventaService.Registrar(ventaDto);

            }
            catch (Exception e)
            {
                response.status = false;
                response.message = e.Message;
            }

            return Ok(response);
        }

        [HttpGet]
        [Route("historial")]
        public async Task<IActionResult> Historial(string buscarPor, string nroVenta, string fechaInicio, string fechaFin)
        {
            var response = new Response<List<VentaDTO>>();
            nroVenta = nroVenta is null ? "" : nroVenta;
            fechaInicio = fechaInicio is null ? "" : fechaInicio;
            fechaFin = fechaFin is null ? "" : fechaFin;

            try
            {
                response.status = true;
                response.value = await _ventaService.Historial(buscarPor, nroVenta, fechaInicio, fechaFin);

            }
            catch (Exception e)
            {
                response.status = false;
                response.message = e.Message;
            }

            return Ok(response);
        }

        [HttpGet]
        [Route("reporte")]
        public async Task<IActionResult> Reporte(string fechaInicio, string fechaFin)
        {
            var response = new Response<List<ReporteDTO>>();
            fechaInicio = fechaInicio is null ? "" : fechaInicio;
            fechaFin = fechaFin is null ? "" : fechaFin;

            try
            {
                response.status = true;
                response.value = await _ventaService.Reporte(fechaInicio, fechaFin);

            }
            catch (Exception e)
            {
                response.status = false;
                response.message = e.Message;
            }

            return Ok(response);
        }

    }
}
