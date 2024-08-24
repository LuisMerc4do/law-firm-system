using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using law_firm_management.Controllers;
using law_firm_management.Dto.MessageDto;
using law_firm_management.interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace law_firm_management.Tests
{
    public class MessageControllerTests
    {
        private readonly Mock<IMessageManager> _mockMessageManager;
        private readonly Mock<ILogger<MessageController>> _mockLogger;
        private readonly MessageController _controller;

        public MessageControllerTests()
        {
            _mockMessageManager = new Mock<IMessageManager>();
            _mockLogger = new Mock<ILogger<MessageController>>();
            _controller = new MessageController(_mockMessageManager.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task GetAllMessages_ReturnsOkResult_WithListOfMessages()
        {
            // Arrange
            var messages = new List<MessageModelDto> { new MessageModelDto(), new MessageModelDto() };
            _mockMessageManager.Setup(m => m.GetAllMessagesAsync()).ReturnsAsync(messages);

            // Act
            var result = await _controller.GetAllMessages();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedMessages = Assert.IsType<List<MessageModelDto>>(okResult.Value);
            Assert.Equal(2, returnedMessages.Count);
        }
        [Fact]
        public async Task GetMessageById_WithValidId_ReturnsOkResult()
        {
            // Arrange
            var messageId = 1;
            var messageDto = new MessageModelDto { MessageId = messageId };
            _mockMessageManager.Setup(m => m.GetMessageByIdAsync(messageId)).ReturnsAsync(messageDto);

            // Act
            var result = await _controller.GetMessageById(messageId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedMessage = Assert.IsType<MessageModelDto>(okResult.Value);
            Assert.Equal(messageId, returnedMessage.MessageId);
        }

        [Fact]
        public async Task GetMessageById_WithInvalidId_ReturnsNotFound()
        {
            // Arrange
            var messageId = 1;
            _mockMessageManager.Setup(m => m.GetMessageByIdAsync(messageId)).ReturnsAsync((MessageModelDto)null);

            // Act
            var result = await _controller.GetMessageById(messageId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task CreateMessage_WithValidData_ReturnsCreatedAtActionResult()
        {
            // Arrange
            var createMessageDto = new CreateMessageDto
            {
                CaseId = 1,
                SenderId = "1",
                Content = "Test Message"
            };
            var createdMessage = new MessageModelDto { MessageId = 1, Content = createMessageDto.Content };
            _mockMessageManager.Setup(m => m.CreateMessageAsync(It.IsAny<MessageModelDto>())).ReturnsAsync(createdMessage);

            // Act
            var result = await _controller.CreateMessage(createMessageDto);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(nameof(MessageController.GetMessageById), createdAtActionResult.ActionName);
            Assert.Equal(1, createdAtActionResult.RouteValues["id"]);
            var returnedMessage = Assert.IsType<MessageModelDto>(createdAtActionResult.Value);
            Assert.Equal(createMessageDto.Content, returnedMessage.Content);
        }

        [Fact]
        public async Task CreateMessage_WithInvalidModel_ReturnsBadRequest()
        {
            // Arrange
            _controller.ModelState.AddModelError("Content", "Content is required");
            var createMessageDto = new CreateMessageDto();

            // Act
            var result = await _controller.CreateMessage(createMessageDto);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task UpdateMessage_WithValidData_ReturnsOkResult()
        {
            // Arrange
            var messageId = 1;
            var updateMessageDto = new UpdateMessageDto { Content = "Updated Message" };
            var updatedMessage = new MessageModelDto { MessageId = messageId, Content = updateMessageDto.Content };
            _mockMessageManager.Setup(m => m.UpdateMessageAsync(messageId, updateMessageDto)).ReturnsAsync(updatedMessage);

            // Act
            var result = await _controller.UpdateMessage(messageId, updateMessageDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedMessage = Assert.IsType<MessageModelDto>(okResult.Value);
            Assert.Equal(updateMessageDto.Content, returnedMessage.Content);
        }

        [Fact]
        public async Task UpdateMessage_WithInvalidId_ReturnsNotFound()
        {
            // Arrange
            var messageId = 1;
            var updateMessageDto = new UpdateMessageDto { Content = "Updated Message" };
            _mockMessageManager.Setup(m => m.UpdateMessageAsync(messageId, updateMessageDto)).ReturnsAsync((MessageModelDto)null);

            // Act
            var result = await _controller.UpdateMessage(messageId, updateMessageDto);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task DeleteMessage_WithValidId_ReturnsOkResult()
        {
            // Arrange
            var messageId = 1;
            var deletedMessage = new MessageModelDto { MessageId = messageId };
            _mockMessageManager.Setup(m => m.DeleteMessageAsync(messageId)).ReturnsAsync(deletedMessage);

            // Act
            var result = await _controller.DeleteMessage(messageId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedMessage = Assert.IsType<MessageModelDto>(okResult.Value);
            Assert.Equal(messageId, returnedMessage.MessageId);
        }

        [Fact]
        public async Task DeleteMessage_WithInvalidId_ReturnsNotFound()
        {
            // Arrange
            var messageId = 1;
            _mockMessageManager.Setup(m => m.DeleteMessageAsync(messageId)).ReturnsAsync((MessageModelDto)null);

            // Act
            var result = await _controller.DeleteMessage(messageId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task DeleteMessage_WhenExceptionOccurs_ReturnsInternalServerError()
        {
            // Arrange
            var messageId = 1;
            _mockMessageManager.Setup(m => m.DeleteMessageAsync(messageId)).ThrowsAsync(new Exception("Test exception"));

            // Act
            var result = await _controller.DeleteMessage(messageId);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
        }
    }
}