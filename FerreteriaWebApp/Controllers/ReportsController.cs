using FerreteriaWebApp.Models;
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

        [HttpGet]
        public async Task<ActionResult> GenerarReporte(int idFactura)
        {
            var datos = await ObtenerDatosFactura(idFactura);

            if (datos == null)
            {
                return HttpNotFound();
            }

            ReportViewer reportViewer = new ReportViewer();
            reportViewer.ProcessingMode = ProcessingMode.Local;
            reportViewer.LocalReport.ReportPath = Server.MapPath("~/Reportes/FacturaId.rdlc");

            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("FacturaIdDataSet", datos.Tables[0]));

            byte[] bytes = reportViewer.LocalReport.Render("PDF");

            return File(bytes, "application/pdf", "Factura.pdf");
        }

        public async Task<ActionResult> VisualizarReporte(int idFactura)
        {
            var datos = await ObtenerDatosFactura(idFactura);
            if (datos == null)
            {
                return new HttpStatusCodeResult(404, "Datos no encontrados");
            }

            ReportViewer reportViewer = new ReportViewer();
            reportViewer.ProcessingMode = ProcessingMode.Local;
            reportViewer.LocalReport.ReportPath = Server.MapPath("~/Reportes/FacturaId.rdlc");
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("FacturaIdDataSet", datos.Tables[0]));

            byte[] bytes = reportViewer.LocalReport.Render("PDF");
            return File(bytes, "application/pdf");
        }

        public async Task<DataSet> ObtenerDatosFactura(int idFactura)
        {
            _httpClient.BaseAddress = new Uri("https://localhost:44333/");
            var response = await _httpClient.GetAsync($"api/Reportes/FacturaDetalle/{idFactura}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                var resp = JsonConvert.DeserializeObject<ApiResponse<List<ReporteFacturaDetalle>>>(content);

                return ConvertToDataSet(resp.data);
            }
            return null;
        }

        private DataSet ConvertToDataSet(List<ReporteFacturaDetalle> reporteFactura)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            // Agrega las columnas necesarias
            dt.Columns.Add("IdFactura", typeof(int));

            // Agrega las filas
            foreach (var item in reporteFactura)
            {
                DataRow row = dt.NewRow();
                row["IdFactura"] = item.IdFactura;
                dt.Rows.Add(row);
            }

            ds.Tables.Add(dt);
            return ds;
        }


    }
}