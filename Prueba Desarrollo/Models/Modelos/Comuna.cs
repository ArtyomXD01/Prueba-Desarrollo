using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Modelos
{
    public class Comuna
    {
        public int IdComuna { get; set; }
        public int IdRegion { get; set; }
        public string NombreComuna { get; set; }
        public string? Informacion { get; set; }
    }
}
