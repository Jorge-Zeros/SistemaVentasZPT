﻿﻿using CapaDatos;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using static ConfiguracionUsuario;

namespace CapaLogica
{
    public class cProveedor
    {
        public string IdProveedor { get; set; }
        public string Documento { get; set; }
        public string RazonSocial { get; set; }
        public string Correo { get; set; }
        public string Telefono { get; set; }
        public string Estado { get; set; }
        public DateTime FechaRegistro { get; set; }

        private cDatos odatos = new cDatosSQL();
        public string Mensaje;

        public DataTable Listar() => odatos.TraerDataTable("uspListarProveedor");
        public bool Insertar()
        {
            DataRow ofila = odatos.TraerDataRow("uspInsertarProveedor", Documento, RazonSocial, Correo, Telefono);
            Mensaje = ofila[1].ToString();
            byte CodigoError = Convert.ToByte(ofila[0]);
            return CodigoError == 0;
        }
        public bool Modificar()
        {
            DataRow ofila = odatos.TraerDataRow("uspModificarProveedor", IdProveedor, Documento, RazonSocial, Correo, Telefono);
            Mensaje = ofila[1].ToString();
            byte CodigoError = Convert.ToByte(ofila[0]);
            return CodigoError == 0;
        }
        public bool Eliminar()
        {
            DataRow ofila = odatos.TraerDataRow("uspEliminarProveedor", IdProveedor);
            Mensaje = ofila[1].ToString();
            byte CodigoError = Convert.ToByte(ofila[0]);
            return CodigoError == 0;
        }
        public string SiguienteID() => odatos.TraerValor("uspGenerarCodigo", "PROVEEDOR");
        public void CargarInformacion()
        {
            DataRow ofila = odatos.TraerDataRow("uspBuscarProveedor", "IdProveedor", IdProveedor);
            if (ofila != null)
            {
                try
                {
                    IdProveedor = ofila["IdProveedor"].ToString();
                    Documento = ofila["Documento"].ToString();
                    RazonSocial = ofila["RazonSocial"].ToString();
                    Correo = ofila["Correo"].ToString();
                    Telefono = ofila["Telefono"].ToString();
                    Estado = ofila["Estado"].ToString(); ;
                    FechaRegistro = Convert.ToDateTime(ofila["FechaRegistro"]);



                }
                catch (Exception ex)
                {
                    Mensaje = ex.Message;

                }
            }
        }
    }
}