using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace FerreteriaWebApp.Models
{
    public class FormaPagoModel
    {
       public byte IdFormaPago { get; set; }
       public string NombreFormaPago { get; set; }
     public  string Descripcion { get; set; }
            }

    public class ResponseFormaPago
    {
        public int respuesta { get; set; }
        public string descripcion_respuesta { get; set; }
    }
}