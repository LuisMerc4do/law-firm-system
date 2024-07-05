using law_firm_management.Dto.DocumentDto;
using law_firm_management.interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace law_firm_management.Controllers
{
    [Route("api/v1/documents")]
    [ApiController]
    public class DocumentController : ControllerBase
    {
        private readonly IDocumentManager _documentManager;
        private readonly ILogger<DocumentController> _logger;

        public DocumentController(IDocumentManager documentManager, ILogger<DocumentController> logger)
        {
            _documentManager = documentManager;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllDocuments()
        {
            try
            {
                var documents = await _documentManager.GetAllDocumentsAsync();
                return Ok(documents);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving all documents.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDocumentById(int id)
        {
            try
            {
                var documentDto = await _documentManager.GetDocumentByIdAsync(id);
                if (documentDto == null)
                {
                    return NotFound();
                }
                return Ok(documentDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving the document with ID {Id}.", id);
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateDocument([FromBody] CreateDocumentDto createDocumentDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var createdDocument = await _documentManager.CreateDocumentAsync(new DocumentModelDto
                {
                    CaseId = createDocumentDto.CaseId,
                    FileName = createDocumentDto.FileName,
                    FilePath = createDocumentDto.FilePath,
                    UploadDate = DateTime.Now // Set upload date here
                });

                return CreatedAtAction(nameof(GetDocumentById), new { id = createdDocument.DocumentId }, createdDocument);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating a new document.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDocument(int id, [FromBody] UpdateDocumentDto updateDocumentDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var updatedDocument = await _documentManager.UpdateDocumentAsync(id, updateDocumentDto);
                if (updatedDocument == null)
                {
                    return NotFound();
                }

                return Ok(updatedDocument);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the document with ID {Id}.", id);
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDocument(int id)
        {
            try
            {
                var deletedDocument = await _documentManager.DeleteDocumentAsync(id);
                if (deletedDocument == null)
                {
                    return NotFound();
                }

                return Ok(deletedDocument);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the document with ID {Id}.", id);
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
    }
}
