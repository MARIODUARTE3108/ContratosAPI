using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contratos.Domain.Entities
{
    public class Person
    {
        public int Id { get; private set; }
        public string Nome { get; private set; } = default!;
        public string Email { get; private set; } = default!;
        public string PasswordHash { get; private set; } = default!;

        private Person() { }
        public Person(string nome, string email, string passwordHash)
        {
            SetNome(nome);
            SetEmail(email);
            PasswordHash = passwordHash;
        }
        public void SetNome(string nome)
        {
            if (string.IsNullOrWhiteSpace(nome)) throw new ArgumentException("Nome obrigatório");
            Nome = nome.Trim();
        }
        public void SetEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) throw new ArgumentException("Email obrigatório");
            Email = email.Trim().ToLowerInvariant();
        }
        public void SetPasswordHash(string hash)
        {
            if (string.IsNullOrWhiteSpace(hash)) throw new ArgumentException("Hash inválido");
            PasswordHash = hash;
        }
    }
}
