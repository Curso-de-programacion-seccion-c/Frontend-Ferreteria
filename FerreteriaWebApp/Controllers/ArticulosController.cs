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
        // GET: Articulos
        public ActionResult Index()
        {
            List<ArticulosModel> articulos = new List<ArticulosModel>(); //Esto crea una lista para guardar los articulos de la bd
            string connectionString = "Data Source=DESKTOP-9CUINBH;Initial Catalog=FerreteriaDB;Integrated Security=True;";

            using (SqlConnection conn = new SqlConnection(connectionString))//Conexion a la base de datos
            {
                SqlCommand cmd = new SqlCommand("ObtenerArticulos", conn); //Reemplazar Obtener Articulos por el nombre del sp
                cmd.CommandType = CommandType.StoredProcedure;

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    articulos.Add(new ArticulosModel
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


        // GET: Articulos/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Articulos/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Articulos/Create
        [HttpPost]
        public ActionResult EditarArticulo(ArticulosModel articulo)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["FerreteriaDB"].ConnectionString;

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



        // GET: Articulos/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Articulos/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Articulos/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Articulos/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
