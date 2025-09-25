using Contratos.Application.DTOs;
using Contratos.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Contratos.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IPersonService _svc;
        public AuthController(IPersonService svc) => _svc = svc;

        [HttpPost("register")]
        public async Task<ActionResult<PersonOutputDto>> Register([FromBody] PersonCreateDto dto)
            => Ok(await _svc.CreateAsync(dto));

        [HttpPost("login")]
        public async Task<ActionResult<LoginResponseDto>> Login([FromBody] LoginRequestDto dto)
            => Ok(await _svc.LoginAsync(dto));
    }
}
