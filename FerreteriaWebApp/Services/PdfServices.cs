using FerreteriaWebApp.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Web;

namespace FerreteriaWebApp.Services
{
    public class PdfServices
    {

        #region Reporte por ID Factura
        public byte[] GenerarReporteIdFactura(List<ReporteFacturaDetalle> reporteFacturas)
        {
			try
			{
				using(MemoryStream ms = new MemoryStream())
				{
                    Document documento = new Document(PageSize.A4, 30, 30, 30, 30);
                    PdfWriter writer = PdfWriter.GetInstance(documento, ms);

                    documento.Open();

                    // Obtener el primer registro para datos generales (son los mismos en todos los registros)
                    var primerRegistro = reporteFacturas.First();

                    // Agregar título y logo
                    AgregarEncabezado(documento, primerRegistro.IdFactura);

                    // Información de la empresa
                    AgregarInformacionEmpresa(documento);

                    // Información de la factura
                    AgregarInformacionFactura(documento, primerRegistro);

                    // Información del cliente
                    AgregarInformacionCliente(documento, primerRegistro);

                    // Información del empleado
                    AgregarInformacionEmpleado(documento, primerRegistro);

                    // Tabla con detalles de artículos
                    AgregarTablaArticulos(documento, reporteFacturas);

                    // Información de total y forma de pago
                    AgregarInformacionPago(documento, primerRegistro);

                    // Pie de página
                    AgregarPiePagina(documento);

                    documento.Close();
                    return ms.ToArray();
                }
			}
			catch (Exception)
			{
				throw;
			}
        }

        private void AgregarEncabezado(Document documento, int idFactura)
        {
            // Título
            Paragraph titulo = new Paragraph($"FACTURA #{idFactura}", new Font(Font.FontFamily.HELVETICA, 18, Font.BOLD));
            titulo.Alignment = Element.ALIGN_CENTER;
            titulo.SpacingAfter = 15f;
            documento.Add(titulo);
        }

        private void AgregarInformacionEmpresa(Document documento)
        {
            // Información de la empresa
            Paragraph infoEmpresa = new Paragraph();
            infoEmpresa.Add(new Chunk("FERRETERÍA", new Font(Font.FontFamily.HELVETICA, 14, Font.BOLD)));
            infoEmpresa.Add(Chunk.NEWLINE);
            infoEmpresa.Add(new Chunk("Dirección: Calle Principal #123, Ciudad", new Font(Font.FontFamily.HELVETICA, 10)));
            infoEmpresa.Add(Chunk.NEWLINE);
            infoEmpresa.Add(new Chunk("Teléfono: (502) 2222-3333", new Font(Font.FontFamily.HELVETICA, 10)));
            infoEmpresa.Add(Chunk.NEWLINE);
            infoEmpresa.Add(new Chunk("NIT: 123456-7", new Font(Font.FontFamily.HELVETICA, 10)));
            infoEmpresa.Add(Chunk.NEWLINE);
            infoEmpresa.SpacingAfter = 15f;
            documento.Add(infoEmpresa);
        }

        private void AgregarInformacionFactura(Document documento, ReporteFacturaDetalle factura)
        {
            // Información de la factura
            PdfPTable tablaInfoFactura = new PdfPTable(2);
            tablaInfoFactura.WidthPercentage = 100;
            tablaInfoFactura.SetWidths(new float[] { 1f, 1f });

            PdfPCell celdaTituloFactura = new PdfPCell(new Phrase("DATOS DE LA FACTURA", new Font(Font.FontFamily.HELVETICA, 12, Font.BOLD)));
            celdaTituloFactura.Colspan = 2;
            celdaTituloFactura.HorizontalAlignment = Element.ALIGN_CENTER;
            celdaTituloFactura.BackgroundColor = new BaseColor(240, 240, 240);
            celdaTituloFactura.Padding = 5f;
            tablaInfoFactura.AddCell(celdaTituloFactura);

            tablaInfoFactura.AddCell(new Phrase("Número de Factura:", new Font(Font.FontFamily.HELVETICA, 10, Font.BOLD)));
            tablaInfoFactura.AddCell(new Phrase(factura.IdFactura.ToString(), new Font(Font.FontFamily.HELVETICA, 10)));

            tablaInfoFactura.AddCell(new Phrase("Fecha:", new Font(Font.FontFamily.HELVETICA, 10, Font.BOLD)));
            tablaInfoFactura.AddCell(new Phrase(factura.Fecha.ToString("dd/MM/yyyy"), new Font(Font.FontFamily.HELVETICA, 10)));

            tablaInfoFactura.AddCell(new Phrase("Forma de Pago:", new Font(Font.FontFamily.HELVETICA, 10, Font.BOLD)));
            tablaInfoFactura.AddCell(new Phrase(factura.NombreFormaPago, new Font(Font.FontFamily.HELVETICA, 10)));

            tablaInfoFactura.SpacingAfter = 15f;
            documento.Add(tablaInfoFactura);
        }

