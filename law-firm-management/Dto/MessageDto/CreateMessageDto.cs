namespace law_firm_management.Dto.MessageDto;

public class CreateMessageDto
{
    public int CaseId { get; set; }
    public string SenderId { get; set; }
    public string Content { get; set; }
}
