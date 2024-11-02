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
    public class UsuarioService : IUsuarioService
    {
        private readonly IGenericRepository<Usuario> _usuarioRepository;
        private readonly IMapper _mapper;

        public UsuarioService(IGenericRepository<Usuario> usuarioRepository, IMapper mapper)
        {
            _usuarioRepository = usuarioRepository;
            _mapper = mapper;
        }

        public async Task<List<UsuarioDTO>> Lista()
        {
            try
            {
                var queryUsuario = await _usuarioRepository.Consultar();
                var listaUsuarios = queryUsuario.Include(rol => rol.IdRolNavigation).ToList();

                return _mapper.Map<List<UsuarioDTO>>(listaUsuarios);
            }
            catch
            {
                throw;
            }
        }

        public async Task<SesionDTO> ValidarCredenciales(string correo, string clave)
        {
            try
            {
                var queryUser = await _usuarioRepository.Consultar(u => u.Correo == correo && u.Clave == clave);

                if (queryUser.FirstOrDefault() == null) throw new TaskCanceledException("El usuario no existe");

                Usuario user = queryUser.Include(rol => rol.IdRolNavigation).First();

                return _mapper.Map<SesionDTO>(user);
            }
            catch
            {
                throw;
            }
        }

        public async Task<UsuarioDTO> Crear(UsuarioDTO usuarioDto)
        {
            try
            {
                var user = await _usuarioRepository.Crear(_mapper.Map<Usuario>(usuarioDto));

                if(user.IdUsuario == 0) throw new TaskCanceledException("No se pudo crear el usuario");

                var query = await _usuarioRepository.Consultar(u => u.IdUsuario == user.IdUsuario);
                user = query.Include(rol => rol.IdRolNavigation).First();
                return _mapper.Map<UsuarioDTO>(user);
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> Editar(UsuarioDTO usuarioDTO)
        {
            try
            {
                var user = _mapper.Map<Usuario>(usuarioDTO);
                var userEncontrado = await _usuarioRepository.Obtener(u => u.IdUsuario == user.IdUsuario);

                if (userEncontrado == null) throw new TaskCanceledException("No se pudo crear el usuario");

                userEncontrado.NombreCompleto = user.NombreCompleto;
                userEncontrado.Correo = user.Correo;
                userEncontrado.IdRol = user.IdRol;
                userEncontrado.Clave = user.Clave;
                userEncontrado.EsActivo = user.EsActivo;

                bool respuesta = await _usuarioRepository.Editar(userEncontrado);

                if(!respuesta) throw new TaskCanceledException("No se pudo editar");

                return respuesta;
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> Eliminar(int idUsuario)
        {
            try
            {
                var usuarioEncontrado = await _usuarioRepository.Obtener(u => u.IdUsuario == idUsuario);

                if(usuarioEncontrado == null) throw new TaskCanceledException("No se pudo crear el usuario");

                bool respuesta = await _usuarioRepository.Delete(usuarioEncontrado);

                if (!respuesta) throw new TaskCanceledException("No se pudo eliminar");

                return respuesta;
            }
            catch
            {
                throw;
            }
        }
    }
}
