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
    public class RegionRepo: RepoBase ,IRegionRepo
    {
        public RegionRepo(IConfiguration configuration) : base(configuration) { }

        public async Task<bool> TestConnectionAsync()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    await conn.OpenAsync();
                    return conn.State == ConnectionState.Open;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error de conexión: {ex.Message}");
                return false;
            }
        }
        public async Task<List<Region>> GetAllAsync()
        {
            var regiones = new List<Region>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("Region_ReadAll", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    await conn.OpenAsync();

                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            regiones.Add(new Region
                            {
                                IdRegion = reader.GetInt32(reader.GetOrdinal("IdRegion")),
                                NombreRegion = reader.GetString(reader.GetOrdinal("Region"))
                            });
                        }
                    }
                }
            }

            return regiones;
        }

        public async Task<Region?> GetByIdAsync(int id)
        {
            Region? region = null;

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("Region_Read", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@IdRegion", id);

                    await conn.OpenAsync();

                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            region = new Region
                            {
                                IdRegion = reader.GetInt32(reader.GetOrdinal("IdRegion")),
                                NombreRegion = reader.GetString(reader.GetOrdinal("Region"))
                            };
                        }
                    }
                }
            }
            return region;
        }

        public async Task<List<Comuna>> GetComunasByRegionIdAsync(int regionId)
        {
            var comunas = new List<Comuna>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("Comuna_ReadByRegion", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@IdRegion", regionId);

                    await conn.OpenAsync();

                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            comunas.Add(new Comuna
                            {
                                IdComuna = reader.GetInt32(reader.GetOrdinal("IdComuna")),
                                IdRegion = reader.GetInt32(reader.GetOrdinal("IdRegion")),
                                NombreComuna = reader.GetString(reader.GetOrdinal("Comuna")),
                                Informacion = reader.IsDBNull(reader.GetOrdinal("Informacion")) ? null : reader.GetString(reader.GetOrdinal("Informacion"))
                            });
                        }
                    }
                }
            }

            return comunas;
        }

        public async Task<bool> UpdateAsync(Region region)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("Region_Merge", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@IdRegion", region.IdRegion);
                    cmd.Parameters.AddWithValue("@Region", region.NombreRegion);
                    
                    await conn.OpenAsync();
                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            region.IdRegion = reader.GetInt32(0);  // Columna "Id"
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
