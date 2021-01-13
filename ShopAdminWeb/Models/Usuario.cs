using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopAdminWeb.Models
{
    public class Usuario
    {
        public int UsuarioID { get; set; }
        public Contacto Contacto { get; set; }
        public string NombreUsuario { get; set; }
        public string Contraseña { get; set; }
        public DateTime FechaRegistro { get; set; }
    }
}
