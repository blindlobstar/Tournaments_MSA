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
        public async Task HandleAsync(UserAdded @event)
        {
            var user = new UserDto()
            {
                Id = @event.Id,
                Login = @event.Login
            };
            await _userRepository.Add(user);
        }
    }
}
