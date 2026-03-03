using System.Threading.Tasks;
using Global.Contracts;
using SiteManagement.API.Entities;
using SiteManagement.API.Persistence.Interfaces;
using SiteManagement.API.Services.Interfaces;

namespace SiteManagement.API.Services
{ 
    public class SettingsService : ISettingsService
    {
        private readonly ISettingsRepository _settingsRepository;

        public SettingsService(ISettingsRepository settingsRepository)
        {
            _settingsRepository = settingsRepository;
        }
        public async Task<SiteSettings> GetAsync()
        {
            return await _settingsRepository.GetAsync();
        }

        public async Task<Result> UpdateAsync(string id, SiteSettings settingsForUpdate)
        {
            return await _settingsRepository.UpdateAsync(id, settingsForUpdate);
        }
    }
}
