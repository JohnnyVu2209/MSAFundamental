using MSA.BankService.Entities;

namespace MSA.BankService.Dtos
{
    public static class Extensions
    {
        public static AccountDto AsDto(this Account account)
        {
            return new AccountDto(
                account.Id,
                account.AccountBalance
            );
        }
    }
}