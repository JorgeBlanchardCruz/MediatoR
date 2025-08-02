using Products;
using SharedKernel.Transversal.Responses;

namespace Requests.Products.Commands;

internal class CreateProductHandler : IRequestHandler<CreateProductCommand, Response<Product>>
{
    private readonly IProductsRepository _ProductsRepository;

    public CreateProductHandler(IProductsRepository productsRepository)
    {
        _ProductsRepository = productsRepository;
    }

    public async Task<Response<Product>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var entity = Product.Create(request.Name, request.Description, request.Price);

        await _ProductsRepository.InsertAsync(entity, cancellationToken);

        return new Response<Product>
        {
            Data = entity,
            IsSuccess = true,
            Message = $"{typeof(Product).Name} successfully"
        }; ;
    }
}
