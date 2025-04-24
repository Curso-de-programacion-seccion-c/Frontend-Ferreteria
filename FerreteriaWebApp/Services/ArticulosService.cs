using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FerreteriaWebApp.Models;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace FerreteriaWebApp.Services
{
    public class ArticulosService
    {
        private readonly string _baseUrl = "https://localhost:44300/Articulos";
    }
}