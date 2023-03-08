﻿using System;
using AutoMapper;
using CopetSystem.Application.DTOs.Area;
using CopetSystem.Application.Interfaces;
using CopetSystem.Domain.Entities;
using CopetSystem.Domain.Interfaces;

namespace CopetSystem.Application.Services
{
  public class AreaService : IAreaService
  {
    #region Global Scope
    private readonly IAreaRepository _areaRepository;
    private readonly IMainAreaRepository _mainAreaRepository;
    private readonly IMapper _mapper;
    public AreaService(IAreaRepository areaRepository, IMainAreaRepository mainAreaRepository, IMapper mapper)
    {
      _areaRepository = areaRepository;
      _mainAreaRepository = mainAreaRepository;
      _mapper = mapper;
    }
    #endregion

    #region Public Methods
    public async Task<DetailedReadAreaDTO> Create(CreateAreaDTO dto)
    {
      var entity = await _areaRepository.GetByCode(dto.Code);
      if (entity != null)
        throw new Exception($"Já existe uma Principal para o código {dto.Code}");

      // Verifica id da área princial
      if (dto.MainAreaId == null)
        throw new Exception($"O Id da Área Principal não pode ser vazio.");

      // Valida se existe área principal
      var area = await _mainAreaRepository.GetById(dto.MainAreaId);
      if (area.DeletedAt != null)
        throw new Exception($"A Área Principal informada está inativa.");

      // Cria nova área
      entity = await _areaRepository.Create(_mapper.Map<Area>(dto));
      return _mapper.Map<DetailedReadAreaDTO>(entity);
    }

    public async Task<DetailedReadAreaDTO> Delete(Guid id)
    {
      var model = await _areaRepository.Delete(id);
      return _mapper.Map<DetailedReadAreaDTO>(model);
    }

    public async Task<IQueryable<ResumedReadAreaDTO>> GetAreasByMainArea(Guid? mainAreaId, int skip, int take)
    {
      var entities = await _areaRepository.GetAreasByMainArea(mainAreaId, skip, take);
      return _mapper.Map<IEnumerable<ResumedReadAreaDTO>>(entities).AsQueryable();
    }

    public async Task<DetailedReadAreaDTO> GetById(Guid? id)
    {
      var entity = await _areaRepository.GetById(id);
      return _mapper.Map<DetailedReadAreaDTO>(entity);
    }

    public async Task<DetailedReadAreaDTO> Update(Guid? id, UpdateAreaDTO dto)
    {
      // Recupera entidade que será atualizada
      var entity = await _areaRepository.GetById(id);

      // Atualiza atributos permitidos
      entity.UpdateName(dto.Name);
      entity.UpdateCode(dto.Code);
      entity.UpdateMainArea(dto.MainAreaId);

      // Salva entidade atualizada no banco
      var model = await _areaRepository.Update(entity);
      return _mapper.Map<DetailedReadAreaDTO>(model);
    }
    #endregion
  }
}