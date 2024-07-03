using law_firm_management.Dto.CaseDto;

namespace law_firm_management.Dto.DocumentDto;

public class DocumentModelDto
{
    public int DocumentId { get; set; }
    public int CaseId { get; set; }
    public string FileName { get; set; }
    public string FilePath { get; set; }
    public DateTime UploadDate { get; set; }
    public CaseModelDto Case { get; set; }
}