        private void AgregarInformacionCliente(Document documento, ReporteFacturaDetalle factura)
        {
            // Información del cliente
            PdfPTable tablaCliente = new PdfPTable(2);
            tablaCliente.WidthPercentage = 100;
            tablaCliente.SetWidths(new float[] { 1f, 1f });

            PdfPCell celdaTituloCliente = new PdfPCell(new Phrase("DATOS DEL CLIENTE", new Font(Font.FontFamily.HELVETICA, 12, Font.BOLD)));
            celdaTituloCliente.Colspan = 2;
            celdaTituloCliente.HorizontalAlignment = Element.ALIGN_CENTER;
            celdaTituloCliente.BackgroundColor = new BaseColor(240, 240, 240);
            celdaTituloCliente.Padding = 5f;
            tablaCliente.AddCell(celdaTituloCliente);

            tablaCliente.AddCell(new Phrase("Nombre:", new Font(Font.FontFamily.HELVETICA, 10, Font.BOLD)));
            tablaCliente.AddCell(new Phrase(factura.NombreCliente, new Font(Font.FontFamily.HELVETICA, 10)));

            tablaCliente.AddCell(new Phrase("NIT:", new Font(Font.FontFamily.HELVETICA, 10, Font.BOLD)));
            tablaCliente.AddCell(new Phrase(factura.NitCliente, new Font(Font.FontFamily.HELVETICA, 10)));

            tablaCliente.AddCell(new Phrase("DPI:", new Font(Font.FontFamily.HELVETICA, 10, Font.BOLD)));
            tablaCliente.AddCell(new Phrase(factura.DpiCliente, new Font(Font.FontFamily.HELVETICA, 10)));

            tablaCliente.AddCell(new Phrase("Teléfono:", new Font(Font.FontFamily.HELVETICA, 10, Font.BOLD)));
            tablaCliente.AddCell(new Phrase(factura.telefono, new Font(Font.FontFamily.HELVETICA, 10)));

            tablaCliente.SpacingAfter = 15f;
            documento.Add(tablaCliente);
        }

        private void AgregarInformacionEmpleado(Document documento, ReporteFacturaDetalle factura)
        {
            // Información del empleado
            PdfPTable tablaEmpleado = new PdfPTable(2);
            tablaEmpleado.WidthPercentage = 100;
            tablaEmpleado.SetWidths(new float[] { 1f, 1f });

            PdfPCell celdaTituloEmpleado = new PdfPCell(new Phrase("ATENDIDO POR", new Font(Font.FontFamily.HELVETICA, 12, Font.BOLD)));
            celdaTituloEmpleado.Colspan = 2;
            celdaTituloEmpleado.HorizontalAlignment = Element.ALIGN_CENTER;
            celdaTituloEmpleado.BackgroundColor = new BaseColor(240, 240, 240);
            celdaTituloEmpleado.Padding = 5f;
            tablaEmpleado.AddCell(celdaTituloEmpleado);

            tablaEmpleado.AddCell(new Phrase("Nombre:", new Font(Font.FontFamily.HELVETICA, 10, Font.BOLD)));
            tablaEmpleado.AddCell(new Phrase(factura.NombreEmpleado, new Font(Font.FontFamily.HELVETICA, 10)));

            tablaEmpleado.AddCell(new Phrase("Puesto:", new Font(Font.FontFamily.HELVETICA, 10, Font.BOLD)));
            tablaEmpleado.AddCell(new Phrase(factura.Puesto, new Font(Font.FontFamily.HELVETICA, 10)));

            tablaEmpleado.AddCell(new Phrase("DPI:", new Font(Font.FontFamily.HELVETICA, 10, Font.BOLD)));
            tablaEmpleado.AddCell(new Phrase(factura.DpiEmpleado, new Font(Font.FontFamily.HELVETICA, 10)));

            tablaEmpleado.AddCell(new Phrase("Email:", new Font(Font.FontFamily.HELVETICA, 10, Font.BOLD)));
            tablaEmpleado.AddCell(new Phrase(factura.EmailEmpleado, new Font(Font.FontFamily.HELVETICA, 10)));

            tablaEmpleado.AddCell(new Phrase("Rol:", new Font(Font.FontFamily.HELVETICA, 10, Font.BOLD)));
            tablaEmpleado.AddCell(new Phrase(factura.RolDelEmpleado, new Font(Font.FontFamily.HELVETICA, 10)));

            tablaEmpleado.SpacingAfter = 15f;
            documento.Add(tablaEmpleado);
        }

