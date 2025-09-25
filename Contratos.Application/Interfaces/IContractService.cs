using Contratos.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contratos.Application.Interfaces
{
    public interface IContractService
    {
        Task<ContractOutputDto> CreateAsync(ContractCreateDto dto);
        Task<ContractOutputDto> UpdateAsync(int id, ContractUpdateDto dto);
        Task<ContractOutputDto?> GetByIdAsync(int id);
        Task<List<ContractOutputDto>> ListAsync();
        Task<List<ContractOutputDto>> ListByEmpresaAsync(int empresaId);
    }
}
