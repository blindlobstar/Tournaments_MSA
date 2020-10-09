using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Common.DelayedPR.Client.Abstraction
{
    public interface IDelayedPRController
    {
        Task<string> Shedule(Expression<Func<Task>> expression, DateTimeOffset runAt);
        Task<string> Shedule(Expression<Action> expression, DateTimeOffset runAt);
    }
}