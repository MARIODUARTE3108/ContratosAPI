using Bogus;
using Contratos.Application.DTOs;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Contratos.Teste.Tests
{
    public class SupplierEndpointsTests : IClassFixture<CustomWebAppFactory>
    {
        private readonly HttpClient _client;

        public SupplierEndpointsTests(CustomWebAppFactory factory)
        {
            _client = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });

            _client.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Test");
        }
        [Fact]
        public async Task Get_Contratos_Cadastrados()
        {
            var login = await RegisterAndLoginAsync();

            _client.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", login.Token);

            var resp = await _client.GetAsync("/api/suppliers");

            resp.StatusCode.Should().Be(HttpStatusCode.OK);

            var persons = await resp.Content.ReadFromJsonAsync<List<Person>>();
            persons.Should().NotBeNull();
        }
        private async Task<LoginResponseDto> RegisterAndLoginAsync()
        {
            var faker = new Faker("pt_BR");

            var senha = faker.Internet.Password(8);
            var novo = new PersonCreateDto(
                faker.Person.FirstName,
                faker.Person.Email,
                senha
            );

            var reg = await _client.PostAsJsonAsync("/api/auth/register", novo);

            reg.StatusCode.Should().Be(HttpStatusCode.OK);

            var loginReq = new LoginRequestDto(novo.Email, senha);

            var loginResp = await _client.PostAsJsonAsync("/api/auth/login", loginReq);
            loginResp.StatusCode.Should().Be(HttpStatusCode.OK);

            var login = await loginResp.Content.ReadFromJsonAsync<LoginResponseDto>();
            login.Should().NotBeNull();
            login!.Token.Should().NotBeNullOrWhiteSpace();

            return login;
        }
    }
}
