using System.Diagnostics.CodeAnalysis;

namespace SiteManagement.Contracts.Dtos
{
    [ExcludeFromCodeCoverage]
    public class SiteSettingsForUpdateDto
    {
        public string PhoneNumber { get; set; }
        public string Schedule { get; set; }
        public string Address { get; set; }
        public string Fax { get; set; }
        public string FeedbackEmail { get; set; }
        public string BusinessEmail { get; set; }
        public string FacebookUrl { get; set; }
        public string InstagramUrl { get; set; }
    }
}
