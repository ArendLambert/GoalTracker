using Core.Models;
using DataAccess.Services.Interfaces;
using GoalTrackerApp.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GoalTrackerApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ThemeController : ControllerBase
    {
        private readonly IThemeService _themeService;
        private readonly IThemeSetService _themeSetService;
        private readonly IImportanceThemeService _importanceThemeService;
        public ThemeController(IThemeService themeService, IThemeSetService themeSetService, IImportanceThemeService importanceThemeService)
        {
            _themeService = themeService;
            _themeSetService = themeSetService;
            _importanceThemeService = importanceThemeService;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<ThemeSetContract>>> GetAllPublicAsync()
        {
            IEnumerable<ThemeSetModel> themeSets = await _themeSetService.GetAllPublicAsync();
            IEnumerable<ThemeModel> themes = await _themeService.GetAllAsync();
            return Ok(await Task.WhenAll(themeSets.Where(x => themes.Any(theme => theme.Id == x.IdTheme))
                .Select(async x => new ThemeSetContract
                {
                    Public = x.Public,
                    Id = x.Id,
                    Theme = themes.First(theme => theme.Id == x.IdTheme),
                    IdUserCreator = x.IdUserCreator,
                    ImportanceThemes = await _importanceThemeService.GetByThemeIdAsync(x.IdTheme),
                })));
        }

        [HttpGet]
        [Route("getbyid/{userId:guid}/{id:guid}")]
        [Authorize]
        public async Task<ActionResult<ThemeSetContract>> GetByIdAsync(Guid userId, Guid id)
        {
            try
            {
                ThemeSetModel? themeSet = await _themeSetService.GetByIdAsync(id);
                if (themeSet == null)
                {
                    return NotFound("ThemeSet not found");
                }
                if (userId != themeSet.IdUserCreator && !themeSet.Public)
                {
                    return Forbid();
                }
                ThemeModel? theme = await _themeService.GetByIdAsync(themeSet.IdTheme);
                if (theme == null)
                {
                    return NotFound("Theme not found");
                }
                return Ok(new ThemeSetContract
                {
                    Public = themeSet.Public,
                    Id = themeSet.Id,
                    IdUserCreator = themeSet.IdUserCreator,
                    Theme = theme,
                    ImportanceThemes = await _importanceThemeService.GetByThemeIdAsync(theme.Id),
                });
            }catch(Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        [Route("createtheme")]
        [Authorize]
        public async Task<ActionResult> CreateThemeAsync([FromBody] ThemeSetContract themeSetContract)
        {
            if (themeSetContract == null || themeSetContract.Theme == null)
            {
                return BadRequest("Invalid theme data.");
            }
            try
            {
                await _themeService.AddAsync(themeSetContract.Theme.Name, themeSetContract.Theme.PrimaryColor,
                    themeSetContract.Theme.SecondaryColor, themeSetContract.Theme.AccentColor, themeSetContract.Theme.BackgroundColor,
                    themeSetContract.Theme.TextColor, themeSetContract.Theme.BorderColor, themeSetContract.Theme.ShadowColor,
                    themeSetContract.Theme.CardBackground, themeSetContract.Theme.ButtonColor, themeSetContract.Theme.ButtonTextColor);
                foreach (ImportanceThemeModel importanceTheme in themeSetContract.ImportanceThemes)
                {
                    await _importanceThemeService.AddAsync(importanceTheme.Id, importanceTheme.IdTheme,
                        importanceTheme.BackgroundColor, importanceTheme.TextColor);
                }
                await _themeSetService.AddAsync(themeSetContract.Theme.Id, themeSetContract.IdUserCreator, themeSetContract.Public);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest($"Error creating theme: {ex.Message}");
            }
        }

        [HttpPut]
        [Authorize]
        [Route("update")]
        public async Task<ActionResult> UpdateThemeAsync([FromBody] ThemeSetContract themeSetContract)
        {
            if (themeSetContract == null || themeSetContract.Theme == null)
            {
                return BadRequest("Invalid theme data.");
            }
            if (themeSetContract.RequestUserId != themeSetContract.IdUserCreator)
            {
                return Forbid();
            }
            try
            {
                await _themeService.UpdateAsync(themeSetContract.Theme);
                foreach (ImportanceThemeModel importanceTheme in themeSetContract.ImportanceThemes)
                {
                    await _importanceThemeService.UpdateAsync(importanceTheme);
                }
                await _themeSetService.UpdateAsync(new ThemeSetModel(themeSetContract.Id, themeSetContract.Theme.Id,
                    themeSetContract.IdUserCreator, themeSetContract.Public));
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest($"Error updating theme: {ex.Message}");
            }
        }

        [HttpDelete]
        [Route("delete/{userId:guid}/{id:guid}")]
        [Authorize]
        public async Task<ActionResult> DeleteThemeAsync(Guid userId, Guid id)
        {
            try
            {
                ThemeSetModel? themeSet = await _themeSetService.GetByIdAsync(id);
                if (themeSet == null)
                {
                    return NotFound("Theme set not found.");
                }
                if (userId != themeSet.IdUserCreator)
                {
                    return Forbid();
                }
                await _themeSetService.DeleteAsync(id);
                foreach (ImportanceThemeModel importanceTheme in await _importanceThemeService.GetByThemeIdAsync(themeSet.IdTheme))
                {
                    await _importanceThemeService.DeleteAsync(importanceTheme.Id);
                }
                await _themeService.DeleteAsync(themeSet.IdTheme);
                return Ok();
            }
            catch(Exception ex)
            {
                return BadRequest($"Error deleting theme: {ex.Message}");
            }
        }

    }
}
