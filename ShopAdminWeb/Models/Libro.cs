using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopAdminWeb.Models
{
    public class Libro
    {
        public int IDLibro { get; set; }
        public Proveedor Proveedor { get; set; }
        public Sucursal Sucursal { get; set; }
        public string Genero { get; set; }
        public string Autor { get; set; }
        public string Tipo { get; set; }
        public string Descripcion { get; set; }
        public int CantidadDisponible { get; set; }
        public bool Estado { get; set; }
        public double Precio { get; set; }
        public string Resena { get; set; }

        public Libro(){
        }

        public Libro(string autor, int cantidad, string tipo)
        {
            Autor = autor;
            CantidadDisponible = cantidad;
            Tipo = tipo;
        }
    }
}
