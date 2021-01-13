using NUnit.Framework;
using Shop.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Shop.Models;
using Moq;
using Shop.Utilities.DBConnection;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using System.Threading;

namespace NUnit.Coverlet.Collector
{
    public class LoginControllerTests
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
        public void VerifyLoginReturnsTrue()
        {
            // Arrange

            Cliente cliente = new Cliente();
            cliente.ClienteID = 23;
            Contacto contacto = new Contacto();
            contacto.Nombre = "Juan";
            contacto.Apellido = "Emilio";
            Usuario usuario = new Usuario();
            usuario.UsuarioID = 1;
            cliente.Contacto = contacto;
            cliente.Usuario = usuario;

            var mockDB = new Mock<IDBConnectionAngelo>();
            mockDB.Setup(t => t.VerifyLogin(It.IsAny<string>(),It.IsAny<Usuario>())).Returns(true);
            mockDB.Setup(t => t.GetUserInformation(It.IsAny<string>(), 
                It.IsAny<string>())).Returns(cliente);

            var controller = new LoginController(_config, mockDB.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = new DefaultHttpContext()
                    {
                        Session = new DummySession(),
                    }
                }
            };

            controller.ControllerContext.HttpContext.Request.Headers["device-id"] = "20317";

            JsonResult expected = new JsonResult(true);

            // Act
            var result = controller.VerificarLogin("angelo@test.com", "12345");

            // Assert
            Assert.AreEqual(expected.Value, result.Value);
        }

        [Test]
        public void VerifyLoginReturnsFalse()
        {
            // Arrange

            Cliente cliente = new Cliente();
            cliente.ClienteID = 23;
            Contacto contacto = new Contacto();
            contacto.Nombre = "Juan";
            contacto.Apellido = "Emilio";
            Usuario usuario = new Usuario();
            usuario.UsuarioID = 1;
            cliente.Contacto = contacto;
            cliente.Usuario = usuario;

            var mockDB = new Mock<IDBConnectionAngelo>();
            mockDB.Setup(t => t.VerifyLogin(It.IsAny<string>(), It.IsAny<Usuario>())).Throws(new System.Exception());
            mockDB.Setup(t => t.GetUserInformation(It.IsAny<string>(),
                It.IsAny<string>())).Returns(cliente);

            //var mockConfig = new Mock<IConfiguration>();
            //mockConfig.Setup(t => t.GetConnectionString(It.IsAny<string>())).Returns("TestConnection");

            //var httpContext = new DefaultHttpContext();
            //httpContext.Request.Headers["X-Custom-Header"] = "88-test-tcb";


            var controller = new LoginController(_config, mockDB.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = new DefaultHttpContext()
                    {
                        Session = new DummySession(),
                    }
                }
            };

            controller.ControllerContext.HttpContext.Request.Headers["device-id"] = "20317";

            JsonResult expected = new JsonResult(false);

            // Act
            var result = controller.VerificarLogin("angelo@test.com", "12345");

            // Assert
            Assert.AreEqual(expected.Value, result.Value);
        }
    }

    public class DummySession : ISession
    {
        Dictionary<string, object> sessionStorage = new Dictionary<string, object>();

        string ISession.Id => throw new System.NotImplementedException();

        bool ISession.IsAvailable => throw new System.NotImplementedException();

        IEnumerable<string> ISession.Keys
        {
            get { return sessionStorage.Keys; }
        }

        void ISession.Clear()
        {
            throw new System.NotImplementedException();
        }

        Task ISession.CommitAsync(CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        Task ISession.LoadAsync(CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        void ISession.Remove(string key)
        {
            throw new System.NotImplementedException();
        }

        void ISession.Set(string key, byte[] value)
        {
            sessionStorage[key] = value;
        }

        void SetString(string key, string value)
        {
            
        }

        void SetInt32(string key, int value)
        {

        }

        bool ISession.TryGetValue(string key, out byte[] value)
        {
            throw new System.NotImplementedException();
        }
    }
}
