using AutoMapper;
using Contratos.Application.DTOs;
using Contratos.Application.Interfaces;
using Contratos.Domain.Entities;
using Contratos.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contratos.Application.Services
{
    public class ContractService : IContractService
    {
        private readonly IContractRepository _repo;
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public ContractService(IContractRepository repo, IUnitOfWork uow, IMapper mapper)
        {
            _repo = repo; _uow = uow; _mapper = mapper;
        }
        public async Task<ContractOutputDto> CreateAsync(ContractCreateDto dto)
        {
            var c = new Contract(dto.Numero, dto.Descricao, dto.Valor, dto.EmpresaId, dto.DataInicio, dto.DataFim, dto.Status);
            await _repo.AddAsync(c);
            await _uow.SaveChangesAsync();
            return _mapper.Map<ContractOutputDto>(c);
        }

        public async Task<ContractOutputDto?> GetByIdAsync(int id)
        {
            var c = await _repo.GetByIdAsync(id);
            return c is null ? null : _mapper.Map<ContractOutputDto>(c);
        }

        public async Task<List<ContractOutputDto>> ListAsync()
             => _mapper.Map<List<ContractOutputDto>>(await _repo.ListAsync());

        public async Task<List<ContractOutputDto>> ListByEmpresaAsync(int empresaId)
                => _mapper.Map<List<ContractOutputDto>>(await _repo.ListByEmpresaAsync(empresaId));


        public async Task<ContractOutputDto> UpdateAsync(int id, ContractUpdateDto dto)
        {
            var c = await _repo.GetByIdAsync(id) ?? throw new KeyNotFoundException("Contrato não encontrado");
            c.SetNumero(dto.Numero);
            c.SetPeriodo(dto.DataInicio, dto.DataFim);
            c.AlterarStatus(dto.Status);
            _repo.Update(c);
            await _uow.SaveChangesAsync();
            return _mapper.Map<ContractOutputDto>(c);
        }
    }
}
