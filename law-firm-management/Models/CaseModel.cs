using Microsoft.AspNetCore.Identity;

namespace law_firm_management.Models
{
    public class CaseModel
    {
        public int CaseId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateClosed { get; set; }

        // Use string type for UserId properties
        public string CreatedById { get; set; }
        public AppUser CreatedBy { get; set; } // Use IdentityUser here
        public string AssignedToId { get; set; }
        public AppUser AssignedTo { get; set; } // Use IdentityUser here

        // Navigation properties
        public List<DocumentModel> Documents { get; set; } = new List<DocumentModel>();
        public List<MessageModel> Messages { get; set; } = new List<MessageModel>();
    }
}
