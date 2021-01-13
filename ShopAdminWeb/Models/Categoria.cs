using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopAdminWeb.Models
{
    public class Categoria
    {
        public int CategoriaID { get; set; }
        public string Nombre { get; set; }
        public DateTime FechaRegistro { get; set;}
        
        public Categoria() { }
        public Categoria(int id, string nombre)
        {
            CategoriaID = id;
            Nombre = nombre;
        }
    }
}
