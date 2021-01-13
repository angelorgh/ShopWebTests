using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopAdminWeb.Models
{
    public class Producto
    {
        public int ProductoID { get; set; }
        public string Nombre { get; set; }
        public int EstadoProductoID { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string Descripcion { get; set; }
        public double Precio { get; set; }
        public EstadoProducto Estado { get; set; }
        public Marca Marca { get; set; }
        public Categoria Categoria { get; set; }
        public string Image { get; set; }
    }
}
