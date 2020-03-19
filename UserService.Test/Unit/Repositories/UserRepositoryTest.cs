using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Common.Data.MongoDB.Models;
using MongoDB.Driver;
using Moq;
using NUnit.Framework;
using UserService.Core.Data;
using UserService.Core.Models;
using UserService.Data;
using UserService.Data.Repositories;

namespace UserService.Test.Unit.Repositories
{
    public class UserRepositoryTest
    {
        private IDatabaseSettings _settings;
        private Mock<UserContext> _mockContext;
        private Mock<IMongoCollection<User>> _mockCollection;
        private List<User> _userList;
        private IUserRepository _repository;

        [SetUp]
        public void SetUp()
        {
            _settings = new DatabaseSettings
            {
                DatabaseName = "UserDd",
                CollectionName = "Users",
                ConnectionString = "mongodb://localhost:27017"
            };
            _userList = new List<User>
            {
                new User()
                {
                    Id = "5e7398bde6ab1940182c5cfd",
                    Login = "GJOleg",
                    Password = "1234",
                    Name = "Oleg",
                    Role = "User"
                },
                new User()
                {
                    Id = "5e7398bde6ab1940182c5cfe",
                    Login = "Admin",
                    Password = "1234",
                    Name = "Admin",
                    Role = "Admin"
                }
            };
            _mockContext = new Mock<UserContext>();
            _mockCollection = new Mock<IMongoCollection<User>>();
        }

        [Test]
        public async Task Get_OlegKirill()
        {
            //Arrange
            Mock<IAsyncCursor<User>> userCursorMock = new Mock<IAsyncCursor<User>>();
            userCursorMock.Setup(uc => uc.Current).Returns(_userList);
            userCursorMock
                .SetupSequence(_ => _.MoveNext(It.IsAny<CancellationToken>()))
                .Returns(true)
                .Returns(false);

            _mockCollection.Setup(c =>  c.FindSync(It.IsAny<FilterDefinition<User>>(),
                It.IsAny<FindOptions<User>>(),
                It.IsAny<CancellationToken>())).Returns(userCursorMock.Object);

            _mockContext.Setup(c => c.GetCollection(_settings.CollectionName)).Returns(_mockCollection.Object);

            _repository = new UserRepository(_mockContext.Object);
            
            //Act
            var users = _repository.Get();

            //Assert
            Assert.NotNull(users);
            Assert.AreEqual(_userList, users);
        }

    }
}