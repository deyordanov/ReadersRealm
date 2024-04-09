namespace ReadersRealm.Services.Tests.OrderDetailsTests;

using System.Linq.Expressions;
using Data.Models.OrderDetails;
using Data.OrderDetailsServices;
using Data.OrderDetailsServices.Contracts;
using Moq;
using ReadersRealm.Data.Models;
using ReadersRealm.Data.Repositories.Contracts;
using Web.ViewModels.OrderDetails;

public class OrderDetailsRetrievalTests
{
    private Mock<IUnitOfWork> _mockUnitOfWork;

    [SetUp]
    public void Setup()
    {
        this._mockUnitOfWork = new Mock<IUnitOfWork>();
    }

    [Test]
    public async Task GetAllByOrderHeaderIdAsync_ShouldReturnOrderDetails()
    {
        // Arrange
        IOrderDetailsRetrievalService service 
            = new OrderDetailsRetrievalService(this._mockUnitOfWork.Object);

        Guid orderHeaderId = Guid.NewGuid();
        List<OrderDetails> allOrderDetailsList = new List<OrderDetails>()
        {
            new OrderDetails()
            {
                Id = Guid.NewGuid(),
                Book = new Book()
                {
                    Id = Guid.NewGuid(),
                    Title = "Title1",
                    ISBN = "ISBN1",
                },
                OrderHeaderId = orderHeaderId,
                Count = 1,
                Price = 1,
                Order = new Order()
                {
                    Id = Guid.NewGuid(),
                },
            },
            new OrderDetails()
            {
                Id = Guid.NewGuid(),
                Book = new Book()
                {
                    Id = Guid.NewGuid(),
                    Title = "Title2",
                    ISBN = "ISBN2",
                },
                OrderHeaderId = orderHeaderId,
                Count = 1,
                Price = 1,
                Order = new Order()
                {
                    Id = Guid.NewGuid(),
                },
            },
            new OrderDetails()
            {
                Id = Guid.NewGuid(),
                Book = new Book()
                {
                    Id = Guid.NewGuid(),
                    Title = "Title3",
                    ISBN = "ISBN3",
                },
                OrderHeaderId = orderHeaderId,
                Count = 1,
                Price = 1,
                Order = new Order()
                {
                    Id = Guid.NewGuid(),
                },
            }
        };

        this._mockUnitOfWork.Setup(uow => uow
                .OrderDetailsRepository
                .GetAsync(It.IsAny<Expression<Func<OrderDetails, bool>>>(),
                    It.IsAny<Func<IQueryable<OrderDetails>,
                        IOrderedQueryable<OrderDetails>>>(),
                    It.IsAny<string>()))
            .ReturnsAsync(allOrderDetailsList);

        // Act
        IEnumerable<OrderDetailsViewModel> allOrderDetails
            = await service.GetAllByOrderHeaderIdAsync(Guid.NewGuid());

        // Assert
        Assert.IsNotNull(allOrderDetails);
        Assert.That(allOrderDetails.Count(), Is.EqualTo(allOrderDetailsList.Count));

        foreach (OrderDetailsViewModel orderDetails in allOrderDetails)
        {
            OrderDetails orderDetailsFromList = allOrderDetailsList
                .FirstOrDefault(od => od.Id == orderDetails.Id)!;

            Assert.IsNotNull(orderDetailsFromList);
            Assert.That(orderDetails.Id, Is.EqualTo(orderDetailsFromList.Id));
            Assert.That(orderDetails.OrderHeaderId, Is.EqualTo(orderDetailsFromList.OrderHeaderId));
            Assert.That(orderDetails.BookId, Is.EqualTo(orderDetailsFromList.BookId));
            Assert.That(orderDetails.Count, Is.EqualTo(orderDetailsFromList.Count));
            Assert.That(orderDetails.Price, Is.EqualTo(orderDetailsFromList.Price));
        }
    }

    [Test]
    public async Task GetAllOrderDetailsForReceiptAsDtosAsync_ShouldReturnCorrectOrderDetails()
    {
        // Arrange
        IOrderDetailsRetrievalService service 
            = new OrderDetailsRetrievalService(this._mockUnitOfWork.Object);

        Guid orderHeaderId = Guid.NewGuid();
        List<OrderDetails> allOrderDetailsList = new List<OrderDetails>()
        {
            new OrderDetails()
            {
                Id = Guid.NewGuid(),
                Book = new Book()
                {
                    Id = Guid.NewGuid(),
                    Title = "Title1",
                    ISBN = "ISBN1",
                },
                OrderHeaderId = orderHeaderId,
                Count = 1,
                Price = 1,
                Order = new Order()
                {
                    Id = Guid.NewGuid(),
                },
            },
            new OrderDetails()
            {
                Id = Guid.NewGuid(),
                Book = new Book()
                {
                    Id = Guid.NewGuid(),
                    Title = "Title2",
                    ISBN = "ISBN2",
                },
                OrderHeaderId = orderHeaderId,
                Count = 1,
                Price = 1,
                Order = new Order()
                {
                    Id = Guid.NewGuid(),
                },
            },
            new OrderDetails()
            {
                Id = Guid.NewGuid(),
                Book = new Book()
                {
                    Id = Guid.NewGuid(),
                    Title = "Title3",
                    ISBN = "ISBN3",
                },
                OrderHeaderId = orderHeaderId,
                Count = 1,
                Price = 1,
                Order = new Order()
                {
                    Id = Guid.NewGuid(),
                },
            }
        };

        this._mockUnitOfWork.Setup(uow => uow
                .OrderDetailsRepository
                .GetAsync(It.IsAny<Expression<Func<OrderDetails, bool>>>(),
                    It.IsAny<Func<IQueryable<OrderDetails>,
                        IOrderedQueryable<OrderDetails>>>(),
                    It.IsAny<string>()))
            .ReturnsAsync(allOrderDetailsList);

        // Act
        IEnumerable<OrderDetailsReceiptDto> allOrderDetails
            = await service.GetAllOrderDetailsForReceiptAsDtosAsync(Guid.NewGuid());

        // Assert
        Assert.IsNotNull(allOrderDetails);
        Assert.That(allOrderDetails.Count(), Is.EqualTo(allOrderDetailsList.Count));

        foreach (OrderDetailsReceiptDto orderDetails in allOrderDetails)
        {
            OrderDetails orderDetailsFromList = allOrderDetailsList
                .FirstOrDefault(od => od.Id == orderDetails.Id)!;

            Assert.IsNotNull(orderDetailsFromList);
            Assert.That(orderDetails.Id, Is.EqualTo(orderDetailsFromList.Id));
            Assert.That(orderDetails.OrderHeaderId, Is.EqualTo(orderDetailsFromList.OrderHeaderId));
            Assert.That(orderDetails.BookId, Is.EqualTo(orderDetailsFromList.BookId));
            Assert.That(orderDetails.Count, Is.EqualTo(orderDetailsFromList.Count));
            Assert.That(orderDetails.Price, Is.EqualTo(orderDetailsFromList.Price));
        }
    }
}