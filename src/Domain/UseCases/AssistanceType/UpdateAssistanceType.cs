using Domain.Contracts.AssistanceType;
using Domain.Interfaces.UseCases;
using AutoMapper;
using Domain.Interfaces.Repositories;
using Domain.Validation;

namespace Domain.UseCases
{
    public class UpdateAssistanceType : IUpdateAssistanceType
    {
        #region Global Scope
        private readonly IAssistanceTypeRepository _repository;
        private readonly IMapper _mapper;
        public UpdateAssistanceType(IAssistanceTypeRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        #endregion

        public async Task<DetailedReadAssistanceTypeOutput> Execute(Guid? id, UpdateAssistanceTypeInput input)
        {
            // Verifica se o id foi informado
            UseCaseException.NotInformedParam(id is null, nameof(id));

            // Verifica se nome foi informado
            UseCaseException.NotInformedParam(string.IsNullOrEmpty(input.Name), nameof(input.Name));

            // Recupera entidade que será atualizada
            var entity = await _repository.GetById(id)
                ?? throw UseCaseException.NotFoundEntityById(nameof(Entities.AssistanceType));

            // Verifica se a entidade foi excluída
            UseCaseException.BusinessRuleViolation(entity.DeletedAt != null,
                "O Bolsa de Assistência informado já foi excluído.");

            // Verifica se o nome já está sendo usado
            UseCaseException.BusinessRuleViolation(
                !string.Equals(entity.Name, input.Name, StringComparison.OrdinalIgnoreCase)
                    && await _repository.GetAssistanceTypeByName(input.Name!) != null,
                "Já existe um Bolsa de Assistência para o nome informado.");

            // Atualiza atributos permitidos
            entity.Name = input.Name;
            entity.Description = input.Description;

            // Salva entidade atualizada no banco
            var model = await _repository.Update(entity);
            return _mapper.Map<DetailedReadAssistanceTypeOutput>(model);
        }
    }
}