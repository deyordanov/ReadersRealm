namespace ReadersRealm.Services.Tests.CategoryTests;

using Common.Exceptions.Category;
using Data.CategoryServices;
using Data.CategoryServices.Contracts;
using Moq;
using ReadersRealm.Data.Models;
using ReadersRealm.Data.Repositories.Contracts;
using Web.ViewModels.Author;
using Web.ViewModels.Category;
using Web.ViewModels.Category;

[TestFixture]
public class CategoryCrudTests
{
    private Mock<IUnitOfWork>? _mockUnitOfWork;

    private Category? _existingCategory;

    [SetUp]
    public void SetUp()
    {
        this._mockUnitOfWork = new Mock<IUnitOfWork>();

        this._existingCategory = new Category()
        {
            Id = 0,
            Name = "Name",
        };

        this._mockUnitOfWork.Setup(uow => uow
                .CategoryRepository
                .AddAsync(It.IsAny<Category>()))
            .Returns(Task.CompletedTask);

        this._mockUnitOfWork.Setup(uow => uow
                .CategoryRepository
                .GetByIdAsync(this._existingCategory!.Id))
            .ReturnsAsync(this._existingCategory);
    }

    [Test]
    public async Task CreateCategoryAsync_ShouldCreateCategory()
    {
        //Arrange
        ICategoryCrudService service
            = new CategoryCrudService(this._mockUnitOfWork!.Object);

        CreateCategoryViewModel bookModel = new CreateCategoryViewModel()
        {
            Name = this._existingCategory!.Name,
        };

        //Act
        await service.CreateCategoryAsync(bookModel);

        //Assert
        this._mockUnitOfWork.Verify(uow => uow
            .CategoryRepository
            .AddAsync(It.Is<Category>(b =>
                b.Name == this._existingCategory!.Name)));

        this._mockUnitOfWork.Verify(uow => uow.SaveAsync(), Times.Once());
    }

    [Test]
    public async Task EditCategoryAsync_ShouldUpdateCategoryCorrectly()
    {
        //Arrange
        ICategoryCrudService service
            = new CategoryCrudService(this._mockUnitOfWork!.Object);

        EditCategoryViewModel bookModel = new EditCategoryViewModel()
        {
            Id = this._existingCategory!.Id,
            Name = "UpdatedName",
        };

        //Act
        await service.EditCategoryAsync(bookModel);

        //Assert
        this._mockUnitOfWork.Verify(uow => uow.SaveAsync(), Times.Once());

        Assert.That(bookModel.Id, Is.EqualTo(this._existingCategory!.Id));
        Assert.That(bookModel.Name, Is.EqualTo(this._existingCategory!.Name));
    }

    [Test]
    public void EditCategoryAsync_ShouldThrowCategoryNotFoundException()
    {
        //Arrange
        ICategoryCrudService service
            = new CategoryCrudService(this._mockUnitOfWork!.Object);

        EditCategoryViewModel bookModel = new EditCategoryViewModel()
        {
            Id = 1,
            Name = "UpdatedName",
        };

        //Act & Assert
        Assert.ThrowsAsync<CategoryNotFoundException>(async () =>
        await service.EditCategoryAsync(bookModel));
    }

    [Test]
    public async Task DeleteCategoryAsync_ShouldDeleteCategory()
    {
        //Arrange
        ICategoryCrudService service
            = new CategoryCrudService(this._mockUnitOfWork!.Object);

        DeleteCategoryViewModel bookModel = new DeleteCategoryViewModel()
        {
            Id = this._existingCategory!.Id,
            Name = this._existingCategory!.Name,
        };

        //Act
        await service.DeleteCategoryAsync(bookModel);

        //Assert
        this._mockUnitOfWork.Verify(uow => uow
            .CategoryRepository
            .Delete(It.Is<Category>(b =>
                b.Id == this._existingCategory!.Id &&
                b.Name == this._existingCategory!.Name)));

        this._mockUnitOfWork.Verify(uow => uow.SaveAsync(), Times.Once());
    }

    [Test]
    public void DeleteCategoryAsync_ShouldThrowCategoryNotFoundException()
    {
        //Arrange
        ICategoryCrudService service
            = new CategoryCrudService(this._mockUnitOfWork!.Object);

        DeleteCategoryViewModel bookModel = new DeleteCategoryViewModel()
        {
            Id = 1,
            Name = this._existingCategory!.Name,
        };

        //Act & Assert
        Assert.ThrowsAsync<CategoryNotFoundException>(async () =>
            await service.DeleteCategoryAsync(bookModel));
    }
}