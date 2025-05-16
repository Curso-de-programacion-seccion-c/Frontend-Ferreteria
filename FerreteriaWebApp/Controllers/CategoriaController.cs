using FerreteriaWebApp.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace FerreteriaWebApp.Controllers
{
    public class CategoriaController : Controller
    {
        private readonly HttpClient _httpClient = new HttpClient();

        //LISTAR TODAS LAS CATEGORIAS
        public async Task<ActionResult> Index()
        {
            _httpClient.BaseAddress = new Uri("https://localhost:44333/");

            var response = await _httpClient.GetAsync("api/categorias");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                dynamic data = JsonConvert.DeserializeObject(json);
                var categoriasJson = JsonConvert.SerializeObject(data.result);
                var categorias = JsonConvert.DeserializeObject<List<CategoriaModel>>(categoriasJson);
                return View("CategoriaView", categorias);
            }

            return View("CategoriaView", new List<CategoriaModel>());
        }

        //BUSCAR POR ID
        [HttpPost]
        public async Task<ActionResult> BuscarPorId(string idCategoria)
        {
            _httpClient.BaseAddress = new Uri("https://localhost:44333/");
            ViewBag.IdBuscado = idCategoria;

            if (string.IsNullOrWhiteSpace(idCategoria))
            {
                var responseTodo = await _httpClient.GetAsync("api/categorias");
                if (responseTodo.IsSuccessStatusCode)
                {
                    var jsonTodo = await responseTodo.Content.ReadAsStringAsync();
                    dynamic dataTodo = JsonConvert.DeserializeObject(jsonTodo);
                    var categoriasJsonTodo = JsonConvert.SerializeObject(dataTodo.result);
                    List<CategoriaModel> categoriasTodo = JsonConvert.DeserializeObject<List<CategoriaModel>>(categoriasJsonTodo);
                    return View("CategoriaView", categoriasTodo);
                }

                return View("CategoriaView", new List<CategoriaModel>());
            }

            var response = await _httpClient.GetAsync("api/categorias");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                dynamic data = JsonConvert.DeserializeObject(json);
                var categoriasJson = JsonConvert.SerializeObject(data.result);
                List<CategoriaModel> categorias = JsonConvert.DeserializeObject<List<CategoriaModel>>(categoriasJson);

                if (byte.TryParse(idCategoria, out byte id))
                {
                    var categoriaEncontrada = categorias.Find(c => c.IdCategoria == id);
                    var resultado = new List<CategoriaModel>();
                    if (categoriaEncontrada != null)
                        resultado.Add(categoriaEncontrada);

                    return View("CategoriaView", resultado);
                }
            }

            return View("CategoriaView", new List<CategoriaModel>());
        }

        
        //CREAR CATEGORIA
        [HttpPost]
        public async Task<ActionResult> CrearCategoria(string nombreCategoria)
        {
            _httpClient.BaseAddress = new Uri("https://localhost:44333/");

            var nuevaCategoria = new CategoriaModel
            {
                NombreCategoria = nombreCategoria
            };

            var json = JsonConvert.SerializeObject(nuevaCategoria);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("api/categorias", content);
            return response.IsSuccessStatusCode ? new HttpStatusCodeResult(200) : new HttpStatusCodeResult(500);
        }

        //EDITAR CATEGORIA
        [HttpPost]
        public async Task<ActionResult> EditarCategoria(byte idCategoria, string nombreCategoria)
        {
            _httpClient.BaseAddress = new Uri("https://localhost:44333/");

            var categoriaEditada = new CategoriaModel
            {
                IdCategoria = idCategoria,
                NombreCategoria = nombreCategoria
            };

            var json = JsonConvert.SerializeObject(categoriaEditada);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync($"api/categorias/{idCategoria}", content);
            return response.IsSuccessStatusCode ? new HttpStatusCodeResult(200) : new HttpStatusCodeResult(500);
        }

        //ELIMINAR CATEGORIA
        [HttpPost]
        public async Task<ActionResult> EliminarCategoria(byte idCategoria)
        {
            _httpClient.BaseAddress = new Uri("https://localhost:44333/");

            var response = await _httpClient.DeleteAsync($"api/categorias/{idCategoria}");

            return response.IsSuccessStatusCode
                ? new HttpStatusCodeResult(200)
                : new HttpStatusCodeResult(500);
        }
    }
}
