using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Objects;
using System.Data.Objects.DataClasses;

namespace MedNeg.Perfiles
{
    public class BlPerfiles
    {

        MedDAL.Perfiles.DALPerfiles odalPerfiles;

        /// <summary>
        /// BL - Constructor
        /// </summary>
        public BlPerfiles()
        {
            odalPerfiles = new MedDAL.Perfiles.DALPerfiles();
        }

        public IQueryable<MedDAL.DAL.perfiles> BuscarEnum()
        {
            return odalPerfiles.BuscarEnum();
        }


        public bool NuevoRegistro(MedDAL.DAL.perfiles oPerfil)
        {
            return  odalPerfiles.NuevoRegistro(oPerfil);
        }

        /// <summary>
        /// BL - Buscar un perfil
        /// </summary>
        /// <param name="sCadena">Parametro a buscar</param>
        /// <param name="iTipo">Filtro</param>
        /// <returns></returns>
        public object Buscar(string sCadena, int iTipo)
        {
            return odalPerfiles.Buscar(sCadena, iTipo);
        }

        /// <summary>
        /// BL - Función que muestra todos los registros de la tabla
        /// </summary>
        /// <returns></returns>
        public object MostrarLista()
        {
            return odalPerfiles.MostrarLista();
        }

        /// <summary>
        /// BL - Buscar un registro por su llave primaria
        /// </summary>
        /// <param name="id">Llave primaria</param>
        /// <returns></returns>
        public object Buscar(int id)
        {
            return  odalPerfiles.Buscar(id);
        }

        /// <summary>
        /// BL - Buscar un registro por su nombre
        /// </summary>
        /// <param name="sNombre">Nombre del perfil</param>
        /// <returns></returns>
        public object Buscar(string sNombre)
        {
            return odalPerfiles.Buscar(sNombre);
        }

        /// <summary>
        /// BL - Editar un perfil
        /// </summary>
        /// <param name="oUsuario">Usuario a editar</param>
        /// <returns></returns>
        public bool EditarRegistro(MedDAL.DAL.perfiles oPerfil)
        {
            return odalPerfiles.EditarRegistro(oPerfil);
        }

        // <summary>
        /// BL - Eliminar un perfil
        /// </summary>
        /// <param name="iIdPerfil">ID perfil a eliminar</param>
        /// <returns></returns>
        public bool EliminarRegistro(int iIdPerfil)
        {
            return odalPerfiles.EliminarRegistro(iIdPerfil);
        }
       
    }
}
