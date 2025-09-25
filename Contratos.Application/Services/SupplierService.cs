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
    public class SupplierService : ISupplierService
    {
        private readonly ISupplierRepository _repo;
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public SupplierService(ISupplierRepository repo, IUnitOfWork uow, IMapper mapper)
        {
            _repo = repo; _uow = uow; _mapper = mapper;
        }
        public async Task<SupplierOutputDto> CreateAsync(SupplierCreateDto dto)
        {
            var emp = new Supplier(dto.Nome, dto.Cnpj, dto.Email, dto.Telefone);
            await _repo.AddAsync(emp);
            await _uow.SaveChangesAsync();
            return _mapper.Map<SupplierOutputDto>(emp);
        }

        public async Task<SupplierOutputDto?> GetByIdAsync(int id)
        {
            var e = await _repo.GetByIdAsync(id);
            return e is null ? null : _mapper.Map<SupplierOutputDto>(e);
        }

        public async Task<List<SupplierOutputDto>> ListAsync()
                => _mapper.Map<List<SupplierOutputDto>>(await _repo.ListAsync());

    }
}
