using Moq;
using MyMessenger.Domain.Entities;
using MyMessenger.Domain.Interfaces;
using MyMessenger.Domain.Repositories;

namespace MyMessenger.Domain.Tests
{
    public class UnitOfWorkTests
    {
        private readonly Mock<DatabaseContext> dbContext;
        public UnitOfWorkTests()
        {
            dbContext = new Mock<DatabaseContext>();
        }
        [Fact]
        public void UnitOfWork_CreatesInstance()
        {
            var sut = new UnitOfWork(dbContext.Object);

            Assert.NotNull(sut);
        }

        [Theory]
        [InlineData(typeof(Chat))]
        [InlineData(typeof(Message))]
        public void UnitOfWork_GetRepository_ReturnsRepository(Type type)
        {
            var sut = new UnitOfWork(dbContext.Object);
            var repositoryType = typeof(IGenericRepository<>).MakeGenericType(type);

            var repository = sut.GetType().GetMethod("GetRepository").MakeGenericMethod(type).Invoke(sut, null);

            Assert.NotNull(repository);
            Assert.True(repositoryType.IsAssignableFrom(repository.GetType()));
        }
            
        [Fact]
        public async Task UnitOfWork_SaveAsync_CallsSaveChangesAsync()
        {
            var sut = new UnitOfWork(dbContext.Object);

            await sut.SaveAsync();

            dbContext.Verify(c => c.SaveChangesAsync(default), Times.Once());
        }

        [Fact]
        public void UnitOfWork_Dispose_DisposesContext()
        {
            var sut = new UnitOfWork(dbContext.Object);

            sut.Dispose();

            dbContext.Verify(c => c.Dispose(), Times.Once());
        }
    }
}