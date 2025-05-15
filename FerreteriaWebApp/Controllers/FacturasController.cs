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

        public async Task<JsonResult> ListarArticulos()
        {
            _httpClient.BaseAddress = new Uri("https://localhost:44333/");

            var response = await _httpClient.GetAsync($"rest/api/obtenerArticulos");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                var resp = JsonConvert.DeserializeObject<List<ArticulosModel>>(content);

                return Json(resp, JsonRequestBehavior.AllowGet);
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

        public async Task<ActionResult> BuscarFacturaPorId(int? idFactura)
        {
            _httpClient.BaseAddress = new Uri("https://localhost:44333/");

            if (idFactura == null)
            {
                var resposeAll = _httpClient.GetAsync($"api/facturas").Result;
                if (resposeAll.IsSuccessStatusCode)
                {
                    var content = resposeAll.Content.ReadAsStringAsync().Result;

                    var resp = JsonConvert.DeserializeObject<CustomApiResponse<List<ListarFacturas>>>(content);
                    if (resp == null || resp.result == null)
                    {
                        return Json(null, JsonRequestBehavior.AllowGet);
                    }
                    return Json(resp.result, JsonRequestBehavior.AllowGet);
                }
                return Json(null, JsonRequestBehavior.AllowGet);
            }

            var response = await _httpClient.GetAsync($"api/factura/{idFactura}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                var resp = JsonConvert.DeserializeObject<CustomApiResponse<ListarFacturas>>(content);

                if (resp == null || resp.result == null)
                {
                    return Json(null, JsonRequestBehavior.AllowGet);
                }
                return Json(resp.result, JsonRequestBehavior.AllowGet);
            }
            return Json(null, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Detalle(int id)
        {
            _httpClient.BaseAddress = new Uri("https://localhost:44333/");

            var response = _httpClient.GetAsync($"api/DetalleVenta/factura/{id}").Result;

            ViewBag.IdFactura = id;
            if (response.IsSuccessStatusCode)
            {
                var content = response.Content.ReadAsStringAsync().Result;

                var resp = JsonConvert.DeserializeObject<CustomApiResponse<List<ListarDetalleVenta>>>(content);

                if (resp == null || resp.result == null)
                {
                    return View("Detalle", new List<ListarDetalleVenta>());
                }

                return View("Detalle", resp.result);
            }
            return View("Detalle", new List<ListarDetalleVenta>());
        }

        public Task<JsonResult> AgregarDetalleFactura(FacturaDetalle facturaDetalle)
        {
            _httpClient.BaseAddress = new Uri("https://localhost:44333/");
            var json = JsonConvert.SerializeObject(facturaDetalle);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            var response = _httpClient.PostAsync($"api/DetalleVenta", content).Result;

            if (response.IsSuccessStatusCode)
            {
                var contertResponse = response.Content.ReadAsStringAsync().Result;
                var resp = JsonConvert.DeserializeObject<CustomApiResponse<FacturaDetalle>>(contertResponse);

                if (resp == null || resp.result == null)
                {
                    return Task.FromResult(Json(new { success = false, message = "Error al agregar el detalle de la factura." }, JsonRequestBehavior.AllowGet));
                }

                return Task.FromResult(Json(new { success = true, message = "Detalle de factura agregado correctamente.", data = resp.result }, JsonRequestBehavior.AllowGet));
            }

            return Task.FromResult(Json(new { success = false, message = "Error al agregar el detalle de la factura." }, JsonRequestBehavior.AllowGet));
        }

        public Task<JsonResult> EditarDetalleFactura(FacturaDetalle facturaDetalle)
        {
            _httpClient.BaseAddress = new Uri("https://localhost:44333/");
            var json = JsonConvert.SerializeObject(facturaDetalle);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            var response = _httpClient.PutAsync($"api/DetalleVenta/{facturaDetalle.IdDetalleVenta}", content).Result;

            if (response.IsSuccessStatusCode)
            {
                var contertResponse = response.Content.ReadAsStringAsync().Result;
                var resp = JsonConvert.DeserializeObject<CustomApiResponse<FacturaDetalle>>(contertResponse);

                if (resp == null || resp.result == null)
                {
                    return Task.FromResult(Json(new { success = false, message = "Error al editar el detalle de la factura." }, JsonRequestBehavior.AllowGet));
                }

                return Task.FromResult(Json(new { success = true, message = "Detalle de factura editado correctamente.", data = resp.result }, JsonRequestBehavior.AllowGet));
            }

            return Task.FromResult(Json(new { success = false, message = "Error al editar el detalle de la factura." }, JsonRequestBehavior.AllowGet));
        }

        [HttpPost]
        [ActionName("EliminarDetalleFactura")]
        public Task<JsonResult> EliminarDetalleFactura(int idDetalle)
        {
            _httpClient.BaseAddress = new Uri("https://localhost:44333/");
            var response = _httpClient.DeleteAsync($"api/DetalleVenta/{idDetalle}").Result;

            if (response.IsSuccessStatusCode)
            {
                var contertResponse = response.Content.ReadAsStringAsync().Result;
                var resp = JsonConvert.DeserializeObject<CustomApiResponse<bool>>(contertResponse);

                if (resp == null || !resp.result)
                {
                    return Task.FromResult(Json(new { success = false, message = "Error al eliminar el detalle de la factura." }, JsonRequestBehavior.AllowGet));
                }

                return Task.FromResult(Json(new { success = true, message = "Detalle de factura eliminado correctamente." }, JsonRequestBehavior.AllowGet));
            }

            return Task.FromResult(Json(new { success = false, message = "Error al eliminar el detalle de la factura." }, JsonRequestBehavior.AllowGet));
        }

        // BUSCAR DETALLE FACTURA POR ID
        [HttpPost]
        public async Task<ActionResult> BuscarDetallePorId(int? id, int idFactura)
        {
            _httpClient.BaseAddress = new Uri("https://localhost:44333/");

            // ID nulo busca todos
            if (id == null)
            {
                var resposeAll = _httpClient.GetAsync($"api/DetalleVenta/factura/{idFactura}").Result;
                if (resposeAll.IsSuccessStatusCode)
                {
                    var content = resposeAll.Content.ReadAsStringAsync().Result;

                    var resp = JsonConvert.DeserializeObject<CustomApiResponse<List<ListarDetalleVenta>>>(content);
                    if (resp == null || resp.result == null)
                    {
                        return Json(null, JsonRequestBehavior.AllowGet);
                    }
                    return Json(resp.result, JsonRequestBehavior.AllowGet);
                }
                return Json(null, JsonRequestBehavior.AllowGet);
            }

            var response = await _httpClient.GetAsync($"api/DetalleVenta/detalle/{id}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                var resp = JsonConvert.DeserializeObject<CustomApiResponse<ListarDetalleVenta>>(content);

                if (resp == null || resp.result == null)
                {
                    return Json(null, JsonRequestBehavior.AllowGet);
                }
                return Json(resp.result, JsonRequestBehavior.AllowGet);
            }
            return Json(null, JsonRequestBehavior.AllowGet);
        }





        public class CustomApiResponse<T>
        {
            public int status { get; set; }
            public string message { get; set; }
            public T result { get; set; }
        }
    }
}