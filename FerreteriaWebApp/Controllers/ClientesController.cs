using FerreteriaWebApp.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace FerreteriaWebApp.Controllers
{
    public class ClientesController : Controller
    {
        private readonly HttpClient _httpClient = new HttpClient();

        // GET: Clientes
        public async Task<ActionResult> Index()
        {
            _httpClient.BaseAddress = new Uri("https://localhost:44333/");
            var response = await _httpClient.GetAsync("rest/api/obtenerClientes");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var clientes = JsonConvert.DeserializeObject<List<Models.ClientesModel>>(content);

                return View(clientes);
            }
            return View(new List<Models.ClientesModel>());
        }

        [HttpPost]
        public async Task<JsonResult> Agregar(ClientesModel clientes)
        {
            _httpClient.BaseAddress = new Uri("https://localhost:44333/");
            var json = JsonConvert.SerializeObject(clientes);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("rest/api/insertarCliente", content);

            if (response.IsSuccessStatusCode)
            {
                var contentResponse = await response.Content.ReadAsStringAsync();
                var responseClientes = JsonConvert.DeserializeObject<ResponseClientes>(contentResponse);
                if (!responseClientes.DescripcionRespuesta.Contains("exitosamente"))
                {
                    if(responseClientes.DescripcionRespuesta.Contains("UNIQUE"))
                    {
                        return Json(new { success = false, message = "El cliente ya existe." });
                    }
                    return Json(new { success = false, message = responseClientes.DescripcionRespuesta });
                }
                return Json(new { success = true, message = "Cliente agregado exitosamente." });
            }
            else
            {
                var contentResponse = await response.Content.ReadAsStringAsync();
                var responseClientes = JsonConvert.DeserializeObject<ResponseClientes>(contentResponse);
                return Json(new { success = false, message = responseClientes.DescripcionRespuesta });
            }
        }

        [HttpPost]
        public async Task<JsonResult> Editar(ClientesModel clientes)
        {
            _httpClient.BaseAddress = new Uri("https://localhost:44333/");
            var json = JsonConvert.SerializeObject(clientes);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync($"rest/api/actualizarCliente/{clientes.IdCliente}", content);

            if (response.IsSuccessStatusCode)
            {
                var contentResponse = await response.Content.ReadAsStringAsync();
                var responseClientes = JsonConvert.DeserializeObject<ResponseClientes>(contentResponse);
                if (responseClientes.DescripcionRespuesta.Contains("Error"))
                {
                    return Json(new { success = false, message = responseClientes.DescripcionRespuesta });
                }
                return Json(new { success = true, message = "Cliente editado exitosamente." });
            }
            else
            {
                var contentResponse = await response.Content.ReadAsStringAsync();
                var responseClientes = JsonConvert.DeserializeObject<ResponseClientes>(contentResponse);
                return Json(new { success = false, message = responseClientes.DescripcionRespuesta });
            }
        }

        [HttpPost]
        public async Task<JsonResult> Eliminar(ClientesModel clientes)
        {
            _httpClient.BaseAddress = new Uri("https://localhost:44333/");
            var json = JsonConvert.SerializeObject(clientes);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            var response = await _httpClient.DeleteAsync($"rest/api/eliminarCliente/{clientes.IdCliente}");

            if (response.IsSuccessStatusCode)
            {
                var contentResponse = await response.Content.ReadAsStringAsync();
                var responseClientes = JsonConvert.DeserializeObject<ResponseClientes>(contentResponse);
                if (responseClientes.DescripcionRespuesta.Contains("Error"))
                {
                    if (responseClientes.DescripcionRespuesta.Contains("FK"))
                    {
                        return Json(new { success = false, message = "No se puede eliminar el cliente porque tiene ventas asociadas." });
                    }
                    return Json(new { success = false, message = responseClientes.DescripcionRespuesta });
                }
                return Json(new { success = true, message = "Cliente eliminado exitosamente." });
            }
            else
            {
                var contentResponse = await response.Content.ReadAsStringAsync();
                var responseClientes = JsonConvert.DeserializeObject<ResponseClientes>(contentResponse);
                return Json(new { success = false, message = responseClientes.DescripcionRespuesta });
            }
        }

        public async Task<JsonResult> ObtenerCliente(int idCliente)
        {
            _httpClient.BaseAddress = new Uri("https://localhost:44333/");
            var response = await _httpClient.GetAsync($"rest/api/obtenerCliente/{idCliente}");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var cliente = JsonConvert.DeserializeObject<Models.ClientesModel>(content);
                return Json(cliente, JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = false, message = "Error al obtener el cliente." }, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> BuscarPorId(int? id)
        {
            _httpClient.BaseAddress = new Uri("https://localhost:44333/");
            if (id == null)
            {
                //Buscar todos
                var response = await _httpClient.GetAsync("rest/api/obtenerClientes");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var clientes = JsonConvert.DeserializeObject<List<Models.ClientesModel>>(content);

                    if (clientes == null || clientes.Count == 0)
                    {
                        return Json(new { success = false, message = "No se encontraron clientes.", data = new List<ClientesModel>() }, JsonRequestBehavior.AllowGet);
                    }
                    return Json(new { success = true, message = "Clientes encontrados.", data = clientes }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { success = false, message = "Error al obtener los clientes.", data = new List<ClientesModel>() }, JsonRequestBehavior.AllowGet) ;
            }

            //Buscar por id
            var responseId = await _httpClient.GetAsync($"rest/api/obtenerCliente/{id}");
            if (responseId.IsSuccessStatusCode)
            {
                var contentId = await responseId.Content.ReadAsStringAsync();
                var cliente = JsonConvert.DeserializeObject<Models.ClientesModel>(contentId);
                if (cliente == null)
                {
                    return Json(new { success = false, message = "Cliente no encontrado." }, JsonRequestBehavior.AllowGet);
                }

                return Json(new { success = true, message = "Cliente encontrado.", data = cliente });
            }
            else
            {
                return Json(new { success = false, message = "Error al obtener el cliente.", data = new ClientesModel() }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}