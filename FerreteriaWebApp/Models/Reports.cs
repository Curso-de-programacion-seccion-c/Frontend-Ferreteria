using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FerreteriaWebApp.Models
{
    public class ReporteFacturaDetalle
    {
        public int IdFactura { get; set; }
        public DateTime Fecha { get; set; }
        public decimal Total_Pago { get; set; }
        public int IdCliente { get; set; }
        public string NombreCliente { get; set; }
        public string telefono { get; set; }
        public string NitCliente { get; set; }
        public string DpiCliente { get; set; }
        public int IdEmpleado { get; set; }
        public string NombreEmpleado { get; set; }
        public string DpiEmpleado { get; set; }
        public string Puesto { get; set; }
        public string EmailEmpleado { get; set; }
        public string RolDelEmpleado { get; set; }
        public string NombreFormaPago { get; set; }
        public int IdDetalleVenta { get; set; }
        public int IdArticulo { get; set; }
        public string NombreArticulo { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }

        public decimal Subtotal { get; set; }
        public decimal PrecioSinIVA { get; set; }
        public decimal IVA { get; set; }
    }

    public class BodyReqReportByDate
    {
        public DateTime fechaInicio { get; set; }
        public DateTime fechaFin { get; set; }
    }

    public class ResponseReporteFecha
    {
        public int IdFactura { get; set; }
        public DateTime Fecha { get; set; }
        public decimal Total_Pago { get; set; }
        public int IdCliente { get; set; }
        public string NombreCliente { get; set; }
        public string telefono { get; set; }
        public string NIT { get; set; }
        public int IdEmpleado { get; set; }
        public string NombreEmpleado { get; set; }
        public string RolDelEmpleado { get; set; }
        public string FormaPago { get; set; }

    }

}