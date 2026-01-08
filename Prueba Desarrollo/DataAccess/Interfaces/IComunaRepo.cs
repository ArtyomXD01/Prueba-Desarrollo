using Models.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Interfaces
{
    public interface IComunaRepo
    {
        Task<Comuna?> GetByIdAsync(int id);
        Task<bool> UpdateAsync(Comuna comuna);
    }
}
