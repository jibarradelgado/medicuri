using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Objects;
using System.Data.Objects.DataClasses;
using MedDAL.PermisosPerfiles;

namespace MedNeg.PermisosPerfiles
{
    public class BlPermisosPerfiles
    {
        MedDAL.PermisosPerfiles.DALPermisosPerfiles odalPermisosPefiles;


        public BlPermisosPerfiles()
        {
            odalPermisosPefiles = new MedDAL.PermisosPerfiles.DALPermisosPerfiles();
        }


       /// <summary>
        /// Recuperar un permiso de un perfil
       /// </summary>
       /// <param name="iIdPerfil">Id Perfil</param>
       /// <param name="iIdPermiso">Id Permiso</param>
       /// <returns></returns>
        public object RecuperarPermiso(int iIdPerfil, int iIdPermiso)
        {
            return odalPermisosPefiles.RecuperarPermiso(iIdPerfil, iIdPermiso);
            //return odalPermisosUsuarios.RecuperarPermisos(iId);
        }


        /// <summary>
        /// BL - Editar un permiso de usuario
        /// </summary>
        /// <param name="iIdPermiso">Id de Permiso</param>
        /// <param name="iIdPerfil">Id de Perfil</param>
        /// <param name="sTipoAcceso">Tipo de acceso L E T N</param>
        /// <returns></returns>
        public bool EditarPermiso(int iIdPermiso, int iIdPerfil, string sTipoAcceso)
        {
            return odalPermisosPefiles.EditarPermiso(iIdPermiso, iIdPerfil, sTipoAcceso);
        }

        /// <summary>
        /// BL - Eliminar los permisos de un perfil
        /// </summary>
        /// <param name="iIdPerfil">ID Perfil</param>
        /// <returns>bool</returns>
        public bool EliminarPermisos(int iIdPerfil)
        {
            return odalPermisosPefiles.EliminarPermisos(iIdPerfil);
                
        }

        /// <summary>
        ///  BL - Registrar permisos
        /// </summary>
        /// <param name="oPermiso">Permiso a registrar</param>
        /// <returns></returns>
        public bool AgregarPermisos(MedDAL.DAL.permisos_perfiles oPermisoPerfil)
        {
            return odalPermisosPefiles.AgregarPermisos(oPermisoPerfil);
        }

    }
}
