using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopAdminWeb.Models
{
    public class CategoriaMarcaProducto
    {
        public Categoria categoria {get;set;}
        public List<Producto> productos { get; set; }
        public List<Marca> marcas { get; set; }
        public CategoriaMarcaProducto () { }
        public CategoriaMarcaProducto(Categoria c, List<Producto> p, List<Marca> m) {categoria = c; productos = p; marcas = m;}
    }
}
