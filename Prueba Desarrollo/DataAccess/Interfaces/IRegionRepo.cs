using Models.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Interfaces
{
    public interface IRegionRepo
    {
        Task<List<Region>> GetAllAsync();
        Task<Region?> GetByIdAsync(int id);
        Task<List<Comuna>> GetComunasByRegionIdAsync(int regionId);
        Task<bool> UpdateAsync(Region region);
    }
}
