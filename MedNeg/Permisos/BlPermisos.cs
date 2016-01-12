using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Objects;
using System.Data.Objects.DataClasses;
using MedDAL.Permisos;


namespace MedNeg.Permisos
{
    public class BlPermisos
    {

        MedDAL.Permisos.DALPermisos oPermiso;


        /// <summary>
        /// BL - Constructor
        /// </summary>
         public BlPermisos()
        {
            oPermiso = new MedDAL.Permisos.DALPermisos();
        }


         /// <summary>
         /// BL - Buscar un permiso por su nombre
         /// </summary>
         /// <param name="oPermiso">Permiso</param>
         /// <returns></returns>
         public object Buscar(string sPermiso)
         {
             return oPermiso.Buscar(sPermiso);
         }


    }
}
