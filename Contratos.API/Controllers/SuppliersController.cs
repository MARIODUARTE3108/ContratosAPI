using Contratos.Application.DTOs;
using Contratos.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Contratos.API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/suppliers")]
    public class SuppliersController : ControllerBase
    {
        private readonly ISupplierService _svc;
        public SuppliersController(ISupplierService svc) => _svc = svc;

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] SupplierCreateDto dto)
            => Ok(await _svc.CreateAsync(dto));

        [HttpGet]
        public async Task<IActionResult> List() => Ok(await _svc.ListAsync());

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
            => (await _svc.GetByIdAsync(id)) is { } e ? Ok(e) : NotFound();
    }
}
