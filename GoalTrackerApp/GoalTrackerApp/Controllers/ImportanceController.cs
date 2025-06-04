using Core.Models;
using DataAccess.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GoalTrackerApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ImportanceController : ControllerBase
    {
        private readonly IImportanceService _importanceService;
        public ImportanceController(IImportanceService importanceService)
        {
            _importanceService = importanceService;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<ImportanceModel>>> GetAllAsync()
        {
            return Ok((await _importanceService.GetAllAsync()));
        }

        [HttpGet]
        [Route("{id:guid}")]
        [Authorize]
        public async Task<ActionResult<ImportanceModel?>> GetByIdAsync(Guid id)
        {
            ImportanceModel? importance = await _importanceService.GetByIdAsync(id);
            if (importance == null)
            {
                return NotFound();
            }
            return Ok(importance);
        }
        
    }
}
