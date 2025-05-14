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
    public class FacturasController : Controller
    {
        private readonly HttpClient _httpClient = new HttpClient();
        // GET: Facturas
        public async Task<ActionResult> Index()
        {
            _httpClient.BaseAddress = new Uri("https://localhost:44333/");

            var response = await _httpClient.GetAsync($"api/facturas");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                var resp = JsonConvert.DeserializeObject<ApiResponse<List<ListarFacturas>>>(content);

                if (resp == null || resp.data == null)
                {
                    return View("Index", new List<ListarFacturas>());
                }

                return View("Index", resp.data);
            }
            return View();
        }

        public async Task<JsonResult> ListarEmpleado()
        {
            _httpClient.BaseAddress = new Uri("https://localhost:44333/");

            var response = await _httpClient.GetAsync($"api/empleados");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                var resp = JsonConvert.DeserializeObject<ApiResponse<List<EmpleadoDB>>>(content);

                return Json(resp.data, JsonRequestBehavior.AllowGet);
            }
            return Json(null, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> ListarCliente()
        {
            _httpClient.BaseAddress = new Uri("https://localhost:44333/");

            var response = await _httpClient.GetAsync($"rest/api/obtenerClientes");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                var resp = JsonConvert.DeserializeObject<List<ClienteData>>(content);

                return Json(resp, JsonRequestBehavior.AllowGet);
            }
            return Json(null, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> ListarFormaPago()
        {
            _httpClient.BaseAddress = new Uri("https://localhost:44333/");

            var response = await _httpClient.GetAsync($"api/FormaPago");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                var resp = JsonConvert.DeserializeObject<ApiResponse<List<FormasPagoDB>>>(content);

                return Json(resp.data, JsonRequestBehavior.AllowGet);
            }
            return Json(null, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<JsonResult> AgregarFactura(FacturaModel facturaModel)
        {
            try
            {
                _httpClient.BaseAddress = new Uri("https://localhost:44333/");
                var json = JsonConvert.SerializeObject(facturaModel);
                var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync($"api/factura", content);

                if (response.IsSuccessStatusCode)
                {
                    var contertResponse = await response.Content.ReadAsStringAsync();
                    var resp = JsonConvert.DeserializeObject<CustomApiResponse<FacturaModel>>(contertResponse);

                    if (resp == null || resp.result == null)
                    {
                        return Json(new { success = false, message = "Error al agregar la factura." }, JsonRequestBehavior.AllowGet);
                    }

                    return Json(new { success = true, message = "Factura agregada correctamente.", data = resp.result }, JsonRequestBehavior.AllowGet);
                }

                return Json(new { success = false, message = "Error al agregar la factura." }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { success = false, message = "Error al agregar la factura." }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<JsonResult> EditarFactura(FacturaModel facturaModel)
        {
            try
            {
                _httpClient.BaseAddress = new Uri("https://localhost:44333/");
                var json = JsonConvert.SerializeObject(facturaModel);
                var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

                var response = await _httpClient.PutAsync($"api/factura/{facturaModel.idFactura}", content);

                if (response.IsSuccessStatusCode)
                {
                    var contertResponse = await response.Content.ReadAsStringAsync();
                    var resp = JsonConvert.DeserializeObject<CustomApiResponse<FacturaModel>>(contertResponse);

                    if (resp == null || resp.result == null)
                    {
                        return Json(new { success = false, message = "Error al editar la factura." }, JsonRequestBehavior.AllowGet);
                    }

                    return Json(new { success = true, message = "Factura editada correctamente.", data = resp.result }, JsonRequestBehavior.AllowGet);
                }

                return Json(new { success = false, message = "Error al editar la factura." }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { success = false, message = "Error al editar la factura." }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<JsonResult> EliminarFactura(int idFactura)
        {
            try
            {
                _httpClient.BaseAddress = new Uri("https://localhost:44333/");
                var response = await _httpClient.DeleteAsync($"api/factura/{idFactura}");

                if (response.IsSuccessStatusCode)
                {
                    var contertResponse = await response.Content.ReadAsStringAsync();
                    var resp = JsonConvert.DeserializeObject<CustomApiResponse<bool>>(contertResponse);

                    if (resp == null || !resp.result)
                    {
                        return Json(new { success = false, message = "Error al eliminar la factura." }, JsonRequestBehavior.AllowGet);
                    }

                    return Json(new { success = true, message = "Factura eliminada correctamente." }, JsonRequestBehavior.AllowGet);
                }

                return Json(new { success = false, message = "Error al eliminar la factura." }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { success = false, message = "Error al eliminar la factura." }, JsonRequestBehavior.AllowGet);
            }
        }

        public class CustomApiResponse<T>
        {
            public int status { get; set; }
            public string message { get; set; }
            public T result { get; set; }
        }
    }
}