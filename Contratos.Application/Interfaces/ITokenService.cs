using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contratos.Application.Interfaces
{
    public interface ITokenService
    {
        string Generate(int userId, string email, string nome);
    }
}
