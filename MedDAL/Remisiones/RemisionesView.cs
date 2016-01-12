using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MedDAL.Remisiones
{
    public class RemisionesView
    {
        int iIdRemision, iIdCliente, iIdPedido;
        string sNombre, sApellidos, sFolio, sEstatus;
        DateTime tFecha;

        public int idRemision{
            get { return iIdRemision; }
            set { this.iIdRemision = value; }
        }
        public int idCliente{
            get { return iIdCliente; }
            set { this.iIdCliente = value; }
        }
        public int idPedido{
            get { return iIdPedido; }
            set { this.iIdPedido = value; }
        }
        public string Nombre{
            get { return sNombre; }
            set { this.sNombre = value; }
        }
        public string Apellidos{
            get { return sApellidos; }
            set { this.sApellidos = value; }
        }
        public string Folio{
            get { return sFolio; }
            set { this.sFolio = value; }
        }
        public string Estatus{
            get { return sEstatus; }
            set { this.sEstatus = value; }
        }
        public DateTime Fecha{
            get { return tFecha; }
            set { this.tFecha = value; }
        }

        public RemisionesView() { }
    }
}
