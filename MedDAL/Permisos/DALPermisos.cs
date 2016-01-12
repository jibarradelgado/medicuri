using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Objects;
using System.Data.Objects.DataClasses;

namespace MedDAL.Permisos
{
    public class DALPermisos
    {

         DAL.medicuriEntities oMedicuriEntities;


        /// <summary>
        /// Constructor
        /// </summary>
         public DALPermisos()
        {
            oMedicuriEntities = new DAL.medicuriEntities();
        }

         /// <summary>
         /// Recuperar un permiso mediante su nombre
         /// </summary>
         /// <param name="sPermiso">Permiso</param>
         /// <returns>(object)oQuery.First<MedDAL.DAL.permiso></returns>
         public object Buscar(string sPermiso)
         {
             /*var oQuery = from q in oMedicuriEntities.usuarios
                          where q.Usuario == sUsuario
                          select q;*/
             //return (object)oQuery.First<MedDAL.DAL.usuarios>();

             var oQuery = from q in oMedicuriEntities.permisos
                          where q.Descripcion == sPermiso
                          select q;

             return (object)oQuery.First<MedDAL.DAL.permisos>();

         }

    }
}
