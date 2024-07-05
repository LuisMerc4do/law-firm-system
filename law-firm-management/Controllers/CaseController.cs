using law_firm_management.Dto.CaseDto;
using law_firm_management.interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace law_firm_management.Controllers
{
    [Route("api/v1/cases")]
    [ApiController]
    public class CaseController : ControllerBase
    {
        private readonly ICaseManager _caseManager;
        private readonly ILogger<CaseController> _logger;

        public CaseController(ICaseManager caseManager, ILogger<CaseController> logger)
        {
            _caseManager = caseManager;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCases()
        {
            try
            {
                var cases = await _caseManager.GetAllCasesAsync();
                return Ok(cases);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving all cases.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCaseById(int id)
        {
            try
            {
                var caseDto = await _caseManager.GetCaseByIdAsync(id);
                if (caseDto == null)
                {
                    return NotFound();
                }
                return Ok(caseDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving the case with ID {Id}.", id);
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateCase([FromBody] CreateCaseDto createCaseDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var createdCase = await _caseManager.CreateCaseAsync(new CaseModelDto
                {
                    Title = createCaseDto.Title,
                    Description = createCaseDto.Description,
                    CreatedById = createCaseDto.CreatedById,
                    AssignedToId = createCaseDto.AssignedToId,
                    DateCreated = DateTime.Now // Set creation date here
                });

                return CreatedAtAction(nameof(GetCaseById), new { id = createdCase.CaseId }, createdCase);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating a new case.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCase(int id, [FromBody] UpdateCaseDto updateCaseDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var updatedCase = await _caseManager.UpdateCaseAsync(id, updateCaseDto);
                if (updatedCase == null)
                {
                    return NotFound();
                }

                return Ok(updatedCase);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the case with ID {Id}.", id);
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCase(int id)
        {
            try
            {
                var deletedCase = await _caseManager.DeleteCaseAsync(id);
                if (deletedCase == null)
                {
                    return NotFound();
                }

                return Ok(deletedCase);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the case with ID {Id}.", id);
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
    }
}
