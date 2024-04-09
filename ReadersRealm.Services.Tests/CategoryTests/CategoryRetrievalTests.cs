namespace ReadersRealm.Services.Tests.CategoryTests;

using System.Linq.Expressions;
using AngleSharp.Css.Dom;
using Common;
using Common.Exceptions.Category;
using Data.AuthorServices.Contracts;
using Data.CategoryServices;
using Data.CategoryServices.Contracts;
using Data.CategoryServices.Contracts;
using Moq;
using ReadersRealm.Data.Models;
using ReadersRealm.Data.Repositories.Contracts;
using ReadersRealm.Web.ViewModels.Category;
using Web.ViewModels.Author;
using Web.ViewModels.Category;

[TestFixture]
public class CategoryRetrievalTests
{
    private Mock<IUnitOfWork>? _mockUnitOfWork;
    private Mock<IAuthorRetrievalService>? _mockAuthorRetrievalService;
    private Mock<ICategoryRetrievalService>? _mockCategoryRetrievalService;

    private Category? _existingCategory;
    private List<Category>? _allCategories;

    [SetUp]
    public void SetUp()
    {
        this._mockUnitOfWork = new Mock<IUnitOfWork>();

        this._existingCategory = new Category()
        {
            Id = 0,
            Name = "Name",
        };

        this._allCategories = new List<Category>()
        {
            new Category()
            {
                Id = 1,
                Name = "Name1",
            },
            new Category()
            {
                Id = 2,
                Name = "Name2",
            },
            new Category()
            {
                Id = 3,
                Name = "Name3",
            },
        };

        this._mockUnitOfWork.Setup(uow => uow
            .CategoryRepository
            .GetAsync(It.IsAny<Expression<Func<Category, bool>>>(),
                It.IsAny<Func<IQueryable<Category>,
                    IOrderedQueryable<Category>>>(),
                It.IsAny<string>()))
            .ReturnsAsync(this._allCategories);

        this._mockUnitOfWork.Setup(uow => uow
                .CategoryRepository
                .GetByIdAsync(this._existingCategory!.Id))
            .ReturnsAsync(this._existingCategory);
    }

    [Test]
    public async Task GetAllAsync_ShouldReturnAllCategories()
    {
        //Arrange
        ICategoryRetrievalService service
            = new CategoryRetrievalService(this._mockUnitOfWork!.Object);

        int categoriesCount = this._allCategories!.Count;
        //Act
        IEnumerable<AllCategoriesViewModel>? categories =
            await service.GetAllAsync();

        //Assert
        foreach (AllCategoriesViewModel category in categories)
        {
            Assert.IsTrue(this._allCategories!
                .Any(c => c.Name == category.Name));
        }
    }

    [Test]
    public async Task GetAllListAsync_ShouldReturnAllCategories()
    {
        //Arrange
        ICategoryRetrievalService service
            = new CategoryRetrievalService(this._mockUnitOfWork!.Object);

        int categoriesCount = this._allCategories!.Count;
        //Act
        IEnumerable<AllCategoriesListViewModel>? categories =
            await service.GetAllListAsync();

        //Assert
        foreach (AllCategoriesListViewModel category in categories)
        {
            Assert.IsTrue(this._allCategories!
                .Any(c => c.Name == category.Name));
        }
    }

    [Test]
    public async Task GetCategoryForEditAsync_ShouldReturnCorrectCategory()
    {
        //Arrange
        ICategoryRetrievalService service
            = new CategoryRetrievalService(this._mockUnitOfWork!.Object);

        //Act
        EditCategoryViewModel categoryModel
            = await service.GetCategoryForEditAsync(this._existingCategory!.Id);

        //Assert
        Assert.That(categoryModel.Id, Is.EqualTo(this._existingCategory!.Id));
        Assert.That(categoryModel.Name, Is.EqualTo(this._existingCategory!.Name));
    }

    [Test]
    public void GetCategoryForEditAsync_ShouldThrowCategoryNotFoundException()
    {
        //Arrange
        ICategoryRetrievalService service
            = new CategoryRetrievalService(this._mockUnitOfWork!.Object);

        //Act & Assert
        Assert.ThrowsAsync<CategoryNotFoundException>(async () =>
            await service.GetCategoryForEditAsync(1));
    }

    [Test]
    public async Task GetCategoryForDeleteAsync_ShouldReturnCorrectCategory()
    {
        //Arrange
        ICategoryRetrievalService service
            = new CategoryRetrievalService(this._mockUnitOfWork!.Object);

        //Act
        DeleteCategoryViewModel categoryModel =
            await service.GetCategoryForDeleteAsync(this._existingCategory!.Id);

        //Assert
        Assert.That(categoryModel.Id, Is.EqualTo(this._existingCategory!.Id));
        Assert.That(categoryModel.Name, Is.EqualTo(this._existingCategory!.Name));
    }

    [Test]
    public void GetCategoryForDeleteAsync_ShouldTrowCategoryNotFoundException()
    {
        //Arrange
        ICategoryRetrievalService service
            = new CategoryRetrievalService(this._mockUnitOfWork!.Object);

        //Act & Assert
        Assert.ThrowsAsync<CategoryNotFoundException>(async () =>
            await service.GetCategoryForDeleteAsync(1));
    }

    [Test]
    public void GetCategoryForCreateAsync_ShouldReturnCorrectCategory()
    {
        //Arrange
        ICategoryRetrievalService service
            = new CategoryRetrievalService(this._mockUnitOfWork!.Object);

        //Act
        CreateCategoryViewModel categoryModel =
            service.GetCategoryForCreate();

        //Assert
        Assert.That(categoryModel.Name, Is.EqualTo(string.Empty));
    }

    [Test]
    public async Task CategoryExistsAsync_ShouldReturnTrue()
    {
        //Arrange
        ICategoryRetrievalService service
            = new CategoryRetrievalService(this._mockUnitOfWork!.Object);

        int categoryId = 1;
        this._mockUnitOfWork!.Setup(uow => uow
                .CategoryRepository
                .GetFirstOrDefaultByFilterAsync(It.IsAny<Expression<Func<Category, bool>>>(),
                    It.IsAny<bool>()))
            .ReturnsAsync(this._allCategories!
                .FirstOrDefault(c => c.Id == categoryId));

        //Act & Assert
        Assert.IsTrue(await service.CategoryExistsAsync(categoryId));
    }

    [Test]
    public async Task CategoryExistsAsync_ShouldReturnFalse()
    {
        //Arrange
        ICategoryRetrievalService service
            = new CategoryRetrievalService(this._mockUnitOfWork!.Object);

        int categoryId = 999;
        this._mockUnitOfWork!.Setup(uow => uow
                .CategoryRepository
                .GetFirstOrDefaultByFilterAsync(It.IsAny<Expression<Func<Category, bool>>>(),
                    It.IsAny<bool>()))
            .ReturnsAsync(this._allCategories!
                .FirstOrDefault(c => c.Id == categoryId));

        //Act & Assert
        Assert.IsFalse(await service.CategoryExistsAsync(categoryId));
    }
}