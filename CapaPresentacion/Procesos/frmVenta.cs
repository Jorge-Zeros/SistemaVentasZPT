﻿using capaLogica;
using CapaLogica;
using CapaPresentacion.Mantenimiento;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Resources.ResXFileRef;

namespace CapaPresentacion.Procesos
{
    public partial class frmVenta : Form
    {
        public string usuario;
        cEmpleado oEmpleado = new cEmpleado();
        cUtilitarios oUtilitarioas = new cUtilitarios();


        cVenta oVenta = new cVenta();
        cDetalleVenta oDetalleVenta = new cDetalleVenta();

        public frmVenta(string pUser)
        {
            InitializeComponent();
            usuario = pUser;
            
        }
        private void frmVenta_Load(object sender, EventArgs e)
        {
            oEmpleado.Correo = usuario;
            oEmpleado.CargarInformacion();
            txtIDEmpleado.Text = oEmpleado.IdEmpleado;
            txtNombreEmpleado.Text = oEmpleado.NombreCompleto;

            Start();
        }
        private void Start()
        {
            lblMensaje.Visible = false;
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            oVenta.IdEmpleado = txtIDEmpleado.Text;
            if (rbBoleta.Checked) oVenta.TipoDocumento = "Boleta";
            else oVenta.TipoDocumento = "Factura";
            oVenta.NumeroDocumento = txtNroDocumento.Text;
            oVenta.DocumentoCliente = txtDocumentoCliente.Text;
            oVenta.NombreCliente = txtNombreCliente.Text;
            oVenta.MontoPago = decimal.Parse(txtMontoTotal.Text); //Verificar
            oVenta.MontoCambio = decimal.Parse(txtMontoTotal.Text); //Verificar
            oVenta.MontoTotal = decimal.Parse(txtMontoTotal.Text);
            oVenta.FechaRegistro = dtpFechaRegistro.Value;
            oVenta.Insertar();

            //int maxIdVenta = oVenta.RecureparMaxID();
            //ocular IDProdcuto
            for (int i = 0; i < dgvProductos.Rows.Count; i++)
            {
                oDetalleVenta.IdVenta = oVenta.IdVenta; //maxIdVenta
                oDetalleVenta.IdProducto = dgvProductos.Rows[i].Cells["coIdProducto"].ToString();
                oDetalleVenta.PrecioUnitario = int.Parse(dgvProductos.Rows[i].Cells["coPrecio"].ToString());
                oDetalleVenta.Cantidad = int.Parse(dgvProductos.Rows[i].Cells["coCantidad"].ToString());
                oDetalleVenta.SubTotal = decimal.Parse(dgvProductos.Rows[i].Cells["coSubTotal"].ToString());
                oDetalleVenta.FechaRegistro = DateTime.Now;
                oDetalleVenta.Insertar();
            }
            lblMensaje.Text = oVenta.Mensaje;
            tMensaje.Start();
            CargarFormularioCombrobante();
            LimpiarTextbox();

            // -------- Cadavez que se ejecute un detalle venta ejecuta un trigger actualiza un stock del producto (SQL)
        }

        public string CorreoDialog {  get; set; }
        private void btnBuscarCliente_Click(object sender, EventArgs e)
        {
            using(var formBuscarCliente = new sfrmBuscarCliente(this))
            {
                DialogResult resultado = formBuscarCliente.ShowDialog();
                if(resultado == DialogResult.OK)
                {
                    cCliente clienteAux = new cCliente();
                    clienteAux.Correo = CorreoDialog;
                    clienteAux.CargarInformacion();

                    txtIdCliente.Text = clienteAux.IdCliente;
                    txtDocumentoCliente.Text = clienteAux.Documento;
                    txtNombreCliente.Text = clienteAux.NombreCompleto;
                    txtCorreoCliente.Text = clienteAux.Correo;
                    txtTelefonoCliente.Text = clienteAux.Telefono;                    
                }
            }
        }


        public string IdProductoDialog {  get; set; }
        private void btnAgregarProducto_Click(object sender, EventArgs e)
        {
            using(var formBuscarProducto = new sfrmBuscarProducto(this))
            {
                DialogResult resultado = formBuscarProducto.ShowDialog();
                if(resultado == DialogResult.OK)
                {
                    cProducto auxProducto = new cProducto();
                    auxProducto.IdProducto = IdProductoDialog;
                    auxProducto.CargarInformacion();
                    RellenarProductos(dgvProductos, auxProducto);

                }
            }
        }
        private void RellenarProductos(DataGridView pdgvProductos, cProducto producto)
        {
            pdgvProductos.Rows.Add(producto.IdProducto, producto.Nombre, producto.PrecioVenta,producto.Stock,1, producto.PrecioCompra);
            CalcularTotal();
            //pdgvProductos.Rows.Add(producto.IdProducto, producto.Codigo, producto.Nombre, producto.Descripcion, 
            //                       producto.IdCategoria, producto.Stock, producto.PrecioCompra, producto.PrecioVenta, 
            //                       producto.Estado, producto.FechaRegistro, producto.Imagen);

        }
        private void FC_ActualizarMontos(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow Fila = dgvProductos.CurrentRow;
            if( Fila != null && dgvProductos.Rows.Count > 0)
            {
                Fila.Cells["coSubTotal"].Value = decimal.Parse(Fila.Cells["coPrecio"].Value.ToString()) * decimal.Parse(Fila.Cells["coCantidad"].Value.ToString());
            }
            CalcularTotal();
        }
        private void CalcularTotal()
        {
            decimal total = 0;
            foreach (DataGridViewRow row in dgvProductos.Rows)
            {
                total += Convert.ToDecimal(row.Cells["coSubTotal"].Value.ToString());
            }
            txtMontoTotal .Text = total.ToString("0.00");
        }
        private void CargarFormularioCombrobante()
        {

        }
        private void LimpiarTextbox()
        {
            dgvProductos.Rows.Clear();
            txtIdCliente.Text = "";
            txtDocumentoCliente.Text = "";
            txtNombreCliente.Text = "";
            txtCorreoCliente.Text = "";
            txtTelefonoCliente.Text = "";
        }
        int tiempoMensaje = 10;
        private void tMensaje_Tick(object sender, EventArgs e)
        {
            tiempoMensaje--;
            if(tiempoMensaje == 0)
            {
                tiempoMensaje = 10;
                lblMensaje.Visible = false;
            }
        }

        private void btnQuitarProducto_Click(object sender, EventArgs e)
        {
            DataGridViewRow Fila = dgvProductos.CurrentRow;
            if(Fila != null )
            {
                int indiceRow = dgvProductos.CurrentRow.Index;
                dgvProductos.Rows.RemoveAt(indiceRow);
            }
        }

        private void dgvProductos_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            CalcularTotal();
        }
    }
}