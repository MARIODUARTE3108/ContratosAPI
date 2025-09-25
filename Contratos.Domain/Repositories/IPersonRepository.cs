using Contratos.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contratos.Domain.Repositories
{
    public interface IPersonRepository : IRepository<Person>
    {
        Task<Person?> GetByEmailAsync(string email);
    }
}
