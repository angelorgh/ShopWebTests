namespace ShopAdminWeb.Models
{
    public class Pago
    {
        public int IDPago { get; set; }
        public Cliente Cliente { get; set; }
        public decimal Monto { get; set; }
    }
}