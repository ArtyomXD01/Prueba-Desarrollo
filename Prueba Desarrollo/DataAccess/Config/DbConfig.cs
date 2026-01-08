using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;


namespace DataAccess.Config
{
        public class DatabaseConfig
        {
            private readonly IConfiguration _configuration;

            public DatabaseConfig(IConfiguration configuration)
            {
                _configuration = configuration;
            }

        public string GetConnectionString()
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("La cadena de conexión 'DefaultConnection' no está configurada en appsettings.json");
            }

            return connectionString;
        }
    }
}
