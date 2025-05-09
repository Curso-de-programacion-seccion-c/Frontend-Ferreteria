using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FerreteriaWebApp.Models
{
    public class RolesModel
    {
        public byte IdRol { get; set; }

        public string Nombre { get; set; }

        public decimal Sueldo { get; set; }
    }

    public class ResponseRol
    {
        public int respuesta { get; set; }
        public string descripcion_respuesta { get; set; }
    }
}