using Contratos.Domain.Entities;
using Contratos.Domain.Repositories;
using Contratos.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contratos.Infrastructure.Repositories
{
    public class ContractRepository : RepositoryBase<Contract>, IContractRepository
    {
        public ContractRepository(AppDbContext ctx) : base(ctx) { }

        public Task<List<Contract>> ListByEmpresaAsync(int empresaId)
            => _db.AsNoTracking().Where(c => c.SupplierId == empresaId).ToListAsync();
        public Task<List<Contract>> ListAsync()
        {
                  var contratos =  _db.AsNoTracking().Include(x => x.Supplier).ToListAsync();
            return contratos;
        }
    }
}
