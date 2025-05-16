using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace FerreteriaWebApp.Models
{
	public class EmpleadoModel
	{
        public int idEmpleado { get; set; }
        public string Dpi { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Puesto { get; set; }
        public string CorreoElectronico { get; set; }
        public string Telefono { get; set; }
        public string FechaContratacion { get; set; }
        public byte IdRol { get; set; }
        public string nombreRol { get; set; }
        public decimal Sueldo { get; set; }
    }
    public class ResponseEmpleado
    {
        public int respuesta { get; set; }
        public string descripcion_respuesta { get; set; }
    }

}