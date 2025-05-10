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
                return new HttpStatusCodeResult(200);
            }
            else
            {
                return new HttpStatusCodeResult(500);
            }
        }

        public class EliminarRolRequest
        {
            public int idRol { get; set; }
        }
    }    
}