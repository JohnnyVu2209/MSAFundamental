using MSA.Common.Contracts.Domain;

namespace MSA.BankService.Entities
{
    public class Account : IEntity
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public decimal AccountBalance { get; set; }
    }
}
