using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contratos.Application.DTOs
{
    public record SupplierCreateDto(string Nome, string Cnpj, string Email, string Telefone);
    public record SupplierOutputDto(int Id, string Nome, string Cnpj, string Email, string Telefone);
}
