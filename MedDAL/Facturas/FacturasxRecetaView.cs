using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MedDAL.Facturas
{
    public class FacturasxRecetaView
    {
        int iIdLineaCredito;
        string sFuenteCuenta;
        decimal dMonto, dFacturado;
        DateTime tFechaVencimiento;
        bool bActivo;

        public int idLineaCredito {
            get { return iIdLineaCredito; }
            set { this.iIdLineaCredito = value; }
        }
        public string FuenteCuenta {
            get { return sFuenteCuenta; }
            set { this.sFuenteCuenta = value; }
        }
        public decimal Monto {
            get { return dMonto; }
            set { this.dMonto = value; }
        }
        public decimal Facturado {
            get { return dFacturado; }
            set { this.dFacturado = value; }
        }
        public DateTime FechaVencimiento {
            get { return tFechaVencimiento; }
            set { this.tFechaVencimiento = value; }
        }
        public bool Activo {
            get { return bActivo; }
            set { this.bActivo = value; }
        }

        public FacturasxRecetaView() { }
    }
}
