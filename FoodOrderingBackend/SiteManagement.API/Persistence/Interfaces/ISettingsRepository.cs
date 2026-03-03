using System.Threading.Tasks;
using Global.Contracts;
using SiteManagement.API.Entities;

namespace SiteManagement.API.Persistence.Interfaces
{
    public interface ISettingsRepository
    {
        Task<SiteSettings> GetAsync();
        Task<Result> UpdateAsync(string id, SiteSettings siteSettings);
    }
}
