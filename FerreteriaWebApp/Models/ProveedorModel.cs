using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FerreteriaWebApp.Models
{
	public class ProveedorModel
	{
        public byte IdProveedor { get; set; }
        public string NombreProveedor { get; set; }
        public string Telefono { get; set; }
        public string NombreContacto { get; set; }
    }
    public class ResponseProveedor
    {
        public int respuesta { get; set; }
        public string descripcion_respuesta { get; set; }
    }
    public class ProveedorResponse
    {
        public int Status { get; set; }
        public string Message { get; set; }
        public List<ProveedorModel> Result { get; set; }
    }

}