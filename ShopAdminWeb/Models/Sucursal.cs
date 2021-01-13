using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopAdminWeb.Models
{
    public class Sucursal
    {
        public int IDSucursal { get; set; }
        public string Nombre { get; set; }
        public string Direccion { get; set; }
        public DateTime HoraInicio { get; set; }
        public DateTime HoraFin { get; set; }
        //public List<Empleado> Empleados { get; set; }
        public List<Libro> Libros { get; set; }

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
