using Contratos.Domain.Enuns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contratos.Domain.Entities
{
    public class Contract
    {
        public int Id { get; private set; }
        public string Numero { get; private set; } = default!;
        public string? Descricao { get; private set; } = default!;
        public long? Valor { get; private set; } = default!;

        public int SupplierId { get; private set; }
        public Supplier Supplier { get; private set; } = default!;
        public DateOnly DataInicio { get; private set; }
        public DateOnly DataFim { get; private set; }
        public ContractStatus Status { get; private set; }

        private Contract() { }
        public Contract(string numero, string? descricao, long? valor, int supplierId, DateOnly inicio, DateOnly fim, ContractStatus status)
        {
            SupplierId = supplierId;
            Status = status;
            SetNumero(numero);
            Descricao = descricao;
            Valor = valor;

            SetPeriodo(inicio, fim);

            Status = status;
            AplicarRegraVencimento();
        }
        public void SetNumero(string numero)
        {
            if (string.IsNullOrWhiteSpace(numero)) throw new ArgumentException("Numero contrato obrigatório");
            Numero = numero.Trim();
        }
        public void SetPeriodo(DateOnly inicio, DateOnly fim)
        {
            if (fim < inicio) throw new ArgumentException("Data fim não pode ser menor que início");
            DataInicio = inicio;
            DataFim = fim;
        }

        public void AlterarStatus(ContractStatus status)
        {
            Status = status;
            AplicarRegraVencimento();
        }
        private void AplicarRegraVencimento()
        {
            var hoje = DateOnly.FromDateTime(DateTime.Today);
            if (DataFim < hoje)
                Status = ContractStatus.Vencido;
        }
    }
}
