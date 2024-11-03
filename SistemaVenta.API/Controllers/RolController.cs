using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SistemaVenta.API.Utilidad;
using SistemaVenta.BLL.Servicios.Contrato;
using SistemaVenta.DTO;

namespace SistemaVenta.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolController : ControllerBase
    {
        private readonly IRolService _rolService;

        public RolController(IRolService rolService)
        {
            _rolService = rolService;
        }

        [HttpGet]
        public async Task<IActionResult> Lista()
        {
            var response = new Response<List<RolDTO>>();

            try
            {
                response.status = true;
                response.value = await _rolService.Lista();

            }catch(Exception e)
            {
                response.status = false;
                response.message = e.Message;

            }

            return Ok(response);
        }
    }
}
