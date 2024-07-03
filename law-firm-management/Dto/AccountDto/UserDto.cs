using System.ComponentModel.DataAnnotations;
using law_firm_management.Dto.CaseDto;
using law_firm_management.Dto.MessageDto;
using law_firm_management.Dto.NotificationDto;

namespace law_firm_management.Dto.AccountDto;

public class UserDto
{
    public string Id { get; set; } = string.Empty;
    [Required]
    public string FirstName { get; set; } = string.Empty;
    [Required]
    public string LastName { get; set; } = string.Empty;
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
    [Required]
    [Phone]
    public string PhoneNumber { get; set; } = string.Empty;
    public string Token { get; set; } = string.Empty;
    public List<CaseModelDto> CreatedCases { get; set; } = new List<CaseModelDto>();
    public List<CaseModelDto> AssignedCases { get; set; } = new List<CaseModelDto>();
    public List<MessageModelDto> Messages { get; set; } = new List<MessageModelDto>();
    public List<NotificationModelDto> Notifications { get; set; } = new List<NotificationModelDto>();
}