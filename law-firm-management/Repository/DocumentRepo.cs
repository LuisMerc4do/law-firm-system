using law_firm_management.Data;
using law_firm_management.Dto.DocumentDto;
using law_firm_management.interfaces;
using law_firm_management.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace law_firm_management.Repository
{
    public class DocumentRepo : IDocumentManager
    {
        private readonly ApplicationDBContext _context;
        private readonly IMemoryCache _cache;
        private readonly ILogger<DocumentRepo> _logger;

        public DocumentRepo(ApplicationDBContext context, IMemoryCache cache, ILogger<DocumentRepo> logger)
        {
            _context = context;
            _cache = cache;
            _logger = logger;
        }

        public async Task<DocumentModelDto> CreateDocumentAsync(DocumentModelDto documentDto)
        {
            var documentModel = new DocumentModel
            {
                FileName = documentDto.FileName,
                FilePath = documentDto.FilePath,
                UploadDate = DateTime.Now, // Example: Set the upload date
                CaseId = documentDto.CaseId // Assuming CaseId is set in DTO
            };

            await _context.Documents.AddAsync(documentModel);
            await _context.SaveChangesAsync();

            _cache.Remove("documentRecords"); // Invalidate cache

            _logger.LogInformation("Document created and cache invalidated.");

            // Convert DocumentModel to DocumentModelDto and return
            return new DocumentModelDto
            {
                DocumentId = documentModel.DocumentId,
                FileName = documentModel.FileName,
                FilePath = documentModel.FilePath,
                UploadDate = documentModel.UploadDate,
                CaseId = documentModel.CaseId // Assuming CaseId is needed in DTO
                // Include other properties as needed
            };
        }

        public async Task<DocumentModelDto> DeleteDocumentAsync(int id)
        {
            var documentModel = await _context.Documents.FirstOrDefaultAsync(x => x.DocumentId == id);
            if (documentModel == null)
            {
                _logger.LogWarning("Document with ID {Id} not found.", id);
                return null;
            }

            _context.Documents.Remove(documentModel);
            await _context.SaveChangesAsync();

            _cache.Remove("documentRecords"); // Invalidate cache

            _logger.LogInformation("Document deleted and cache invalidated.");

            // Convert DocumentModel to DocumentModelDto and return
            return new DocumentModelDto
            {
                DocumentId = documentModel.DocumentId,
                FileName = documentModel.FileName,
                FilePath = documentModel.FilePath,
                UploadDate = documentModel.UploadDate,
                CaseId = documentModel.CaseId // Assuming CaseId is needed in DTO
                // Include other properties as needed
            };
        }

        public async Task<List<DocumentModelDto>> GetAllDocumentsAsync()
        {
            var documentModels = await _context.Documents.ToListAsync();

            // Convert List<DocumentModel> to List<DocumentModelDto> and return
            return documentModels.Select(documentModel => new DocumentModelDto
            {
                DocumentId = documentModel.DocumentId,
                FileName = documentModel.FileName,
                FilePath = documentModel.FilePath,
                UploadDate = documentModel.UploadDate,
                CaseId = documentModel.CaseId // Assuming CaseId is needed in DTO
                // Include other properties as needed
            }).ToList();
        }

        public async Task<DocumentModelDto> GetDocumentByIdAsync(int id)
        {
            var documentModel = await _context.Documents
                .Include(d => d.Case) // Include related Case if necessary
                .FirstOrDefaultAsync(x => x.DocumentId == id);

            if (documentModel == null)
            {
                _logger.LogWarning("Document with ID {Id} not found.", id);
                return null;
            }

            // Convert DocumentModel to DocumentModelDto and return
            return new DocumentModelDto
            {
                DocumentId = documentModel.DocumentId,
                FileName = documentModel.FileName,
                FilePath = documentModel.FilePath,
                UploadDate = documentModel.UploadDate,
                CaseId = documentModel.CaseId // Assuming CaseId is needed in DTO
                // Include other properties as needed
            };
        }

        public async Task<DocumentModelDto> UpdateDocumentAsync(int id, UpdateDocumentDto documentDto)
        {
            var documentModel = await _context.Documents.FirstOrDefaultAsync(x => x.DocumentId == id);
            if (documentModel == null)
            {
                _logger.LogWarning("Document with ID {Id} not found.", id);
                return null;
            }

            documentModel.FileName = documentDto.FileName;
            documentModel.FilePath = documentDto.FilePath;

            await _context.SaveChangesAsync();

            _cache.Remove("documentRecords"); // Invalidate cache

            _logger.LogInformation("Document updated and cache invalidated.");

            // Convert DocumentModel to DocumentModelDto and return
            return new DocumentModelDto
            {
                DocumentId = documentModel.DocumentId,
                FileName = documentModel.FileName,
                FilePath = documentModel.FilePath,
                UploadDate = documentModel.UploadDate,
                CaseId = documentModel.CaseId // Assuming CaseId is needed in DTO
                // Include other properties as needed
            };
        }
    }
}
