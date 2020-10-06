using System;
using Common.Data.MongoDB.Models;
using NUnit.Framework;
using UserService.Data;

namespace UserService.Test.Unit.Contexts
{
    public class UserContextTest
    {
        private IDatabaseSettings _settings;


        [SetUp]
        public void SetUp()
        {
            _settings = new DatabaseSettings()
            {
                DatabaseName = "UserDd",
                CollectionName = "Users",
                ConnectionString = "mongodb://localhost:27017"
            };
        }

        [Test]
        public void Constructor_notnull()
        {
            //Act
            var context = new UserContext(_settings);

            //Assert
            Assert.NotNull(context);
        }

        [Test]
        public void GetCollection_User_notnull()
        {
            //Act
            var context = new UserContext(_settings);
            var userCollection = context.GetCollection("User");

            //Assert
            Assert.NotNull(userCollection);
        }

        [Test]
        public void GetCollection_hz()
        {
            //Act
            var context = new UserContext(_settings);
            //Assert
            Assert.Throws(typeof(ArgumentException), () => context.GetCollection(""));
        }
    }
}