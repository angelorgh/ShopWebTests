using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shop.Utilities.DBConnection;
using Microsoft.Extensions.Configuration;
using Shop.Models;
using Shop.Utilities;

namespace Shop.Controllers
{
    public class LoginController : Controller
    {
        private static IConfiguration _config;
        private readonly IDBConnectionAngelo _dBConnectionAngelo;
        private readonly string connectionstring;

        public LoginController(IConfiguration config, IDBConnectionAngelo dBConnectionAngelo)
        {
            _config = config;
            _dBConnectionAngelo = dBConnectionAngelo;
            connectionstring = _config.GetConnectionString("AngeloConnection");
        }
        //private static string connectionstring = _config.GetConnectionString("AngeloConnection");

        public IActionResult LoginPage()
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

        public ActionResult LogOut()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("MainPage", "Home");
        }

        public JsonResult VerificarLogin(string nombreusuario, string password) {
            string connectionstr = _config.GetConnectionString("AngeloConnection");
            Usuario user = new Usuario() { NombreUsuario = nombreusuario, Contraseña = password };
            
            try
            {
                bool result = _dBConnectionAngelo.VerifyLogin(connectionstr, user);
                
                if (result == true)
                {
                    Cliente cliente = _dBConnectionAngelo.GetUserInformation(connectionstr, user.NombreUsuario);
                    HttpContext.Session.SetString("Username",user.NombreUsuario);
                    HttpContext.Session.SetString("Nombre",cliente.Contacto.Nombre);
                    HttpContext.Session.SetString("Apellido",cliente.Contacto.Apellido);
                    HttpContext.Session.SetInt32("IDCliente",cliente.ClienteID);
                    HttpContext.Session.SetInt32("IDUsuario", cliente.Usuario.UsuarioID);
                    List<CarritoItem> cart = _dBConnectionAngelo.GetUserShoppingCart(connectionstr, cliente.ClienteID);
                    SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
                }
                return Json(result);
            }
            catch (Exception exception)
            {
                Exception ex = exception;
                return Json(false);
            }
        }

    }
}
