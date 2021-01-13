using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Shop.Models;
using Shop.Utilities;
using Shop.Utilities.DBConnection;

namespace Shop.Controllers
{
    public class OrderController : Controller
    {
        private static IConfiguration _config;
        private readonly IDBConnectionAngelo _dBConnectionAngelo;
        private readonly string connectionstring;


        public OrderController(IConfiguration config, IDBConnectionAngelo dBConnectionAngelo)
        {
            _config = config;
            _dBConnectionAngelo = dBConnectionAngelo;
            connectionstring = _config.GetConnectionString("AngeloConnection");
        }
        //private static string connectionstring = _config.GetConnectionString("AngeloConnection");

        public IActionResult OrderPage()
        {
            string connectionstr = _config.GetConnectionString("AngeloConnection");

            var cart = SessionHelper.GetObjectFromJson <List<CarritoItem>>(HttpContext.Session, "cart");

            if (cart == null)
            {
                var cart2 = new List<CarritoItem>();
                SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart2);
                ViewBag.Cart = cart2;
                ViewBag.Total = 0;
            }
            else
            {
                ViewBag.Cart = cart;
                ViewBag.Total = cart.Sum(item => item.Producto.Precio * item.Cantidad);
            }

            int idcliente = Convert.ToInt32(HttpContext.Session.GetInt32("IDCliente"));
            ViewBag.Direcciones = DBConnectionAngelo.GetUserDirecciones(connectionstr, idcliente);
            return View();
        }

        public JsonResult Order(int iddireccion, string metodopago)
        {
            string connectionstr = _config.GetConnectionString("AngeloConnection");

            try
            {
                int idusuario = Convert.ToInt32(HttpContext.Session.GetInt32("IDUsuario"));
                bool result = _dBConnectionAngelo.Order(connectionstr, idusuario, iddireccion);
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