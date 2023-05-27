using Adapters.Gateways.Area;
using Adapters.Gateways.Base;
using Adapters.Interfaces;
using AutoMapper;
using Domain.Contracts.Area;
using Domain.Interfaces.UseCases;

namespace Adapters.PresenterController
{
    public class AreaPresenterController : IAreaPresenterController
    {
        #region Global Scope
        private readonly ICreateArea _createArea;
        private readonly IUpdateArea _updateArea;
        private readonly IDeleteArea _deleteArea;
        private readonly IGetAreasByMainArea _getAreasByMainArea;
        private readonly IGetAreaById _getAreaById;
        private readonly IMapper _mapper;

        public AreaPresenterController(ICreateArea createArea, IUpdateArea updateArea, IDeleteArea deleteArea, IGetAreasByMainArea getAreasByMainArea, IGetAreaById getAreaById, IMapper mapper)
        {
            _createArea = createArea;
            _updateArea = updateArea;
            _deleteArea = deleteArea;
            _getAreasByMainArea = getAreasByMainArea;
            _getAreaById = getAreaById;
            _mapper = mapper;
        }
        #endregion

        public async Task<Response> Create(Request request)
        {
            var dto = request as CreateAreaRequest;
            var input = _mapper.Map<CreateAreaInput>(dto);
            var result = await _createArea.Execute(input);
            return _mapper.Map<DetailedReadAreaResponse>(result);
        }

        public async Task<Response> Delete(Guid? id)
        {
            var result = await _deleteArea.Execute(id);
            return _mapper.Map<DetailedReadAreaResponse>(result);
        }

        public async Task<IEnumerable<Response>> GetAreasByMainArea(Guid? mainAreaId, int skip, int take)
        {
            var result = await _getAreasByMainArea.Execute(mainAreaId, skip, take);
            return _mapper.Map<IEnumerable<ResumedReadAreaResponse>>(result);
        }

        public Task<IEnumerable<Response>> GetAll(int skip, int take)
        {
            throw new NotImplementedException();
        }

        public async Task<Response> GetById(Guid? id)
        {
            var result = await _getAreaById.Execute(id);
            return _mapper.Map<DetailedReadAreaResponse>(result);
        }

        public async Task<Response> Update(Guid? id, Request request)
        {
            var dto = request as UpdateAreaRequest;
            var input = _mapper.Map<UpdateAreaInput>(dto);
            var result = await _updateArea.Execute(id, input);
            return _mapper.Map<DetailedReadAreaResponse>(result);
        }
    }
}