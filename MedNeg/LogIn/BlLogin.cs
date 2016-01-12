using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace MedNeg.LogIn
{
    public class BlLogIn
    {
        #region Declaracion de Variables/objetos
        Usuarios.BlUsuarios blUser;
        PermisosUsuarios.BlPermisosUsuarios blPermisosUsuarios;
        MedDAL.DAL.usuarios userValidate,oUsuario;
        MedDAL.Configuracion.DALConfiguracion cConfiguracion;
        int idUsuario;
        string cColor,sNombreUsuario;
        #endregion

        public BlLogIn()
        {
            blUser = new Usuarios.BlUsuarios();
            blPermisosUsuarios = new PermisosUsuarios.BlPermisosUsuarios();
            cConfiguracion = new MedDAL.Configuracion.DALConfiguracion();
        }

        /// <summary>
        /// Valida si el usuario existe en el sistema para autenticarlo
        /// </summary>
        /// <param name="sUsuario"></param>
        /// <param name="sContrasenia"></param>
        /// <returns></returns>
        public bool ValidarUsuario(string sUsuario, string sContrasenia)
        {
            userValidate = (MedDAL.DAL.usuarios)blUser.Buscar(sUsuario);
            if (userValidate != null)
            {
                idUsuario = userValidate.idUsuario;
                sNombreUsuario = userValidate.Nombre+ " " + userValidate.Apellidos;
                string sContra = blUser.EncriptarMD5(sContrasenia);
                return userValidate.Contrasena.Equals(blUser.EncriptarMD5(sContrasenia));                
            }
            else
                return false;
        }

        /// <summary>
        /// Carga una hashtable que contiene los permisos del usuario (menu desplegable)
        /// </summary>
        /// <returns></returns>
        public Hashtable CargarPermisos()
        {
            MedDAL.DAL.permisos_usuarios permisoUsuario;
            Hashtable htPermisos = new Hashtable();
            int count=1;
            char cPermiso;
            string[] listaPermisos = { "usuarios", "perfiles", 
                                       "clientes", "vendedores", "proveedores", "estados", "municipios", "poblaciones", "colonias", 
                                       "almacenes", "productos", "inventarios", 
                                       "pedidos", "recetas", "remisiones", "facturas", 
                                       "causes", "bitacora", 
                                       "configuracion", "campos editables", "tipos", "cuentas x cobrar", 
                                       "tipos de iva", "ensambles", "lineas de credito"};
               
            foreach (string permiso in listaPermisos) {
                permisoUsuario = (MedDAL.DAL.permisos_usuarios)blPermisosUsuarios.RecuperarPermisos(idUsuario, count);
                cPermiso = (permisoUsuario.TipoAcceso.ToString().ToCharArray())[0];
                if (cPermiso!='N')
                    htPermisos.Add(permiso, cPermiso);
                count++;
            }
            permisoUsuario = (MedDAL.DAL.permisos_usuarios)blPermisosUsuarios.RecuperarPermisos(idUsuario, 16);
            cPermiso = (permisoUsuario.TipoAcceso.ToString().ToCharArray())[0];
            if (cPermiso != 'N')
                htPermisos.Add("facturas x receta", cPermiso);
            htPermisos.Add("reportes",'T');
            htPermisos.Add("cambiar contraseña", 'T');
            htPermisos.Add("movimientos", 'T');
            return htPermisos;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cColorConfig"></param>
        /// <returns></returns>
        public string CargarConfiguracion(string cColorConfig)
        {
            cColor = cColorConfig ?? "Azul1";
            return cColor;
        }

        /// <summary>
        /// BL - GT: Recuperar el nombre de usuario que se ha logeado
        /// </summary>
        /// <param name="sUsuario"></param>
        /// <returns></returns>
        public string NombreDelUsuario()
        {
            return sNombreUsuario;

        }

        /// <summary>
        /// BL - GT: Recuperar el id de usuario que se ha logeado
        /// </summary>
        /// <returns></returns>
        public int IdDelUsuario()
        {
            return idUsuario;
        }
    }
}
