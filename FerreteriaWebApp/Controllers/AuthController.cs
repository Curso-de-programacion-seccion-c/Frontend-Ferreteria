using FerreteriaWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Net.Http.Json;
using Newtonsoft.Json;

namespace FerreteriaWebApp.Controllers
{
    public class AuthController : Controller
    {
        [HttpGet]
        public ActionResult LoginView()
        {
            return View();
        }
        public ActionResult Logout()
        {
            Session.Clear(); // 
            Session.Abandon(); // Finaliza la sesión

            return RedirectToAction("Auth"); // Redirige al login
        }

        [HttpPost]
        public async Task<JsonResult> VerificarLogin(LoginRequest request)
        {
            using (HttpClient client = new HttpClient())
            {
                //var json = JsonConvert.SerializeObject(facturaModel);
                //var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                //var response = await _httpClient.PostAsync($"api/factura", content);
                client.BaseAddress = new Uri("https://localhost:44333/"); // Asegúrate que este puerto sea el de tu API
                var json = JsonConvert.SerializeObject(request);
                var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                var response = await client.PostAsync("rest/api/LoginUsuario", content);

                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<ResponseUsuario>(jsonString);

                    if (result != null && result.respuesta == 1)
                    {
                        // Solo para ejemplo: guardar el nombre en sesión
                        Session["NombreUsuario"] = result.NombreEmpleado + " " + result.ApellidoEmpleado;

                        return Json(new { success = true }); // 🔁 Aquí tu JS hará el redirect
                    }
                }
            }

            return Json(new { success = false });
        }
    }

}
