namespace Repaso_t7.Models.Repository
{
    public interface IProductoRepository
    {
        Task<IEnumerable<Producto>> GetAllProductos();
        Task<bool> GuardarProducto(string nombre, decimal precio);
        Task<Producto> GetProductoById(int id);
        Task<bool> UpdateProducto(int id, string nombre, decimal precio);
        Task<bool> DeleteProducto(int id);
        Task<bool> CheckLogin(string nombre, string pwd);
        Task<Usuario> GetUser(string nombre, string pwd);
        Task<IEnumerable<Usuario>> GetAllUsers();
    }
}
