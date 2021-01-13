using System.Collections.Generic;
using System;

namespace Shop.Models
{
    public class Orden
    {
        public int IDOrden { get; set; }
        public EstadoOrden EstadoOrden { get; set; }
        public Direccion Direccion { get; set; }
        public Cliente Cliente { get; set; }
        public DateTime FechaGen { get; set; }
        public decimal TotalItebis { get; set; }
        public decimal SubTotal { get; set; }
        public decimal Total { get; set; }
        public List<DetalleOrden> detalles { get; set; }
    }
}