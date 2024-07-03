using Microsoft.AspNetCore.Identity;

namespace law_firm_management.Models;

public class MessageModel
{
    public int MessageId { get; set; }
    public string Content { get; set; }
    public DateTime DateSent { get; set; }
    public string SenderId { get; set; }
    public AppUser Sender { get; set; }
    public int CaseId { get; set; }
    public CaseModel Case { get; set; }
}
