using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using SistemaVenta.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaVenta.BLL.Servicios.Contrato
{
    public interface IVentaService
    {
        Task<VentaDTO> Registrar(VentaDTO ventaDto);
        Task<List<VentaDTO>> Historial(string buscarPor, string nroVenta, string fechaInicio, string fechaFin);
        Task<List<ReporteDTO>> Reporte(string fechaInicio, string fechaFin);
    }
}
