using Products;

namespace Requests.Products.Commands;

public interface IProductsRepository
{
    Task InsertAsync(Product product, CancellationToken cancellationToken);
}

public class ProductsRepository : IProductsRepository
{
    public async Task InsertAsync(Product product, CancellationToken cancellationToken)
    {
        // Simulate asynchronous database insertion
        await Task.Delay(100, cancellationToken); // Simulating async operation
        Console.WriteLine($"Product '{product.Name}' inserted successfully.");
    }
}