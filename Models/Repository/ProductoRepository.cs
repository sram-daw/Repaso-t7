using Dapper;

namespace Repaso_t7.Models.Repository
{
    public class ProductoRepository : IProductoRepository
    {
        private readonly Conexion _conexion;

        public ProductoRepository(Conexion conexion)
        {
            _conexion = conexion;
        }

        public async Task<IEnumerable<Producto>> GetAllProductos()
        {

            var query = "SELECT * FROM Producto";
            using (var conexion = _conexion.ObtenerConexion())
            {
                var productos = await conexion.QueryAsync<Producto>(query);
                return productos.ToList();
            }

        }

        public async Task<bool> GuardarProducto(string nombre, decimal precio)
        {

            var query = "INSERT INTO Producto (Nombre, Precio) VALUES (@nombre, @precio)";
            using (var conexion = _conexion.ObtenerConexion())
            {
                int filasAfectadas = await conexion.ExecuteAsync(query, new { nombre, precio });
                return filasAfectadas > 0;
            }

        }

        public async Task<Producto> GetProductoById(int id)
        {

            var query = "SELECT * FROM Producto WHERE Id=@id";
            using (var conexion = _conexion.ObtenerConexion())
            {
                var producto = await conexion.QueryFirstOrDefaultAsync<Producto>(query, new { id });
                return producto;
            }

        }

        public async Task<bool> UpdateProducto(int id, string nombre, decimal precio)
        {

            var query = "UPDATE Producto SET Nombre=@nombre, Precio=@precio WHERE Id=@id";
            using (var conexion = _conexion.ObtenerConexion())
            {
                int filasAfectadas = await conexion.ExecuteAsync(query, new { nombre, precio, id });
                return filasAfectadas > 0;
            }
        }

        public async Task<bool> DeleteProducto(int id)
        {
            var query = "DELETE FROM Producto WHERE Id=@id";
            using (var conexion = _conexion.ObtenerConexion())
            {
                int filasAfectadas = await conexion.ExecuteAsync(query, new { id });
                return filasAfectadas > 0;
            }
        }
        public async Task<bool> CheckLogin(string nombre, string pwd)
        {
            var query = "SELECT * FROM Usuarios WHERE usuario=@nombre AND contraseña=@pwd";
            using (var conexion = _conexion.ObtenerConexion())
            {
                var usuario = await conexion.QueryFirstOrDefaultAsync<Usuario>(query, new { nombre, pwd });
                if (usuario != null)
                {
                    return true;
                }
                else return false;

            }
        }

        public async Task<Usuario> GetUser(string nombre, string pwd)
        {
            var query = "SELECT * FROM Usuarios WHERE usuario=@nombre AND contraseña=@pwd";
            using (var conexion = _conexion.ObtenerConexion())
            {
                var usuario = await conexion.QueryFirstOrDefaultAsync<Usuario>(query, new { nombre, pwd });
                if (usuario != null)
                {
                    return usuario;
                }
                else return null;

            }
        }
        public async Task<IEnumerable<Usuario>> GetAllUsers()
        {
            var query = "SELECT * FROM Usuarios";
            using (var conexion = _conexion.ObtenerConexion())
            {
                var usuarios = await conexion.QueryAsync<Usuario>(query);
                if (usuarios != null)
                {
                    return usuarios;
                }
                else return null;

            }
        }

    }
}
