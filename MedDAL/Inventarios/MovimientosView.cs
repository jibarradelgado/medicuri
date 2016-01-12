using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MedDAL.Inventarios
{
    public class MovimientosView
    {
        int iIdInventarioMov;
        string sConcepto, sObservaciones, sPedimento;
        DateTime tFecha;

        public int idInventarioMov 
        {
            get { return iIdInventarioMov; }
            set { this.iIdInventarioMov = value; }
        }
        public string Concepto {
            get { return sConcepto; }
            set { this.sConcepto = value; }
        }
        public string Observaciones
        {
            get { return sObservaciones; }
            set { this.sObservaciones = value; }
        }
        public string Pedimento
        {
            get { return sPedimento; }
            set { this.sPedimento = value; }
        }
        public DateTime Fecha
        {
            get { return tFecha; }
            set { this.tFecha = value; }
        }


        public MovimientosView() { }
    }
}
