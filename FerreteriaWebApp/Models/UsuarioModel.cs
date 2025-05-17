using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FerreteriaWebApp.Models
{
	public class UsuarioModel
	{
        public int IdUsuario { get; set; }
        public int IdEmpleado { get; set; }
        public string NombreEmpleado { get; set; }
        public string CodigoUsuario { get; set; }
        public string Username { get; set; }
        public string UserPassword { get; set; }
        public byte IsActive { get; set; }
    }
    
    public class responseUsuario
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
    public class UsuarioResponse
    {
        public int Status { get; set; }
        public string Message { get; set; }
        public List<UsuarioModel> Result { get; set; }
    }
    //public class EmpleadoDB
    //{
    //    public int idEmpleado;
    //    public string Dpi;
    //    public string Nombre;
    //    public string Apellido;
    //    public string Puesto;
    //    public string CorreoElectronico;
    //    public string Telefono;
    //    public string FechaContratacion { get; set; }
    //    public byte IdRol { get; set; }
    //    public string nombreRol { get; set; }
    //    public decimal Sueldo { get; set; }
    //}
}