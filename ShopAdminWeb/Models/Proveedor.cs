using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopAdminWeb.Models
{
    public class Proveedor
    {
        public int IDProveedor { get; set; }
        public string Nombre { get; set; }
        public string Telefono { get; set; }
        public string Direccion { get; set; }
        public List<Libro> Libros { get; set; }

        public Proveedor() { 
        }

        public void addLibro(Libro libro)
        {
            this.Libros.Add(libro);
        }

        public void removeLibro(int idLibro)
        {
            this.Libros.RemoveAll(l => l.IDLibro == idLibro);
        }
    }
}
