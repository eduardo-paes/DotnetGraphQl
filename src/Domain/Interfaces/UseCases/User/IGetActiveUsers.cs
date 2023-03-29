using System.Linq;
using System.Threading.Tasks;
using Domain.Contracts.User;

namespace Domain.Interfaces.UseCases.User
{
    public interface IGetActiveUsers
    {
        Task<IQueryable<UserReadOutput>> Execute(int skip, int take);
    }
}