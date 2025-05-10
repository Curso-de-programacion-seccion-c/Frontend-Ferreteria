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
    public class RolesController : Controller
    {
        private readonly HttpClient _httpClient = new HttpClient();


        public async Task<ActionResult> Index()
        {
            _httpClient.BaseAddress = new Uri("https://localhost:44333/");

            var response = await _httpClient.GetAsync($"rest/api/obtenerRoles");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                var resp = JsonConvert.DeserializeObject<List<RolesModel>>(content);

                return View("Index",resp);
            }
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> AgregarRol(RolesModel rolesModel)
        {
            _httpClient.BaseAddress = new Uri("https://localhost:44333/");
            var json = JsonConvert.SerializeObject(rolesModel);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"rest/api/insertarRol", content);

            if (response.IsSuccessStatusCode)
            {
                var contentResponse = await response.Content.ReadAsStringAsync();
                var resp = JsonConvert.DeserializeObject<ResponseRol>(contentResponse);
                return new HttpStatusCodeResult(200);
            }
            else
            {
                return new HttpStatusCodeResult(500);
            }
        }

        [HttpPost]
        public async Task<ActionResult> EditarRol(RolesModel roles)
        {
            _httpClient.BaseAddress = new Uri("https://localhost:44333/");
            
            var newBody = new
            {
                Nombre = roles.Nombre,
                Sueldo = roles.Sueldo
            };

            var json = JsonConvert.SerializeObject(newBody);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"rest/api/actualizarRol/{roles.IdRol}", content);

            if (response.IsSuccessStatusCode)
            {
                var contentResponse = await response.Content.ReadAsStringAsync();
                var resp = JsonConvert.DeserializeObject<ResponseRol>(contentResponse);
                return new HttpStatusCodeResult(200);
            }
            else
            {
                return new HttpStatusCodeResult(500);
            }
        }

        [HttpPost]
        public async Task<ActionResult> EliminarRol(int IdRol)
        {
            _httpClient.BaseAddress = new Uri("https://localhost:44333/");
            var response = await _httpClient.DeleteAsync($"rest/api/eliminarRol/{IdRol}");

            if (response.IsSuccessStatusCode)
            {
                var contentReponse = await response.Content.ReadAsStringAsync();
                var roleResponse = JsonConvert.DeserializeObject<ResponseRol>(contentReponse);

                if (roleResponse.respuesta == 0)
                {
                    if(roleResponse.descripcion_respuesta.Contains("FK"))
                    {
                        return Json(new { success = false, message = "No se puede eliminar el rol porque está asociado a un empleado." }, JsonRequestBehavior.AllowGet);
                    }

                    return Json(new { success = false, message = "Error al eliminar el rol." }, JsonRequestBehavior.AllowGet);
                }

                return Json(new { success = true, message = "Rol eliminado correctamente." }, JsonRequestBehavior.AllowGet);
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
                var responseAll = await _httpClient.GetAsync("rest/api/obtenerRoles");
                if (responseAll.IsSuccessStatusCode)
                {
                    var contenido = await responseAll.Content.ReadAsStringAsync();
                    var lista = JsonConvert.DeserializeObject<List<RolesModel>>(contenido);
                    return Json(lista, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return new HttpStatusCodeResult(500, "Error al obtener los roles.");
                }
            }

            // Si hay ID, buscamos uno específico
            var response = await _httpClient.GetAsync($"rest/api/obtenerRol/{id}");
            if (response.IsSuccessStatusCode)
            {
                var contenido = await response.Content.ReadAsStringAsync();
                var rol = JsonConvert.DeserializeObject<RolesModel>(contenido);
                return Json(rol, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return new HttpStatusCodeResult(500, "Error al buscar el rol.");
            }
        }

    }
}