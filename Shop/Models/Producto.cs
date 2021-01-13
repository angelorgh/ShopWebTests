using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Models
{
    public class Producto
    {
        public int ProductoID { get; set; }
        public string Nombre { get; set; }
        public int EstadoProductoID { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string Descripcion { get; set; }
        public decimal Precio { get; set; }
        
        public EstadoProducto estado { get; set; }
        public Marca marca { get; set; }
        public Categoria categoria { get; set; }
        public string Image { get; set; }
        public string Marca { get; set; }
    }
}
