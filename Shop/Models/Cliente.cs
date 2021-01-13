using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Models
{
    public class Cliente
    {
        public int ClienteID { get; set; }
        public Usuario Usuario { get; set; }
        public Contacto Contacto { get; set; }

        public Cliente() { }
    }
}
