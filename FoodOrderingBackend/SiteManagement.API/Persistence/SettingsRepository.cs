using System;
using System.Threading.Tasks;
using Global.Contracts;
using MongoDB.Driver;
using SiteManagement.API.Entities;
using SiteManagement.API.Persistence.Interfaces;

namespace SiteManagement.API.Persistence
{
    public class SettingsRepository : ISettingsRepository
    {
        private readonly IMongoCollection<SiteSettings> _siteSettingsCollection;

        public SettingsRepository(IMongoClient mongoClient)
        {
            _siteSettingsCollection =
                mongoClient.GetDatabase("SiteManagementDb").GetCollection<SiteSettings>("SiteSettings");
        }
        public async Task<SiteSettings> GetAsync()
        {
            return await _siteSettingsCollection.FindAsync(siteSettings => true).Result.FirstOrDefaultAsync();
        }

        public async Task<Result> UpdateAsync(string id, SiteSettings siteSettings)
        {
            siteSettings.Id = id;
            try
            {
                var filter = Builders<SiteSettings>.Filter.Where(s => s.Id == id);
                var result = await _siteSettingsCollection.ReplaceOneAsync(filter, siteSettings);
                return result.IsAcknowledged 
                    ? Result.Ok() 
                    : Result.Fail("Could not update");
            }
            catch (Exception e)
            {
                return Result.Fail(e.Message);
            }
        }
    }
}
