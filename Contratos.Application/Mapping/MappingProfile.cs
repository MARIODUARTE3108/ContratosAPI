using AutoMapper;
using Contratos.Application.DTOs;
using Contratos.Domain.Entities;
using Contratos.Domain.Enuns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contratos.Application.Mapping
{
    public class MappingProfile : Profile
    {
        
             public MappingProfile()
        {
            // Mapeamentos simples
            CreateMap<Person, PersonOutputDto>();
            CreateMap<Supplier, SupplierOutputDto>();

            // Contract -> ContractOutputDto (record posicional)
            // Casamos cada parâmetro do construtor via ForCtorParam
            CreateMap<Contract, ContractOutputDto>()
                .ForCtorParam("Id", opt => opt.MapFrom(s => s.Id))
                .ForCtorParam("Numero", opt => opt.MapFrom(s => s.Numero))
                .ForCtorParam("Descricao", opt => opt.MapFrom(s => s.Descricao ?? string.Empty))
                .ForCtorParam("EmpresaId", opt => opt.MapFrom(s => s.SupplierId))  // <- nomes diferentes
                .ForCtorParam("DataInicio", opt => opt.MapFrom(s => s.DataInicio))
                .ForCtorParam("DataFim", opt => opt.MapFrom(s => s.DataFim))
                .ForCtorParam("Status", opt => opt.MapFrom(s => s.Status))
                .ForCtorParam("EmpresaNome", opt => opt.MapFrom(s => s.Supplier.Nome));


            // Se você tiver um DTO de criação, já deixe o mapeamento (ajuste o nome se for diferente)
            // CreateDto -> Entity (usa o seu ctor público de Contract)
            CreateMap<ContractCreateDto, Contract>()
            .ConstructUsing(s => new Contract(
                s.Numero,
                s.Descricao,
                s.Valor,
                s.EmpresaId,                  // mapeia EmpresaId -> supplierId
                s.DataInicio,
                s.DataFim,
                (ContractStatus)s.Status
            ));

        }
    }
}
