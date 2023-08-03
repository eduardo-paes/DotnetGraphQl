using Domain.UseCases.Ports.AssistanceType;

namespace Domain.UseCases.Interfaces.AssistanceType
{
    public interface IDeleteAssistanceType
    {
        Task<DetailedReadAssistanceTypeOutput> ExecuteAsync(Guid? id);
    }
}