using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FerreteriaWebApp.Models
{
    public class ApiResponse<T>
    {
        [JsonProperty("status")]
        public int StatusCode { get; set; }
        public string Message { get; set; }
        [JsonProperty("result")]
        public T data { get; set; }
    }

}