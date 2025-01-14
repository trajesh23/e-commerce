using E_Commerce.Business.Interfaces;
using E_Commerce.Business.Types;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SettingsController : ControllerBase
    {
        private readonly ISettingService _settingService;

        public SettingsController(ISettingService settingService)
        {
            _settingService = settingService;
        }

        [HttpPatch]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ToggleMaintenance()
        {
            await _settingService.ToggleMaintenance();

            var maintenanceState = _settingService.GetMaintenanceState();

            return Ok(new ServiceMessage
            {
                IsSucceed = true,
                Message = $"Is maintenance mode active?: {maintenanceState}"
            });
        }
    }
}
