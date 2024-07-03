using law_firm_management.Models;

namespace law_firm_management.Dto.CaseDto;

public class UpdateCaseDto
{
    public string Title { get; set; }
    public string Description { get; set; }
    public string AssignedToId { get; set; }
    public DateTime? DateClosed { get; set; }
}