        private void AgregarTablaArticulos(Document documento, List<ReporteFacturaDetalle> datosFactura)
        {
            // Encabezado para la tabla de artículos
            Paragraph tituloArticulos = new Paragraph("DETALLE DE ARTÍCULOS", new Font(Font.FontFamily.HELVETICA, 12, Font.BOLD));
            tituloArticulos.Alignment = Element.ALIGN_CENTER;
            tituloArticulos.SpacingAfter = 5f;
            documento.Add(tituloArticulos);

            // Tabla de artículos
            PdfPTable tablaArticulos = new PdfPTable(6);
            tablaArticulos.WidthPercentage = 100;
            tablaArticulos.SetWidths(new float[] { 6f, 2f, 2f, 2f, 2f, 2f });

            // Encabezados de columnas
            PdfPCell celdaDescripcion = new PdfPCell(new Phrase("Descripción", new Font(Font.FontFamily.HELVETICA, 10, Font.BOLD)));
            celdaDescripcion.BackgroundColor = new BaseColor(220, 220, 220);
            celdaDescripcion.HorizontalAlignment = Element.ALIGN_CENTER;
            celdaDescripcion.Padding = 5f;
            tablaArticulos.AddCell(celdaDescripcion);

            PdfPCell celdaCantidad = new PdfPCell(new Phrase("Cantidad", new Font(Font.FontFamily.HELVETICA, 10, Font.BOLD)));
            celdaCantidad.BackgroundColor = new BaseColor(220, 220, 220);
            celdaCantidad.HorizontalAlignment = Element.ALIGN_CENTER;
            celdaCantidad.Padding = 5f;
            tablaArticulos.AddCell(celdaCantidad);

            PdfPCell celdaPrecioUnitario = new PdfPCell(new Phrase("Precio Unitario", new Font(Font.FontFamily.HELVETICA, 10, Font.BOLD)));
            celdaPrecioUnitario.BackgroundColor = new BaseColor(220, 220, 220);
            celdaPrecioUnitario.HorizontalAlignment = Element.ALIGN_CENTER;
            celdaPrecioUnitario.Padding = 5f;
            tablaArticulos.AddCell(celdaPrecioUnitario);

            // Precio Sin IVA
            PdfPCell celdaPrecioSinIVA = new PdfPCell(new Phrase("Precio Sin IVA", new Font(Font.FontFamily.HELVETICA, 10, Font.BOLD)));
            celdaPrecioSinIVA.BackgroundColor = new BaseColor(220, 220, 220);
            celdaPrecioSinIVA.HorizontalAlignment = Element.ALIGN_CENTER;
            celdaPrecioSinIVA.Padding = 5f;
            tablaArticulos.AddCell(celdaPrecioSinIVA);

            // IVA
            PdfPCell celdaIVA = new PdfPCell(new Phrase("IVA", new Font(Font.FontFamily.HELVETICA, 10, Font.BOLD)));
            celdaIVA.BackgroundColor = new BaseColor(220, 220, 220);
            celdaIVA.HorizontalAlignment = Element.ALIGN_CENTER;
            celdaIVA.Padding = 5f;
            tablaArticulos.AddCell(celdaIVA);

            PdfPCell celdaSubtotal = new PdfPCell(new Phrase("Subtotal", new Font(Font.FontFamily.HELVETICA, 10, Font.BOLD)));
            celdaSubtotal.BackgroundColor = new BaseColor(220, 220, 220);
            celdaSubtotal.HorizontalAlignment = Element.ALIGN_CENTER;
            celdaSubtotal.Padding = 5f;
            tablaArticulos.AddCell(celdaSubtotal);

            // Agrupar los artículos por nombre y sumar cantidades (para no duplicar líneas del mismo artículo)
            var articulosAgrupados = datosFactura
                .GroupBy(d => d.NombreArticulo)
                .Select(g => new {
                    NombreArticulo = g.Key,
                    CantidadTotal = g.Sum(d => d.Cantidad),
                    PrecioUnitario = g.First().PrecioUnitario,
                    PrecioSinIva = g.First().PrecioSinIVA,
                    IVA = g.First().IVA,
                    Subtotal = g.Sum(d => d.Subtotal)
                })
                .ToList();

            // Para cada artículo agrupado
            foreach (var articulo in articulosAgrupados)
            {
                PdfPCell celdaNombreArticulo = new PdfPCell(new Phrase(articulo.NombreArticulo, new Font(Font.FontFamily.HELVETICA, 10)));
                celdaNombreArticulo.Padding = 5f;
                tablaArticulos.AddCell(celdaNombreArticulo);

                PdfPCell celdaCantidadArticulo = new PdfPCell(new Phrase(articulo.CantidadTotal.ToString(), new Font(Font.FontFamily.HELVETICA, 10)));
                celdaCantidadArticulo.HorizontalAlignment = Element.ALIGN_CENTER;
                celdaCantidadArticulo.Padding = 5f;
                tablaArticulos.AddCell(celdaCantidadArticulo);

                PdfPCell celdaSubtotalArticulo = new PdfPCell(new Phrase($"Q {articulo.PrecioUnitario:N2}", new Font(Font.FontFamily.HELVETICA, 10)));
                celdaSubtotalArticulo.HorizontalAlignment = Element.ALIGN_RIGHT;
                celdaSubtotalArticulo.Padding = 5f;
                tablaArticulos.AddCell(celdaSubtotalArticulo);

                PdfPCell celdaPrecioSinIVAArticulo = new PdfPCell(new Phrase($"Q {articulo.PrecioSinIva:N2}", new Font(Font.FontFamily.HELVETICA, 10)));
                celdaPrecioSinIVAArticulo.HorizontalAlignment = Element.ALIGN_RIGHT;
                celdaPrecioSinIVAArticulo.Padding = 5f;
                tablaArticulos.AddCell(celdaPrecioSinIVAArticulo);

                PdfPCell celdaIVAArticulo = new PdfPCell(new Phrase($"Q {articulo.IVA:N2}", new Font(Font.FontFamily.HELVETICA, 10)));
                celdaIVAArticulo.HorizontalAlignment = Element.ALIGN_RIGHT;
                celdaIVAArticulo.Padding = 5f;
                tablaArticulos.AddCell(celdaIVAArticulo);

                PdfPCell celdaPrecioUnitarioArticulo = new PdfPCell(new Phrase($"Q {articulo.Subtotal:N2}", new Font(Font.FontFamily.HELVETICA, 10)));
                celdaPrecioUnitarioArticulo.HorizontalAlignment = Element.ALIGN_RIGHT;
                celdaPrecioUnitarioArticulo.Padding = 5f;
                tablaArticulos.AddCell(celdaPrecioUnitarioArticulo);
            }

            tablaArticulos.SpacingAfter = 15f;
            documento.Add(tablaArticulos);
        }

