using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shop.Models;

namespace Shop.Utilities
{
    public class SessionHelper
    {
        public static void SetObjectAsJson(ISession session, string key, object value)
        {
            session.SetString(key,JsonConvert.SerializeObject(value));
        }

        public static T GetObjectFromJson<T>(ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
        }

        public static int GetNumberOfItemsInCart(List<CarritoItem> carrito)
        {
            int count = 0;

            foreach (var item in carrito)
            {
                count += item.Cantidad;
            }

            return count;
        }

        public static decimal GetTotalPriceOfCart(List<CarritoItem> carrito)
        {
            decimal count = 0;

            foreach (var item in carrito)
            {
                count += (item.Cantidad * item.Producto.Precio);
            }

            return count;
        }
        
        public static decimal GetTotalITBISOfCart(List<CarritoItem> carrito)
        {
            decimal count = 0;

            foreach (var item in carrito)
            {
                count += (item.ITBIS);
            }

            return count;
        }
    }
}