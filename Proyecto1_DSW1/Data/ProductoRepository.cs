using Proyecto1_DSW1.Models;
using Microsoft.Data.SqlClient;

namespace Proyecto1_DSW1.Data
{
    public class ProductoRepository
    {
        private readonly string _connectionString;

        public ProductoRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")!;
        }

        public async Task<List<ProductoModel>> ObtenerProductosAsync()
        {
            var lista = new List<ProductoModel>();
            var sql = "SELECT Id, Nombre, Precio, Cantidad, Estado FROM Productos WHERE Estado = 1";

            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(sql, conn);
            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                lista.Add(new ProductoModel
                {
                    Id       = reader.GetInt32(0),
                    Nombre   = reader.GetString(1),
                    Precio   = reader.GetDecimal(2),
                    Cantidad = reader.GetInt32(3),
                    Estado   = reader.GetBoolean(4)
                });
            }
            return lista;
        }

        public async Task<(List<ProductoModel> Productos, int TotalRegistros)> ObtenerProductosPaginadoAsync(int pagina, int registrosPorPagina)
        {
            var lista = new List<ProductoModel>();
            int offset = (pagina - 1) * registrosPorPagina;

            var sql = $@"SELECT Id, Nombre, Precio, Cantidad, Estado 
                         FROM Productos 
                         ORDER BY Id 
                         OFFSET {offset} ROWS FETCH NEXT {registrosPorPagina} ROWS ONLY;
                         SELECT COUNT(*) FROM Productos;";

            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(sql, conn);
            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                lista.Add(new ProductoModel
                {
                    Id       = reader.GetInt32(0),
                    Nombre   = reader.GetString(1),
                    Precio   = reader.GetDecimal(2),
                    Cantidad = reader.GetInt32(3),
                    Estado   = reader.GetBoolean(4)
                });
            }

            await reader.NextResultAsync();
            int total = 0;
            if (await reader.ReadAsync())
                total = reader.GetInt32(0);

            return (lista, total);
        }

        public async Task AgregarProductoAsync(ProductoModel p)
        {
            var sql = @"INSERT INTO Productos (Nombre, Precio, Cantidad, Estado) 
                        VALUES (@Nombre, @Precio, @Cantidad, @Estado)";

            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Nombre",   p.Nombre);
            cmd.Parameters.AddWithValue("@Precio",   p.Precio);
            cmd.Parameters.AddWithValue("@Cantidad", p.Cantidad);
            cmd.Parameters.AddWithValue("@Estado",   p.Estado);

            await conn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }
    }
}

//keloke mamayema