using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Objects;
using System.Data.Objects.DataClasses;
using MedDAL.PermisosUsuarios;

namespace MedNeg.PermisosUsuarios
{

    public class BlPermisosUsuarios
    {

        MedDAL.PermisosUsuarios.DALPermisosUsuarios odalPermisosUsuarios;

        public BlPermisosUsuarios()
        {
            odalPermisosUsuarios = new MedDAL.PermisosUsuarios.DALPermisosUsuarios();
        }


        /// <summary>
        ///  BL - Registrar permisos
        /// </summary>
        /// <param name="oPermiso">Permiso a registrar</param>
        /// <returns></returns>
        public bool AgregarPermisos(MedDAL.DAL.permisos_usuarios oPermisoUsuario)
        {
            return odalPermisosUsuarios.AgregarPermisos(oPermisoUsuario);
        }


        /// <summary>
        /// BL - Recupera todos los permisos del usuario
        /// </summary>
        /// <param name="iId">Id usuario a buscar</param>
        /// <returns></returns>
        public object RecuperarPermisos(int iIdUsuario, int iIdPermiso)
        {
            return odalPermisosUsuarios.RecuperarPermiso(iIdUsuario, iIdPermiso);
            //return odalPermisosUsuarios.RecuperarPermisos(iId);
        }


        /// <summary>
        /// BL - Editar un permiso de usuario
        /// </summary>
        /// <param name="iIdPermiso">Id de Permiso</param>
        /// <param name="iIdUsuario">Id de Usuario</param>
        /// <param name="sTipoAcceso">Tipo de acceso L E T N</param>
        /// <returns></returns>
        public bool EditarPermiso(int iIdPermiso, int iIdUsuario, string sTipoAcceso)
        {
           return odalPermisosUsuarios.EditarPermiso(iIdPermiso, iIdUsuario, sTipoAcceso);
        }


        /// <summary>
        /// BL - Eliminar los permisos de un usuario
        /// </summary>
        /// <param name="iIdUsuario">ID usuario</param>
        /// <returns>bool</returns>
        public bool EliminarPermisos(int iIdUsuario)
        {
            return odalPermisosUsuarios.EliminarPermisos(iIdUsuario);

        }
    }
}
