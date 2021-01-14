using System;
using Grpc.Net.Client;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GrpcUtils
{
    public sealed class GrpcCaller<TService>
    {
        private readonly Dictionary<int, GrpcChannel> _channels;
        private readonly GrpcChannel _channel;
        private readonly int _count;

        public GrpcCaller(string uri)
        {
            _channels = new Dictionary<int, GrpcChannel>();
            _channel = GrpcChannel.ForAddress(uri);
        }

        public GrpcCaller(string uri, int count)
        {
            if (count < 2)
                throw new ArgumentException("Count must be more than 1", nameof(count));

            _channels = new Dictionary<int, GrpcChannel>();
            
            for (var i = 0; i < count; i++)
            {
                _channels.Add(i, GrpcChannel.ForAddress(uri));
            }
        }

        public Task<T> Call<T>(Func<GrpcChannel, Task<T>> func)
        {
            if (!(_channel is null))
                return func(_channel);
            
            var random = new Random();
            var key = random.Next(0, _count - 1);
            return func(_channels[key]);
        }

        public T Call<T>(Func<GrpcChannel, T> func)
        {
            if (!(_channel is null))
                return func(_channel);

            var random = new Random();
            var key = random.Next(0, _count - 1);
            return func(_channels[key]);
        }
    }
}
