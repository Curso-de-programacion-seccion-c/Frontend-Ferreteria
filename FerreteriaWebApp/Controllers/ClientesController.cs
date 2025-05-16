using FerreteriaWebApp.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace FerreteriaWebApp.Controllers
{
    public class ClientesController : Controller
    {
        private readonly HttpClient _httpClient = new HttpClient();

        public async Task<ActionResult> Index()
        {
            _httpClient.BaseAddress = new Uri("https://localhost:44333/");

            var response = await _httpClient.GetAsync("rest/api/obtenerClientes");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var clientes = JsonConvert.DeserializeObject<List<ClientesModel>>(content);
                return View("Index", clientes);
            }
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> AgregarCliente(ClientesModel cliente)
        {
            _httpClient.BaseAddress = new Uri("https://localhost:44333/");
            var json = JsonConvert.SerializeObject(cliente);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("rest/api/insertarCliente", content);

            if (response.IsSuccessStatusCode)
            {
                return new HttpStatusCodeResult(200);
            }
            else
            {
                return new HttpStatusCodeResult(500);
            }
        }

        [HttpPost]
        public async Task<ActionResult> EditarCliente(ClientesModel cliente)
        {
            _httpClient.BaseAddress = new Uri("https://localhost:44333/");

            var json = JsonConvert.SerializeObject(cliente);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync($"rest/api/actualizarCliente/{cliente.IdCliente}", content);

            if (response.IsSuccessStatusCode)
            {
                return new HttpStatusCodeResult(200);
            }
            else
            {
                return new HttpStatusCodeResult(500);
            }
        }

        [HttpPost]
        public async Task<ActionResult> EliminarCliente(int idCliente)
        {
            _httpClient.BaseAddress = new Uri("https://localhost:44333/");
            var response = await _httpClient.DeleteAsync($"rest/api/eliminarCliente/{idCliente}");

            if (response.IsSuccessStatusCode)
            {
                return Json(new { success = true, message = "Cliente eliminado correctamente." }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { success = false, message = "Error al eliminar el cliente." }, JsonRequestBehavior.AllowGet);
            }
        }

        public async Task<ActionResult> BuscarPorId(int? id)
        {
            _httpClient.BaseAddress = new Uri("https://localhost:44333/");

            if (id == null || id == 0)
            {
                var responseAll = await _httpClient.GetAsync("rest/api/obtenerClientes");
                if (responseAll.IsSuccessStatusCode)
                {
                    var contenido = await responseAll.Content.ReadAsStringAsync();
                    var lista = JsonConvert.DeserializeObject<List<ClientesModel>>(contenido);
                    return Json(lista, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return new HttpStatusCodeResult(500, "Error al obtener los clientes.");
                }
            }

            var response = await _httpClient.GetAsync($"rest/api/obtenerCliente/{id}");
            if (response.IsSuccessStatusCode)
            {
                var contenido = await response.Content.ReadAsStringAsync();
                var cliente = JsonConvert.DeserializeObject<ClientesModel>(contenido);
                return Json(cliente, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return new HttpStatusCodeResult(500, "Error al buscar el cliente.");
            }
        }
    }
}
