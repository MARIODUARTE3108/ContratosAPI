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
    public class PersonRepository : RepositoryBase<Person>, IPersonRepository
    {
        public PersonRepository(AppDbContext ctx) : base(ctx) { }
        public Task<Person?> GetByEmailAsync(string email)
            => _db.FirstOrDefaultAsync(x => x.Email == email.ToLower());
    }
}
