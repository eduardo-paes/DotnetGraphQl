using AutoMapper;
using Domain.Interfaces.Repositories;
using Domain.Ports.Activity;
using Domain.UseCases.Interfaces.ActivityType;
using Domain.UseCases.Ports.Activity;

namespace Domain.UseCases.Interactors.ActivityType
{
    public class GetLastNoticeActivities : IGetLastNoticeActivities
    {
        private readonly IActivityTypeRepository _activityTypeRepository;
        private readonly IMapper _mapper;
        public GetLastNoticeActivities(IActivityTypeRepository activityTypeRepository, IMapper mapper)
        {
            _activityTypeRepository = activityTypeRepository;
            _mapper = mapper;
        }
        async Task<IEnumerable<ActivityTypeOutput>> IGetLastNoticeActivities.GetLastNoticeActivities()
        {
            // Obtém os tipos de atividades do último edital
            IList<Entities.ActivityType> activityTypes = (IList<Entities.ActivityType>)await _activityTypeRepository.GetLastNoticeActivities();

            // Mapeia os tipos de atividades para o output
            _ = _mapper.Map<IEnumerable<ActivityTypeOutput>>(activityTypes);

            // return output;

            // Mapeia os tipos de atividades para o output
            List<ActivityTypeOutput> activityTypesOutput = new();
            foreach (Entities.ActivityType type in activityTypes)
            {
                // Mapeia as atividades para o output
                List<ActivityOutput> activitiesOutput = new();
                foreach (Entities.Activity activity in type.Activities!)
                {
                    activitiesOutput.Add(new ActivityOutput
                    {
                        Id = activity.Id,
                        Name = activity.Name,
                        Points = activity.Points,
                        Limits = activity.Limits,
                        DeletedAt = activity.DeletedAt
                    });
                }

                // Adiciona o tipo de atividade ao output
                activityTypesOutput.Add(new ActivityTypeOutput
                {
                    Id = type.Id,
                    Name = type.Name,
                    Unity = type.Unity,
                    DeletedAt = type.DeletedAt,
                    Activities = activitiesOutput
                });
            }

            // Retorna os tipos de atividades do último edital
            return activityTypesOutput;
        }
    }
}