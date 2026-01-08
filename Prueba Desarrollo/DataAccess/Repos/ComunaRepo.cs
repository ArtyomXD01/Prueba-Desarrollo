using DataAccess.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Models.Modelos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repos
{
    public class ComunaRepo : RepoBase, IComunaRepo

    {
        public ComunaRepo(IConfiguration configuration) : base(configuration) { }

        public async Task<Comuna?> GetByIdAsync(int id)
        {
            Comuna? comuna = null;

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("Comuna_Read", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@IdComuna", id);

                    await conn.OpenAsync();

                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            comuna = new Comuna
                            {
                                IdComuna = reader.GetInt32(reader.GetOrdinal("IdComuna")),
                                IdRegion = reader.GetInt32(reader.GetOrdinal("IdRegion")),
                                NombreComuna = reader.GetString(reader.GetOrdinal("Comuna")),
                                Informacion = reader.IsDBNull(reader.GetOrdinal("Informacion"))
                                    ? null
                                    : reader.GetString(reader.GetOrdinal("Informacion"))
                            };
                        }
                    }
                }
            }

            return comuna;
        }

        public async Task<bool> UpdateAsync(Comuna comuna)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("Comuna_merge", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@IdComuna", comuna.IdComuna);
                    cmd.Parameters.AddWithValue("@IdRegion", comuna.IdRegion);
                    cmd.Parameters.AddWithValue("@Comuna", comuna.NombreComuna);

                    // Manejo especial para XML
                    SqlParameter xmlParam = new SqlParameter("@Informacion", SqlDbType.Xml);
                    if (string.IsNullOrEmpty(comuna.Informacion))
                    {
                        xmlParam.Value = DBNull.Value;
                    }
                    else
                    {
                        xmlParam.Value = comuna.Informacion;
                    }
                    cmd.Parameters.Add(xmlParam);

                    await conn.OpenAsync();
                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {

                            comuna.IdComuna = reader.GetInt32(0);  // Columna "Id"
                            int success = reader.GetInt32(1);  // Columna "Success"

                            return success == 1;
                        }
                    }
                    return false;
                }
            }
        }

    }
}
