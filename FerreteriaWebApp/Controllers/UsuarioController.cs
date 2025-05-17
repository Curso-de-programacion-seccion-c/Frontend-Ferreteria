using FerreteriaWebApp.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace FerreteriaWebApp.Controllers

{

    public class UsuarioController : Controller
    {
        private readonly HttpClient _httpClient = new HttpClient();

        public async Task<ActionResult> Index()
        {
            _httpClient.BaseAddress = new Uri("https://localhost:44333/");

            var response = await _httpClient.GetAsync("rest/api/ObtenerUsuarios");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                // Deserializa como UsuarioResponse, no como lista directa
                // var apiResponse = JsonConvert.DeserializeObject<UsuarioResponse>(content);

                // var listaUsuario = apiResponse?.Result ?? new List<UsuarioModel>();
                var listaUsuario = JsonConvert.DeserializeObject<List<UsuarioModel>>(content);


                return View("UsuarioView", listaUsuario);
            }

            return View("UsuarioView", new List<UsuarioModel>()); // Vista vacía si falla

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

        [HttpPost]
        public async Task<JsonResult> AgregarUsuario(UsuarioModel usuarioModel)
        {
            try
            {
                _httpClient.BaseAddress = new Uri("https://localhost:44333/");
                var json = JsonConvert.SerializeObject(usuarioModel);
                var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync($"rest/api/CrearUsuario", content);

                if (response.IsSuccessStatusCode)
                {
                    var contertResponse = await response.Content.ReadAsStringAsync();
                    var resp = JsonConvert.DeserializeObject<CustomApiResponse<UsuarioModel>>(contertResponse);

                    if (resp != null || resp.result != null)
                    {
                        return Json(new { success = true, message = "Usuario Agregado correctamente." }, JsonRequestBehavior.AllowGet);
                    }

                    return Json(new { success = false, message = "Erro al agregar."}, JsonRequestBehavior.AllowGet);
                }

                return Json(new { success = false, message = "Error al agregar el Usuario." }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { success = false, message = "Error al agregar el Usuario." }, JsonRequestBehavior.AllowGet);
            }
        }

        /*[HttpPost]
        public async Task<JsonResult> AgregarUsuario(UsuarioModel usuarioModel)
        {
            _httpClient.BaseAddress = new Uri("https://localhost:44333/");
            var json = JsonConvert.SerializeObject(usuarioModel);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("rest/api/CrearUsuario", content);

            if (response.IsSuccessStatusCode)
            {
                var cont = await response.Content.ReadAsStringAsync();
                var res = JsonConvert.DeserializeObject<responseUsuario>(cont);
                if (res.respuesta == 1)
                {
                    return Json(new { success = true, message = "Usuario creado correctamente." }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { success = false, message = res.descripcion_respuesta }, JsonRequestBehavior.AllowGet);
                }
                
            }
            return Json(new { success = false, message = "Error al crear el usuario." }, JsonRequestBehavior.AllowGet);

        }*/

        [HttpPost]
        public async Task<JsonResult> EditarUsuario(UsuarioModel usuarioModel)
        {
            try
            {
                _httpClient.BaseAddress = new Uri("https://localhost:44333/");
                var json = JsonConvert.SerializeObject(usuarioModel);
                var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

                var response = await _httpClient.PutAsync($"rest/api/ActualizarUsuario", content);

                if (response.IsSuccessStatusCode)
                {
                    var contertResponse = await response.Content.ReadAsStringAsync();
                    var resp = JsonConvert.DeserializeObject<CustomApiResponse<UsuarioModel>>(contertResponse);

                    if (resp != null || resp.result != null)
                    {
                        return Json(new { success = true, message = "Usuario Editado Correctamente" }, JsonRequestBehavior.AllowGet);
                    }

                    return Json(new { success = false, message = "Error al editar el Usuario", data = resp.result }, JsonRequestBehavior.AllowGet);
                }

                return Json(new { success = false, message = "Error al editar el Usuario." }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { success = false, message = "Error al editar el Usuario" }, JsonRequestBehavior.AllowGet);
            }
        }

        /*[HttpPost]
        public async Task<ActionResult> EditarUsuario(UsuarioModel usuario)
        {
            _httpClient.BaseAddress = new Uri("https://localhost:44333/");

            var newBody = new
            {
                IdUsuario = usuario.IdUsuario,
                IdEmpleado = usuario.IdEmpleado,
                NombreEmpleado = usuario.NombreEmpleado,
                CodigoUsuario = usuario.CodigoUsuario,
                Username = usuario.Username,
                UserPassword = usuario.UserPassword,
                IsActive = usuario.IsActive
            };

            var json = JsonConvert.SerializeObject(newBody);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"rest/api/ActualizarUsuario/{usuario.IdUsuario}", content);

            if (response.IsSuccessStatusCode)
            {
                return new HttpStatusCodeResult(200);
            }

            return new HttpStatusCodeResult(500);
        }*/


        [HttpPost]
        public async Task<ActionResult> EliminarUsuario(int IdUsuario)
        {
            _httpClient.BaseAddress = new Uri("https://localhost:44333/");
            var response = await _httpClient.DeleteAsync($"rest/api/EliminarUsuario/{IdUsuario}");

            if (response.IsSuccessStatusCode)
            {
                return Json(new { success = true, message = "Usuario eliminado correctamente." }, JsonRequestBehavior.AllowGet);
            }

            return new HttpStatusCodeResult(500);
        }

        public async Task<ActionResult> BuscarPorId(int? id)
        {
            _httpClient.BaseAddress = new Uri("https://localhost:44333/");

            if (id == null || id == 0)
            {
                var responseAll = await _httpClient.GetAsync("rest/api/ObtenerUsuarios");
                if (!responseAll.IsSuccessStatusCode)
                {
                    return new HttpStatusCodeResult(500, "Error al obtener los Usuarios.");
                }

                var contenido = await responseAll.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<List<UsuarioModel>>(contenido);

                // Siempre devolver un array, incluso si es vacío
                return Json(data, JsonRequestBehavior.AllowGet);
            }

            var response = await _httpClient.GetAsync($"rest/api/ObtenerUsuarioPorId/{id}");
            if (response.IsSuccessStatusCode)
            {
                var contenido = await response.Content.ReadAsStringAsync();
                var usuario = JsonConvert.DeserializeObject<UsuarioModel>(contenido);
                return Json(usuario, JsonRequestBehavior.AllowGet);
            }

            return new HttpStatusCodeResult(500, "Error al buscar el usuario.");
        }
            public class CustomApiResponse<T>
        {
            public int status { get; set; }
            public string message { get; set; }
            public T result { get; set; }
        }
    }
    
}