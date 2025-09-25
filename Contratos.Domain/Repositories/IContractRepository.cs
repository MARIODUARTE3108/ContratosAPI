using Contratos.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contratos.Domain.Repositories
{
    public interface IContractRepository : IRepository<Contract>
    {
        Task<List<Contract>> ListByEmpresaAsync(int empresaId);
        Task<List<Contract>> ListAsync();
    }
}
