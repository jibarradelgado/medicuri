using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MedDAL.Municipios
{
    public class MunicipiosView
    {
        int iIdMunicipio;
        string sClave, sNombre;
        bool bActivo;

        public int idMunicipio
        {
            get { return iIdMunicipio; }
            set { this.iIdMunicipio = value; }
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

        public MunicipiosView() { }
    }
}
