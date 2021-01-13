using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Shop.Utilities;
using Shop.Models;
using Shop.Utilities.DBConnection;

namespace Shop.Controllers
{
    public class CartController : Controller
    {
        private static IConfiguration _config;
        private readonly IDBConnectionAngelo _dBConnectionAngelo;
        private readonly string connectionstring;

        public CartController(IConfiguration config, IDBConnectionAngelo dBConnectionAngelo)
        {
            _config = config;
            _dBConnectionAngelo = dBConnectionAngelo;
            connectionstring = _config.GetConnectionString("AngeloConnection");
        }
        //private static string connectionstring = _config.GetConnectionString("AngeloConnection");
        
        // GET
        public IActionResult CartPage()
        {
            string connectionstr = _config.GetConnectionString("AngeloConnection");
            bool isUserConnected = string.IsNullOrEmpty(HttpContext.Session.GetString("Username"));
            
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
            return View();
        }

        public IActionResult AddToCart(int idproducto, int cantidad = 1)
        {
            string connectionstr = _config.GetConnectionString("AngeloConnection");

            Producto producto = DBConnectionAngelo.GetProductByID(connectionstr,idproducto);

            bool isUserConnected = string.IsNullOrEmpty(HttpContext.Session.GetString("Username"));
            
            if (isUserConnected)
            {
                if (SessionHelper.GetObjectFromJson<List<CarritoItem>>(HttpContext.Session,"cart") == null)
                {
                    List<CarritoItem> cart = new List<CarritoItem>();
                    cart.Add(new CarritoItem{Producto = DBConnectionAngelo.GetProductByID(connectionstr,idproducto), Cantidad = 1});
                    SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
                }
                else
                {
                    List<CarritoItem> cart =
                        SessionHelper.GetObjectFromJson<List<CarritoItem>>(HttpContext.Session, "cart");
                    int index = IsExist(idproducto);
                    if (index != -1)
                    {
                        cart[index].Cantidad++;
                    }
                    else
                    {
                        cart.Add(new CarritoItem{Producto = DBConnectionAngelo.GetProductByID(connectionstr,idproducto),Cantidad = 1});
                    }
                    SessionHelper.SetObjectAsJson(HttpContext.Session, "cart",cart);
                }
            }
            else
            {
                int idcliente = Convert.ToInt32(HttpContext.Session.GetInt32("IDCliente"));
                bool result = DBConnectionAngelo.AddCarritoItem(connectionstr, idcliente, idproducto, cantidad);
                var cart = _dBConnectionAngelo.GetUserShoppingCart(connectionstr, idcliente);
                SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
            }

            return Redirect("CartPage");
        }

        public IActionResult RemoveFromCart(int idproducto, int cantidad = 1)
        {
            string connectionstr = _config.GetConnectionString("AngeloConnection");
            bool isUserConnected = string.IsNullOrEmpty(HttpContext.Session.GetString("Username"));

            if (isUserConnected)
            {
                List<CarritoItem> cart = SessionHelper.GetObjectFromJson<List<CarritoItem>>(HttpContext.Session, "cart");
                int index = IsExist(idproducto);
                cart.RemoveAt(index);
                SessionHelper.SetObjectAsJson(HttpContext.Session, "cart",cart);
            }
            else
            {
                int idcliente = Convert.ToInt32(HttpContext.Session.GetInt32("IDCliente"));
                bool result = DBConnectionAngelo.DeleteCarritoItem(connectionstr, idcliente, idproducto, cantidad);
                var cart = _dBConnectionAngelo.GetUserShoppingCart(connectionstr, idcliente);
                SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
            }

            return Redirect("CartPage");
        }

        private int IsExist(int id)
        {
            List<CarritoItem> cart =
                SessionHelper.GetObjectFromJson<List<CarritoItem>>(HttpContext.Session, "cart");
            for (int i = 0; i < cart.Count; i++)
            {
                if (cart[i].Producto.ProductoID.Equals(id))
                {
                    return i;
                }
            }
            return -1;
        }
    }
}