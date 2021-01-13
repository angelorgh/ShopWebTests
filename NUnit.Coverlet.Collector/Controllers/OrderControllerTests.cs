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
    public class OrderControllerTests
    {
        private IConfiguration _config;

        [SetUp]
        public void Setup()
        {
            IServiceCollection services = new ServiceCollection();
            services.AddSingleton<IConfiguration>(Configuration);
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
                value = BitConverter.GetBytes(1);
                return true;
            }
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
        public void Order_Returns_True_When_Order_IsDone_Succesfully()
        {
            var mockDB = new Mock<IDBConnectionAngelo>();
            mockDB.Setup(t => t.Order(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>())).Returns(true);

            var controller = new OrderController(_config, mockDB.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = new DefaultHttpContext()
                    {
                        Session = new DummySession()
                    }
                }
            };

            controller.ControllerContext.HttpContext.Request.Headers["device-id"] = "20317";

            JsonResult expected = new JsonResult(true);

            var result = controller.Order(1, "PayPal");

            Assert.AreEqual(expected.Value, result.Value);
        }

        [Test]
        public void Order_Returns_False_When_Order_IsDone_UnSuccesfully()
        {
            var mockDB = new Mock<IDBConnectionAngelo>();
            mockDB.Setup(t => t.Order(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>())).Returns(false);

            var controller = new OrderController(_config, mockDB.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = new DefaultHttpContext()
                    {
                        Session = new DummySession()
                    }
                }
            };

            controller.ControllerContext.HttpContext.Request.Headers["device-id"] = "20317";

            JsonResult expected = new JsonResult(false);

            var result = controller.Order(1, "PayPal");

            Assert.AreEqual(expected.Value, result.Value);
        }

        [Test]
        public void Order_Returns_False_When_Exception()
        {
            var mockDB = new Mock<IDBConnectionAngelo>();
            mockDB.Setup(t => t.Order(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>())).Throws(new System.Exception());

            var controller = new OrderController(_config, mockDB.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = new DefaultHttpContext()
                    {
                        Session = new DummySession()
                    }
                }
            };

            controller.ControllerContext.HttpContext.Request.Headers["device-id"] = "20317";

            JsonResult expected = new JsonResult(false);

            var result = controller.Order(1, "PayPal");

            Assert.AreEqual(expected.Value, result.Value);
        }

    }
}
