using Autofac.Extras.Moq;
using DevSkill.Inventory.Application.Services;
using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Domain.RepositoryContracts;
using Moq;
using NUnit.Framework;
using Shouldly;
using System;
using System.Diagnostics.CodeAnalysis;

namespace DevSkill.Inventory.Application.Tests
{
    [TestFixture]
    public class ProductManagementServiceTests
    {
        private AutoMock _moq;
        private IProductManagementService _productManagementService;
        private Mock<IInventoryUnitOfWork> _inventoryUnitOfWorkMock;
        private Mock<IProductRepository> _productRepositoryMock;
        private Mock<IActivityLogRepository> _activityLogRepositoryMock;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _moq = AutoMock.GetLoose();
        }

        [SetUp]
        public void Setup()
        {
            _productManagementService = _moq.Create<ProductManagementService>();
            _inventoryUnitOfWorkMock = _moq.Mock<IInventoryUnitOfWork>();
            _productRepositoryMock = _moq.Mock<IProductRepository>();
            _activityLogRepositoryMock = _moq.Mock<IActivityLogRepository>();

            _inventoryUnitOfWorkMock.Setup(x => x.ProductRepository).Returns(_productRepositoryMock.Object);
            _inventoryUnitOfWorkMock.Setup(x => x.Save()).Verifiable();
            _activityLogRepositoryMock.Setup(x => x.Add(It.IsAny<ActivityLog>())).Verifiable();
        }

        [TearDown]
        public void Teardown()
        {
            _inventoryUnitOfWorkMock?.Reset();
            _productRepositoryMock?.Reset();
            _activityLogRepositoryMock?.Reset();
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            _moq?.Dispose();
        }

        [Test]
        public void InsertProduct_TitleNotDuplicate_InsertSuccessfully()
        {
            // Arrange
            var product = new Product
            {
                Id = Guid.NewGuid(),
                Title = "Football",
                Price = 20,
                Quantity = 5,
                MinLevel = 1,
                Notes = "New product",
                CreatedDate = new DateTime(2024, 11, 15)
            };

            _productRepositoryMock.Setup(x => x.IsTitleDuplicate(product.Title, null)).Returns(false);
            _productRepositoryMock.Setup(x => x.Add(It.IsAny<Product>())).Callback<Product>(p => p.Id = product.Id);

            // Act
            _productManagementService.InsertProduct(product, "admin");

            // Assert
            _productRepositoryMock.Verify(x => x.Add(It.Is<Product>(p => p.Title == product.Title)), Times.Once);
            _activityLogRepositoryMock.Verify(x => x.Add(It.Is<ActivityLog>(log => log.ItemName == product.Title && log.Action == "Inserted")), Times.Once);
            _inventoryUnitOfWorkMock.Verify(x => x.Save(), Times.Once);
        }
    }
}
