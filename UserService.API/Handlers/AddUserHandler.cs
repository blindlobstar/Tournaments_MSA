using Common.Contracts.UserService.Commands;
using Common.Core.DataExchange.Handlers;
using System.Threading.Tasks;
using UserService.Core.Data;
using UserService.Core.Models;

namespace UserService.API.Handlers
{
    public class AddUserHandler : ICommandHandler<AddUser>
    {
        private readonly IUserRepository _userRepository;
        public AddUserHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public Task HandleAsync(AddUser command)
        {
            var newUser = new User()
            {
                Login = command.Login,
                Name = command.Name,
                Password = command.Password,
                Role = command.Role
            };
            return Task.Run(() => _userRepository.Add(newUser));
        }
    }
}
