namespace MSA.BankService.Dtos
{
    public record CreateAccountDto
    (
        Guid CustomerId,
        decimal AccountBalance
    );

    public record AccountDto(
        Guid Id,
        decimal AccountBalance
    );
}