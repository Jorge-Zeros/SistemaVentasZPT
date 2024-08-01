﻿using CapaLogica;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using System.IO;

namespace CapaPresentacion.Reportes
{
    public partial class frmFacturaCompra : Form
    {
        cCompra oCompra = new cCompra();
        cDetalleCompra oDetalleCompra = new cDetalleCompra();

        public frmFacturaCompra()
        {
            InitializeComponent();
        }

        void actualizarLista()
        {
            string IdCompra = txtIdCompra.Text;
            string Campo = cbCampo.Text;
            string Contenido = txtContenido.Text;
            dgvProductos.DataSource = oDetalleCompra.BuscarProductos(IdCompra, Campo, Contenido);
            foreach (DataGridViewRow Fila in dgvProductos.Rows)
            {
                decimal PrecioUnitario = (decimal)Fila.Cells["PrecioUnitario"].Value / (decimal)1.18;
                Fila.Cells["PrecioUnitario"].Value = PrecioUnitario.ToString("F2");
                decimal Importe = (decimal)Fila.Cells["Importe"].Value / (decimal)1.18;
                Fila.Cells["Importe"].Value = Importe.ToString("F2");
            }
        }

        public void actualizarMuestra(string idCompra)
        {
            // Recuperando Valores
            DataTable Compra = oCompra.Buscar("IdCompra", idCompra);
            DataRow CompraValores = Compra.Rows[0];
            //------------------------------------------
            cProveedor auxProveedor = new cProveedor();
            auxProveedor.IdProveedor = CompraValores["IdProveedor"].ToString();
            auxProveedor.CargarInformacion();
            //------------------------------------------
            // Mostrando Valores
            txtIdCompra.Text = CompraValores["IdCompra"].ToString();
            txtNroDocumento.Text = CompraValores["NumeroDocumento"].ToString();
            dtpFecha.Value = (DateTime)CompraValores["FechaRegistro"];
            txtIdEmpleado.Text = CompraValores["IdEmpleado"].ToString();
            txtNombreEmpleado.Text = CompraValores["NombreEmpleado"].ToString();
            txtMontoTotal.Text = CompraValores["MontoTotal"].ToString();
            //txtDocumentoCliente.Text = CompraValores["DocumentoProveedor"].ToString();
            txtDocumentoCliente.Text = auxProveedor.Documento;
            txtNombreCliente.Text = CompraValores["NombreProveedor"].ToString();
            //txtTelfProveedor.Text = CompraValores["TelfProveedor"].ToString();
            txtTelfProveedor.Text = auxProveedor.Telefono;

            // Calcular los montos
            decimal SubTotal = decimal.Parse(CompraValores["MontoTotal"].ToString()) / (decimal)1.18;
            decimal IGV = SubTotal * (decimal)0.18;
            txtSubTotal.Text = SubTotal.ToString("F2");
            txtIGV.Text = IGV.ToString("F2");

            // Mostrar Productos
            cbCampo.Text = "NombreProducto";
            actualizarLista();  
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            sfrmBuscarCompra hijo = new sfrmBuscarCompra(this, "FACTURA");
            hijo.Show();
        }

        private void cbCampo_SelectedIndexChanged(object sender, EventArgs e)
        {
            actualizarLista();
        }

        private void txtContenido_TextChanged(object sender, EventArgs e)
        {
            actualizarLista();
        }

        private void btnImprimir_Click(object sender, EventArgs e)
        {
            sfdGuardar.FileName = DateTime.Now.ToString("ddMMyyyyHHmmss");

            // Construir la ruta relativa al archivo HTML
            string projectDirectory = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.FullName;
            string htmlFilePath = Path.Combine(projectDirectory, "Recursos", "FacturaCompra.html");
            string imagePath = Path.Combine(projectDirectory, "Recursos", "Logo.png");

            // Leer el contenido del archivo HTML
            string paginahtml_texto = File.ReadAllText(htmlFilePath);

            // Reemplazar valores plantilla
            paginahtml_texto = paginahtml_texto.Replace("@ID_FACTURA", txtNroDocumento.Text);
            paginahtml_texto = paginahtml_texto.Replace("@PROVEEDOR", txtNombreCliente.Text);
            paginahtml_texto = paginahtml_texto.Replace("@RUC", txtDocumentoCliente.Text);
            paginahtml_texto = paginahtml_texto.Replace("@TELEFONO", txtTelfProveedor.Text);
            paginahtml_texto = paginahtml_texto.Replace("@FECHA", dtpFecha.Value.ToLongDateString());
            paginahtml_texto = paginahtml_texto.Replace("@EMPLEADO", txtNombreEmpleado.Text);
            paginahtml_texto = paginahtml_texto.Replace("@ID_EMPLEADO", txtIdEmpleado.Text);
            // Valores de los precios
            paginahtml_texto = paginahtml_texto.Replace("@SUBTOTAL", txtSubTotal.Text);
            paginahtml_texto = paginahtml_texto.Replace("@IGV", txtIGV.Text);
            paginahtml_texto = paginahtml_texto.Replace("@TOTAL", txtMontoTotal.Text);

            // Insertar Datos de la compra
            string filas = string.Empty;
            foreach (DataGridViewRow row in dgvProductos.Rows)
            {
                filas += "<tr>";
                filas += "<td>" + row.Cells["Cantidad"].Value.ToString() + "</td>";
                filas += "<td>" + row.Cells["NombreProducto"].Value.ToString() + "</td>";
                filas += "<td>" + row.Cells["PrecioUnitario"].Value.ToString() + "</td>";
                filas += "<td>" + row.Cells["Importe"].Value.ToString() + "</td>";
                filas += "</tr>";
            }
            paginahtml_texto = paginahtml_texto.Replace("@FILAS", filas);

            if (sfdGuardar.ShowDialog() == DialogResult.OK)
            {
                using (FileStream stream = new FileStream(sfdGuardar.FileName, FileMode.Create))
                {
                    Document pdfDoc = new Document(PageSize.A4, 25, 25, 25, 25);

                    PdfWriter writer = PdfWriter.GetInstance(pdfDoc, stream);

                    pdfDoc.Open();

                    pdfDoc.Add(new Phrase(""));

                    iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(imagePath);
                    img.ScaleToFit(90, 70);
                    img.Alignment = iTextSharp.text.Image.UNDERLYING;
                    img.SetAbsolutePosition(pdfDoc.LeftMargin + 20, pdfDoc.Top - 70);
                    pdfDoc.Add(img);

                    using (StringReader sr = new StringReader(paginahtml_texto))
                    {
                        XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, sr);
                    }

                    pdfDoc.Close();

                    stream.Close();
                }
            }
        }
    }
}
