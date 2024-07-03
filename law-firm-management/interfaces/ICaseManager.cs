using law_firm_management.Dto.CaseDto;
using law_firm_management.Models;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace law_firm_management.interfaces;

public interface ICaseManager
{
    Task<CaseModelDto> CreateCaseAsync(CaseModelDto caseDto);
    Task<CaseModelDto> GetCaseByIdAsync(int id);
    Task<List<CaseModelDto>> GetAllCasesAsync();
    Task<CaseModelDto> UpdateCaseAsync(int id, UpdateCaseDto caseDto);
    Task<CaseModelDto> DeleteCaseAsync(int id);
}
