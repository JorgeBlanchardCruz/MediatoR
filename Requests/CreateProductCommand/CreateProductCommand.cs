using Products;
using SharedKernel.Transversal.Responses;

namespace Requests.Products.Commands;

public sealed record CreateProductCommand : IRequest<Response<Product>>
{
    public string Name { get; set; } = string.Empty!;
    public string Description { get; set; } = string.Empty!;
    public decimal Price { get; set; }
}
