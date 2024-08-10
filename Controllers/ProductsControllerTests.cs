
using Microsoft.AspNetCore.Mvc;
using Moq;
using VendingMachineApp.Controllers;
using VendingMachineApp.Data;
using VendingMachineApp.Models;
using Xunit;

public class ProductsControllerTests
{
    [Fact]
    public async Task Index_ReturnsAViewResult_WithAListOfProducts()
    {
        // Arrange
        var mockRepo = new Mock<IRepository<Product>>();
        mockRepo.Setup(repo => repo.GetAllAsync()).ReturnsAsync(GetTestProducts());
        var controller = new ProductsController(mockRepo.Object);

        // Act
        var result = await controller.Index();

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<IEnumerable<Product>>(viewResult.ViewData.Model);
        Assert.Equal(2, model.Count());
    }

    private IEnumerable<Product> GetTestProducts()
    {
        return new List<Product>
        {
            new Product { ID = 1, Name = "Coke", Price = 3.99m, QuantityAvailable = 10 },
            new Product { ID = 2, Name = "Pepsi", Price = 6.88m, QuantityAvailable = 20 }
        };
    }
}

internal interface IRepository<T>
{
}