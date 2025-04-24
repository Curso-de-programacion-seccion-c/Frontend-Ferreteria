using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace FerreteriaWebApp.Models
{
    public class ArticulosModel
    {
        public int IdArticulo { get; set; }
        public int CodeArticulo { get; set; }
        public string NombreArticulo { get; set; }
        public decimal Precio { get; set; }
        public int Stock { get; set; }
        public string Descripcion { get; set; }
        public int IdCategoria { get; set; }
        public int IdProveedor { get; set; }
    }

  //  return View("Articulos");
}