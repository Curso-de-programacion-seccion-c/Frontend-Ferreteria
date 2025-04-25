using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FerreteriaWebApp.Models;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;

namespace FerreteriaWebApp.Controllers
{
    public class ArticulosController : Controller
    {
        // GET: MOSTRAR LOS ARTICULOS EN UNA TABLA
        public ActionResult Index()
        {
            List<ArticulosModel> articulos = new List<ArticulosModel>(); //Esto crea una lista para guardar los articulos de la bd
            string connectionString = "Data Source=DESKTOP-9CUINBH;Initial Catalog=FerreteriaDB;Integrated Security=True;"; //Cambiar el Data Source por la direccion de la maquina

            using (SqlConnection conn = new SqlConnection(connectionString))//Conexion a la base de datos
            {
                SqlCommand cmd = new SqlCommand("ObtenerArticulos", conn); //Reemplazar Obtener Articulos por el nombre del sp
                cmd.CommandType = CommandType.StoredProcedure;

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read()) //El ciclo lee los datos que envio el sp fila por fila
                {
                    articulos.Add(new ArticulosModel //Por cada fila del resultado, se crea un objeto y lo añade a la lista
                    {
                        IdArticulo = Convert.ToInt32(reader["IdArticulo"]),
                        CodeArticulo = Convert.ToInt32(reader["CodeArticulo"]),
                        NombreArticulo = reader["NombreArticulo"].ToString(),
                        Precio = Convert.ToDecimal(reader["Precio"]),
                        Stock = Convert.ToInt32(reader["Stock"]),
                        Descripcion = reader["Descripcion"].ToString(),
                        IdCategoria = Convert.ToInt32(reader["IdCategoria"]),
                        IdProveedor = Convert.ToInt32(reader["IdProveedor"])
                    });
                }
            }

            return View("ArticulosView", articulos); //Muestra la vista, reemplazar ArticulosView por el nombre de la vista tal cual como se llama
        }

        // POST: EDITAR UN ARTICULO DE LA TABLA
        [HttpPost]
        public ActionResult EditarArticulo(ArticulosModel articulo)
        {
            try
            {
                string connectionString = "Data Source=DESKTOP-9CUINBH;Initial Catalog=FerreteriaDB;Integrated Security=True;"; // TEMPORAL
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("ModificarArticulo", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CodeArticulo", articulo.CodeArticulo);
                    cmd.Parameters.AddWithValue("@Nombre", articulo.NombreArticulo);
                    cmd.Parameters.AddWithValue("@Stock", articulo.Stock);
                    cmd.Parameters.AddWithValue("@PrecioUnitario", articulo.Precio);
                    cmd.Parameters.AddWithValue("@Descripcion", articulo.Descripcion);
                    cmd.Parameters.AddWithValue("@FechaRegistro", DateTime.Now); 
                    cmd.Parameters.AddWithValue("@IdProveedor", articulo.IdProveedor);
                    cmd.Parameters.AddWithValue("@IdCategoria", articulo.IdCategoria);

                    con.Open();
                    cmd.ExecuteNonQuery();
                }

                return new HttpStatusCodeResult(200);
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(500, ex.Message); 
            }

        }


        //EDITAR
        [HttpPost]
        public ActionResult AgregarArticulo(ArticulosModel articulo)
        {
            try
            {
                string connectionString = "Data Source=DESKTOP-9CUINBH;Initial Catalog=FerreteriaDB;Integrated Security=True;";
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("InsertarArticulo", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CodeArticulo", articulo.CodeArticulo);
                    cmd.Parameters.AddWithValue("@NombreArticulo", articulo.NombreArticulo);
                    cmd.Parameters.AddWithValue("@Precio", articulo.Precio);
                    cmd.Parameters.AddWithValue("@Stock", articulo.Stock);
                    cmd.Parameters.AddWithValue("@Descripcion", articulo.Descripcion);
                    cmd.Parameters.AddWithValue("@IdCategoria", articulo.IdCategoria);
                    cmd.Parameters.AddWithValue("@IdProveedor", articulo.IdProveedor);

                    con.Open();
                    cmd.ExecuteNonQuery();
                }
                return new HttpStatusCodeResult(200);
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(500, ex.Message);
            }
        }



        // ELIMINAR UN ARTICULO DE LA TABLA
        [HttpPost]
        public ActionResult EliminarArticulo(int idArticulo)
        {
            try
            {
                string connectionString = "Data Source=DESKTOP-9CUINBH;Initial Catalog=FerreteriaDB;Integrated Security=True;";
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("EliminarArticulo", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@IdArticulo", idArticulo);
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
                return new HttpStatusCodeResult(200);
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(500, ex.Message);
            }
        }
    }
}
