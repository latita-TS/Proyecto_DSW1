using Proyecto1_DSW1.Models;
using Microsoft.Data.SqlClient;

namespace Proyecto1_DSW1.Data
{
    public class UsuarioRepository
    {
        private readonly string _connectionString;

        public UsuarioRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")!;
        }

        public async Task RegistrarUsuarioAsync(UsuarioModel u)
        {
            var sql = @"INSERT INTO Usuario (Nombre, Correo, Contrasena, FechaRegistro) 
                        VALUES (@Nombre, @Correo, @Contrasena, @FechaRegistro)";

            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Nombre", u.Nombre);
            cmd.Parameters.AddWithValue("@Correo", u.Correo);
            cmd.Parameters.AddWithValue("@Contrasena", u.Contrasena);
            cmd.Parameters.AddWithValue("@FechaRegistro", u.FechaRegistro);

            await conn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task<bool> CorreoExisteAsync(string correo)
        {
            var sql = "SELECT COUNT(*) FROM Usuario WHERE Correo = @Correo";

            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Correo", correo);

            await conn.OpenAsync();
            int count = (int)await cmd.ExecuteScalarAsync()!;
            return count > 0;
        }
    }
}
