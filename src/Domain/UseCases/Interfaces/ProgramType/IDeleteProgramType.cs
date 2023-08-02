using Domain.UseCases.Ports.ProgramType;

namespace Domain.UseCases.Interfaces.ProgramType
{
    public interface IDeleteProgramType
    {
        Task<DetailedReadProgramTypeOutput> Execute(Guid? id);
    }
}