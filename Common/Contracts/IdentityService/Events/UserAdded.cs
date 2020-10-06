using Common.Core.DataExchange.Messages;

namespace Common.Contracts.IdentityService.Events
{
    public class UserAdded : IEvent
    {
        public string Id { get; set; }
        public string Login { get; set; }
    }
}
