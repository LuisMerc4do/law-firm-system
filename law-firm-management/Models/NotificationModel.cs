namespace law_firm_management.Models;

public class NotificationModel
{
    public int NotificationId { get; set; }
    public string Message { get; set; }
    public DateTime DateSent { get; set; }
    public bool IsRead { get; set; }
    public string UserId { get; set; }
    public AppUser User { get; set; }
}
