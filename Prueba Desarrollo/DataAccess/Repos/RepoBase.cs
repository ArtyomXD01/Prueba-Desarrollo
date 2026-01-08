using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repos
{
    public abstract class RepoBase
    {
        protected readonly string _connectionString;

        protected RepoBase(IConfiguration configuration)
        {
            // Intentar leer desde configuration
            _connectionString = configuration?.GetConnectionString("DefaultConnection");

            if (string.IsNullOrEmpty(_connectionString))
            {
                _connectionString = "packet size=4096;user id=sa;data source=DESKTOP-CF7MHO8\\DESARROLLO;persist security info=True;initial catalog=Prueba Desarrollador;password=Pruebas123";
            }
        }
    }
}
