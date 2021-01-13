using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Shop.Utilities.DBConnection;
using Shop.Models;
using System.Data;

namespace Shop.Controllers
{
    public class SearchController : Controller
    {
        private static string connectionstring = "Data Source=ditribution-server.database.windows.net;Initial Catalog=DBlogistica;User ID=johnduran;Password=Nji9mko0";
        public static CategoriaMarcaProducto categoriaMarcaProducto;
        public ActionResult SearchPage(int identificador, string nombre_categoria, string searchvalue, string min_value, string max_value)
        {
            if (searchvalue == null)
            {
                categoriaMarcaProducto = PopulateModelView(identificador, nombre_categoria, min_value, max_value);
            }
            else
            {
                categoriaMarcaProducto = PopulateModelView(searchvalue, min_value, max_value);
            }
            return View(categoriaMarcaProducto);
        }
        public CategoriaMarcaProducto PopulateModelView (int idCategoria, string NombreCategoria, string min_value, string max_value)
        {
            DataTable dbMarca = DBConnectionGabriel.getMarca(connectionstring);
            DataTable dbProducto = new DataTable();
            if (min_value != null)
            {
                Decimal minvalue = Convert.ToDecimal(min_value);
                Decimal maxvalue = Convert.ToDecimal(max_value);
                dbProducto = DBConnectionGabriel.getProductsByCategoriaandPriceRange(connectionstring, idCategoria, minvalue, maxvalue);
            }
            else
            {
                dbProducto = DBConnectionGabriel.getProductsByCategoria(connectionstring, idCategoria);
            }
            

            Categoria categoria = new Categoria(idCategoria, NombreCategoria);
            List<Producto> productos = PopulateModelProducto(dbProducto);
            List<Marca> marcas = PopulateModelMarca(dbMarca);

            CategoriaMarcaProducto categoriaMarcaProducto = new CategoriaMarcaProducto(categoria, productos, marcas);
            return categoriaMarcaProducto;
        }
        public CategoriaMarcaProducto PopulateModelView(string searchvalue, string min_value, string max_value)
        {
            DataTable dbMarca = DBConnectionGabriel.getMarca(connectionstring);
            DataTable dbProducto = new DataTable();
            if (min_value != null)
            {
                Decimal minvalue = Convert.ToDecimal(min_value);
                Decimal maxvalue = Convert.ToDecimal(max_value);
                dbProducto = DBConnectionGabriel.searchProductoCategoriaMarca(connectionstring, searchvalue, minvalue, maxvalue, true);
            }
            else
            {
                dbProducto = DBConnectionGabriel.searchProductoCategoriaMarca(connectionstring, searchvalue, 0, 0);
            }

            Categoria categoria = PopulateModelCategoria(dbProducto);
            List<Producto> productos = PopulateModelProducto(dbProducto);
            List<Marca> marcas = PopulateModelMarca(dbMarca);

            CategoriaMarcaProducto categoriaMarcaProducto = new CategoriaMarcaProducto(categoria, productos, marcas);
            return categoriaMarcaProducto;
        }
        
        public static List<Marca> PopulateModelMarca(DataTable dt)
        {
            List<Marca> marcas = new List<Marca>();
            foreach (DataRow dr in dt.Rows)
            {
                marcas.Add(
                    new Marca
                    {
                        MarcaID = Convert.ToInt32(dr["IDMarcaProducto"]),
                        Nombre = Convert.ToString(dr["MarcaProducto"]),
                        FechaRegistro = Convert.ToDateTime(dr["FechaRegistro"])
                    });
            }
            return marcas;
        }
        public static Categoria PopulateModelCategoria(DataTable dt)
        {
            Categoria categoria = new Categoria();
            foreach (DataRow dr in dt.Rows)
            {
                categoria = new Categoria(Convert.ToInt32(dr["IDCategoriaProducto"]), Convert.ToString(dr["CategoriaProducto"]));
            }
            return categoria;
        }
        public static List<Producto> PopulateModelProducto(DataTable dt)
        {
            List<Producto> productos = new List<Producto>();
            foreach (DataRow dr in dt.Rows)
            {
                productos.Add(
                    new Producto
                    {
                        ProductoID = Convert.ToInt32(dr["IDProducto"]),
                        Nombre = Convert.ToString(dr["Nombre"]),
                        Image = "~/images/Imagenes/" + Convert.ToString(Convert.ToInt32(dr["IDProducto"])) + ".jpg",
                        Precio = Convert.ToDecimal(dr["Precio"]),
                        FechaRegistro = Convert.ToDateTime(dr["FechaRegistro"]),
                        Descripcion = Convert.ToString(dr["Descripcion"])
                    });
            }
            return productos;
        }
        
    }
}
