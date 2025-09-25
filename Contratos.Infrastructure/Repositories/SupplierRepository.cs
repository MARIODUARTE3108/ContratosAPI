using Contratos.Domain.Entities;
using Contratos.Domain.Repositories;
using Contratos.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contratos.Infrastructure.Repositories
{
    public class SupplierRepository : RepositoryBase<Supplier>, ISupplierRepository
    {
        public SupplierRepository(AppDbContext ctx) : base(ctx) { }

    }
}
