using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopAdminWeb.Models
{
    public class EstadoProducto
    {
        public int EstadoProductoID { get; set; }
        public string Nombre { get; set; }
        public DateTime FechaRegistro { get; set; }
    }
}
