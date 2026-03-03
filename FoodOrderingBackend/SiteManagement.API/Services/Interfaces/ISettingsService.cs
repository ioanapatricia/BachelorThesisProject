using System.Threading.Tasks;
using Global.Contracts;
using SiteManagement.API.Entities;

namespace SiteManagement.API.Services.Interfaces
{
    public interface ISettingsService
    {
        Task<SiteSettings> GetAsync();
        Task<Result> UpdateAsync(string id, SiteSettings settingsForUpdate);
    }
}