        private void AgregarInformacionPago(Document documento, ReporteFacturaDetalle factura)
        {
            // Información de pago (totales)
            PdfPTable tablaTotales = new PdfPTable(2);
            tablaTotales.WidthPercentage = 50;
            tablaTotales.HorizontalAlignment = Element.ALIGN_RIGHT;

            tablaTotales.AddCell(new Phrase("TOTAL:", new Font(Font.FontFamily.HELVETICA, 12, Font.BOLD)));
            tablaTotales.AddCell(new Phrase($"Q {factura.Total_Pago:N2}", new Font(Font.FontFamily.HELVETICA, 12, Font.BOLD)));

            documento.Add(tablaTotales);
        }

        private void AgregarPiePagina(Document documento)
        {
            // Pie de página
            Paragraph piePagina = new Paragraph();
            piePagina.SpacingBefore = 30f;
            piePagina.Add(new Chunk("¡Gracias por su compra!", new Font(Font.FontFamily.HELVETICA, 10, Font.ITALIC)));
            piePagina.Add(Chunk.NEWLINE);
            piePagina.Add(new Chunk("Este documento es una representación impresa de un Documento Tributario Electrónico (DTE)", new Font(Font.FontFamily.HELVETICA, 8)));
            piePagina.Alignment = Element.ALIGN_CENTER;
            documento.Add(piePagina);
        }
        #endregion


