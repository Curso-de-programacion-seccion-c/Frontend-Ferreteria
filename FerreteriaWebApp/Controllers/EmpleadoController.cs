using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FerreteriaWebApp.Models;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

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

                ViewBag.Roles = new SelectList(await GetRoles(), "idRol", "Nombre");
                return View("Index", resp.result);
            }

            return View(new List<EmpleadoModel>());
        }

        public async Task<List<RolesModel>> GetRoles()
        {
            //_httpClient.BaseAddress = new Uri("https://localhost:44333/");

            var response = await _httpClient.GetAsync($"rest/api/obtenerRoles");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var resp = JsonConvert.DeserializeObject<List<RolesModel>>(content);
                return resp;
            }

            return new List<RolesModel>();
        }

        [HttpPost]
        public async Task<JsonResult> Agregar(EmpleadoModel empleado)
        {
            _httpClient.BaseAddress = new Uri("https://localhost:44333/");
            var json = JsonConvert.SerializeObject(empleado);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("/api/empleados", content);

            var contentresponse = await response.Content.ReadAsStringAsync();
            var apiResp = JsonConvert.DeserializeObject<ApiResponsedata<string>>(contentresponse);

            if (response.IsSuccessStatusCode)
            {
                return Json(new { success = true, message = "Empleado agregado correctamente." }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                if (apiResp.Status == 500)
                {
                    return Json(new { success = false, message = "Error al agregar el empleado." }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { success = false, message = apiResp.Message }, JsonRequestBehavior.AllowGet);
                }
            }

        }

        [HttpPost]
        public async Task<JsonResult> Editar(EmpleadoModel empleado)
        {
            _httpClient.BaseAddress = new Uri("https://localhost:44333/");
            var json = JsonConvert.SerializeObject(empleado);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync($"/api/empleados/{empleado.idEmpleado}", content);

            var contentresponse = await response.Content.ReadAsStringAsync();
            var apiResp = JsonConvert.DeserializeObject<ApiResponsedata<string>>(contentresponse);

            if (response.IsSuccessStatusCode)
            {
                return Json(new { success = true, message = "Empleado editado correctamente." }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                if (apiResp.Status == 500)
                {
                    return Json(new { success = false, message = "Error al editar el empleado." }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { success = false, message = apiResp.Message }, JsonRequestBehavior.AllowGet);
                }
            }
        }

        [HttpPost]
        public async Task<JsonResult> Eliminar(int id)
        {
            _httpClient.BaseAddress = new Uri("https://localhost:44333/");
            var response = await _httpClient.DeleteAsync($"/api/empleados/{id}");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var apiResp = JsonConvert.DeserializeObject<CustomApiResponse<string>>(content);

                if (apiResp.status != 200)
                {
                    if (apiResp.message.Contains("FK"))
                    {
                        return Json(new { success = false, message = "Error al eliminar el empleado." }, JsonRequestBehavior.AllowGet);
                    }
                }

                return Json(new { success = true, message = "Empleado eliminado correctamente." }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var content = await response.Content.ReadAsStringAsync();
                var apiResp = JsonConvert.DeserializeObject<CustomApiResponse<string>>(content);
                return Json(new { success = false, message = apiResp.message }, JsonRequestBehavior.AllowGet);
            }
        }

        public async Task<JsonResult> BuscarPorId(int? id)
        {
            _httpClient.BaseAddress = new Uri("https://localhost:44333/");

            if(id == null)
            {
                //Buscar todos
                var response = await _httpClient.GetAsync($"/api/empleados");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var resp = JsonConvert.DeserializeObject<CustomApiResponse<List<EmpleadoModel>>>(content);
                    return Json(new { success = true, data = resp.result }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { success = false, message = "Error al buscar empleados.", data = new List<EmpleadoModel>() }, JsonRequestBehavior.AllowGet);
                }
            }

            var responseId = await _httpClient.GetAsync($"/api/empleados/{id}");
            if (responseId.IsSuccessStatusCode)
            {
                var content = await responseId.Content.ReadAsStringAsync();
                var resp = JsonConvert.DeserializeObject<ApiResponsedata<EmpleadoModel>>(content);
                return Json(new { success = true, data = resp.data }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { success = false, message = "Error al buscar empleado.", data = new List<EmpleadoModel>() }, JsonRequestBehavior.AllowGet);
            }
        }

        // Estructuras para mapear respuesta de API
        public class CustomApiResponse<T>
        {
            public int status { get; set; }
            public string message { get; set; }
            public T result { get; set; }
        }

        public class ApiResponsedata<T>
        {
            public int Status { get; set; }
            public string Message { get; set; }
            public T data { get; set; }
        }
    }
}
