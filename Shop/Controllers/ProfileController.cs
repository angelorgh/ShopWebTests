using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Shop.Models;
using Shop.Utilities.DBConnection;
using System;
using System.Collections.Generic;

namespace Shop.Controllers
{
    public class ProfileController : Controller
    {
        private static IConfiguration _config;
        private readonly IDBConnectionAngelo _dBConnectionAngelo;
        private readonly string connectionstring;

        public ProfileController(IConfiguration config, IDBConnectionAngelo dBConnectionAngelo)
        {
            _config = config;
            _dBConnectionAngelo = dBConnectionAngelo;
            connectionstring = _config.GetConnectionString("AngeloConnection");
        }
        //private static string connectionstring = _config.GetConnectionString("AngeloConnection");

        // GET
        public IActionResult ProfilePage()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("Username")))
            {
                return RedirectToAction("LoginPage", "Login");
            }
            else
            {
                return View();
            }
        }
        public IActionResult DirectionPage()
        {
            string connectionstr = _config.GetConnectionString("AngeloConnection");
            int idcliente = Convert.ToInt32(HttpContext.Session.GetInt32("IDCliente"));
            ViewBag.Direcciones = DBConnectionAngelo.GetUserDirecciones(connectionstr, idcliente);
            return View();
        }
        public IActionResult AddDirectionPage()
        {
            return View();
        }
        public IActionResult UpdateDirectionPage(int idDireccion)
        {
            string connectionstr = _config.GetConnectionString("AngeloConnection");
            ViewBag.Direccion = DBConnectionAngelo.GetDireccion(connectionstr, idDireccion);
            return View();
        }
        public IActionResult UserOrdersPage()
        {
            string connectionstr = _config.GetConnectionString("AngeloConnection");
            int idcliente = Convert.ToInt32(HttpContext.Session.GetInt32("IDCliente"));
            ViewBag.Ordenes = PopulateOrder(connectionstr, idcliente);
            return View();
        }
        
        public List<Orden> PopulateOrder(string connectionstr,int idcliente)
        {
            List<Orden> ordens = _dBConnectionAngelo.GetUserOrders(connectionstr, idcliente);
            List<DetalleOrden> detalleOrdenes = new List<DetalleOrden>();
            foreach (Orden orden in ordens)
            {
                decimal subtotal = 0, totalimp = 0, total = 0;
                orden.detalles = _dBConnectionAngelo.GetUserOrdersDetails(connectionstr, orden.IDOrden);
                foreach (DetalleOrden detalles in orden.detalles)
                {
                    subtotal += detalles.Producto.Precio;
                    totalimp += detalles.Itebis;
                }
                total = subtotal + totalimp;
                orden.SubTotal = subtotal;
                orden.TotalItebis = totalimp;
                orden.Total = total;
            }
            return ordens;
        }

        public JsonResult EliminarDireccion(int idDireccion)
        {
            string connectionstr = _config.GetConnectionString("AngeloConnection");

            try
            {
                bool result = DBConnectionAngelo.DeleteDirection(connectionstr, idDireccion);
                return Json(result);
            }
            catch (Exception)
            {
                return Json(false);
            }
        }
        public JsonResult AgregarDireccion(string _provincia, string _ciudad, string _direccion1, string _direccion2)
        {
            string connectionstr = _config.GetConnectionString("AngeloConnection");
            int idcliente = Convert.ToInt32(HttpContext.Session.GetInt32("IDCliente"));

            try
            {
                bool result = DBConnectionAngelo.AddDirection(connectionstr, idcliente, _provincia, _ciudad, _direccion1, _direccion2);
                
                return Json(result);
            }
            catch (Exception)
            {
                return Json(false);
            }
        }
        public JsonResult ActualizarDireccion(int _id, string _provincia, string _ciudad, string _direccion1, string _direccion2)
        {
            string connectionstr = _config.GetConnectionString("AngeloConnection");

            try
            {
                bool result = DBConnectionAngelo.UpdateDirection(connectionstr, _id, _provincia, _ciudad, _direccion1, _direccion2);

                return Json(result);
            }
            catch (Exception)
            {
                return Json(false);
            }
        }
    }
}