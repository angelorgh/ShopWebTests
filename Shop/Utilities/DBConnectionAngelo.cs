using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Shop.Models;

namespace Shop.Utilities.DBConnection
{
    public interface IDBConnectionAngelo
    {
        public bool RegisterUser(string constring, Usuario user, Contacto contact);
        public bool VerifyLogin(string connectionstring, Usuario user);
        public Cliente GetUserInformation(string connectionstring, string username);
        public List<CarritoItem> GetUserShoppingCart(string connectionstring, int clienteid);
        public bool Order(string connectionstring, int idusuario, int iddireccion);
        public List<Orden> GetUserOrders(string connectionstring, int idcliente);
        public List<DetalleOrden> GetUserOrdersDetails(string connectionstring, int idorden);
    }

    public class DBConnectionAngelo : IDBConnectionAngelo
    {
        //private static IConfiguration _config;

        public DBConnectionAngelo()
        {
            //_config = config;
        }

        /*
        private static string connectionstring = _config.GetConnectionString("AngeloConnection");
        */


        private static SqlConnection con;

        private static void connection(string connectionstring)
        {
            con = new SqlConnection(connectionstring);
        }


        public bool RegisterUser(string constring, Usuario user, Contacto contact)
        {

            connection(constring);
            SqlCommand cmd = new SqlCommand("pp_insertclient", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@nombre", contact.Nombre);
            cmd.Parameters.AddWithValue("@apellidos", contact.Apellido);
            cmd.Parameters.AddWithValue("@cedula", contact.Cedula);
            cmd.Parameters.AddWithValue("@telefono", contact.Telefono);
            cmd.Parameters.AddWithValue("@email", contact.Email);
            cmd.Parameters.AddWithValue("@nombreusuario", user.NombreUsuario);
            cmd.Parameters.AddWithValue("@key", user.Contraseña);

            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();

            if (i >= 1)
                return true;
            else
                return false;
        }

        public bool VerifyLogin(string connectionstring, Usuario user)
        {
            connection(connectionstring);

            SqlCommand cmd = new SqlCommand("pp_verificarlogin", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@username", user.NombreUsuario);
            cmd.Parameters.AddWithValue("@password", user.Contraseña);

            var returnParameter = cmd.Parameters.Add("@ReturnValue", SqlDbType.Int);
            returnParameter.Direction = ParameterDirection.ReturnValue;

            con.Open();
            int i = cmd.ExecuteNonQuery();
            int result = (int)returnParameter.Value;
            con.Close();

            if (result != 1)
                return false;
            else
                return true;
        }

        // public static Contacto GetUserInformation(string connectionstring, string username)
        // {
        //     connection(connectionstring);
        //     
        //     SqlCommand cmd = new SqlCommand("pp_selectuserbyname", con);
        //     cmd.CommandType = CommandType.StoredProcedure;
        //
        //     cmd.Parameters.AddWithValue("@username", username);
        //     
        //     SqlDataAdapter sd = new SqlDataAdapter(cmd);
        //     DataTable dt = new DataTable();
        //     
        //     con.Open();
        //     sd.Fill(dt);
        //     con.Close();
        //     
        //     Contacto contacto = new Contacto();
        //
        //     contacto.Nombre = Convert.ToString(dt.Rows[0]["Nombre"]);
        //     contacto.Apellido = Convert.ToString(dt.Rows[0]["Apellido"]);
        //     contacto.Cedula = Convert.ToString(dt.Rows[0]["Cedula"]);
        //     contacto.Email = Convert.ToString(dt.Rows[0]["email"]);
        //
        //     return contacto;
        // }

        public Cliente GetUserInformation(string connectionstring, string username)
        {
            connection(connectionstring);

            SqlCommand cmd = new SqlCommand("pp_selectuserbyname", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@username", username);

            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            con.Open();
            sd.Fill(dt);
            con.Close();

            Cliente cliente = new Cliente();
            Contacto contacto = new Contacto();
            Usuario usuario = new Usuario();

            contacto.ContactoID = Convert.ToInt32(dt.Rows[0]["IDContacto"]);
            contacto.Nombre = Convert.ToString(dt.Rows[0]["Nombre"]);
            contacto.Apellido = Convert.ToString(dt.Rows[0]["Apellido"]);
            contacto.Cedula = Convert.ToString(dt.Rows[0]["Cedula"]);
            contacto.Telefono = Convert.ToString(dt.Rows[0]["Telefono"]);
            contacto.Email = Convert.ToString(dt.Rows[0]["email"]);
            contacto.Cedula = Convert.ToString(dt.Rows[0]["Cedula"]);

            usuario.NombreUsuario = Convert.ToString(dt.Rows[0]["NombreUsuario"]);
            usuario.UsuarioID = Convert.ToInt32(dt.Rows[0]["IDUsuario"]);

            cliente.ClienteID = Convert.ToInt32(dt.Rows[0]["IDCliente"]);
            cliente.Contacto = contacto;
            cliente.Usuario = usuario;

            return cliente;
        }

        public static Producto GetProductByID(string connectionstring, int id)
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

            product.Nombre = Convert.ToString(dt.Rows[0]["Nombre"]);
            product.Image = "~/images/Imagenes/" + Convert.ToString(Convert.ToInt32(dt.Rows[0]["IDProducto"])) + ".jpg";
            product.Descripcion = Convert.ToString(dt.Rows[0]["Descripcion"]);
            product.ProductoID = Convert.ToInt32(dt.Rows[0]["IDProducto"]);
            product.Precio = Convert.ToDecimal(dt.Rows[0]["Precio"]);
            product.FechaRegistro = Convert.ToDateTime(dt.Rows[0]["FechaRegistro"]);

            estadoProducto.Estado = Convert.ToString(dt.Rows[0]["EstadoProducto"]);
            estadoProducto.EstadoProductoID = Convert.ToInt32(dt.Rows[0]["IDEstadoProducto"]);
            estadoProducto.FechaRegistro = Convert.ToDateTime(dt.Rows[0]["FechaRegistro"]);

            marca.MarcaID = Convert.ToInt32(dt.Rows[0]["IDMarcaProducto"]);
            marca.Nombre = Convert.ToString(dt.Rows[0]["MarcaProducto"]);
            marca.FechaRegistro = Convert.ToDateTime(dt.Rows[0]["FechaRegistro"]);

            product.marca = marca;
            product.estado = estadoProducto;

            return product;
        }

        public List<CarritoItem> GetUserShoppingCart(string connectionstring, int clientid)
        {
            connection(connectionstring);

            SqlCommand cmd = new SqlCommand("pp_selectshoppingcart", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@idcliente", clientid);

            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            con.Open();
            sd.Fill(dt);
            con.Close();

            List<CarritoItem> cart = new List<CarritoItem>();
            foreach (DataRow dr in dt.Rows)
            {
                cart.Add(
                    new CarritoItem()
                    {
                        IDCarritoItem = Convert.ToInt32(dr["IDCarritoItem"]),
                        Cantidad = Convert.ToInt32(dr["Cantidad"]),
                        ITBIS = Convert.ToDecimal(dr["ITBIS"]),
                        Producto = new Producto
                        {
                            Nombre = Convert.ToString(dr["Nombre"]),
                            ProductoID = Convert.ToInt32(dr["IDProducto"]),
                            Image = "~/images/Imagenes/" + Convert.ToString(Convert.ToInt32(dr["IDProducto"])) + ".jpg",
                            Descripcion = Convert.ToString(dr["Descripcion"]),
                            Precio = Convert.ToDecimal(dr["Precio"]),
                            marca = new Marca
                            {
                                MarcaID = Convert.ToInt32(dr["IDMarcaProducto"]),
                                Nombre = Convert.ToString(dr["MarcaProducto"])
                            },
                            categoria = new Categoria
                            {
                                CategoriaID = Convert.ToInt32(dr["IDCategoriaProducto"]),
                                Nombre = Convert.ToString(dr["CategoriaProducto"])
                            },
                            estado = new EstadoProducto
                            {
                                EstadoProductoID = Convert.ToInt32(dr["IDEstadoProducto"]),
                                Estado = Convert.ToString(dr["EstadoProducto"])
                            }
                        }
                    });
            }

            return cart;
        }

        public static bool AddCarritoItem(string connectionstring, int idcliente, int idproducto, int cantidad)
        {
            connection(connectionstring);

            SqlCommand cmd = new SqlCommand("pp_insertcarritoitem", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@idcliente", idcliente);
            cmd.Parameters.AddWithValue("@idproducto", idproducto);
            cmd.Parameters.AddWithValue("@cantidad", cantidad);

            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();

            if (i >= 1)
                return true;
            else
                return false;
        }

        public static bool DeleteCarritoItem(string connectionstring, int idcliente, int idproducto, int cantidad)
        {
            connection(connectionstring);

            SqlCommand cmd = new SqlCommand("pp_deletecarritoitem", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@idcliente", idcliente);
            cmd.Parameters.AddWithValue("@idproducto", idproducto);
            cmd.Parameters.AddWithValue("@cantidad", cantidad);

            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();

            if (i >= 1)
                return true;
            else
                return false;
        }

        public static List<Direccion> GetUserDirecciones(string connectionstring, int idcliente)
        {
            connection(connectionstring);

            SqlCommand cmd = new SqlCommand("pp_selectdireccion", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@idcliente", idcliente);

            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            con.Open();
            sd.Fill(dt);
            con.Close();

            List<Direccion> direcciones = new List<Direccion>();
            foreach (DataRow dr in dt.Rows)
            {
                direcciones.Add(
                    new Direccion
                    {
                        IDDireccion = Convert.ToInt32(dr["IDDireccion"]),
                        Direccion1 = Convert.ToString(dr["Direccion1"]),
                        Direccion2 = Convert.ToString(dr["Direccion2"]),
                        Provincia = Convert.ToString(dr["Provincia"]),
                        Ciudad = Convert.ToString(dr["Ciudad"])
                    });
            }

            return direcciones;
        }
        public static Direccion GetDireccion(string connectionstring, int iddireccion)
        {
            connection(connectionstring);

            SqlCommand cmd = new SqlCommand("pp_selectdireccionbyid", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@idDireccion", iddireccion);

            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            con.Open();
            sd.Fill(dt);
            con.Close();


            Direccion direccion = new Direccion();
            foreach (DataRow dr in dt.Rows)
            {
                Cliente cliente = new Cliente();
                cliente.ClienteID = Convert.ToInt32(dr["IDCliente"]);
                direccion = new Direccion(Convert.ToInt32(dr["IDDireccion"]), cliente, Convert.ToString(dr["Direccion1"]), Convert.ToString(dr["Direccion2"]), Convert.ToString(dr["Provincia"]), Convert.ToString(dr["Ciudad"]));
            }
            return direccion;


        }

        public bool Order(string connectionstring, int idusuario, int iddireccion)
        {
            connection(connectionstring);
            SqlCommand cmd = new SqlCommand("pp_insertorden", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@idusuario", idusuario);
            cmd.Parameters.AddWithValue("@idDireccion", iddireccion);

            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();

            if (i >= 1)
                return true;
            else
                return false;
        }
        public static bool DeleteDirection(string connectionstring, int iddireccion)
        {
            connection(connectionstring);
            SqlCommand cmd = new SqlCommand("pp_deletedireccionbyid", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@iddireccion", iddireccion);

            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();
            if (i >= 1)
                return true;
            else
                return false;
        }
        public static bool AddDirection(string connectionstring, int idcliente, string pronvicia, string ciudad, string direccion1, string direccion2)
        {
            connection(connectionstring);
            SqlCommand cmd = new SqlCommand("pp_insertdireccion", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@idcliente", idcliente);
            cmd.Parameters.AddWithValue("@Provincia", pronvicia);
            cmd.Parameters.AddWithValue("@Ciudad ", ciudad);
            cmd.Parameters.AddWithValue("@Direccion1", direccion1);
            cmd.Parameters.AddWithValue("@Direccion2", direccion2);

            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();
            if (i >= 1)
                return true;
            else
                return false;
        }
        public static bool UpdateDirection(string connectionstring, int iddireccion, string pronvicia, string ciudad, string direccion1, string direccion2)
        {
            connection(connectionstring);
            SqlCommand cmd = new SqlCommand("pp_updateDireccion", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@IDDireccion", iddireccion);
            cmd.Parameters.AddWithValue("@Provincia", pronvicia);
            cmd.Parameters.AddWithValue("@Ciudad", ciudad);
            cmd.Parameters.AddWithValue("@Direccion1", direccion1);
            cmd.Parameters.AddWithValue("@Direccion2", direccion2);

            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();
            if (i >= 1)
                return true;
            else
                return false;
        }

        public List<Orden> GetUserOrders(string connectionstring, int idcliente)
        {
            connection(connectionstring);

            SqlCommand cmd = new SqlCommand("pp_selectordenbyclient", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@idcliente", idcliente);

            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            con.Open();
            sd.Fill(dt);
            con.Close();

            List<Orden> ordenes = new List<Orden>();
            foreach (DataRow dr in dt.Rows)
            {
                EstadoOrden eo = new EstadoOrden();
                eo.IDEstadoOrden = Convert.ToInt32(dr["IDEstadoOrden"]);
                Cliente c = new Cliente();
                c.ClienteID = Convert.ToInt32(dr["IDCliente"]);
                Direccion d = new Direccion();
                d.IDDireccion = Convert.ToInt32(dr["IDDireccion"]);
                ordenes.Add(
                    new Orden
                    {
                        IDOrden = Convert.ToInt32(dr["IDOrden"]),
                        FechaGen = Convert.ToDateTime(dr["FechaGen"]),
                        EstadoOrden = eo,
                        Direccion = d,
                        Cliente = c
                    });
            }

            return ordenes;
        }
        public List<DetalleOrden> GetUserOrdersDetails(string connectionstring, int idorden)
        {
            connection(connectionstring);

            SqlCommand cmd = new SqlCommand("pp_selectdetalleordenbyorden", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@idorden", idorden);

            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            con.Open();
            sd.Fill(dt);
            con.Close();

            List<DetalleOrden> detallesordenes = new List<DetalleOrden>();
            foreach (DataRow dr in dt.Rows)
            {
                Orden o = new Orden();
                o.IDOrden = Convert.ToInt32(dr["IDOrden"]);
                Producto p = new Producto();
                p.ProductoID = Convert.ToInt32(dr["IDProducto"]);
                p.Nombre = Convert.ToString(dr["NombreProducto"]);
                p.Precio = Convert.ToDecimal(dr["Precio"]);
                p.Image = "~/images/Imagenes/" + Convert.ToString(Convert.ToInt32(dr["IDProducto"])) + ".jpg";
                detallesordenes.Add(
                    new DetalleOrden
                    {
                        DetalleOrdenId = Convert.ToInt32(dr["IDDetalleOrden"]),
                        Producto = p,
                        Orden = o,
                        Cantidad = Convert.ToInt32(dr["Cantidad"]),
                        Itebis = Convert.ToDecimal(dr["Itbis"])
                    });
            }

            return detallesordenes;
        }
        public static Producto GetProductDetailsOrder(string connectionstring, int idproducto)
        {
            connection(connectionstring);

            SqlCommand cmd = new SqlCommand("pp_selectproductbyid", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@id", idproducto);

            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            con.Open();
            sd.Fill(dt);
            con.Close();


            Producto producto = new Producto();
            foreach (DataRow dr in dt.Rows)
            {
                producto = new Producto
                {
                    ProductoID = Convert.ToInt32(dr["IDProducto"]),
                    Nombre = Convert.ToString(dr["Nombre"]),
                    Precio = Convert.ToDecimal(dr["Precio"]),
                    Descripcion = Convert.ToString(dr["Descripcion"])
                };

            }
            return producto;


        }

    }
}