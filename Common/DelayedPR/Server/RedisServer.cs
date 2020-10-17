using Common.DelayedPR.Dto;
using Common.DistributedCache.CustomExtensions;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace Common.DelayedPR.Server
{
    public class RedisServer : HostedService
    {
        private readonly IServer _cacheServer;
        private readonly IDistributedCache _distributedCache;
        private readonly ILogger<RedisServer> _logger;

        public RedisServer(IServer cacheServer, 
            IDistributedCache distributedCache, 
            ILogger<RedisServer> logger)
        {
            _cacheServer = cacheServer;
            _distributedCache = distributedCache;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            while(!cancellationToken.IsCancellationRequested)
            {
                var keys = _cacheServer.KeysAsync();
                
                await foreach(var key in keys)
                {
                    var procedure = await _distributedCache.GetEntity<CachedJob>(key.ToString());

                    if (procedure is null)
                        continue;

                    if (procedure.RunAt > DateTime.UtcNow)
                        continue;

                    _logger.LogInformation($"Start executing {procedure.ClassName}.{procedure.MethodName}");
                    try
                    {
                        var classType = Assembly.Load(procedure.AssemblyName).GetType(procedure.ClassName);
                        var instance = Activator.CreateInstance(classType);
                        var method = classType.GetMethod(procedure.MethodName);

                        var parameters = from param in procedure.MethodParameters
                                         let paramType = Type.GetType(param.TypeName)
                                         select TypeMapper(paramType, param.Value);
                        try
                        {
                            var procedureTask = Task.Run(() => method.Invoke(instance, parameters.ToArray()), cancellationToken);
                        }
                        catch (Exception e)
                        {
                            _logger.LogError($"Error accure when tring to invoke method: {procedure.ClassName}.{procedure.MethodName}", e);
                        }
                        await _distributedCache.RemoveAsync(key);
                    } 
                    catch(MissingMethodException ex)
                    {
                        _logger.LogError($"Error accured while calling method {procedure.ClassName}.{procedure.MethodName}", ex);
                    }
                    catch(ArgumentOutOfRangeException argEx)
                    {
                        _logger.LogError($"Error accured while calling method {procedure.ClassName}.{procedure.MethodName}", argEx);
                    }
                    
                }

                await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);
            }
        }

        private object TypeMapper(Type type, string value) =>
            Type.GetTypeCode(type) switch
            {
                TypeCode.Int32 => int.Parse(value),
                TypeCode.Int64 => long.Parse(value),
                TypeCode.Double => double.Parse(value),
                TypeCode.Int16 => short.Parse(value),
                TypeCode.Boolean => bool.Parse(value),
                TypeCode.DateTime => DateTime.Parse(value),
                TypeCode.String => value,
                _ => throw new ArgumentOutOfRangeException($"Wrong argument type {type.Name}")
            };
    }
}
