using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MSA.BankService.Dtos;
using MSA.BankService.Entities;
using MSA.BankService.Infrastructure.Data;
using MSA.Common.Contracts.Domain;
using MSA.Common.PostgresMassTransit.PostgresDB;

namespace MSA.BankService.Controllers;

[ApiController]
[Route("v1/account")]
[Authorize]
public class BankController : ControllerBase
{
    private readonly IRepository<Account> repository;
    private readonly PostgresUnitOfWork<MainDbContext> unitOfWork;

    public BankController(
        IRepository<Account> repository,
        PostgresUnitOfWork<MainDbContext> unitOfWork
    )
    {
        this.repository = repository;
        this.unitOfWork = unitOfWork;
    }

    [HttpGet]
    [Authorize("read_access")]
    public async Task<IEnumerable<Account>> GetAccounts()
    {
        var accounts = (await repository.GetAllAsync()).ToList();
        return accounts;
    }

    [HttpGet("{id}")]
    [Authorize("read_access")]
    public async Task<AccountDto> GetAccount(Guid id)
    {
        var account = await repository.GetAsync(id);
        return account.AsDto();
    }

    [HttpPost]
    public async Task<ActionResult<AccountDto>> PostAsync(CreateAccountDto createAccountDto)
    {
        var account = new Account{
            Id = Guid.NewGuid(),
            CustomerId = createAccountDto.CustomerId,
            AccountBalance = createAccountDto.AccountBalance
        };
        await repository.CreateAsync(account);

        await unitOfWork.SaveChangeAsync();

        return CreatedAtAction(nameof(PostAsync), account.AsDto());
    }
}