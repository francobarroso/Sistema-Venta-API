using Microsoft.EntityFrameworkCore;
using SistemaVenta.DAL.Repositorios.Contrato;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SistemaVenta.DAL.Repositorios
{
    public class GenericRepository<TModel> : IGenericRepository<TModel> where TModel : class
    {
        private readonly DbventaContext _dbventaContext;

        public GenericRepository(DbventaContext dbventaContext)
        {
            _dbventaContext = dbventaContext;
        }

        public async Task<TModel> Obtener(Expression<Func<TModel, bool>> filtro)
        {
            try
            {
                TModel model = await _dbventaContext.Set<TModel>().FirstOrDefaultAsync(filtro);
                return model;
            }
            catch
            {
                throw;
            }
        }

        public async Task<TModel> Crear(TModel model)
        {
            try
            {
                _dbventaContext.Set<TModel>().Add(model);
                await _dbventaContext.SaveChangesAsync();
                return model;
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> Delete(TModel model)
        {
            try
            {
                _dbventaContext.Set<TModel>().Remove(model);
                await _dbventaContext.SaveChangesAsync();
                return true;
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> Editar(TModel model)
        {
            try
            {
                _dbventaContext.Set<TModel>().Update(model);
                await _dbventaContext.SaveChangesAsync();
                return true;
            }
            catch
            {
                throw;
            }
        }

        public async Task<IQueryable<TModel>> Consultar(Expression<Func<TModel, bool>> filtro = null)
        {
            try
            {
                IQueryable<TModel> query = filtro == null ? _dbventaContext.Set<TModel>() : _dbventaContext.Set<TModel>().Where(filtro);
                return query;
            }
            catch
            {
                throw;
            }
        }
    }
}
