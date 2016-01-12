using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Objects;
using System.Data.Objects.DataClasses;

namespace MedNeg.Almacenes
{
    public class BlAlmacenes
    {
        MedDAL.Almacenes.DALAlmacenes odalAlmacenes;
        MedDAL.AlmacenesContactos.DALAlmacenesContactos odalContactosAlmacenes;

        /// <summary>
        /// BL - Constructor
        /// </summary>
         public BlAlmacenes()
        {
            odalAlmacenes = new MedDAL.Almacenes.DALAlmacenes();
            odalContactosAlmacenes = new MedDAL.AlmacenesContactos.DALAlmacenesContactos();
        }

        /// <summary>
        /// Obtiene todos los almacenes activos
        /// </summary>
        /// <returns></returns>
        public IQueryable<MedDAL.DAL.almacenes> BuscarAlmacenesActivos()
        {
            return odalAlmacenes.BuscarAlmacenesActivos();
        }

        /// <summary>
        /// Obtiene todos los almacenes
        /// </summary>
        /// <returns></returns>
        public List<MedDAL.DAL.almacenes> ObtenerAlmacenes()
        {
            return odalAlmacenes.ObtenerAlmacenes();
        }

       

        /// <summary>
        /// Busca a los almacenes que coincidan con la cadena y el filtro especificado.
        /// Si la cadena es vacía, regresa todos los registros, todo esto lo filtra por almacen
        /// </summary>
        /// <param name="sCadena">La cadena por la cual buscar</param>
        /// <param name="iFiltro"></param>
        /// <returns></returns>
        public IQueryable<MedDAL.Almacenes.AlmacenesView> BuscarFiltradaAlmacenes(string sCadena, int iFiltro, int iIdAlmacen)
        {
            return odalAlmacenes.BuscarFiltradaAlmacenes(sCadena, iFiltro, iIdAlmacen);
        }


        /// <summary>
        /// Busca a los almacenes que coincidan con la cadena y el filtro especificado.
        /// Si la cadena es vacía, regresa todos los registros.
        /// </summary>
        /// <param name="sCadena">La cadena por la cual buscar</param>
        /// <param name="iFiltro"></param>
        /// <returns></returns>
        public IQueryable<MedDAL.Almacenes.AlmacenesView> Buscar(string sCadena, int iFiltro)
        {
            return odalAlmacenes.Buscar(sCadena, iFiltro);
        }

        /// <summary>
        /// Obtiene al almacen que coincida con la clave
        /// </summary>
        /// <param name="sClave"></param>
        /// <returns></returns>
        public MedDAL.DAL.almacenes Buscar(string sClave)
        {
            return odalAlmacenes.Buscar(sClave);
        }

        public MedDAL.DAL.almacenes Buscar(int idAlmacen)
        {
            return odalAlmacenes.Buscar(idAlmacen);
        }

        /// <summary>
        /// Registra un nuevo almacén
        /// </summary>
        /// <param name="oAlmacen"></param>
        /// <returns></returns>
        public bool NuevoRegistro(MedDAL.DAL.almacenes oAlmacen)
        {
            return odalAlmacenes.NuevoRegistro(oAlmacen);
        }

        /// <summary>
        /// Actualiza un almacén 
        /// </summary>
        /// <param name="oAlmacen"></param>
        /// <returns></returns>
        public bool EditarRegistro(MedDAL.DAL.almacenes oAlmacen)
        {
            return odalAlmacenes.EditarRegistro(oAlmacen);
        }

        /// <summary>
        /// Intenta eliminar un registro de la base de datos.
        /// </summary>
        /// <param name="oAlmacen"></param>
        /// <returns></returns>
        public bool EliminarRegistro(MedDAL.DAL.almacenes oAlmacen)
        {
            if (odalContactosAlmacenes.EliminarRegistro(oAlmacen))
            {
                return odalAlmacenes.EliminarRegistro(oAlmacen);
            }
            else
            {
                return false;
            }
        }
    }
}
