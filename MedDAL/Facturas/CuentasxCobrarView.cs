using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MedDAL.Facturas
{
    public class CuentasxCobrarView
    {
        int iIdFactura;
        string sNombre, sApellidos, sFolio, sPedido, sRemision, sReceta, sEstatus;
        DateTime tFecha, tFechaAplicacion;

        public int idFactura
        {
            get { return iIdFactura; }
            set { this.iIdFactura = value; }
        }
        public string Nombre
        {
            get { return sNombre; }
            set { this.sNombre = value; }
        }
        public string Apellidos
        {
            get { return sApellidos; }
            set { this.sApellidos = value; }
        }
        public string Folio
        {
            get { return sFolio; }
            set { this.sFolio = value; }
        }
        public string pedido
        {
            get { return sPedido; }
            set { this.sPedido = value; }
        }
        public string remision
        {
            get { return sRemision; }
            set { this.sRemision = value; }
        }
        public string receta
        {
            get { return sReceta; }
            set { this.sReceta = value; }
        }
        public string Estatus
        {
            get { return sEstatus; }
            set { this.sEstatus = value; }
        }
        public DateTime Fecha
        {
            get { return tFecha; }
            set { this.tFecha = value; }
        }
        public DateTime FechaAplicacion
        {
            get { return tFechaAplicacion; }
            set { this.tFechaAplicacion = value; }
        }

        public CuentasxCobrarView() { }
    }
}
