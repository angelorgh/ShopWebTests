using NUnit.Framework;
using ShopAdminWeb.Controllers;
using ShopAdminWeb.Utilities;
using ShopAdminWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;

using System.Collections.Generic;

using System;


namespace NUnit.Coverlet.MSBuild
{
    public class ConsultarControllerTests 
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void EditarProductoActionResult_Returns_ViewResult_When_ProductID_IsProvided() 
        {
            Producto producto = new Producto();
            producto.ProductoID = 1;
            producto.Nombre = "MSI NVIDIA RTX 3080 SuperClocked";
            producto.Descripcion = "Tarjeta grafica de ultima generacion NVIDIA";
            producto.FechaRegistro = DateTime.Now;
            producto.Precio = 1099.99;

            EstadoProducto estadoProducto = new EstadoProducto();
            estadoProducto.EstadoProductoID = 1;
            estadoProducto.Nombre = "Disponible";
            estadoProducto.FechaRegistro = DateTime.Now;

            Categoria categoria = new Categoria();
            categoria.CategoriaID = 1;
            categoria.Nombre = "Tarjetas Graficas";
            categoria.FechaRegistro = DateTime.Now;

            Marca marca = new Marca();
            marca.MarcaID = 1;
            marca.Nombre = "MSI";
            marca.FechaRegistro = DateTime.Now;

            producto.Estado = estadoProducto;
            producto.Categoria = categoria;
            producto.Marca = marca;

            List<Marca> listaMarcas = new List<Marca> 
            {
                new Marca{ MarcaID = 1 , Nombre = "MSI", FechaRegistro = DateTime.Now },
                new Marca{ MarcaID = 2 , Nombre = "ASUS", FechaRegistro = DateTime.Now },
                new Marca{ MarcaID = 3 , Nombre = "AMD", FechaRegistro = DateTime.Now },
                new Marca{ MarcaID = 4 , Nombre = "INTEL", FechaRegistro = DateTime.Now },
            };

            List<Categoria> listaCategorias = new List<Categoria>
            {
                new Categoria{CategoriaID = 1, Nombre = "Tarjetas Graficas", FechaRegistro = DateTime.Now },
                new Categoria{CategoriaID = 2, Nombre = "Monitores", FechaRegistro = DateTime.Now },
                new Categoria{CategoriaID = 3, Nombre = "Procesadores", FechaRegistro = DateTime.Now },
                new Categoria{CategoriaID = 4, Nombre = "Laptops", FechaRegistro = DateTime.Now },
                new Categoria{CategoriaID = 5, Nombre = "Teclados", FechaRegistro = DateTime.Now },
            };

            List<EstadoProducto> listaEstados = new List<EstadoProducto>
            {
                new EstadoProducto {EstadoProductoID = 1, Nombre = "Disponible", FechaRegistro = DateTime.Now},
                new EstadoProducto {EstadoProductoID = 2, Nombre = "Agotado", FechaRegistro = DateTime.Now},
                new EstadoProducto {EstadoProductoID = 3, Nombre = "En Transito", FechaRegistro = DateTime.Now},
            };

            var mockDB = new Mock<IDBConnection>();
            mockDB.Setup(t => t.GetProductByID(It.IsAny<int>())).Returns(producto);
            mockDB.Setup(t => t.GetAllMarcas()).Returns(listaMarcas);
            mockDB.Setup(t => t.GetAllCategorias()).Returns(listaCategorias);
            mockDB.Setup(t => t.GetAllEstadoProducto()).Returns(listaEstados);

            var controller = new ConsultarController(mockDB.Object);

            var expected = new ViewResult();

            var result = controller.EditarProducto(3);

            //Assert.AreEqual(producto, result.Model);
            Assert.IsAssignableFrom<ViewResult>(result);

            //Assert.AreEqual(expected, result);
        }

        [Test]
        public void EditarProductoActionResult_Returns_RedirectToActionResult_When_ProductID_IsNotProvided() 
        {
            var mockDB = new Mock<IDBConnection>();
            mockDB.Setup(t => t.GetProductByID(It.IsAny<int>())).Returns(new Producto());
            mockDB.Setup(t => t.GetAllMarcas()).Returns(new List<Marca>());
            mockDB.Setup(t => t.GetAllCategorias()).Returns(new List<Categoria>());
            mockDB.Setup(t => t.GetAllEstadoProducto()).Returns(new List<EstadoProducto>());

            var controller = new ConsultarController(mockDB.Object);

            var expected = new ViewResult();

            var result = controller.EditarProducto(null);

            //Assert.AreEqual(producto, result.Model);
            Assert.IsAssignableFrom<RedirectToActionResult>(result);

            //Assert.AreEqual(expected, result);
        }

        [Test]
        public void EditarProductoJsonResult_Returns_True_When_Update_IsDone_Correctly()
        {
            var mockDB = new Mock<IDBConnection>();
            mockDB.Setup(t => t.UpdateProducto(It.IsAny<int>(),It.IsAny<int>(),It.IsAny<int>(), 
                It.IsAny<int>(),It.IsAny<string>(),It.IsAny<string>(),It.IsAny<double>())).Returns(true);

            var controller = new ConsultarController(mockDB.Object);

            var expected = new JsonResult(true);

            var result = controller.EditarProducto(1, 1, 1, 1, "MSI NVIDIA 3070 SuperClocked Mega", "Nuevo producto de NVIDIA", 999.99);

            Assert.AreEqual(expected.Value, result.Value);
        }

        [Test]
        public void EditarProductoJsonResult_Returns_False_When_Update_IsDone_InCorrectly()
        {
            var mockDB = new Mock<IDBConnection>();
            mockDB.Setup(t => t.UpdateProducto(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(),
                It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<double>())).Returns(false);

            var controller = new ConsultarController(mockDB.Object);

            var expected = new JsonResult(false);

            var result = controller.EditarProducto(1, 1, 1, 1, "MSI NVIDIA 3070 SuperClocked Mega", "Nuevo producto de NVIDIA", 999.99);

            Assert.AreEqual(expected.Value, result.Value);
        }

        [Test]
        public void EditarProductoJsonResult_Returns_False_When_Exception()
        {
            var mockDB = new Mock<IDBConnection>();
            mockDB.Setup(t => t.UpdateProducto(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(),
                It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<double>())).Throws(new System.Exception());

            var controller = new ConsultarController(mockDB.Object);

            var expected = new JsonResult(false);

            var result = controller.EditarProducto(1, 1, 1, 1, "MSI NVIDIA 3070 SuperClocked Mega", "Nuevo producto de NVIDIA", 999.99);

            Assert.AreEqual(expected.Value, result.Value);
        }


    }
}
