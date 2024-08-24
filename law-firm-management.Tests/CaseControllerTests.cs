using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using law_firm_management.Controllers;
using law_firm_management.Dto.CaseDto;
using law_firm_management.interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace law_firm_management.Tests
{
    public class CaseControllerTests
    {
        private readonly Mock<ICaseManager> _mockCaseManager;
        private readonly Mock<ILogger<CaseController>> _mockLogger;
        private readonly CaseController _controller;

        public CaseControllerTests()
        {
            _mockCaseManager = new Mock<ICaseManager>();
            _mockLogger = new Mock<ILogger<CaseController>>();
            _controller = new CaseController(_mockCaseManager.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task GetAllCases_ReturnsOkResult_WithListOfCases()
        {
            // Arrange
            var cases = new List<CaseModelDto> { new CaseModelDto(), new CaseModelDto() };
            _mockCaseManager.Setup(m => m.GetAllCasesAsync()).ReturnsAsync(cases);

            // Act
            var result = await _controller.GetAllCases();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedCases = Assert.IsType<List<CaseModelDto>>(okResult.Value);
            Assert.Equal(2, returnedCases.Count);
        }

        [Fact]
        public async Task GetCaseById_WithValidId_ReturnsOkResult()
        {
            // Arrange
            var caseId = 1;
            var caseDto = new CaseModelDto { CaseId = caseId };
            _mockCaseManager.Setup(m => m.GetCaseByIdAsync(caseId)).ReturnsAsync(caseDto);

            // Act
            var result = await _controller.GetCaseById(caseId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedCase = Assert.IsType<CaseModelDto>(okResult.Value);
            Assert.Equal(caseId, returnedCase.CaseId);
        }

        [Fact]
        public async Task CreateCase_WithValidData_ReturnsCreatedAtActionResult()
        {
            // Arrange
            var createCaseDto = new CreateCaseDto
            {
                Title = "Test Case",
                Description = "Test Description",
                CreatedById = "1",
                AssignedToId = "2"
            };
            var createdCase = new CaseModelDto { CaseId = 1, Title = createCaseDto.Title };
            _mockCaseManager.Setup(m => m.CreateCaseAsync(It.IsAny<CaseModelDto>())).ReturnsAsync(createdCase);

            // Act
            var result = await _controller.CreateCase(createCaseDto);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(nameof(CaseController.GetCaseById), createdAtActionResult.ActionName);
            Assert.Equal(1, createdAtActionResult.RouteValues["id"]);
            var returnedCase = Assert.IsType<CaseModelDto>(createdAtActionResult.Value);
            Assert.Equal(createCaseDto.Title, returnedCase.Title);
        }

        [Fact]
        public async Task UpdateCase_WithValidData_ReturnsOkResult()
        {
            // Arrange
            var caseId = 1;
            var updateCaseDto = new UpdateCaseDto { Title = "Updated Title" };
            var updatedCase = new CaseModelDto { CaseId = caseId, Title = updateCaseDto.Title };
            _mockCaseManager.Setup(m => m.UpdateCaseAsync(caseId, updateCaseDto)).ReturnsAsync(updatedCase);

            // Act
            var result = await _controller.UpdateCase(caseId, updateCaseDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedCase = Assert.IsType<CaseModelDto>(okResult.Value);
            Assert.Equal(updateCaseDto.Title, returnedCase.Title);
        }

        [Fact]
        public async Task DeleteCase_WithValidId_ReturnsOkResult()
        {
            // Arrange
            var caseId = 1;
            var deletedCase = new CaseModelDto { CaseId = caseId };
            _mockCaseManager.Setup(m => m.DeleteCaseAsync(caseId)).ReturnsAsync(deletedCase);

            // Act
            var result = await _controller.DeleteCase(caseId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedCase = Assert.IsType<CaseModelDto>(okResult.Value);
            Assert.Equal(caseId, returnedCase.CaseId);
        }
    }
}