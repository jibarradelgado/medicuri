using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MedNeg.Causes
{
    public class BLCausesMedicamentos
    {
        string sClave;
        string sNombre;
        string sPresentacion;
        int iIdMedicamento;        
        string  sDescripcion;
        string sCuadroBasico;

        public string Clave
        {            
            get { return this.sClave; }
            set { this.sClave = value; }
        }
        public string Nombre 
        {
            get { return this.sNombre; }
            set { this.sNombre = value; }
        }
        public string Presentacion
        {
            get { return this.sPresentacion; }
            set { this.sPresentacion = value; }
        }
        public int idMedicamento 
        {
            get { return this.iIdMedicamento; }
            set { this.iIdMedicamento = value; }
        }
        public string Descripcion
        {
            get { return this.sDescripcion; }
            set { this.sDescripcion = value; }
        }
        public string CuadroBasico
        {
            get { return this.sCuadroBasico; }
            set { this.sCuadroBasico = value; }
        }

        public BLCausesMedicamentos() 
        { 
        
        }

        public BLCausesMedicamentos(string sClave, string sNombre, string sPresentacion, int idMedicamento, string sDescripcion, string sCuadroBasico)
        {
            this.sClave = sClave;
            this.sNombre = sNombre;
            this.sPresentacion = sPresentacion;
            this.idMedicamento = idMedicamento;
            this.sDescripcion = sDescripcion;
            this.sCuadroBasico = sCuadroBasico;
        }
    }
}
