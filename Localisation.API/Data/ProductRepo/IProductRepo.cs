using Localisation.API.Model;

namespace Localisation.API.Data
{
    public interface IProductRepo
    {
        Task<IEnumerable<Product>> GetAllProducts();
        Task<Product?> GetProductById(int id);
        Task CreateProduct(Product product);
        Task <IEnumerable<Product>> GetProductsByRoomId(int id);

    }
}
