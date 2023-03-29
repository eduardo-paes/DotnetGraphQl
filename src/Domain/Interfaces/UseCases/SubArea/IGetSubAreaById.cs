using System;
using System.Threading.Tasks;
using Domain.Contracts.SubArea;

namespace Domain.Interfaces.UseCases.SubArea
{
    public interface IGetSubAreaById
    {
        Task<DetailedReadSubAreaOutput> Execute(Guid? id);
    }
}