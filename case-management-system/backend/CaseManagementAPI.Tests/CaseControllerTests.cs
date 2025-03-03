using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using CaseManagementAPI.Controllers;
using CaseManagementAPI.Data;
using CaseManagementAPI.Models;

namespace CaseManagementAPI.Tests
{
    public class CaseControllerTests
    {
        private async Task<AppDbContext> GetDatabaseContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;

            var dbContext = new AppDbContext(options);
            dbContext.Database.EnsureCreated();

            // Seed some test data
            dbContext.CustomerCases.Add(new CustomerCase { Id = 1, CustomerName = "John Doe", CaseChannel = "Email", Status = "Open" });
            dbContext.CustomerCases.Add(new CustomerCase { Id = 2, CustomerName = "Jane Smith", CaseChannel = "WhatsApp", Status = "Closed" });
            await dbContext.SaveChangesAsync();

            return dbContext;
        }

        [Fact]
        public async Task GetCases_ReturnsListOfCases()
        {
            // Arrange
            var dbContext = await GetDatabaseContext();
            var controller = new CaseController(dbContext);

            // Act
            var result = await controller.GetCases();

            // Assert
            var actionResult = Assert.IsType<ActionResult<IEnumerable<CustomerCase>>>(result);
            var cases = Assert.IsType<List<CustomerCase>>(actionResult.Value);
            Assert.Equal(2, cases.Count);
        }

        [Fact]
        public async Task GetCase_ExistingId_ReturnsCase()
        {
            // Arrange
            var dbContext = await GetDatabaseContext();
            var controller = new CaseController(dbContext);
            int testId = 1;

            // Act
            var result = await controller.GetCase(testId);

            // Assert
            var actionResult = Assert.IsType<ActionResult<CustomerCase>>(result);
            var caseItem = Assert.IsType<CustomerCase>(actionResult.Value);
            Assert.Equal("John Doe", caseItem.CustomerName);
        }

        [Fact]
        public async Task GetCase_NonExistingId_ReturnsNotFound()
        {
            // Arrange
            var dbContext = await GetDatabaseContext();
            var controller = new CaseController(dbContext);
            int nonExistingId = 999;

            // Act
            var result = await controller.GetCase(nonExistingId);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task PostCase_ValidCase_ReturnsCreatedCase()
        {
            // Arrange
            var dbContext = await GetDatabaseContext();
            var controller = new CaseController(dbContext);
            var newCase = new CustomerCase { CustomerName = "Alice Johnson", CaseChannel = "Call", Status = "Open" };

            // Act
            var result = await controller.PostCase(newCase);

            // Assert
            var actionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var createdCase = Assert.IsType<CustomerCase>(actionResult.Value);
            Assert.Equal("Alice Johnson", createdCase.CustomerName);
        }

        [Fact]
        public async Task PutCase_ExistingId_UpdatesCase()
        {
            // Arrange
            var dbContext = await GetDatabaseContext();
            var controller = new CaseController(dbContext);
            int existingId = 1;
            var updatedCase = new CustomerCase { Id = existingId, CustomerName = "John Updated", CaseChannel = "Email", Status = "In Progress" };

            // Act
            var result = await controller.PutCase(existingId, updatedCase);

            // Assert
            Assert.IsType<NoContentResult>(result);
            var updatedRecord = await dbContext.CustomerCases.FindAsync(existingId);
            Assert.Equal("John Updated", updatedRecord.CustomerName);
            Assert.Equal("In Progress", updatedRecord.Status);
        }

        [Fact]
        public async Task PutCase_NonExistingId_ReturnsBadRequest()
        {
            // Arrange
            var dbContext = await GetDatabaseContext();
            var controller = new CaseController(dbContext);
            int nonExistingId = 999;
            var updatedCase = new CustomerCase { Id = nonExistingId, CustomerName = "Invalid", CaseChannel = "AI", Status = "Open" };

            // Act
            var result = await controller.PutCase(nonExistingId, updatedCase);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task DeleteCase_ExistingId_RemovesCase()
        {
            // Arrange
            var dbContext = await GetDatabaseContext();
            var controller = new CaseController(dbContext);
            int existingId = 1;

            // Act
            var result = await controller.DeleteCase(existingId);

            // Assert
            Assert.IsType<NoContentResult>(result);
            var deletedCase = await dbContext.CustomerCases.FindAsync(existingId);
            Assert.Null(deletedCase);
        }

        [Fact]
        public async Task DeleteCase_NonExistingId_ReturnsNotFound()
        {
            // Arrange
            var dbContext = await GetDatabaseContext();
            var controller = new CaseController(dbContext);
            int nonExistingId = 999;

            // Act
            var result = await controller.DeleteCase(nonExistingId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}