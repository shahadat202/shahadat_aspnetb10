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
        public void InsertProduct_ValidProduct_ShouldAddProductAndLogActivity()
        {
            // Arrange
            var product = new Product()
            {
                Id = Guid.NewGuid(),
                Title = "Test Product",
                Price = 100
            };
            var username = "test_user";

            // Act
            _productManagementService.InsertProduct(product, username);

            // Assert
            _productRepositoryMock.
                Verify(x => x.Add(It.Is<Product>(y => y == product)), Times.Once);
            _activityLogRepositoryMock.Verify(x => x.Add(It.Is<ActivityLog>(log =>
                log.Username == username &&
                log.Action == "Inserted" &&
                log.ItemName == product.Title)), Times.Once);
            _inventoryUnitOfWorkMock.Verify(x => x.Save(), Times.Once);
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

        [Test]
        public void InsertProduct_InvalidProduct_ThrowsExeption()
        {
            // Arrange
            var product = new Product()
            {
                Id = Guid.NewGuid(),
                Title = null, 
                Price = 100
            };
            var username = "test_user";

            // Act & Assert
            Should.Throw<ArgumentNullException>(() =>
                _productManagementService.InsertProduct(product, username));

            // Verify
            _productRepositoryMock.Verify(x => x.Add(It.IsAny<Product>()), Times.Never);
            _activityLogRepositoryMock.Verify(x => x.Add(It.IsAny<ActivityLog>()), Times.Never);
            _inventoryUnitOfWorkMock.Verify(x => x.Save(), Times.Never);
        }

        [Test]
        public void UpdateProduct_DuplicateTitle_ThrowsExecption()
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

            _inventoryUnitOfWorkMock.Setup(x => x.ProductRepository).Returns(_productRepositoryMock.Object);    
            _productRepositoryMock.Setup(x => x.IsTitleDuplicate(product.Title, product.Id)).Returns(true);

            // Act
            var ex = Assert.Throws<InvalidOperationException>(() => _productManagementService.UpdateProduct(product, "admin"));

            // Assert
            Assert.AreEqual("Title should be unique.", ex?.Message);
        }

        [Test]
        public void DeleteProduct_ConfirmDelete_SuccessfullyDeleted()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var product = new Product { Id = productId, Title = "Test Product" };
            var username = "test_user";

            _productRepositoryMock.Setup(x => x.GetById(productId)).Returns(product);   
            
            // Act
            _productManagementService.DeleteProduct(productId, username);

            // Assert
            _productRepositoryMock.Verify(x => x.GetById(productId), Times.Once()); 
            _productRepositoryMock.Verify(x => x.Remove(productId), Times.Once());
            _inventoryUnitOfWorkMock.Verify(x => x.Save(), Times.Once());
        }

        [Test]
        public void DeleteProduct_ProductDoesNotExist_ThrowsException()
        {
            // Arrange 
            var productId = Guid.NewGuid();
            var username = "test_user";

            _productRepositoryMock.Setup(x => x.GetById(productId)).Returns((Product)null);

            // Act

            // Assert
            Assert.Throws<InvalidOperationException>(() => _productManagementService.DeleteProduct(productId, username));

            _productRepositoryMock.Verify(x => x.GetById(productId), Times.Once);
            _productRepositoryMock.Verify(x => x.Remove(It.IsAny<Guid>()), Times.Never);
            _inventoryUnitOfWorkMock.Verify(x => x.Save(), Times.Never);
        }
    }
}
