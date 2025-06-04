using Core.Models;
using DataAccess.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GoalTrackerApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StatusController : ControllerBase
    {
        private readonly IStatusService _statusService;
        public StatusController(IStatusService statusService)
        {
            _statusService = statusService;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<StatusModel>>> GetAllAsync()
        {
            return Ok(await _statusService.GetAllAsync());
        }

        [HttpGet]
        [Route("{id:guid}")]
        [Authorize]
        public async Task<ActionResult<StatusModel?>> GetByIdAsync(Guid id)
        {
            StatusModel? status = await _statusService.GetByIdAsync(id);
            if (status == null)
            {
                return NotFound();
            }
            return Ok(status);
        }
    }
}
