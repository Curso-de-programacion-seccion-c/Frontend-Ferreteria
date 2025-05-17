using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FerreteriaWebApp.Models
{
	public class LoginRequest
	{
        public string Username { get; set; }
        public string UserPassword { get; set; }
        public string CodigoUsuario { get; set; }
    }
    public class ResponseUsuario
    {
        public int respuesta { get; set; }
        public string descripcion_respuesta { get; set; }
        public int IdUsuario { get; set; }
        public string Username { get; set; }
        public string CodigoUsuario { get; set; }
        public int IdEmpleado { get; set; }
        public string NombreEmpleado { get; set; }
        public string ApellidoEmpleado { get; set; }
        public string Puesto { get; set; }
        public string CorreoElectronico { get; set; }
        public string Telefono { get; set; }
        public int IdRol { get; set; }
        public string NombreRol { get; set; }
        public decimal Sueldo { get; set; }
    }
}