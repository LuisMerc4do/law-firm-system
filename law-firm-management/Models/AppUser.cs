using Microsoft.AspNetCore.Identity;

namespace law_firm_management.Models
{
    public class AppUser : IdentityUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        // Navigation properties
        public List<CaseModel> CreatedCases { get; set; } = new List<CaseModel>();
        public List<CaseModel> AssignedCases { get; set; } = new List<CaseModel>();
        public List<MessageModel> Messages { get; set; } = new List<MessageModel>();
        public List<NotificationModel> Notifications { get; set; } = new List<NotificationModel>();
    }
}