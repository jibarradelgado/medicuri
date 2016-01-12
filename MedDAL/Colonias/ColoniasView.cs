using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MedDAL.Colonias
{
    public class ColoniasView
    {
        int iIdColonia;
        string sClave, sNombre;
        bool bActivo;

        public int idColonia {
            get { return iIdColonia; }
            set { this.iIdColonia = value; }
        }
        public string Clave {
            get { return sClave; }
            set { this.sClave = value; } 
        }
        public string Nombre {
            get { return sNombre; }
            set { this.sNombre = value; }
        }
        public bool Activo {
            get { return bActivo; }
            set { this.bActivo = value; }
        }

        public ColoniasView() { }
    }
}
