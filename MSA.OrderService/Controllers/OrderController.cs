using Microsoft.AspNetCore.Mvc;
using MSA.OrderService.Domain;
using MSA.OrderService.Infrastructure.Data;
using MSA.OrderService.Dtos;
using MSA.Common.Contracts.Domain;
using MSA.Common.PostgresMassTransit.PostgresDB;
using MSA.OrderService.Services;

namespace MSA.OrderService.Controllers;

[ApiController]
[Route("v1/order")]
public class OrderController : ControllerBase
{
    private readonly IRepository<Order> repository;

    private readonly PostgresUnitOfWork<MainDbContext> uow;
    private readonly IProductService productService;
    public OrderController(
        IRepository<Order> repository,
        PostgresUnitOfWork<MainDbContext> uow,
        IProductService productService
        )
    {
        this.repository = repository;
        this.uow = uow;
        this.productService = productService;
    }

    [HttpGet]
    public async Task<IEnumerable<Order>> GetAsync()
    {
        var orders = (await repository.GetAllAsync()).ToList();
        return orders;
    }

    [HttpPost]
    public async Task<ActionResult<Order>> PostAsync(CreateOrderDto createOrderDto)
    {
        // validate and ensure product exist before creating
        var IsProductExisted = await productService.IsProductExisted(createOrderDto.ProductId);
        if(!IsProductExisted)
            return BadRequest();
        var order = new Order { 
            Id = Guid.NewGuid(),
            UserId = createOrderDto.UserId,
            OrderStatus = "Order Submitted"
        };
        await repository.CreateAsync(order);

        await uow.SaveChangeAsync();

        return CreatedAtAction(nameof(PostAsync), order);
    }
}