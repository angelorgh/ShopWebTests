using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Shop.Models;
using Microsoft.Extensions.Configuration;
using Shop.Utilities.DBConnection;
using Shop.Utilities;
using System.Data.SqlClient;
using System.Data;

namespace Shop.Controllers
{
    public class HomeController : Controller
    {
        /*private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }*/
        
        private static IConfiguration _config;

        public HomeController(IConfiguration config)
        {
            _config = config;
        }
        private static string connectionstring = _config.GetConnectionString("AngeloConnection");

        private const string SessionName = "Username";

       
        public PartialViewResult MenuBarHome()
        {
            string connectionstr = _config.GetConnectionString("AngeloConnection");

            List<Categoria> catergorias = DBConnectionGabriel.getCategoria(connectionstr);
            return PartialView(catergorias);
        }

        public IActionResult MainPage()
        {
            string connectionstr = _config.GetConnectionString("AngeloConnection");

            List<Producto> productos = DBConnectionGabriel.getPopularProductos(connectionstr, 8);
            List<Categoria> catergorias = DBConnectionGabriel.getCategoria(connectionstr);

            ProductoCategoria productoCategoria = new ProductoCategoria(productos, catergorias);
            return View(productoCategoria);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
