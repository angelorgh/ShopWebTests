using NUnit.Framework;
using Shop.Controllers;
using Microsoft.AspNetCore.Mvc;
using Shop.Models;
using Moq;
using Shop.Utilities.DBConnection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;
using System;

namespace NUnit.Coverlet.Collector
{
    public class ProfileControllerTests
    {
        private IConfiguration _config;

        [SetUp]
        public void Setup()
        {
            IServiceCollection services = new ServiceCollection();
            services.AddSingleton<IConfiguration>(Configuration);
        }

        public IConfiguration Configuration
        {
            get
            {
                if (_config == null)
                {
                    var builder = new ConfigurationBuilder().AddJsonFile($"appsettings.json", optional: false);
                    _config = builder.Build();
                }

                return _config;
            }
        }

        [Test]
        public void PopulateOrder_Calculates_Total_Correctly()
        {
            List<Orden> listaOrdenes = new List<Orden> 
            {  
                new Orden{ IDOrden = 1, FechaGen = DateTime.Now },
            };

            List<DetalleOrden> listaDetalleOrdenes = new List<DetalleOrden>
            {
                new DetalleOrden { Producto = new Producto {Precio = 110M}, Cantidad = 2 , Itebis = 37.4M},
                new DetalleOrden { Producto = new Producto {Precio = 220M}, Cantidad = 1 , Itebis = 37.4M},
            };

            var mockDB = new Mock<IDBConnectionAngelo>();
            mockDB.Setup(t => t.GetUserOrders(It.IsAny<string>(), It.IsAny<int>())).Returns(listaOrdenes);
            mockDB.Setup(t => t.GetUserOrdersDetails(It.IsAny<string>(), It.IsAny<int>())).Returns(listaDetalleOrdenes);

            var controller = new ProfileController(_config, mockDB.Object);

            var expected = new List<Orden> 
            {
                new Orden{ IDOrden = 1, FechaGen = DateTime.Now, SubTotal = 440, TotalItebis = 74.8M, Total = 514.8M },
            };

            var result = controller.PopulateOrder("teststring", 1);

            Assert.AreEqual(expected[0].SubTotal, result[0].SubTotal);
            Assert.AreEqual(expected[0].TotalItebis, result[0].TotalItebis);
            Assert.AreEqual(expected[0].Total, result[0].Total);


        }
    }
}
