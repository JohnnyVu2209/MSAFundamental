using Newtonsoft.Json;
namespace MSA.OrderService.Services;

public class ProductService : IProductService
{
    private readonly HttpClient httpClient;

    public ProductService(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    public async Task<bool> IsProductExisted(Guid id)
    {
        var result = await httpClient.GetAsync($"v1/product/{id}");
        var response = await result.Content.ReadAsStringAsync();
        var responseConvert = JsonConvert.DeserializeObject<Guid>(response);
        if (responseConvert != null) return await Task.FromResult(true);

        return await Task.FromResult(false);
    }
}