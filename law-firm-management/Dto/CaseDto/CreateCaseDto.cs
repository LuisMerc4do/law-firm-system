namespace law_firm_management.Dto.CaseDto;

public class CreateCaseDto
{
    public string Title { get; set; }
    public string Description { get; set; }
    public string CreatedById { get; set; }
    public string AssignedToId { get; set; }
}
