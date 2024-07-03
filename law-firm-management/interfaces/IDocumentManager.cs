using law_firm_management.Dto.DocumentDto;
using law_firm_management.Models;

namespace law_firm_management.interfaces;

public interface IDocumentManager
{
    Task<DocumentModelDto> CreateDocumentAsync(DocumentModelDto documentDto);
    Task<DocumentModelDto> GetDocumentByIdAsync(int documentId);
    Task<List<DocumentModelDto>> GetAllDocumentsAsync();
    Task<DocumentModelDto> UpdateDocumentAsync(int documentId, UpdateDocumentDto documentDto);
    Task<DocumentModelDto> DeleteDocumentAsync(int documentId);
}
