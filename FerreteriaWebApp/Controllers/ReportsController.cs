using FerreteriaWebApp.Models;
using FerreteriaWebApp.Services;
using Microsoft.Reporting.WebForms;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace FerreteriaWebApp.Controllers
{
    public class ReportsController : Controller
    {
        private readonly HttpClient _httpClient = new HttpClient();
        private readonly ApiService<ReporteFacturaDetalle> apiService = new ApiService<ReporteFacturaDetalle>("api/Reportes/FacturaDetalle/");

        // GET: Reports
        public ActionResult Index(int? idFactura)
        {
            return View("FacturaId", idFactura);
        }

        public ActionResult Factura(int? idFactura)
        {
            return View("FacturaId", idFactura);
        }

        public ActionResult PorFechas()
        {
            ViewBag.ReporteGenerado = false;
            return View("FacturasPorFechas");
        }

        #region Factura por ID

        [HttpGet]
        public async Task<ActionResult> GenerarReporte(int idFactura)
        {
            var datos = await ObtenerDatosFactura(idFactura);

            var pdfServices = new PdfServices();
            byte[] pdfReporte = pdfServices.GenerarReporteIdFactura(datos);

            if (pdfReporte == null)
            {
                return new HttpStatusCodeResult(404, "Datos no encontrados");
            }

            return File(pdfReporte, "application/pdf", $"Factura_{idFactura}.pdf");
        }

        public async Task<ActionResult> VisualizarReporte(int idFactura)
        {
            var datos = await ObtenerDatosFactura(idFactura);

            if(datos == null || datos.Count == 0)
            {
                return new HttpStatusCodeResult(404, "Datos no encontrados");
            }

            var pdfServices = new PdfServices();
            byte[] pdfReporte = pdfServices.GenerarReporteIdFactura(datos);

            if (pdfReporte == null)
            {
                return new HttpStatusCodeResult(404, "Datos no encontrados");
            }

            Response.AppendHeader("Content-Disposition", "inline; filename=Factura_" + idFactura + ".pdf");
            return File(pdfReporte, "application/pdf");
        }

        public async Task<List<ReporteFacturaDetalle>> ObtenerDatosFactura(int idFactura)
        {
            _httpClient.BaseAddress = new Uri("https://localhost:44333/");
            var response = await _httpClient.GetAsync($"api/Reportes/FacturaDetalle/{idFactura}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                var resp = JsonConvert.DeserializeObject<ApiResponse<List<ReporteFacturaDetalle>>>(content);

                return resp.data;
            }
            return null;
        }
        #endregion


        #region Reportes por fechas
        [HttpPost]
        public async Task<ActionResult> GenerarReportePorFechas(DateTime FechaInicio, DateTime FechaFin)
        {
            var reportByDate = new BodyReqReportByDate
            {
                fechaInicio = FechaInicio,
                fechaFin = FechaFin
            };

            var datos = await ObtenerDatosPorFechas(reportByDate);
            if (datos == null || datos.Count == 0)
            {
                TempData["Error"] = "No se encontraron datos para el rango de fechas seleccionado.";
                return RedirectToAction("FacturasPorFechas");
            }

            // Guardar las fechas en ViewBag para que el iframe pueda usarlas
            ViewBag.FechaInicio = FechaInicio.ToString("yyyy-MM-dd");
            ViewBag.FechaFin = FechaFin.ToString("yyyy-MM-dd");
            ViewBag.ReporteGenerado = true;

            return View("FacturasPorFechas");
        }

        public async Task<List<ResponseReporteFecha>> ObtenerDatosPorFechas(BodyReqReportByDate reportByDate)
        {
            _httpClient.BaseAddress = new Uri("https://localhost:44333/");

            var body = new
            {
                fechaInicio = reportByDate.fechaInicio.ToString("yyyy-MM-dd"),
                fechaFin = reportByDate.fechaFin.ToString("yyyy-MM-dd")
            };

            var json = JsonConvert.SerializeObject(body);
            var stringContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("api/Reportes/FacturaPorFecha", stringContent);

            if (response.IsSuccessStatusCode)
            {
                var contentResponse = await response.Content.ReadAsStringAsync();
                var resp = JsonConvert.DeserializeObject<ApiResponse<List<ResponseReporteFecha>>>(contentResponse);
                return resp.data;
            }
            return null;
        }


        [HttpGet]
        public async Task<ActionResult> VisualizarReportePorFechas(string fechaInicio, string fechaFin)
        {
            var reportByDate = new BodyReqReportByDate
            {
                fechaInicio = DateTime.Parse(fechaInicio),
                fechaFin = DateTime.Parse(fechaFin)
            };

            var datos = await ObtenerDatosPorFechas(reportByDate);
            if (datos == null || datos.Count == 0)
            {
                return new HttpStatusCodeResult(404, "Datos no encontrados");
            }

            var pdfServices = new PdfServices();
            byte[] pdfReporte = pdfServices.GenerarReportePorFecha(datos, reportByDate.fechaInicio, reportByDate.fechaFin);
            if (pdfReporte == null)
            {
                return new HttpStatusCodeResult(404, "Datos no encontrados");
            }

            // Configuración importante para mostrar dentro del iframe
            Response.AppendHeader("Content-Disposition", "inline; filename=FacturasPorFechas.pdf");
            return File(pdfReporte, "application/pdf");
        }

        [HttpGet]
        public async Task<ActionResult> DescargarReportePorFechas(string fechaInicio, string fechaFin)
        {
            var reportByDate = new BodyReqReportByDate
            {
                fechaInicio = DateTime.Parse(fechaInicio),
                fechaFin = DateTime.Parse(fechaFin)
            };

            var datos = await ObtenerDatosPorFechas(reportByDate);
            if (datos == null || datos.Count == 0)
            {
                return new HttpStatusCodeResult(404, "Datos no encontrados");
            }

            var pdfServices = new PdfServices();
            byte[] pdfReporte = pdfServices.GenerarReportePorFecha(datos, reportByDate.fechaInicio, reportByDate.fechaFin);
            if (pdfReporte == null)
            {
                return new HttpStatusCodeResult(404, "Datos no encontrados");
            }

            // Para descarga, usamos "attachment" en lugar de "inline"
            return File(pdfReporte, "application/pdf", $"Reporte_{fechaInicio}_{fechaFin}.pdf");
        }
        #endregion
    }
}