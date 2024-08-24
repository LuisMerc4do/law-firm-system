using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using law_firm_management.Controllers;
using law_firm_management.Dto.DocumentDto;
using law_firm_management.interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace law_firm_management.Tests
{
    public class DocumentControllerTests
    {
        private readonly Mock<IDocumentManager> _mockDocumentManager;
        private readonly Mock<ILogger<DocumentController>> _mockLogger;
        private readonly DocumentController _controller;

        public DocumentControllerTests()
        {
            _mockDocumentManager = new Mock<IDocumentManager>();
            _mockLogger = new Mock<ILogger<DocumentController>>();
            _controller = new DocumentController(_mockDocumentManager.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task GetAllDocuments_ReturnsOkResult_WithListOfDocuments()
        {
            // Arrange
            var documents = new List<DocumentModelDto> { new DocumentModelDto(), new DocumentModelDto() };
            _mockDocumentManager.Setup(m => m.GetAllDocumentsAsync()).ReturnsAsync(documents);

            // Act
            var result = await _controller.GetAllDocuments();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedDocuments = Assert.IsType<List<DocumentModelDto>>(okResult.Value);
            Assert.Equal(2, returnedDocuments.Count);
        }

        [Fact]
        public async Task GetDocumentById_WithValidId_ReturnsOkResult()
        {
            // Arrange
            var documentId = 1;
            var documentDto = new DocumentModelDto { DocumentId = documentId };
            _mockDocumentManager.Setup(m => m.GetDocumentByIdAsync(documentId)).ReturnsAsync(documentDto);

            // Act
            var result = await _controller.GetDocumentById(documentId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedDocument = Assert.IsType<DocumentModelDto>(okResult.Value);
            Assert.Equal(documentId, returnedDocument.DocumentId);
        }

        [Fact]
        public async Task CreateDocument_WithValidData_ReturnsCreatedAtActionResult()
        {
            // Arrange
            var createDocumentDto = new CreateDocumentDto
            {
                CaseId = 1,
                FileName = "Test Document",
                FilePath = "/path/to/document"
            };
            var createdDocument = new DocumentModelDto { DocumentId = 1, FileName = createDocumentDto.FileName };
            _mockDocumentManager.Setup(m => m.CreateDocumentAsync(It.IsAny<DocumentModelDto>())).ReturnsAsync(createdDocument);

            // Act
            var result = await _controller.CreateDocument(createDocumentDto);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(nameof(DocumentController.GetDocumentById), createdAtActionResult.ActionName);
            Assert.Equal(1, createdAtActionResult.RouteValues["id"]);
            var returnedDocument = Assert.IsType<DocumentModelDto>(createdAtActionResult.Value);
            Assert.Equal(createDocumentDto.FileName, returnedDocument.FileName);
        }

        [Fact]
        public async Task UpdateDocument_WithValidData_ReturnsOkResult()
        {
            // Arrange
            var documentId = 1;
            var updateDocumentDto = new UpdateDocumentDto { FileName = "Updated Document" };
            var updatedDocument = new DocumentModelDto { DocumentId = documentId, FileName = updateDocumentDto.FileName };
            _mockDocumentManager.Setup(m => m.UpdateDocumentAsync(documentId, updateDocumentDto)).ReturnsAsync(updatedDocument);

            // Act
            var result = await _controller.UpdateDocument(documentId, updateDocumentDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedDocument = Assert.IsType<DocumentModelDto>(okResult.Value);
            Assert.Equal(updateDocumentDto.FileName, returnedDocument.FileName);
        }

        [Fact]
        public async Task DeleteDocument_WithValidId_ReturnsOkResult()
        {
            // Arrange
            var documentId = 1;
            var deletedDocument = new DocumentModelDto { DocumentId = documentId };
            _mockDocumentManager.Setup(m => m.DeleteDocumentAsync(documentId)).ReturnsAsync(deletedDocument);

            // Act
            var result = await _controller.DeleteDocument(documentId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedDocument = Assert.IsType<DocumentModelDto>(okResult.Value);
            Assert.Equal(documentId, returnedDocument.DocumentId);
        }
    }
}