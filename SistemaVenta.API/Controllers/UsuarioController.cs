using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SistemaVenta.API.Utilidad;
using SistemaVenta.BLL.Servicios.Contrato;
using SistemaVenta.DTO;

namespace SistemaVenta.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioService _usuatioService;

        public UsuarioController(IUsuarioService usuatioService)
        {
            _usuatioService = usuatioService;
        }

        [HttpGet]
        public async Task<IActionResult> Lista()
        {
            var response = new Response<List<UsuarioDTO>>();

            try
            {
                response.status = true;
                response.value = await _usuatioService.Lista();

            }catch(Exception e)
            {
                response.status = false;
                response.message = e.Message;
            }

            return Ok(response);
        }

        [HttpPost]
        [Route("IniciarSesion")]
        public async Task<IActionResult> IniciarSesion([FromBody] LoginDTO login)
        {
            var response = new Response<SesionDTO>();

            try
            {
                response.status = true;
                response.value = await _usuatioService.ValidarCredenciales(login.Correo, login.Clave);

            }
            catch (Exception e)
            {
                response.status = false;
                response.message = e.Message;
            }

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] UsuarioDTO usuarioDto)
        {
            var response = new Response<UsuarioDTO>();

            try
            {
                response.status = true;
                response.value = await _usuatioService.Crear(usuarioDto);

            }
            catch (Exception e)
            {
                response.status = false;
                response.message = e.Message;
            }

            return Ok(response);
        }

        [HttpPut]
        public async Task<IActionResult> Editar([FromBody] UsuarioDTO usuarioDto)
        {
            var response = new Response<bool>();

            try
            {
                response.status = true;
                response.value = await _usuatioService.Editar(usuarioDto);

            }
            catch (Exception e)
            {
                response.status = false;
                response.message = e.Message;
            }

            return Ok(response);
        }

        [HttpDelete]
        [Route("{idUsuario:int}")]
        public async Task<IActionResult> Eliminar(int idUsuario)
        {
            var response = new Response<bool>();

            try
            {
                response.status = true;
                response.value = await _usuatioService.Eliminar(idUsuario);

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
