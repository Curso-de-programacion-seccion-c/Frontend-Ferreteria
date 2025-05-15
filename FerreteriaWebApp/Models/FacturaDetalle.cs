using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FerreteriaWebApp.Models
{
    public class FacturaDetalle
    {
        public int IdDetalleVenta { get; set; }
        public int IdFactura { get; set; }
        public int IdArticulo { get; set; }
        public int Cantidad { get; set; }
        public string NombreArticulo { get; set; }
        public int Stock { get; set; }
    }

    public class ListarDetalleVenta
    {
        public int IdDetalleVenta { get; set; }
        public int IdFactura { get; set; }
        public int IdArticulo { get; set; }
        public int Cantidad { get; set; }
        public string NombreArticulo { get; set; }
        public decimal PrecioUnitario { get; set; }
        public DateTime FechaFactura { get; set; }
        public string NombreCliente { get; set; }
    }
}