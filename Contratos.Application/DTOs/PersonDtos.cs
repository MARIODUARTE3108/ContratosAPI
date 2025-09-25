using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contratos.Application.DTOs
{
    public record PersonCreateDto(string Nome, string Email, string Senha);
    public record PersonOutputDto(int Id, string Nome, string Email);
    public record LoginRequestDto(string Email, string Senha);
    public record LoginResponseDto(string Token);
}
