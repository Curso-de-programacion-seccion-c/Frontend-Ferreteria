using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FerreteriaWebApp.Models
{
    public class ClientesModel
    {
        public int IdCliente { get; set; }
        public string Dpi { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string NIT { get; set; }
        public string CorreoElectronico { get; set; }
        public string Telefono { get; set; }
        public DateTime FechaRegistro { get; set; }
    }

    public class ResponseClientes
    {
        public string Respuesta { get; set; }
        public string DescripcionRespuesta { get; set; }
    }
}