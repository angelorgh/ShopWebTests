using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Models
{
    public class DetalleOrden
    {
        public int DetalleOrdenId { get; set; }
        public Orden Orden { get; set; }
        public Producto Producto { get; set; }
        public int Cantidad { get; set; }
        public decimal Itebis { get; set; }

        public DetalleOrden() { }
        public DetalleOrden(int id, Orden o, Producto p, int cantidad, decimal itebis) 
        {
            DetalleOrdenId = id;
            Orden = o;
            Producto = p;
            Cantidad = cantidad;
            Itebis = itebis;
        }
    }
}
