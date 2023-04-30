using Domain.Contracts.SubArea;

namespace Domain.Interfaces.UseCases
{
    public interface IDeleteSubArea
    {
        Task<DetailedReadSubAreaOutput> Execute(Guid? id);
    }
}