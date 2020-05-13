using Common.Core.DataExchange.Messages;

namespace Common.Contracts.UserService.Commands
{
    public class AddUser : ICommand
    {
        public string Name { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
    }
}
