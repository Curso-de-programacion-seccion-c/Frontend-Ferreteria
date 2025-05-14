using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FerreteriaWebApp.Models
{
    public class FacturaModel
    {
        public int idFactura { get; set; }
        public int idEmpleado { get; set; }
        public int idCliente { get; set; }
        public DateTime fecha { get; set; }
        public decimal Total_Pago { get; set; }
        public byte idFormaPago { get; set; }
    }

    public class ListarFacturas
    {
        public int idFactura { get; set; }
        public DateTime fecha { get; set; }
        public decimal Total_Pago { get; set; }
        public int idEmpleado { get; set; }
        public string nombreEmpleado { get; set; }
        public int idCliente { get; set; }
        public string nombreCliente { get; set; }
        public byte idFormaPago { get; set; }
        public string nombreFormaPago { get; set; }
    }

    public class ClienteData
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

    public class EmpleadoDB
    {
        public int idEmpleado;
        public string Dpi;
        public string Nombre;
        public string Apellido;
        public string Puesto;
        public string CorreoElectronico;
        public string Telefono;
        public string FechaContratacion { get; set; }
        public byte IdRol { get; set; }
        public string nombreRol { get; set; }
        public decimal Sueldo { get; set; }
    }

    public class FormasPagoDB
    {
        public byte idFormaPago;
        public string NombreFormaPago;
        public string Descripcion;
    }
}