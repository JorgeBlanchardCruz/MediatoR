namespace Products;

/// <summary>
/// Its only an RootEntity example
/// </summary>
public class Product
{
    #region Properties
    public string Id { get; set; }
    public string Name { get; private set; }
    public string Description { get; private set; }
    public decimal Price { get; private set; }
    public bool IsAvailable { get; private set; }
    #endregion

    private Product(string id, string name, string description, decimal price)
    {
        Id = id;
        Name = name;
        Description = description;
        Price = price;
        IsAvailable = true;
    }

    public static Product Create(string name, string description, decimal price)
    {
        var product = new Product("1", name, description, price);

        return product;
    }
}
