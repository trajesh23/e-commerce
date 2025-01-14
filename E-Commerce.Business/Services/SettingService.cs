using E_Commerce.Business.Interfaces;
using E_Commerce.DataAccess.Respositories.Interfaces;
using E_Commerce.DataAccess.UnitOfWork.Interfaces;
using E_Commerce.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Business.Services
{
    public class SettingService : ISettingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISettingRepository _settingRepository;

        public SettingService(IUnitOfWork unitOfWork, ISettingRepository settingRepository)
        {
            _unitOfWork = unitOfWork;
            _settingRepository = settingRepository;
        }

        public bool GetMaintenanceState()
        {
            var maintenanceState = _settingRepository.GetByIdAsync(1).Result.MaintenanceMode;

            return maintenanceState;
        }

        public async Task ToggleMaintenance()
        {
            var setting = _settingRepository.GetByIdAsync(1).Result;

            setting.MaintenanceMode = !setting.MaintenanceMode;

            await _settingRepository.UpdateByIdAsync(setting.Id);

            try
            {
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception)
            {

                throw new Exception("An error occurred while updating maintenance status.");
            }
        }
    }
}
