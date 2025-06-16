using NUnit.Framework;
using SpendingWeb.Controllers;
using SpendingWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.Sqlite;

namespace SpendingWeb.Tests.Controllers
{
    [TestFixture]
    public class HomeControllerTests
    {
        private HomeController _controller;
        private Mock<ILogger<HomeController>> _loggerMock;
        private SpendSmartDbContext _context;
        private SqliteConnection _connection;

        [SetUp]
        public void Setup()
        {
            // Create and open a SQLite connection in memory
            _connection = new SqliteConnection("DataSource=:memory:");
            _connection.Open();

            // Configure the DbContext to use SQLite
            var options = new DbContextOptionsBuilder<SpendSmartDbContext>()
                .UseSqlite(_connection)
                .Options;

            // Create context and ensure database is created
            _context = new SpendSmartDbContext(options);
            _context.Database.EnsureCreated();

            _loggerMock = new Mock<ILogger<HomeController>>();
            _controller = new HomeController(_loggerMock.Object, _context);
        }

        [TearDown]
        public void Cleanup()
        {
            _context.Dispose();
            _connection.Dispose();
            _controller.Dispose();
        }

        [Test]
        public void Index_ReturnsViewResult()
        {
            // Act
            var result = _controller.Index();

            // Assert
            Assert.That(result, Is.TypeOf<ViewResult>());
        }

        [Test]
        public void Privacy_ReturnsViewResult()
        {
            // Act
            var result = _controller.Privacy();

            // Assert
            Assert.That(result, Is.TypeOf<ViewResult>());
        }

        [Test]
        public void Expense_ReturnsViewResultWithExpensesList()
        {
            // Arrange
            var testExpenses = new List<Expense>
            {
                new Expense { Id = 1, Amount = 100, Description = "Test1" },
                new Expense { Id = 2, Amount = 200, Description = "Test2" }
            };
            _context.Expenses.AddRange(testExpenses);
            _context.SaveChanges();

            // Act
            var result = _controller.Expense() as ViewResult;
            var model = result?.Model as List<Expense>;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(model, Is.Not.Null);
            Assert.That(model?.Count, Is.EqualTo(2));
            Assert.That(result.ViewData["Expense"], Is.EqualTo(300));
        }
    }
}