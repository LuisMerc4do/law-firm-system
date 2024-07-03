namespace law_firm_management.Dto.DocumentDto;

public class CreateDocumentDto
{
    public int CaseId { get; set; }
    public string FileName { get; set; }
    public string FilePath { get; set; }
}
