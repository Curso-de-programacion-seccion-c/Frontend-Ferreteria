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
    public class EmpleadoController : Controller
    {
        private readonly HttpClient _httpClient = new HttpClient();

        public async Task<ActionResult> Index()
        {
            _httpClient.BaseAddress = new Uri("https://localhost:44333/");

            var response = await _httpClient.GetAsync($"/api/empleados");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                var resp = JsonConvert.DeserializeObject<CustomApiResponse<List<EmpleadoModel>>>(content);

                return View("Index", resp.result);
            }
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> AgregarEmpleado(EmpleadoModel Empleado)
        {
            _httpClient.BaseAddress = new Uri("https://localhost:44333/");
            var json = JsonConvert.SerializeObject(Empleado);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"/api/empleados", content);

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
        public async Task<ActionResult> ActualizarEmpleado(EmpleadoModel Empleado)
        {
            _httpClient.BaseAddress = new Uri("https://localhost:44333/");

            var newBody = new
            {
                Dpi = Empleado.Dpi,
                Nombre = Empleado.Nombre,
                Apellido = Empleado.Apellido,
                Puesto = Empleado.Puesto,
                CorreoElectronico = Empleado.CorreoElectronico,
                Telefono = Empleado.Telefono,
                FechaContratacion = Empleado.FechaContratacion,
                Sueldo = Empleado.Sueldo
                // Puedes agregar aquí otros campos que necesites actualizar
            };

            var json = JsonConvert.SerializeObject(newBody);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"api/empleados/{Empleado.idEmpleado}", content);

            if (response.IsSuccessStatusCode)
            {
                var contentResponse = await response.Content.ReadAsStringAsync();
                var resp = JsonConvert.DeserializeObject<CustomApiResponse<List<EmpleadoModel>>>(contentResponse);
                return new HttpStatusCodeResult(200);
            }
            else
            {
                return new HttpStatusCodeResult(500);
            }
        }

        [HttpPost]
        public async Task<ActionResult> EliminarEmpleado(int idEmpleado)
        {
            _httpClient.BaseAddress = new Uri("https://localhost:44333/");
            var response = await _httpClient.DeleteAsync($"api/empleados/{idEmpleado}");

            if (response.IsSuccessStatusCode)
            {
                var contentReponse = await response.Content.ReadAsStringAsync();
                var FormaPagoResponse = JsonConvert.DeserializeObject<CustomApiResponse<string>>(contentReponse);

                if (FormaPagoResponse.status != 200)
                {
                    if (FormaPagoResponse.message.Contains("FK"))
                    {
                        return Json(new { success = false, message = "Error al eliminar el empleado." }, JsonRequestBehavior.AllowGet);
                    }
                }
                return Json(new { success = true, message = "El empleado ha sido eliminada correctamente." }, JsonRequestBehavior.AllowGet);
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
                    var responseAll = await _httpClient.GetAsync("/api/empleados");
                    if (responseAll.IsSuccessStatusCode)
                    {
                        var contenido = await responseAll.Content.ReadAsStringAsync();
                        var lista = JsonConvert.DeserializeObject<CustomApiResponse<List<EmpleadoModel>>>(contenido);
                        return Json(lista.result, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return new HttpStatusCodeResult(500, "Error al obtener los empleados.");
                    }
                }
            }

            // Si hay ID, buscamos uno específico
            var response = await _httpClient.GetAsync($"/api/empleados/{id}");
            if (response.IsSuccessStatusCode)
            {
                var contenido = await response.Content.ReadAsStringAsync();
                var empleado = JsonConvert.DeserializeObject<ApiResponsedata<EmpleadoModel>>(contenido);
                return Json(empleado.data, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return new HttpStatusCodeResult(500, "Error al buscar empleado.");
            }


        }
        public class CustomApiResponse<T>
        {
            public int status { get; set; }
            public string message { get; set; }
            public T result { get; set; }
        }
        public class ApiResponsedata<T>
        {
            [JsonProperty("status")]
            public int StatusCode { get; set; }
            public string Message { get; set; }
            public T data { get; set; }
        }
    }

}



