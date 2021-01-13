using NUnit.Framework;
using Shop.Controllers;
using Microsoft.AspNetCore.Mvc;
using Shop.Models;
using Moq;
using Shop.Utilities.DBConnection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace NUnit.Coverlet.Collector
{
    public class RegisterControllerTests
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
                if(_config == null)
                {
                    var builder = new ConfigurationBuilder().AddJsonFile($"appsettings.json", optional: false);
                    _config = builder.Build();
                }

                return _config;
            }
        }

        [Test]
        public void RegisterUserReturnsTrue()
        {
            // Arrange
            var mockDB = new Mock<IDBConnectionAngelo>();
            mockDB.Setup(t => t.RegisterUser(It.IsAny<string>(), It.IsAny<Usuario>(), It.IsAny<Contacto>())).Returns(true);

            //var mockConfig = new Mock<IConfiguration>();
            //mockConfig.Setup(t => t.GetConnectionString(It.IsAny<string>())).Returns("TestConnection");

            var controller = new RegisterController(_config, mockDB.Object);

            JsonResult expected = new JsonResult(true);

            // Act
            var result = controller.RegistrarUsuario("Pedro", "Mejia", "00000000001", "809-442-4442", "pedromejia@gmail.com", "testing");
            
            // Assert
            Assert.AreEqual(expected.Value, result.Value);
        }

        [Test]
        public void RegisterUserReturnsFalse()
        {
            // Arrange
            var mockDB = new Mock<IDBConnectionAngelo>();
            mockDB.Setup(t => t.RegisterUser(It.IsAny<string>(), It.IsAny<Usuario>(), It.IsAny<Contacto>())).Throws(new System.Exception());


            var controller = new RegisterController(_config, mockDB.Object);

            JsonResult expected = new JsonResult(false);

            // Act  
            var result = controller.RegistrarUsuario("Pedro", "Mejia", "00000000001", "809-442-4442", "pedromejia@gmail.com", "testing");

            // Assert
            Assert.AreEqual(expected.Value, result.Value);
        }
    }
}