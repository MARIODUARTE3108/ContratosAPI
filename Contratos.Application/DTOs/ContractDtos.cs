using Contratos.Domain.Enuns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contratos.Application.DTOs
{
    public record ContractCreateDto(string Numero, string Descricao, long Valor, int EmpresaId, DateOnly DataInicio, DateOnly DataFim, ContractStatus Status);
    public record ContractUpdateDto(string Numero, string Descricao, long Valor, DateOnly DataInicio, DateOnly DataFim, ContractStatus Status);
    public record ContractOutputDto(int Id, string Numero, string Descricao, string EmpresaNome, long Valor, int EmpresaId, DateOnly DataInicio, DateOnly DataFim, ContractStatus Status);
}
