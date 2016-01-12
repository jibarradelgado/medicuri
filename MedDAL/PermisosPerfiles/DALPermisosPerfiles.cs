using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Objects;
using System.Data.Objects.DataClasses;


namespace MedDAL.PermisosPerfiles
{
    public class DALPermisosPerfiles
    {

        DAL.medicuriEntities oMedicuriEntities;


        /// <summary>
        /// DAL - Constructor
        /// </summary>
        public DALPermisosPerfiles()
        {
            oMedicuriEntities = new DAL.medicuriEntities();
        }

        /// <summary>
        /// DAL - Insertar permisos
        /// </summary>
        /// <param name="oPermisoPerfil">Permiso a guardar</param>
        /// <returns>true registrados, false no registrados</returns>
        public bool AgregarPermisos(DAL.permisos_perfiles oPermisoPerfil)
        {
            try
            {
                //Agregar el registro
                oMedicuriEntities.AddTopermisos_perfiles(oPermisoPerfil);
                oMedicuriEntities.SaveChanges();
                
                return true;
            }
            catch
            {
                return false;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="iIdPerfil"></param>
        /// <param name="iIdPermiso"></param>
        /// <returns></returns>
        public object RecuperarPermiso(int iIdPerfil, int iIdPermiso)
        {

            try
            {

                var oQuery = from q in oMedicuriEntities.permisos_perfiles.
                             Where("it.idperfil=@idPerfil and it.idPermiso=@idPermiso",
                             new ObjectParameter("idPerfil", iIdPerfil),
                             new ObjectParameter("idPermiso", iIdPermiso))

                             select q;


                return (object)oQuery.FirstOrDefault<MedDAL.DAL.permisos_perfiles>();
            }
            catch
            {
                return false;

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="iIdPermiso"></param>
        /// <param name="iIdUsuario"></param>
        /// <param name="sTipoAcceso"></param>
        /// <returns></returns>
        public bool EditarPermiso(int iIdPermiso, int iIdPerfil, string sTipoAcceso)
        {
            try
            {

                var oQuery = from q in oMedicuriEntities.permisos_perfiles.
                            Where("it.idPerfil=@idPerfil and it.idPermiso=@idPermiso",
                            new ObjectParameter("idPerfil", iIdPerfil),
                            new ObjectParameter("idPermiso", iIdPermiso))

                             select q;
                DAL.permisos_perfiles oPermisoOriginal = oQuery.First<DAL.permisos_perfiles>();

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
        /// DAL - Eliminar los permisos del perfil
        /// </summary>
        /// <param name="iIdPerfil">Id perfil a eliminar</param>
        /// <returns></returns>
        public bool EliminarPermisos(int iIdPerfil)
        {

            try
            {

                var oQuery = from q in oMedicuriEntities.permisos_perfiles.
                              Where("it.idPerfil=@idPerfil",
                              new ObjectParameter("idPerfil", iIdPerfil))
                             select q;

                //DAL.permisos_usuarios oPermisosOriginales = oQuery.FirstOrDefault<DAL.permisos_usuarios>();

                foreach (DAL.permisos_perfiles oPermiso in oQuery)
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
