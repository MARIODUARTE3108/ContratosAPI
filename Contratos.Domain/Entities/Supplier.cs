using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contratos.Domain.Entities
{
    public class Supplier
    {
        public int Id { get; private set; }
        public string Nome { get; private set; } = default!;
        public string Cnpj { get; private set; } = default!;
        public string Email { get; private set; } = default!;
        public string Telefone { get; private set; } = default!;
        private Supplier() { }
        public Supplier(string nome, string cnpj, string email, string telefone)
        {
            SetNome(nome);
            SetCnpj(cnpj);
            SetEmail(email);
            SetTelefone(telefone);
        }
        public void SetNome(string nome) => Nome = string.IsNullOrWhiteSpace(nome) ? throw new ArgumentException("Nome obrigatório") : nome.Trim();
        public void SetCnpj(string cnpj) => Cnpj = string.IsNullOrWhiteSpace(cnpj) ? throw new ArgumentException("CNPJ obrigatório") : cnpj.Trim();
        public void SetEmail(string email) => Email = string.IsNullOrWhiteSpace(email) ? throw new ArgumentException("Email obrigatório") : email.Trim().ToLowerInvariant();
        public void SetTelefone(string tel) => Telefone = string.IsNullOrWhiteSpace(tel) ? throw new ArgumentException("Telefone obrigatório") : tel.Trim();
    }
}
