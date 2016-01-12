using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MedDAL.Pedidos
{
    public class PedidosView
    {
        int iIdPedido, iIdCliente;
        string sNombre, sApellidos, sFolio, sEstatus;
        DateTime tFecha;

        public int idPedido {
            get { return iIdPedido; }
            set { this.iIdPedido = value; }
        }
        public int idCliente
        {
            get { return iIdCliente; }
            set { this.iIdCliente = value; }
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

        public PedidosView () { }
    }
}
