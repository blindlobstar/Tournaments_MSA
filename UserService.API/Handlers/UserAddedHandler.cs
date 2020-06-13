using Common.Contracts.IdentityService.Events;
using Common.Core.DataExchange.Handlers;
using System.Threading.Tasks;
using UserService.Core.Data;
using UserService.Core.Models;

namespace UserService.API.Handlers
{
    public class UserAddedHandler : IEventHandler<UserAdded>
    {
        private readonly IUserRepository _userRepository;

        public UserAddedHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public Task HandleAsync(UserAdded @event)
        {
            var user = new User()
            {
                Id = @event.Id,
                Login = @event.Login
            };
            return Task.Run(() => _userRepository.Add(user));
        }
    }
}
