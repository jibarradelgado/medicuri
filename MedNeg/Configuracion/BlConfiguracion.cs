using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Objects;
using System.Data.Objects.DataClasses;

namespace MedNeg.Configuracion
{
    public class BlConfiguracion
    {

        MedDAL.Configuracion.DALConfiguracion odalConfiguracion;
        
        
        /// <summary>
        /// Constructor
        /// </summary>
        public BlConfiguracion()
        {
            odalConfiguracion = new MedDAL.Configuracion.DALConfiguracion();
        }


        public bool GuardarDatos(object obj,string sRutaArchivo)
        {
            return odalConfiguracion.SerializarToXml(obj, sRutaArchivo);
        }

        public object CargaDatos(string sRutaArchivo)
        {
            //object objConfiguracion = new object();
            object objConfiguracion = new MedDAL.Configuracion.DALConfiguracion();
            objConfiguracion = odalConfiguracion.DeserializarXml(sRutaArchivo);
            return objConfiguracion;
        }
    }
}