        #region Reporte por Fecha
        public byte[] GenerarReportePorFecha(List<ResponseReporteFecha> reporteFechas, DateTime fechaInicio, DateTime fechaFin)
        {
            try
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    Document documento = new Document(PageSize.A4, 30, 30, 30, 30);
                    PdfWriter writer = PdfWriter.GetInstance(documento, ms);
                    documento.Open();

                    // Agrupar por IdFactura para generar un reporte por factura dentro del rango
                    var facturasAgrupadas = reporteFechas
                        .GroupBy(r => r.IdFactura)
                        .OrderBy(g => g.First().Fecha) // opcional, ordenar por fecha
                        .ToList();

                    //Encabezado 
                    Paragraph encabezado = new Paragraph($"REPORTE DE FACTURAS DEL {fechaInicio.ToString("dd/MM/yyyy")} AL {fechaFin.ToString("dd/MM/yyyy")}", new Font(Font.FontFamily.HELVETICA, 18, Font.BOLD));
                    encabezado.Alignment = Element.ALIGN_CENTER;
                    encabezado.SpacingAfter = 15f;
                    documento.Add(encabezado);

                    // Información de la empresa
                    AgregarInformacionEmpresa(documento);

                    // Tabla de resumen de facturas
                    PdfPTable tablaResumen = new PdfPTable(7);
                    tablaResumen.WidthPercentage = 100;
                    tablaResumen.SetWidths(new float[] { 1f, 1f, 1f, 1f, 1f, 1f, 1f });
                    tablaResumen.AddCell(new Phrase("ID Factura", new Font(Font.FontFamily.HELVETICA, 10, Font.BOLD)));
                    tablaResumen.AddCell(new Phrase("Fecha", new Font(Font.FontFamily.HELVETICA, 10, Font.BOLD)));
                    tablaResumen.AddCell(new Phrase("Forma de Pago", new Font(Font.FontFamily.HELVETICA, 10, Font.BOLD)));
                    tablaResumen.AddCell(new Phrase("Nombre Empleado", new Font(Font.FontFamily.HELVETICA, 10, Font.BOLD)));
                    tablaResumen.AddCell(new Phrase("Rol", new Font(Font.FontFamily.HELVETICA, 10, Font.BOLD)));
                    tablaResumen.AddCell(new Phrase("Cliente", new Font(Font.FontFamily.HELVETICA, 10, Font.BOLD)));
                    tablaResumen.AddCell(new Phrase("Total", new Font(Font.FontFamily.HELVETICA, 10, Font.BOLD)));

                    // Agregar cada factura a la tabla
                    foreach (var factura in facturasAgrupadas)
                    {
                        var primerRegistro = factura.First();
                        tablaResumen.AddCell(new Phrase(primerRegistro.IdFactura.ToString(), new Font(Font.FontFamily.HELVETICA, 10)));
                        tablaResumen.AddCell(new Phrase(primerRegistro.Fecha.ToString("dd/MM/yyyy"), new Font(Font.FontFamily.HELVETICA, 10)));
                        tablaResumen.AddCell(new Phrase(primerRegistro.FormaPago, new Font(Font.FontFamily.HELVETICA, 10)));
                        tablaResumen.AddCell(new Phrase(primerRegistro.NombreEmpleado, new Font(Font.FontFamily.HELVETICA, 10)));
                        tablaResumen.AddCell(new Phrase(primerRegistro.RolDelEmpleado, new Font(Font.FontFamily.HELVETICA, 10)));
                        tablaResumen.AddCell(new Phrase(primerRegistro.NombreCliente, new Font(Font.FontFamily.HELVETICA, 10)));
                        tablaResumen.AddCell(new Phrase($"Q {primerRegistro.Total_Pago:N2}", new Font(Font.FontFamily.HELVETICA, 10)));
                    }

                    // Agregar tabla de resumen al documento
                    documento.Add(tablaResumen);

                    // Agregar total 
                    decimal totalGeneral = facturasAgrupadas.Sum(f => f.First().Total_Pago);
                    Paragraph total = new Paragraph($"TOTAL GENERAL: Q {totalGeneral:N2}", new Font(Font.FontFamily.HELVETICA, 12, Font.BOLD));
                    total.Alignment = Element.ALIGN_RIGHT;
                    total.SpacingBefore = 15f;
                    documento.Add(total);


                    // Pie de página
                    AgregarPiePagina(documento);

                    documento.Close();
                    return ms.ToArray();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}
