using law_firm_management.Data;
using law_firm_management.Dto.CaseDto;
using law_firm_management.interfaces;
using law_firm_management.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace law_firm_management.Repository
{
    public class CaseRepository : ICaseManager
    {
        private readonly ApplicationDBContext _context;
        private readonly IMemoryCache _cache;
        private readonly ILogger<CaseRepository> _logger;

        public CaseRepository(ApplicationDBContext context, IMemoryCache cache, ILogger<CaseRepository> logger)
        {
            _context = context;
            _cache = cache;
            _logger = logger;
        }

        public async Task<CaseModelDto> CreateCaseAsync(CaseModelDto caseDto)
        {
            var caseModel = new CaseModel
            {
                Title = caseDto.Title,
                Description = caseDto.Description,
                AssignedToId = caseDto.AssignedToId,
                DateCreated = DateTime.Now // Example: Set the creation date
            };

            await _context.Cases.AddAsync(caseModel);
            await _context.SaveChangesAsync();

            _cache.Remove("caseRecords"); // Invalidate cache

            _logger.LogInformation("Case created and cache invalidated.");

            // Convert CaseModel back to CaseDto and return
            return new CaseModelDto
            {
                CaseId = caseModel.CaseId,
                Title = caseModel.Title,
                Description = caseModel.Description,
                AssignedToId = caseModel.AssignedToId,
                DateCreated = caseModel.DateCreated
                // Include other properties as needed
            };
        }

        public async Task<CaseModelDto> DeleteCaseAsync(int id)
        {
            var caseModel = await _context.Cases.FirstOrDefaultAsync(x => x.CaseId == id);
            if (caseModel == null)
            {
                _logger.LogWarning("Case with ID {Id} not found.", id);
                return null;
            }

            _context.Cases.Remove(caseModel);
            await _context.SaveChangesAsync();

            _cache.Remove("caseRecords"); // Invalidate cache

            _logger.LogInformation("Case deleted and cache invalidated.");

            // Convert CaseModel back to CaseDto and return
            return new CaseModelDto
            {
                CaseId = caseModel.CaseId,
                Title = caseModel.Title,
                Description = caseModel.Description,
                AssignedToId = caseModel.AssignedToId,
                DateCreated = caseModel.DateCreated
                // Include other properties as needed
            };
        }

        public async Task<List<CaseModelDto>> GetAllCasesAsync()
        {
            var caseModels = await _context.Cases.ToListAsync();

            // Convert List<CaseModel> to List<CaseDto> and return
            return caseModels.Select(caseModel => new CaseModelDto
            {
                CaseId = caseModel.CaseId,
                Title = caseModel.Title,
                Description = caseModel.Description,
                AssignedToId = caseModel.AssignedToId,
                DateCreated = caseModel.DateCreated
                // Include other properties as needed
            }).ToList();
        }

        public async Task<CaseModelDto> GetCaseByIdAsync(int id)
        {
            var caseModel = await _context.Cases
                .Include(a => a.AssignedTo)
                .FirstOrDefaultAsync(x => x.CaseId == id);

            if (caseModel == null)
            {
                _logger.LogWarning("Case with ID {Id} not found.", id);
                return null;
            }

            // Convert CaseModel to CaseDto and return
            return new CaseModelDto
            {
                CaseId = caseModel.CaseId,
                Title = caseModel.Title,
                Description = caseModel.Description,
                AssignedToId = caseModel.AssignedToId,
                DateCreated = caseModel.DateCreated
                // Include other properties as needed
            };
        }

        public async Task<CaseModelDto> UpdateCaseAsync(int id, UpdateCaseDto caseDto)
        {
            var caseModel = await _context.Cases.FirstOrDefaultAsync(x => x.CaseId == id);
            if (caseModel == null)
            {
                _logger.LogWarning("Case with ID {Id} not found.", id);
                return null;
            }

            caseModel.Title = caseDto.Title;
            caseModel.Description = caseDto.Description;
            caseModel.AssignedToId = caseDto.AssignedToId;
            caseModel.DateClosed = caseDto.DateClosed;

            await _context.SaveChangesAsync();

            _cache.Remove("caseRecords"); // Invalidate cache

            _logger.LogInformation("Case updated and cache invalidated.");

            // Convert CaseModel back to CaseDto and return
            return new CaseModelDto
            {
                CaseId = caseModel.CaseId,
                Title = caseModel.Title,
                Description = caseModel.Description,
                AssignedToId = caseModel.AssignedToId,
                DateCreated = caseModel.DateCreated,
                DateClosed = caseModel.DateClosed
                // Include other properties as needed
            };
        }
    }
}
