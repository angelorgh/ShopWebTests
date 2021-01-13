namespace ShopAdminWeb.Models
{
    public class Direccion
    {
        public int IDDireccion { get; set; }
        public Cliente cliente { get; set; }
        public string Direccion1 { get; set; }
        public string Direccion2 { get; set; }
        public string Provincia { get; set; }
        public string Ciudad { get; set; }

        public Direccion() { }
        public Direccion(int direccionid, Cliente client, string direccion1, string direccion2, string provincia, string ciudad) 
        {
            IDDireccion = direccionid;
            cliente = client;
            Direccion1 = direccion1;
            Direccion2 = direccion2;
            Provincia = provincia;
            Ciudad = ciudad;
        }
    }

}