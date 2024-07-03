namespace law_firm_management.Models;

public class DocumentModel
{
    public int DocumentId { get; set; }
    public string FileName { get; set; }
    public string FilePath { get; set; }
    public DateTime UploadDate { get; set; }
    public int CaseId { get; set; }
    public CaseModel Case { get; set; }
}
