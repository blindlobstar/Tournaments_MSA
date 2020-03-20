using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Common.Data.MongoDB.Models;
using MongoDB.Bson;
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
        private Mock<IAsyncCursor<User>> _userCursorMock;

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
            _mockContext = new Mock<UserContext>(_settings);
            _mockCollection = new Mock<IMongoCollection<User>>();

            _userCursorMock = new Mock<IAsyncCursor<User>>();
            _userCursorMock.Setup(uc => uc.Current).Returns(_userList);
            _userCursorMock
                .SetupSequence(_ => _.MoveNext(It.IsAny<CancellationToken>()))
                .Returns(true)
                .Returns(false);

            _userCursorMock
                .SetupSequence(_ => _.MoveNextAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(true))
                .Returns(Task.FromResult(false));


            _mockCollection.Setup(c => c.FindSync(It.IsAny<FilterDefinition<User>>(),
                It.IsAny<FindOptions<User, User>>(),
                It.IsAny<CancellationToken>())).Returns(_userCursorMock.Object);

            _mockContext.Setup(c => c.GetCollection(It.IsAny<string>())).Returns(_mockCollection.Object);
        }

        [Test]
        public async Task Get_OlegAdmin()
        {
            //Arrange
            _repository = new UserRepository(_mockContext.Object);
            
            //Act
            var users = await _repository.Get();

            //Assert
            Assert.NotNull(users);
            Assert.AreEqual(_userList, users);
        }

        [Test]
        public async Task Get_id5e7398bde6ab1940182c5cfd_Oleg()
        {
            //Arrange
            _repository = new UserRepository(_mockContext.Object);

            //Act
            var user = await _repository.Get("5e7398bde6ab1940182c5cfd");

            //Assert
            Assert.NotNull(user);
            Assert.AreEqual("Oleg", user.Name);
            Assert.AreEqual(_userList.First(), user);
        }

    }
}