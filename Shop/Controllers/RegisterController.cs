using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Shop.Models;
using Shop.Utilities.DBConnection;

namespace Shop.Controllers
{
    public class RegisterController : Controller
    {
        private static IConfiguration _config;
        private readonly IDBConnectionAngelo _dBConnectionAngelo;
        private readonly string connectionstring;

        public RegisterController(IConfiguration config, IDBConnectionAngelo dBConnectionAngelo)
        {
            _config = config;
            _dBConnectionAngelo = dBConnectionAngelo;
            connectionstring = _config.GetConnectionString("AngeloConnection");
        }

        //private static string connectionstring = "Data Source = ANGELORGHDESK\\SQLEXPRESS;Initial Catalog = TmpShopDB; Integrated Security = True";

        public IActionResult RegisterPage()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("Username")))
            {
                return View();
            }
            else
            {
                return RedirectToAction("MainPage", "Home");
            }
        }

        public JsonResult RegistrarUsuario(string nombre, string apellido,
                                            string cedula, string telefono, string email, string password)
        {
            string connectionstr = _config.GetConnectionString("AngeloConnection");
            Contacto contact = new Contacto() { Nombre = nombre, Apellido = apellido, Cedula = cedula, Telefono = telefono, Email = email };
            Usuario user = new Usuario() { NombreUsuario = email, Contraseña = password };

            try
            {
                bool result = _dBConnectionAngelo.RegisterUser(connectionstr, user, contact);
                return Json(result);
            }
            catch (Exception ex)
            {
                Exception exc = ex;
                return Json(false);
            }

        }

        

    }
}
