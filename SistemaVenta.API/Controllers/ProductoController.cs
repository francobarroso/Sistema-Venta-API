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
    public class ProductoController : ControllerBase
    {
        private readonly IProductoService _productoService;

        public ProductoController(IProductoService productoService)
        {
            _productoService = productoService;
        }

        [HttpGet]
        public async Task<IActionResult> Lista()
        {
            var response = new Response<List<ProductoDTO>>();

            try
            {
                response.status = true;
                response.value = await _productoService.Lista();

            }
            catch (Exception e)
            {
                response.status = false;
                response.message = e.Message;
            }

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] ProductoDTO productoDTO)
        {
            var response = new Response<ProductoDTO>();

            try
            {
                response.status = true;
                response.value = await _productoService.Crear(productoDTO);

            }
            catch (Exception e)
            {
                response.status = false;
                response.message = e.Message;
            }

            return Ok(response);
        }

        [HttpPut]
        public async Task<IActionResult> Editar([FromBody] ProductoDTO productoDTO)
        {
            var response = new Response<bool>();

            try
            {
                response.status = true;
                response.value = await _productoService.Editar(productoDTO);

            }
            catch (Exception e)
            {
                response.status = false;
                response.message = e.Message;
            }

            return Ok(response);
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            var response = new Response<bool>();

            try
            {
                response.status = true;
                response.value = await _productoService.Eliminar(id);

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
