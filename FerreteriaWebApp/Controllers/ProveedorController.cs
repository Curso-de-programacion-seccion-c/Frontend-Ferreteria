using FerreteriaWebApp.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;


namespace FerreteriaWebApp.Controllers
{
   
    public class ProveedorController : Controller
    {
        private readonly HttpClient _httpClient = new HttpClient();

        public async Task<ActionResult> Index()
        {
            _httpClient.BaseAddress = new Uri("https://localhost:44333/");

            var response = await _httpClient.GetAsync("rest/api/obtenerProveedores");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                // CAMBIO IMPORTANTE AQUÍ
                var data = JsonConvert.DeserializeObject<ProveedorResponse>(content);
                var listaProveedores = data.Result;

                return View("ProveedorView", listaProveedores);
            }

            return View("ProveedorView", new List<ProveedorModel>()); // Si falla, regresa vista vacía
        }

        [HttpPost]
        public async Task<ActionResult> AgregarProveedor(ProveedorModel proveedorModel)
        {
            _httpClient.BaseAddress = new Uri("https://localhost:44333/");
            var json = JsonConvert.SerializeObject(proveedorModel);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("rest/api/crearProveedor", content);

            if (response.IsSuccessStatusCode)
            {
                return new HttpStatusCodeResult(200);
            }

            return new HttpStatusCodeResult(500);
        }

        [HttpPost]
        public async Task<ActionResult> EditarProveedor(ProveedorModel proveedor)
        {
            _httpClient.BaseAddress = new Uri("https://localhost:44333/");

            var newBody = new
            {
                IdProveedor = proveedor.IdProveedor,
                NombreProveedor = proveedor.NombreProveedor,
                Telefono = proveedor.Telefono,
                NombreContacto = proveedor.NombreContacto
            };

            var json = JsonConvert.SerializeObject(newBody);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"rest/api/actualizarProveedor/{proveedor.IdProveedor}", content);

            if (response.IsSuccessStatusCode)
            {
                return new HttpStatusCodeResult(200);
            }

            return new HttpStatusCodeResult(500);
        }

        [HttpPost]
        public async Task<ActionResult> EliminarProveedor(int IdProveedor)
        {
            _httpClient.BaseAddress = new Uri("https://localhost:44333/");
            var response = await _httpClient.DeleteAsync($"rest/api/eliminarProveedor/{IdProveedor}");

            if (response.IsSuccessStatusCode)
            {
                return Json(new { success = true, message = "Proveedor eliminado correctamente." }, JsonRequestBehavior.AllowGet);
            }

            return new HttpStatusCodeResult(500);
        }

        public async Task<ActionResult> BuscarPorId(int? id)
        {
            _httpClient.BaseAddress = new Uri("https://localhost:44333/");

            if (id == null || id == 0)
            {
                var responseAll = await _httpClient.GetAsync("rest/api/obtenerProveedores");
                if (responseAll.IsSuccessStatusCode)
                {
                    var contenido = await responseAll.Content.ReadAsStringAsync();
                    var data = JsonConvert.DeserializeObject<ProveedorResponse>(contenido);
                    return Json(data.Result, JsonRequestBehavior.AllowGet);
                }

                return new HttpStatusCodeResult(500, "Error al obtener los Proveedores.");
            }

            var response = await _httpClient.GetAsync($"rest/api/obtenerProveedor/{id}");
            if (response.IsSuccessStatusCode)
            {
                var contenido = await response.Content.ReadAsStringAsync();
                var json = JsonConvert.DeserializeObject<dynamic>(contenido);
                var proveedorJson = json.result.ToString();
                var proveedor = JsonConvert.DeserializeObject<ProveedorModel>(proveedorJson);
                return Json(proveedor, JsonRequestBehavior.AllowGet);
            }

            return new HttpStatusCodeResult(500, "Error al buscar el proveedor.");
        }

    }
}

