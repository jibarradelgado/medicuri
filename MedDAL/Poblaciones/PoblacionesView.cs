using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MedDAL.Poblaciones
{
    public class PoblacionesView
    {
        int iIdPoblacion;
        string sClave, sNombre;
        bool bActivo;

        public int idPoblacion
        {
            get { return iIdPoblacion; }
            set { this.iIdPoblacion = value; }
        }
        public string Clave
        {
            get { return sClave; }
            set { this.sClave = value; }
        }
        public string Nombre
        {
            get { return sNombre; }
            set { this.sNombre = value; }
        }
        public bool Activo
        {
            get { return bActivo; }
            set { this.bActivo = value; }
        }

        public PoblacionesView() { }
    }
}
