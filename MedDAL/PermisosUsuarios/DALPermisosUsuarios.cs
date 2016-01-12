using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Objects;
using System.Data.Objects.DataClasses;

namespace MedDAL.PermisosUsuarios
{
    public class DALPermisosUsuarios
    {

        DAL.medicuriEntities oMedicuriEntities;


        /// <summary>
        /// Constructor
        /// </summary>
        public DALPermisosUsuarios()
        {
            oMedicuriEntities = new DAL.medicuriEntities();
        }

        /// <summary>
        /// DAL - Insertar permisos
        /// </summary>
        /// <param name="oPermisoUsuario">Permiso a guardar</param>
        /// <returns>true registrados, false no registrados</returns>
        public bool AgregarPermisos(DAL.permisos_usuarios oPermisoUsuario)
        {
            try
            {          
                    //Agregar el registro
                    oMedicuriEntities.AddTopermisos_usuarios(oPermisoUsuario);
                    //oMedicuriEntities.AddObject("permisos_usuarios", oPermisoUsuario);
                    oMedicuriEntities.SaveChanges();
               
               
                return true;
            }
            catch
            {
                return false;
            }
        }


        /// <summary>
        /// Recupera un permiso de usuario
        /// </summary>
        /// <param name="iId">id Usuario</param>
        /// <returns></returns>
        public object RecuperarPermiso(int iIdUsuario, int iIdPermiso)
        {

            try
            {

                var oQuery = from q in oMedicuriEntities.permisos_usuarios.
                             Where("it.idUsuario=@idUsuario and it.idPermiso=@idPermiso",
                             new ObjectParameter("idUsuario", iIdUsuario),
                             new ObjectParameter("idPermiso", iIdPermiso))

                             select q;


                return (object)oQuery.FirstOrDefault<MedDAL.DAL.permisos_usuarios>();
            }
            catch
            {
                return false;

            }
        }


        public bool EditarPermiso(int iIdPermiso,int iIdUsuario,string sTipoAcceso)
        {
            try
            {

                var oQuery = from q in oMedicuriEntities.permisos_usuarios.
                            Where("it.idUsuario=@idUsuario and it.idPermiso=@idPermiso",
                            new ObjectParameter("idUsuario", iIdUsuario),
                            new ObjectParameter("idPermiso", iIdPermiso))

                             select q;
                DAL.permisos_usuarios oPermisoOriginal = oQuery.First<DAL.permisos_usuarios>();

                oPermisoOriginal.TipoAcceso = sTipoAcceso;

                oMedicuriEntities.SaveChanges();
                return true;

            }
            catch
            {
                return false;
            }

        }


        /// <summary>
        /// DAL - Eliminar los permisos del usuario
        /// </summary>
        /// <param name="iIdUsuario">Id usuario a eliminar</param>
        /// <returns></returns>
        public bool EliminarPermisos(int iIdUsuario)
        {

            try
            {

                var oQuery = from q in oMedicuriEntities.permisos_usuarios.
                              Where("it.idUsuario=@idUsuario",
                              new ObjectParameter("idUsuario", iIdUsuario))
                             select q;

                //DAL.permisos_usuarios oPermisosOriginales = oQuery.FirstOrDefault<DAL.permisos_usuarios>();

                foreach (DAL.permisos_usuarios oPermiso in oQuery)
                {
                    oMedicuriEntities.DeleteObject(oPermiso);

                }
                               
                oMedicuriEntities.SaveChanges();

                return true;

            }
            catch
            {
                return false;
            }
        }
        
    }
}
