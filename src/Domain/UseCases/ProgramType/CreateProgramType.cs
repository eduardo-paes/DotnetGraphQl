using Domain.Contracts.ProgramType;
using Domain.Interfaces.UseCases;
using AutoMapper;
using Domain.Interfaces.Repositories;
using Domain.Validation;

namespace Domain.UseCases
{
    public class CreateProgramType : ICreateProgramType
    {
        #region Global Scope
        private readonly IProgramTypeRepository _repository;
        private readonly IMapper _mapper;
        public CreateProgramType(IProgramTypeRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        #endregion

        public async Task<DetailedReadProgramTypeOutput> Execute(CreateProgramTypeInput input)
        {
            // Verifica se nome foi informado
            UseCaseException.NotInformedParam(string.IsNullOrEmpty(input.Name), nameof(input.Name));

            // Verifica se já existe um tipo de programa com o nome indicado
            var entity = await _repository.GetProgramTypeByName(input.Name!);
            if (entity != null)
                throw new Exception("Já existe um Tipo de Programa para o nome informado.");

            // Cria entidade
            entity = await _repository.Create(_mapper.Map<Entities.ProgramType>(input));

            // Salva entidade no banco
            return _mapper.Map<DetailedReadProgramTypeOutput>(entity);
        }
    }
}