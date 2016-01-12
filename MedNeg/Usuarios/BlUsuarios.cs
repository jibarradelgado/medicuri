using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Objects;
using System.Data.Objects.DataClasses;
namespace MedNeg.Usuarios


{
    public class BlUsuarios
    {
        MedDAL.Usuarios.DALUsuarios odalUsuario;

        /// <summary>
        /// Constructor
        /// </summary>
        public BlUsuarios()
        {
            odalUsuario = new MedDAL.Usuarios.DALUsuarios();
        }

        /// <summary>
        /// BL - Buscar un usuario
        /// </summary>
        /// <param name="sCadena">Parametro a buscar</param>
        /// <param name="iTipo">Filtro</param>
        /// <returns></returns>
        public IQueryable<MedDAL.Usuarios.UsuarioView> Buscar(string sCadena, int iTipo)
        {
           return odalUsuario.Buscar(sCadena, iTipo);
        }

        /// <summary>
        /// BL - Función que muestra todos los registros de la tabla
        /// </summary>
        /// <returns></returns>
        public IQueryable<MedDAL.Usuarios.UsuarioView> MostrarLista()
        {
            return odalUsuario.MostrarLista();
        }

        /// <summary>
        /// BL - Función que muestra los usuarios relacionados a un almacen en especifico
        /// </summary>
        /// <param name="iIdAlmacen">Almacen del cual recuperar los usuarios</param>
        /// <returns></returns>
        public IQueryable<MedDAL.Usuarios.UsuarioView> MostrarListaAlmacenFiltrada(int iIdAlmacen)
        {
            return odalUsuario.MostrarListaAlmacenFiltrada(iIdAlmacen);
        }

        /// <summary>
        /// BL - Buscar un registro por su llave primaria
        /// </summary>
        /// <param name="id">Llave primaria</param>
        /// <returns></returns>
        public object Buscar(int id)
        {
           return odalUsuario.Buscar(id);
        }

        /// <summary>
        /// BL - Buscar un usuario por su nombre de usuario
        /// </summary>
        /// <param name="sUsurio">Usuario</param>
        /// <returns></returns>
        public object Buscar(string sUsurio)
        {
            return odalUsuario.Buscar(sUsurio);
        }

        /// <summary>
        /// BL -Validar un usuario por su nombre de usuario
        /// </summary>
        /// <param name="sUsuario">Usuario</param>
        /// <returns></returns>
        public int ValidarUsuarioRepetido(string sUsuario)
        {
            return odalUsuario.ValidarUsuarioRepetido(sUsuario);
        }

        /// <summary>
        ///  BL - Registrar un usuario nuevo
        /// </summary>
        /// <param name="oUsuario">Usuario a registrar</param>
        /// <returns></returns>
        public bool NuevoRegistro(MedDAL.DAL.usuarios oUsuario)
        {
            return odalUsuario.NuevoRegistro(oUsuario);
        }


       

        /// <summary>
        /// BL - Editar un usuario
        /// </summary>
        /// <param name="oUsuario">Usuario a editar</param>
        /// <returns></returns>
        public bool EditarRegistro(MedDAL.DAL.usuarios oUsuario)
        {
            return odalUsuario.EditarRegistro(oUsuario);
        }


        /// <summary>
        /// BL - Cambiar la contraseña de usuario
        /// </summary>
        /// <param name="sNombreUsuario">Usuario a modificar</param>
        /// <param name="sContrasena">Contraseña nueva encriptada en MD5</param>
        /// <returns></returns>
        public bool CambiarContraseña(string sNombreUsuario, string sContrasena)
        {
            return odalUsuario.CambiarContraseña(sNombreUsuario, sContrasena);
        }


        // <summary>
        /// BL - Eliminar un usuario
        /// </summary>
        /// <param name="iIdUsuario">ID usuario a eliminar</param>
        /// <returns></returns>
        public bool EliminarRegistro(int iIdUsuario)
        {
            return odalUsuario.EliminarRegistro(iIdUsuario);
        }

             
        /// <summary>
        /// Recuperar los permisos de un usuario
        /// </summary>
        /// <param name="iId">idUsuario</param>
        /// <returns></returns>
        public object RecuperarPermisos(int iId)
        {
            return odalUsuario.RecuperarPermisos(iId);
        }


        /// <summary>
        /// Recuperar el almacen del usuario
        /// </summary>
        /// <param name="sClave"></param>
        /// <returns></returns>
        public object RecuperarAlmacen(string sClave)
        {
            return odalUsuario.RecuperarAlmacen(sClave);
        }

         public string EncriptarMD5(string input)
       {
           System.Security.Cryptography.MD5CryptoServiceProvider x = new   System.Security.Cryptography.MD5CryptoServiceProvider();
           byte[] bs = System.Text.Encoding.UTF8.GetBytes(input);
           bs = x.ComputeHash(bs);
           System.Text.StringBuilder s = new System.Text.StringBuilder();
           foreach (byte b in bs)
           {
               s.Append(b.ToString("x2").ToLower());
           }
           string sEncriptada = s.ToString();
           return sEncriptada;
       }
    }
}
