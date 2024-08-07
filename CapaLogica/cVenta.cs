﻿using CapaDatos;
using capaLogica;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CapaLogica
{
    public class cVenta
    {
        public string IdVenta { get; set; }
        public string IdEmpleado { get; set; }
        public string TipoDocumento { get; set; }
        public string NumeroDocumento { get; set; }
        public string DocumentoCliente { get; set; }
        public string NombreCliente { get; set; }
        public decimal MontoPago { get; set; }
        public decimal MontoCambio { get; set; }
        public decimal MontoTotal { get; set; }
        public DateTime FechaRegistro { get; set; }
        public int Estado {  get; set; }

        private cDatos odatos = new cDatosSQL();
        public string Mensaje;

        public DataTable Listar() => odatos.TraerDataTable("uspListarVenta");
        public DataTable Buscar(string campo, string contenido) => odatos.TraerDataTable("uspBuscarVenta", campo, contenido);
        public DataTable BuscarComprobante(string Tipo, string campo, string contenido) => odatos.TraerDataTable("uspBuscarComprobanteVenta", Tipo, campo, contenido);
        public bool Insertar()
        {
            DataRow ofila = odatos.TraerDataRow("uspInsertarVenta", IdEmpleado, TipoDocumento, DocumentoCliente, NombreCliente, MontoPago, MontoCambio, MontoTotal);
            Mensaje = ofila[1].ToString();
            byte CodigoError = Convert.ToByte(ofila[0]);
            return CodigoError == 0;
        }
        // generar mro comprobante
        // esto no
        public bool Modificar()
        {
            DataRow ofila = odatos.TraerDataRow("uspModificarVenta", IdVenta, IdEmpleado, DocumentoCliente, NombreCliente, MontoPago, MontoCambio, MontoTotal);
            Mensaje = ofila[1].ToString();
            byte CodigoError = Convert.ToByte(ofila[0]);
            return CodigoError == 0;
        }
        public bool Eliminar()
        {
            DataRow ofila = odatos.TraerDataRow("uspEliminarVenta", IdVenta);
            Mensaje = ofila[1].ToString();
            byte CodigoError = Convert.ToByte(ofila[0]);
            return CodigoError == 0;
        }
        public string SiguienteID() => odatos.TraerValor("uspGenerarCodigo", "VENTA");
        public string GenerarNroDocumento() => odatos.TraerValor("uspGenerarNumeroComprobante", TipoDocumento);
        public bool AnularVenta()
        {
            DataRow ofila = odatos.TraerDataRow("uspAnularVenta", IdVenta);
            Mensaje = ofila[1].ToString();
            byte CodigoError = Convert.ToByte(ofila[0]);
            return CodigoError == 0;
        }
        public void CargarInformacion()
        {
            DataRow ofila = odatos.TraerDataRow("uspBuscarVenta", "IdVenta", IdVenta);
            if (ofila != null)
            {
                try
                {
                    IdVenta = ofila["IdVenta"].ToString();
                    IdEmpleado = ofila["IdEmpleado"].ToString();
                    TipoDocumento = ofila["TipoDocumento"].ToString();
                    NumeroDocumento = ofila["NumeroDocumento"].ToString();
                    DocumentoCliente = ofila["DocumentoCliente"].ToString();
                    NombreCliente = ofila["NombreCliente"].ToString();
                    MontoPago = decimal.Parse(ofila["MontoPago"].ToString());
                    MontoCambio = decimal.Parse(ofila["MontoCambio"].ToString());
                    MontoTotal = decimal.Parse(ofila["MontoTotal"].ToString());
                    FechaRegistro = Convert.ToDateTime(ofila["FechaRegistro"]);
                    Estado = (bool)ofila["Estado"]?1:0;

                }
                catch (Exception ex)
                {
                    Mensaje = ex.Message;
                }
            }
        }
    }
}
