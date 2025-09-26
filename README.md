# Contratos MODEC — .NET 8 + DDD + SQLite + JWT

Sistema de **gestão de contratos** com módulos de **cadastro de pessoas**, **empresas** e **autenticação via JWT**. Arquitetura em **camadas (DDD)**, persistência com **EF Core + SQLite**, **migrações aplicadas automaticamente** no program e **Swagger/OpenAPI** para documentação.


## Visão geral
Este repositório implementa um backend para controle de contratos da **MODEC** (e/ou parceiros), permitindo:
- Autenticar usuários e emitir **tokens JWT**;
- Cadastrar **Pessoas** (usuários internos/externos);
- Cadastrar **Empresas**;
- Criar/atualizar **Contratos** vinculados a Empresas e Pessoas;
- Consultar, filtrar e exportar dados (quando aplicável).

O **SQLite** é utilizado para simplificar a execução local. Em produção, recomenda-se avaliar SQL Server, PostgreSQL ou Azure SQL.
  

```
/src
  /Contratos.API                  # Camada de apresentação (Web API)
  /Contratos.Application          # Casos de uso (Services), DTOs, validações
  /Contratos.Domain               # Entidades, Agregados, Regras de negócio, Repositórios (interfaces)
  /Contratos.Infrastructure       # EF Core, Migrations, Repositórios (implementações), Auth, Seed

/tests
  /Contratos.Tests                # Testes unitários e de integração
```
## Tecnologias
- **Linguagem**: C# 12 / .NET 8
- **Web**: ASP.NET Core Minimal/API Controllers
- **ORM**: EF Core 8 + SQLite
- **Auth**: JWT Bearer (Microsoft.AspNetCore.Authentication.JwtBearer)
- **Docs**: Swashbuckle
- **Mapper**: AutoMapper
- **Validação**: FluentValidation

---

## Pré-requisitos
- **.NET SDK 8.0**
- **Git**
- Nenhuma instalação de DB necessária (SQLite é arquivo local)
