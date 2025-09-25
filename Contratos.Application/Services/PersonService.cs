using AutoMapper;
using Contratos.Application.DTOs;
using Contratos.Application.Interfaces;
using Contratos.Domain.Entities;
using Contratos.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BCrypt.Net;

namespace Contratos.Application.Services
{
    public class PersonService : IPersonService
    {
        private readonly IPersonRepository _repo;
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private readonly ITokenService _token;

        public PersonService(IPersonRepository repo, IUnitOfWork uow, IMapper mapper, ITokenService token)
        {
            _repo = repo; _uow = uow; _mapper = mapper; _token = token;
        }
        public async Task<PersonOutputDto> CreateAsync(PersonCreateDto dto)
        {
            var existe = await _repo.GetByEmailAsync(dto.Email);
            if (existe is not null) throw new InvalidOperationException("E-mail já cadastrado");

            var hash = BCrypt.Net.BCrypt.HashPassword(dto.Senha);
            var user = new Person(dto.Nome, dto.Email, hash);
            await _repo.AddAsync(user);
            await _uow.SaveChangesAsync();
            return _mapper.Map<PersonOutputDto>(user);
        }

        public async Task<List<PersonOutputDto>> ListAsync()
        {
            var list = await _repo.ListAsync();
            return _mapper.Map<List<PersonOutputDto>>(list);
        }        

        public async Task<LoginResponseDto> LoginAsync(LoginRequestDto dto)
        {
            var user = await _repo.GetByEmailAsync(dto.Email) ?? throw new UnauthorizedAccessException("Credenciais inválidas");
            if (!BCrypt.Net.BCrypt.Verify(dto.Senha, user.PasswordHash)) throw new UnauthorizedAccessException("Credenciais inválidas");
            return new LoginResponseDto(_token.Generate(user.Id, user.Email, user.Nome));
        }
    }
}
