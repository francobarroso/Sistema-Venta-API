using SistemaVenta.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaVenta.BLL.Servicios.Contrato
{
    public interface IUsuarioService
    {
        Task<List<UsuarioDTO>> Lista();
        Task<SesionDTO> ValidarCredenciales(string correo, string clave);
        Task<UsuarioDTO> Crear(UsuarioDTO usuarioDto);
        Task<bool> Editar(UsuarioDTO usuarioDTO);
        Task<bool> Eliminar(int idUsuario);
    }
}
