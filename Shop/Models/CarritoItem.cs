

namespace Shop.Models
{
    public class CarritoItem
    {
        public int IDCarritoItem { get; set; }
        public Cliente Cliente { get; set; }
        public Producto Producto { get; set; }
        public int Cantidad { get; set; } = 0;
        public decimal ITBIS { get; set; }

    }
}