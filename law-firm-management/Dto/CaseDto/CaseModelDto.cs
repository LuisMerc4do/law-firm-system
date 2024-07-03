using law_firm_management.Dto.AccountDto;
using law_firm_management.Dto.DocumentDto;
using law_firm_management.Dto.MessageDto;
using law_firm_management.Models;

namespace law_firm_management.Dto.CaseDto;

public class CaseModelDto
{
    public int CaseId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string CreatedById { get; set; }
    public UserDto CreatedBy { get; set; }
    public string AssignedToId { get; set; }
    public UserDto AssignedTo { get; set; }
    public DateTime DateCreated { get; set; }
    public DateTime? DateClosed { get; set; }
    public List<DocumentModelDto> Documents { get; set; }
    public List<MessageModelDto> Messages { get; set; }
}
