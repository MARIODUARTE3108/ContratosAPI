using Contratos.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Contratos.API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/persons")]
    public class PersonsController : ControllerBase
    {
        private readonly IPersonService _svc;
        public PersonsController(IPersonService svc) => _svc = svc;

        [HttpGet]
        public async Task<IActionResult> List() => Ok(await _svc.ListAsync());
    }
}
