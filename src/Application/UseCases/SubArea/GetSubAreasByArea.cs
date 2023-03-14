using Application.DTOs.SubArea;
using Application.Proxies.SubArea;
using AutoMapper;
using Domain.Interfaces;

namespace Application.UseCases.SubArea
{
    public class GetSubAreasByArea : IGetSubAreasByArea
    {
        #region Global Scope
        private readonly ISubAreaRepository _subAreaRepository;
        private readonly IMapper _mapper;
        public GetSubAreasByArea(ISubAreaRepository subAreaRepository, IMapper mapper)
        {
            _subAreaRepository = subAreaRepository;
            _mapper = mapper;
        }
        #endregion

        public async Task<IQueryable<ResumedReadSubAreaDTO>> Execute(Guid? areaId, int skip, int take)
        {
            var entities = await _subAreaRepository.GetSubAreasByArea(areaId, skip, take);
            return _mapper.Map<IEnumerable<ResumedReadSubAreaDTO>>(entities).AsQueryable();
        }
    }
}