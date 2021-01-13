using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopAdminWeb.Models
{
    public class ProductoCategoria
    {
        public List<Producto> productos { get; set; }
        public List<Categoria> categorias { get; set; }

        public ProductoCategoria(List<Producto> p, List<Categoria> c) 
        {
            productos = p;
            categorias = c;
        }
    }
    
}
