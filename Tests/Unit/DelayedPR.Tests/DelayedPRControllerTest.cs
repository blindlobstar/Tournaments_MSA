using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using NUnit.Framework;
using System;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Common.DelayedPR.Client;
using Moq;
using StackExchange.Redis;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using Common.DelayedPR.Server;
using Microsoft.Extensions.Logging;
using System.Threading;

namespace DelayedPR.Tests
{
    public class DelayedPRControllerTest
    {
        private class MethodParameter
        {
            public string TypeName { get; set; }
            public string Value { get; set; }
        }

        private class CachedJob
        {
            public string AssemblyName { get; set; }
            public string ClassName { get; set; }
            public string MethodName { get; set; }
            public DateTime RunAt { get; set; }
            public MethodParameter[] MethodParameters { get; set; }
        }

        private class AnotherTestClass
        {
            public async Task AnotherTestMethod(string a)
            {
                a = "b";
                await Task.Yield();
            }
        }

        private class TestClass
        {
            private readonly AnotherTestClass _anotherTestClass;

            public TestClass()
            {
                
            }
            public TestClass(AnotherTestClass anotherTestClass)
            {
                _anotherTestClass = anotherTestClass;
            }

            public async Task TestMethod(string a, int b)
            {
                await Task.Yield();
            }
        }

        public static async IAsyncEnumerable<RedisKey> GetTestValues()
        {
            yield return new RedisKey("1");
            yield return new RedisKey("2");

            await Task.CompletedTask; // to make the compiler warning go away
        }


        private TestClass _testClassObject;

        [SetUp]
        public void SetUp()
        {
            _testClassObject = new TestClass(new AnotherTestClass());
        }

        [Test]
        public async Task ScheduleTest_TestMethod_EqualTestMethod()
        {
            //Arrange
            var options = Options.Create(new MemoryDistributedCacheOptions());
            var cacheRepository = new MemoryDistributedCache(options);
            var delayedPRController = new DelayedPRController(cacheRepository);

            //Act
            var key = await delayedPRController.Shedule(() => _testClassObject.TestMethod("test", 1), DateTime.UtcNow.AddSeconds(5));
            var data = cacheRepository.GetString(key);

            var cachedObject = JsonConvert.DeserializeObject<CachedJob>(data);

            //Assert
            Assert.IsFalse(string.IsNullOrEmpty(key));
            Assert.AreEqual(typeof(TestClass).Assembly.GetName().FullName, cachedObject.AssemblyName);
            Assert.AreEqual(_testClassObject.GetType().FullName, cachedObject.ClassName);
            Assert.AreEqual("TestMethod", cachedObject.MethodName);
            Assert.AreEqual(typeof(string).FullName, cachedObject.MethodParameters[0].TypeName);
            Assert.AreEqual("test", cachedObject.MethodParameters[0].Value);
            Assert.AreEqual(typeof(int).FullName, cachedObject.MethodParameters[1].TypeName);
            Assert.AreEqual(1, int.Parse(cachedObject.MethodParameters[1].Value));
        }

        [Test]
        public async Task RedisServerTest()
        {
            //Arrange
            var options = Options.Create(new MemoryDistributedCacheOptions());
            var cacheRepository = new MemoryDistributedCache(options);
            var serverMock = new Mock<IServer>();
            serverMock.Setup(x => x.KeysAsync(It.IsAny<int>(),
                It.IsAny<RedisValue>(),
                It.IsAny<int>(),
                It.IsAny<long>(),
                It.IsAny<int>(),
                It.IsAny<CommandFlags>())).Returns(GetTestValues);
            var loggerMock = new Mock<ILogger<RedisServer>>();
            var redisServer = new RedisServer(serverMock.Object, cacheRepository, loggerMock.Object);

            Expression<Func<Task>> expression = () => _testClassObject.TestMethod("test", 1);
            var body = expression.Body as MethodCallExpression;
            var argType = body.Arguments[0].GetType();

            var parameters = from parameter in body.Arguments
                             let exp = parameter as MemberExpression
                             let valueArgument = parameter as ConstantExpression
                             select new MethodParameter()
                             {
                                 TypeName = parameter.Type.FullName,
                                 Value = valueArgument.Value.ToString()
                             };

            var cachedObject = new CachedJob()
            {
                AssemblyName = body.Object.Type.Assembly.GetName().FullName,
                ClassName = body.Object.Type.FullName,
                MethodName = body.Method.Name,
                MethodParameters = parameters.ToArray(),
                RunAt = ((DateTimeOffset)DateTime.UtcNow.AddSeconds(2)).UtcDateTime
            };
            var jsonData = JsonConvert.SerializeObject(cachedObject);
            await cacheRepository.SetStringAsync("1", jsonData);

            var cancellationToken = new CancellationToken();
            var src = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            //Act
            await redisServer.StartAsync(cancellationToken);

            for(int i = 0; i < 3; i++)
            {
                await Task.Delay(TimeSpan.FromSeconds(1));
            }

            src.Cancel();
        }
    }
}