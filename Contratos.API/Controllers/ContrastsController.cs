using Contratos.Application.DTOs;
using Contratos.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Contratos.API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/contracts")]
    public class ContrastsController : ControllerBase
    {
        private readonly IContractService _svc;
        public ContrastsController(IContractService svc) => _svc = svc;

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ContractCreateDto dto)
            => Ok(await _svc.CreateAsync(dto));

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] ContractUpdateDto dto)
            => Ok(await _svc.UpdateAsync(id, dto));

        [HttpGet]
        public async Task<IActionResult> List() => Ok(await _svc.ListAsync());

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
         => (await _svc.GetByIdAsync(id)) is { } c ? Ok(c) : NotFound();

        [HttpGet("empresa/{empresaId:int}")]
        public async Task<IActionResult> ListByEmpresa(int empresaId)
            => Ok(await _svc.ListByEmpresaAsync(empresaId));
    }
}
