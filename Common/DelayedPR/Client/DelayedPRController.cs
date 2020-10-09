using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Common.DelayedPR.Client.Abstraction;
using Common.DelayedPR.Dto;
using Common.DistributedCache.CustomExtensions;
using Microsoft.Extensions.Caching.Distributed;

namespace Common.DelayedPR.Client
{
    public class DelayedPRController : IDelayedPRController
    {
        private readonly IDistributedCache _cacheRepository;

        public DelayedPRController(IDistributedCache cacheRepository)
        {
            _cacheRepository = cacheRepository;
        }

        public async Task<string> Shedule(Expression<Func<Task>> expression, DateTimeOffset runAt)
        {
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
                RunAt = runAt.UtcDateTime
            };

            var key = await _cacheRepository.SetEntity(cachedObject);

            return key;
        }

        public async Task<string> Shedule(Expression<Action> expression, DateTimeOffset runAt)
        {
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
                RunAt = runAt.UtcDateTime
            };

            var key = await _cacheRepository.SetEntity(cachedObject);

            return key;
        }
    }
}