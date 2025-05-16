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
    public class FormaPagoController : Controller
    {
        private readonly HttpClient _httpClient = new HttpClient();


        public async Task<ActionResult> Index()
        {
            _httpClient.BaseAddress = new Uri("https://localhost:44333/");

            var response = await _httpClient.GetAsync($"/api/FormaPago");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                var resp = JsonConvert.DeserializeObject<CustomApiResponse<List<FormaPagoModel>>>(content);

                return View("Index", resp.result);
            }
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> AgregarFormaPago(FormaPagoModel FormaPago)
        {
            _httpClient.BaseAddress = new Uri("https://localhost:44333/");
            var json = JsonConvert.SerializeObject(FormaPago);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"/api/formaspago", content);

            if (response.IsSuccessStatusCode)
            {
                var contentResponse = await response.Content.ReadAsStringAsync();
                var resp = JsonConvert.DeserializeObject<CustomApiResponse<List<FormaPagoModel>>>(contentResponse);
                return new HttpStatusCodeResult(200);
            }
            else
            {
                return new HttpStatusCodeResult(500);
            }
        }

        [HttpPost]
        public async Task<ActionResult> ActualizarFormaPago(FormaPagoModel FormaPago)
        {
            _httpClient.BaseAddress = new Uri("https://localhost:44333/");

            var newBody = new
            {
                NombreFormaPago = FormaPago.NombreFormaPago,
                Descripcion = FormaPago.Descripcion
            };

            var json = JsonConvert.SerializeObject(newBody);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"api/formaspago/{FormaPago.IdFormaPago}", content);

            if (response.IsSuccessStatusCode)
            {
                var contentResponse = await response.Content.ReadAsStringAsync();
                var resp = JsonConvert.DeserializeObject<CustomApiResponse<List<FormaPagoModel>>>(contentResponse);
                return new HttpStatusCodeResult(200);
            }
            else
            {
                return new HttpStatusCodeResult(500);
            }
        }

        [HttpPost]
        public async Task<ActionResult> EliminarFormaPago(int IdFormaPago)
        {
            _httpClient.BaseAddress = new Uri("https://localhost:44333/");
            var response = await _httpClient.DeleteAsync($"api/formaspago/{IdFormaPago}");

            if (response.IsSuccessStatusCode)
            {
                var contentReponse = await response.Content.ReadAsStringAsync();
                var FormaPagoResponse = JsonConvert.DeserializeObject<CustomApiResponse<string>>(contentReponse);

                if (FormaPagoResponse.status != 200)
                {
                    if (FormaPagoResponse.message.Contains("FK"))
                    { 
                        return Json(new { success = false, message = "Error al eliminar la Forma de Pago." }, JsonRequestBehavior.AllowGet);
                    }
                }
                return Json(new { success = true, message = "La Forma de Pago ha sido eliminada correctamente." }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return new HttpStatusCodeResult(500);
            }
        }

        public async Task<ActionResult> BuscarPorId(int? id)
        {
            _httpClient.BaseAddress = new Uri("https://localhost:44333/");

            // Si no hay ID, obtenemos todos los roles
            if (id == null || id == 0)
            {
 
                if (id == null || id == 0)
                {
                    var responseAll = await _httpClient.GetAsync("/api/FormaPago");
                    if (responseAll.IsSuccessStatusCode)
                    {
                        var contenido = await responseAll.Content.ReadAsStringAsync();
                        var lista = JsonConvert.DeserializeObject<List<FormaPagoModel>>(contenido);
                        return Json(lista, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return new HttpStatusCodeResult(500, "Error al obtener las formas de pago.");
                    }
                }
            }

            // Si hay ID, buscamos uno específico
            var response = await _httpClient.GetAsync($"/api/formaspago/{id}");
            if (response.IsSuccessStatusCode)
            {
                var contenido = await response.Content.ReadAsStringAsync();
                var  formapago = JsonConvert.DeserializeObject<CustomApiResponse<FormaPagoModel>>(contenido);
                return Json(formapago.result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return new HttpStatusCodeResult(500, "Error al buscar la Forma de Pago.");
            }


        }

    }
    public class CustomApiResponse<T>
    {
        public int status { get; set; }
        public string message { get; set; }
        public T result { get; set; }
    }
}
