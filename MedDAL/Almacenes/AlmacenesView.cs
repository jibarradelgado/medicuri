using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MedDAL.Almacenes
{
    public class AlmacenesView
    {
        int iIdAlmacen, iIdEstados, iIdMunicipios, iIdPoblaciones, iIdColonias, iIdTipos;
        string sClave, sNombre, sTelefono, sEstado, sPoblacion, sTipo;
        bool bActivo;

        public int idAlmacen {
            get { return iIdAlmacen; }
            set { this.iIdAlmacen = value; } 
        }
        public int idEstados
        {
            get { return iIdEstados; }
            set { this.iIdEstados = value; }
        }
        public int idMunicipios
        {
            get { return iIdMunicipios; }
            set { this.iIdMunicipios = value; }
        }
        public int idPoblaciones
        {
            get { return iIdPoblaciones; }
            set { this.iIdPoblaciones = value; }
        }
        public int idColonias
        {
            get { return iIdColonias; }
            set { this.iIdColonias = value; }
        }
        public int idTipos
        {
            get { return iIdTipos; }
            set { this.iIdTipos = value; }
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
        public string Telefono
        {
            get { return sTelefono; }
            set { this.sTelefono = value; }
        }
        public string Estado
        {
            get { return sEstado; }
            set { this.sEstado = value; }
        }
        public string Poblacion
        {
            get { return sPoblacion; }
            set { this.sPoblacion = value; }
        }
        public string Tipo
        {
            get { return sTipo; }
            set { this.sTipo = value; }
        }
        public bool Activo
        {
            get { return bActivo; }
            set { this.bActivo = value; }
        }

        public AlmacenesView() { }

    }
}
