using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Shop.Models;

namespace Shop.Utilities.DBConnection
{
    public class DBConnectionGabriel
    {
        private static SqlConnection con;

        private static void connection(string connectionstring)
        {
            con = new SqlConnection(connectionstring);
        }


        public static List<Producto> getPopularProductos(string connectionstring, int cantidad)
        {
            connection(connectionstring);
            SqlCommand cmd = new SqlCommand("pp_selectlastproducts", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@cantidad", cantidad);
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            con.Open();
            sd.Fill(dt);
            con.Close();

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
        
        public static List<Categoria> getCategoria(string connectionstring)
        {
            connection(connectionstring);
            SqlCommand cmd = new SqlCommand("pp_selectcategoria", con);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            con.Open();
            sd.Fill(dt);
            con.Close();

            List<Categoria> categorias = new List<Categoria>();
            foreach (DataRow dr in dt.Rows)
            {
                categorias.Add(
                    new Categoria
                    {
                        CategoriaID = Convert.ToInt32(dr["IDCategoriaProducto"]),
                        Nombre = Convert.ToString(dr["CategoriaProducto"]),
                        FechaRegistro = Convert.ToDateTime(dr["FechaRegistro"])
                    });
            }
            return categorias;
            

        }
        public static DataTable getMarca(string connectionstring)
        {
            connection(connectionstring);
            SqlCommand cmd = new SqlCommand("pp_selectmarca", con);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            con.Open();
            sd.Fill(dt);
            con.Close();
            return dt;

        }
        public static DataTable getProductsByCategoria(string connectionstring, int idcategoria)
        {
            connection(connectionstring);
            SqlCommand cmd = new SqlCommand("pp_selectproductbycategoria", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@idcategoria", idcategoria);

            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            con.Open();
            sd.Fill(dt);
            con.Close();
            return dt;

        }
        public static DataTable getProductsByCategoriaandPriceRange(string connectionstring, int idcategoria, decimal minvalue, decimal maxvalue)
        {
            connection(connectionstring);
            SqlCommand cmd = new SqlCommand("pp_selectproductbypriceandcategory", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@idcategoria", idcategoria);
            cmd.Parameters.AddWithValue("@menor", minvalue);
            cmd.Parameters.AddWithValue("@mayor", maxvalue);

            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            con.Open();
            sd.Fill(dt);
            con.Close();
            return dt;
        }
        
        public static DataTable searchProductoCategoriaMarca(string connectionstring, string searchvalue, decimal minvalue, decimal maxvalue, bool princerange = false)
        {
            connection(connectionstring);
            SqlCommand cmd = new SqlCommand("pp_selectproductbyname", con);
            cmd.CommandType = CommandType.StoredProcedure;
            if (princerange)
            {
                cmd.Parameters.AddWithValue("@menor", minvalue);
                cmd.Parameters.AddWithValue("@mayor", maxvalue);
            }
            cmd.Parameters.AddWithValue("@busqueda", searchvalue);

            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            con.Open();
            sd.Fill(dt);
            con.Close();
            return dt;
        }
    }
}
