using System.Collections.Generic;
using System.Linq;
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
                    Id = "3e7398bde6ab1940182c5cfe",
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

        [Test]
        public void Add_newuser_newuser()
        {
            //Arrange
            var newUser = new User()
            {
                Login = "newUser",
                Name = "newUSer",
                Password = "123",
                Role = "User"
            };
            _mockCollection.Setup(c => c.InsertOne(newUser, null, default(CancellationToken)));
            _mockContext.Setup(c => c.GetCollection(It.IsAny<string>())).Returns(_mockCollection.Object);
            _repository = new UserRepository(_mockContext.Object);


            //Act
            var user = _repository.Add(newUser);

            //Assert
            Assert.NotNull(user);
            Assert.AreEqual("newUSer", user.Name);
            Assert.NotNull(user.Id);
        }

        [Test]
        public void Update_Oleg_Sanya()
        {
            //Arrange
            _mockCollection.Setup(c => 
                c.ReplaceOne(It.IsAny<FilterDefinition<User>>(), _userList.First(), It.IsAny<ReplaceOptions>(), It.IsAny<CancellationToken>()));
            _mockContext.Setup(c => c.GetCollection(It.IsAny<string>())).Returns(_mockCollection.Object);
            _repository = new UserRepository(_mockContext.Object);
            var user = _userList.First();
            user.Name = "Sanya";

            //Act
            _repository.Update(user);

            //Assert
            _mockCollection.Verify(c => 
                c.ReplaceOne(It.IsAny<FilterDefinition<User>>(), user, It.IsAny<ReplaceOptions>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public void Delete_Oleg()
        {
            //Arrange
            _mockCollection.Setup(c => 
                c.DeleteOne(It.IsAny<FilterDefinition<User>>(), It.IsAny<CancellationToken>()));
            _mockContext.Setup(c => c.GetCollection(It.IsAny<string>())).Returns(_mockCollection.Object);
            _repository = new UserRepository(_mockContext.Object);
            var user = _userList.First();

            //Act
            _repository.Delete(user);

            //Assert
            _mockCollection.Verify(c => 
                c.DeleteOne(It.IsAny<FilterDefinition<User>>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public void Delete_id5e7398bde6ab1940182c5cfd()
        {
            //Arrange
            var user = _userList.First();
            _mockCollection.Setup(c =>
                c.DeleteOne(It.IsAny<FilterDefinition<User>>(), It.IsAny<CancellationToken>()));
            _mockContext.Setup(c => c.GetCollection(It.IsAny<string>())).Returns(_mockCollection.Object);
            _repository = new UserRepository(_mockContext.Object);
            

            //Act
            _repository.Delete(user.Id);

            //Assert
            _mockCollection.Verify(c =>
                c.DeleteOne(It.IsAny<FilterDefinition<User>>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}