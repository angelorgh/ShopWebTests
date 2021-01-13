using ShopAdminWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace ShopAdminWeb.Utilities
{
    public interface IDBConnection
    {
        public IEnumerable<Producto> GetProductos();
        public Producto GetProductByID(int id);
        public List<Marca> GetAllMarcas();
        public List<Categoria> GetAllCategorias();
        public List<EstadoProducto> GetAllEstadoProducto();
        public bool UpdateProducto(int productoid, int marcaid, int categoriaid, int estadoid, string nombre, string descripcion, double precio);
    }

    public class DBConnection : IDBConnection
    {
        private static readonly string connectionstring = "Data Source=ditribution-server.database.windows.net;Initial Catalog=DBlogistica;Persist Security Info=True;User ID=johnduran;Password=Nji9mko0";

        private static SqlConnection con;

        private static void connection(string connectionstring)
        {
            con = new SqlConnection(connectionstring);
        }

        public IEnumerable<Producto> GetProductos()
        {
            connection(connectionstring);

            //string querystring = "SELECT * FROM tblProducto";

            SqlCommand cmd = new SqlCommand("pp_selectproductbyall", con);
            cmd.CommandType = CommandType.StoredProcedure;
            //cmd.Parameters.AddWithValue("@idcliente");

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
                        Descripcion = Convert.ToString(dr["Descripcion"]),
                        Precio = Convert.ToDouble(dr["Precio"]),
                        FechaRegistro = Convert.ToDateTime(dr["FechaRegistro"]),
                        Marca = new Marca
                        {
                            MarcaID = Convert.ToInt32(dr["IDMarcaProducto"]),
                            Nombre = Convert.ToString(dr["MarcaProducto"])
                        },
                        Categoria = new Categoria
                        {
                            CategoriaID = Convert.ToInt32(dr["IDCategoriaProducto"]),
                            Nombre = Convert.ToString(dr["CategoriaProducto"])
                        },
                        Estado = new EstadoProducto
                        {
                            EstadoProductoID = Convert.ToInt32(dr["IdEstadoProducto"]),
                            Nombre = Convert.ToString(dr["EstadoProducto"])
                        }
                    });
            }

            return productos;
        }

        public static bool DeleteProducto(int ProductoID)
        {
            connection(connectionstring);

            SqlCommand cmd = new SqlCommand("pp_deleteproductbyid", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@idproducto", ProductoID);

            var returnParameter = cmd.Parameters.Add("@ReturnValue", SqlDbType.Int);
            returnParameter.Direction = ParameterDirection.ReturnValue;

            con.Open();
            int i = cmd.ExecuteNonQuery();
            int result = (int)returnParameter.Value;
            con.Close();

            if (i >= 1)
                return true;
            else
                return false;
        }

        public Producto GetProductByID(int id)
        {
            connection(connectionstring);

            SqlCommand cmd = new SqlCommand("pp_selectproductbyid", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@id", id);

            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            con.Open();
            sd.Fill(dt);
            con.Close();

            Producto product = new Producto();
            EstadoProducto estadoProducto = new EstadoProducto();
            Marca marca = new Marca();
            Categoria categoria = new Categoria();

            product.Nombre = Convert.ToString(dt.Rows[0]["Nombre"]);
            product.Descripcion = Convert.ToString(dt.Rows[0]["Descripcion"]);
            product.ProductoID = Convert.ToInt32(dt.Rows[0]["IDProducto"]);
            product.Precio = Convert.ToDouble(dt.Rows[0]["Precio"]);
            product.FechaRegistro = Convert.ToDateTime(dt.Rows[0]["FechaRegistro"]);

            estadoProducto.Nombre = Convert.ToString(dt.Rows[0]["EstadoProducto"]);
            estadoProducto.EstadoProductoID = Convert.ToInt32(dt.Rows[0]["IDEstadoProducto"]);
            estadoProducto.FechaRegistro = Convert.ToDateTime(dt.Rows[0]["FechaRegistro"]);

            marca.MarcaID = Convert.ToInt32(dt.Rows[0]["IDMarcaProducto"]);
            marca.Nombre = Convert.ToString(dt.Rows[0]["MarcaProducto"]);
            marca.FechaRegistro = Convert.ToDateTime(dt.Rows[0]["FechaRegistro"]);

            categoria.CategoriaID = Convert.ToInt32(dt.Rows[0]["IDCategoriaProducto"]);
            categoria.Nombre = Convert.ToString(dt.Rows[0]["CategoriaProducto"]);
            categoria.FechaRegistro = Convert.ToDateTime(dt.Rows[0]["FechaRegistro"]);

            product.Marca = marca;
            product.Estado = estadoProducto;
            product.Categoria = categoria;
            

            return product;
        }

        public static Marca GetMarcaByID(int id)
        {
            connection(connectionstring);

            SqlCommand cmd = new SqlCommand("pp_selectmarcabyid", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@idmarca", id);

            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            con.Open();
            sd.Fill(dt);
            con.Close();

            Marca marca = new Marca();

            marca.MarcaID = Convert.ToInt32(dt.Rows[0]["IDMarcaProducto"]);
            marca.Nombre = Convert.ToString(dt.Rows[0]["MarcaProducto"]);
            marca.FechaRegistro = Convert.ToDateTime(dt.Rows[0]["FechaRegistro"]);

            return marca;
        }

        public List<Marca> GetAllMarcas()
        {
            connection(connectionstring);

            SqlCommand cmd = new SqlCommand("pp_selectmarca", con);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            con.Open();
            sd.Fill(dt);
            con.Close();

            List<Marca> marcas = new List<Marca>();

            foreach (DataRow dr in dt.Rows)
            {
                marcas.Add(
                    new Marca 
                    { 
                        MarcaID = Convert.ToInt32(dr["IDMarcaProducto"]),
                        Nombre = Convert.ToString(dr["MarcaProducto"]),
                        FechaRegistro = Convert.ToDateTime(dr["FechaRegistro"])
                    }
                );
            }

            return marcas;
        }

        public List<Categoria> GetAllCategorias()
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
                    }
                );
            }

            return categorias;
        }

        public List<EstadoProducto> GetAllEstadoProducto()
        {
            connection(connectionstring);

            SqlCommand cmd = new SqlCommand("pp_selectestadoproducto", con);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            con.Open();
            sd.Fill(dt);
            con.Close();

            List<EstadoProducto> estadoProductos = new List<EstadoProducto>();

            foreach (DataRow dr in dt.Rows)
            {
                estadoProductos.Add(
                    new EstadoProducto
                    {
                        EstadoProductoID = Convert.ToInt32(dr["IDEstadoProducto"]),
                        Nombre = Convert.ToString(dr["EstadoProducto"]),
                        FechaRegistro = Convert.ToDateTime(dr["FechaRegistro"])
                    }
                );
            }

            return estadoProductos;
        }

        public bool UpdateProducto(int productoid, int marcaid, int categoriaid, int estadoid, string nombre, string descripcion, double precio) 
        {
            connection(connectionstring);

            SqlCommand cmd = new SqlCommand("pp_updateProducto", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@IDProducto", productoid);
            cmd.Parameters.AddWithValue("@IDMarca", marcaid);
            cmd.Parameters.AddWithValue("@IDCategoria", categoriaid);
            cmd.Parameters.AddWithValue("@IDEstado", estadoid);
            cmd.Parameters.AddWithValue("@Nombre", nombre);
            cmd.Parameters.AddWithValue("@Descripcion", descripcion);
            cmd.Parameters.AddWithValue("@Precio", precio);

            var returnParameter = cmd.Parameters.Add("@ReturnValue", SqlDbType.Int);
            returnParameter.Direction = ParameterDirection.ReturnValue;

            con.Open();
            int i = cmd.ExecuteNonQuery();
            //int result = (int)returnParameter.Value;
            con.Close();

            if (i >= 1)
                return true;
            else
                return false;
        }



        public static bool InsertProducto(int marcaid, int categoriaid, int estadoid, string nombre, string descripcion, double precio)
        {
            connection(connectionstring);

            SqlCommand cmd = new SqlCommand("pp_insertproducto", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@idmarca", marcaid);
            cmd.Parameters.AddWithValue("@idcategoria", categoriaid);
            cmd.Parameters.AddWithValue("@idestado", estadoid);
            cmd.Parameters.AddWithValue("@nombre", nombre);
            cmd.Parameters.AddWithValue("@descripcion", descripcion);
            cmd.Parameters.AddWithValue("@precio", precio);

            var returnParameter = cmd.Parameters.Add("@ReturnValue", SqlDbType.Int);
            returnParameter.Direction = ParameterDirection.ReturnValue;

            con.Open();
            int i = cmd.ExecuteNonQuery();
            //int result = (int)returnParameter.Value;
            con.Close();

            if (i >= 1)
                return true;
            else
                return false;
        }

        public static bool UpdateMarca(int marcaid, string nombre)
        {
            connection(connectionstring);

            SqlCommand cmd = new SqlCommand("pp_updateMarcaProducto", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@IDMarcaProducto", marcaid);
            cmd.Parameters.AddWithValue("@MarcaProducto", nombre);

            //var returnParameter = cmd.Parameters.Add("@ReturnValue", SqlDbType.Int);
            //returnParameter.Direction = ParameterDirection.ReturnValue;

            con.Open();
            int i = cmd.ExecuteNonQuery();
            //int result = (int)returnParameter.Value;
            con.Close();

            if (i >= 1)
                return true;
            else
                return false;
        }

        public static bool InsertMarca(string nombre)
        {
            connection(connectionstring);

            SqlCommand cmd = new SqlCommand("pp_insertmarcaproducto", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@marcaproducto", nombre);

            //var returnParameter = cmd.Parameters.Add("@ReturnValue", SqlDbType.Int);
            //returnParameter.Direction = ParameterDirection.ReturnValue;

            con.Open();
            int i = cmd.ExecuteNonQuery();
            //int result = (int)returnParameter.Value;
            con.Close();

            if (i >= 1)
                return true;
            else
                return false;
        }

        public static Categoria GetCategoriaByID(int id)
        {
            connection(connectionstring);

            SqlCommand cmd = new SqlCommand("pp_selectcategoriabyid", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@idcategoria", id);

            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            con.Open();
            sd.Fill(dt);
            con.Close();

            Categoria categoria = new Categoria();

            categoria.CategoriaID = Convert.ToInt32(dt.Rows[0]["IDCategoriaProducto"]);
            categoria.Nombre = Convert.ToString(dt.Rows[0]["CategoriaProducto"]);
            categoria.FechaRegistro = Convert.ToDateTime(dt.Rows[0]["FechaRegistro"]);

            return categoria;
        }

        public static bool UpdateCategoria(int categoriaid, string nombre)
        {
            connection(connectionstring);

            SqlCommand cmd = new SqlCommand("pp_updateCategoriaProducto", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@IDCategoriaProducto", categoriaid);
            cmd.Parameters.AddWithValue("@categoriaProducto", nombre);

            //var returnParameter = cmd.Parameters.Add("@ReturnValue", SqlDbType.Int);
            //returnParameter.Direction = ParameterDirection.ReturnValue;

            con.Open();
            int i = cmd.ExecuteNonQuery();
            //int result = (int)returnParameter.Value;
            con.Close();

            if (i >= 1)
                return true;
            else
                return false;
        }

        public static bool InsertCategoria(string nombre)
        {
            connection(connectionstring);

            SqlCommand cmd = new SqlCommand("pp_insertcategoriaproducto", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@categoriaProducto", nombre);

            //var returnParameter = cmd.Parameters.Add("@ReturnValue", SqlDbType.Int);
            //returnParameter.Direction = ParameterDirection.ReturnValue;

            con.Open();
            int i = cmd.ExecuteNonQuery();
            //int result = (int)returnParameter.Value;
            con.Close();

            if (i >= 1)
                return true;
            else
                return false;
        }
    }
}
