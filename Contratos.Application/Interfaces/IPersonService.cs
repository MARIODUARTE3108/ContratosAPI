using Contratos.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contratos.Application.Interfaces
{
    public interface IPersonService
    {
        Task<PersonOutputDto> CreateAsync(PersonCreateDto dto);
        Task<LoginResponseDto> LoginAsync(LoginRequestDto dto);
        Task<List<PersonOutputDto>> ListAsync();
    }
}
