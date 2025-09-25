using Contratos.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contratos.Application.Interfaces
{
    public interface ISupplierService
    {
        Task<SupplierOutputDto> CreateAsync(SupplierCreateDto dto);
        Task<List<SupplierOutputDto>> ListAsync();
        Task<SupplierOutputDto?> GetByIdAsync(int id);
    }
}
