using Common.Contracts.UserService.Commands;
using Common.Core.DataExchange.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserService.Core.Data;

namespace UserService.API.Handlers
{
    public class UpdateUserHandler : ICommandHandler<UpdateUser>
    {
        private readonly IUserRepository _userRepository;

        public UpdateUserHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task HandleAsync(UpdateUser command)
        {
            var user = await _userRepository.Get(command.Id);
            if(user == null)
            {
                return;
            }

            user.LastName = string.IsNullOrWhiteSpace(command.LastName) ? user.LastName : command.LastName;
            user.MiddleName = string.IsNullOrWhiteSpace(command.MiddleName) ? user.MiddleName : command.MiddleName;
            user.Phone = string.IsNullOrWhiteSpace(command.Phone) ? user.Phone : command.Phone;
            user.Email = string.IsNullOrWhiteSpace(command.Email) ? user.Email : command.Email;
            user.FirstName = string.IsNullOrWhiteSpace(command.FirstName) ? user.FirstName : command.FirstName;

            _userRepository.Update(user);
        }
    }
}
