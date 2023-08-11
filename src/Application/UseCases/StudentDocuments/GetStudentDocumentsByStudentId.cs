using AutoMapper;
using Domain.Interfaces.Repositories;
using Application.Interfaces.UseCases.StudentDocuments;
using Application.Ports.StudentDocuments;
using Application.Validation;

namespace Application.UseCases.StudentDocuments
{
    public class GetStudentDocumentsByStudentId : IGetStudentDocumentsByStudentId
    {
        #region Global Scope
        private readonly IStudentDocumentsRepository _repository;
        private readonly IMapper _mapper;
        public GetStudentDocumentsByStudentId(
            IStudentDocumentsRepository repository,
            IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        #endregion Global Scope

        public async Task<ResumedReadStudentDocumentsOutput> ExecuteAsync(Guid? studentId)
        {
            // Verifica se o id foi informado
            UseCaseException.NotInformedParam(studentId is null, nameof(studentId));

            // Busca documentos do estudante pelo id do projeto
            Domain.Entities.StudentDocuments? entity = await _repository.GetByStudentIdAsync(studentId);

            // Retorna entidade mapeada
            return _mapper.Map<ResumedReadStudentDocumentsOutput>(entity);
        }
    }
}