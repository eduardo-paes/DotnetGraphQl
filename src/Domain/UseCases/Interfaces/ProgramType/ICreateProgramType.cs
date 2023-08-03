using Domain.UseCases.Ports.ProgramType;

namespace Domain.UseCases.Interfaces.ProgramType
{
    public interface ICreateProgramType
    {
        Task<DetailedReadProgramTypeOutput> ExecuteAsync(CreateProgramTypeInput model);
    }
}