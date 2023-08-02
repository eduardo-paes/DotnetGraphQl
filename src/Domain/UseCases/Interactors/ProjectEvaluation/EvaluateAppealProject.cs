using AutoMapper;
using Domain.Entities.Enums;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Domain.UseCases.Interfaces.ProjectEvaluation;
using Domain.UseCases.Ports.Project;
using Domain.UseCases.Ports.ProjectEvaluation;
using Domain.Validation;

namespace Domain.UseCases.Interactors.ProjectEvaluation
{
    public class EvaluateAppealProject : IEvaluateAppealProject
    {
        #region Global Scope
        private readonly IMapper _mapper;
        private readonly IProjectRepository _projectRepository;
        private readonly IEmailService _emailService;
        private readonly ITokenAuthenticationService _tokenAuthenticationService;
        private readonly IProjectEvaluationRepository _projectEvaluationRepository;
        public EvaluateAppealProject(IMapper mapper,
            IProjectRepository projectRepository,
            IEmailService emailService,
            ITokenAuthenticationService tokenAuthenticationService,
            IProjectEvaluationRepository projectEvaluationRepository)
        {
            _mapper = mapper;
            _projectRepository = projectRepository;
            _emailService = emailService;
            _tokenAuthenticationService = tokenAuthenticationService;
            _projectEvaluationRepository = projectEvaluationRepository;
        }
        #endregion Global Scope

        public async Task<DetailedReadProjectOutput> Execute(EvaluateAppealProjectInput input)
        {
            // Obtém informações do usuário logado.
            Ports.Auth.UserClaimsOutput user = _tokenAuthenticationService.GetUserAuthenticatedClaims();

            // Verifica se o usuário logado é um avaliador.
            UseCaseException.BusinessRuleViolation(user.Role != ERole.ADMIN.GetDescription() || user.Role != ERole.PROFESSOR.GetDescription(),
                "O usuário não é um avaliador.");

            // Busca avaliação do projeto pelo Id.
            Entities.ProjectEvaluation? projectEvaluation = await _projectEvaluationRepository.GetByProjectId(input.ProjectId)
                ?? throw UseCaseException.NotFoundEntityById(nameof(Entities.ProjectEvaluation));

            // Recupera projeto pelo Id.
            Entities.Project project = await _projectRepository.GetById(input.ProjectId)
                ?? throw UseCaseException.NotFoundEntityById(nameof(Entities.Project));

            // Verifica se o avaliador é o professor orientador do projeto.
            UseCaseException.BusinessRuleViolation(projectEvaluation.Project?.ProfessorId == user.Id,
                "Avaliador é o orientador do projeto.");

            // Verifica se o projeto está na fase de recurso.
            UseCaseException.BusinessRuleViolation(projectEvaluation.Project?.Status != EProjectStatus.Evaluation,
                "Projeto não está em fase de avaliação.");

            // Verifica se o edital está na fase de recurso.
            UseCaseException.BusinessRuleViolation(projectEvaluation?.Project?.Notice?.AppealStartDate > DateTime.UtcNow || projectEvaluation?.Project?.Notice?.AppealEndDate < DateTime.UtcNow,
                "O edital não está na fase de recurso.");

            // Verifica se o status da avaliação foi informado.
            UseCaseException.NotInformedParam(input.AppealEvaluationStatus is null,
                nameof(input.AppealEvaluationStatus));

            // Verifica se descrição da avaliação foi informada.
            UseCaseException.NotInformedParam(string.IsNullOrEmpty(input.AppealEvaluationDescription),
                nameof(input.AppealEvaluationDescription));

            // Atualiza a avaliação do recurso.
            projectEvaluation!.AppealEvaluatorId = user.Id;
            projectEvaluation.AppealEvaluationDate = DateTime.UtcNow;

            // Atualiza a descrição e o status da avaliação do recurso.
            projectEvaluation.AppealEvaluationDescription = input.AppealEvaluationDescription;
            projectEvaluation.AppealEvaluationStatus = (EProjectStatus)input.AppealEvaluationStatus!;

            // Atualiza avaliação do projeto.
            _ = await _projectEvaluationRepository.Update(projectEvaluation);

            // Se projeto foi aceito, adiciona prazo para envio da documentação.
            if ((EProjectStatus)input.AppealEvaluationStatus == EProjectStatus.Accepted)
            {
                project.Status = EProjectStatus.Accepted;
                project.StatusDescription = EProjectStatus.Accepted.GetDescription();
            }
            else
            {
                project.Status = EProjectStatus.Canceled;
                project.StatusDescription = EProjectStatus.Canceled.GetDescription();
            }

            // Informa ao professor o resultado da avaliação.
            await _emailService.SendProjectNotificationEmail(
                project.Professor!.User!.Email,
                project.Professor!.User!.Name,
                project.Title,
                project.StatusDescription,
                projectEvaluation.SubmissionEvaluationDescription);

            // Atualiza projeto.
            Entities.Project output = await _projectRepository.Update(project);

            // Mapeia dados de saída e retorna.
            return _mapper.Map<DetailedReadProjectOutput>(output);
        }
    }
}