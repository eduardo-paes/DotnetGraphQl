using Domain.Contracts.SubArea;
using Domain.Interfaces.UseCases.SubArea;
using AutoMapper;
using Domain.Interfaces.Repositories;
using System.Threading.Tasks;
using System;

namespace Domain.UseCases.SubArea
{
    public class UpdateSubArea : IUpdateSubArea
    {
        #region Global Scope
        private readonly ISubAreaRepository _subAreaRepository;
        private readonly IMapper _mapper;
        public UpdateSubArea(ISubAreaRepository subAreaRepository, IMapper mapper)
        {
            _subAreaRepository = subAreaRepository;
            _mapper = mapper;
        }
        #endregion

        public async Task<DetailedReadSubAreaOutput> Execute(Guid? id, UpdateSubAreaInput dto)
        {
            // Recupera entidade que será atualizada
            var entity = await _subAreaRepository.GetById(id);

            // Atualiza atributos permitidos
            entity.Name = dto.Name;
            entity.Code = dto.Code;
            entity.AreaId = dto.AreaId;

            // Salva entidade atualizada no banco
            var model = await _subAreaRepository.Update(entity);
            return _mapper.Map<DetailedReadSubAreaOutput>(model);
        }
    }
}