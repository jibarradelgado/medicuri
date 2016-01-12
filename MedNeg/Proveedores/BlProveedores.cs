using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Objects;
using System.Data.Objects.DataClasses;

namespace MedNeg.Proveedores
{
    public class BlProveedores
    {
        MedDAL.Proveedores.DALProveedores odalProveedores;
        MedDAL.ProveedoresContactos.DALProveedoresContactos odalContactosProveedores;

        public BlProveedores()
        {
            odalProveedores = new MedDAL.Proveedores.DALProveedores();
            odalContactosProveedores = new MedDAL.ProveedoresContactos.DALProveedoresContactos();
        }

        /// <summary>
        /// Obtiene todos los proveedores activos
        /// </summary>
        /// <returns></returns>
        public IQueryable<MedDAL.DAL.proveedores> ObtenerProveedoresActivos()
        {            
            return odalProveedores.ObtenerProveedoresActivos();
        }

        /// <summary>
        /// Obtiene todos los proveedores
        /// </summary>
        /// <returns></returns>
        public List<MedDAL.DAL.proveedores> ObtenerProveedores()
        {
            return odalProveedores.ObtenerProveedores();
        }

        /// <summary>
        /// Busca a los proveedores que coincidan con la cadena y el filtro especificado.
        /// Si la cadena es vacía, regresa todos los registros.
        /// </summary>
        /// <param name="sCadena">La cadena por la cual buscar</param>
        /// <param name="iFiltro"></param>
        /// <returns></returns>
        public IQueryable<MedDAL.Proveedores.ProveedoresView> Buscar(string sCadena, int iFiltro)
        {            
            return odalProveedores.Buscar(sCadena, iFiltro);
        }

        /// <summary>
        /// Obtiene al proveedor que coincida con la clave
        /// </summary>
        /// <param name="sClave"></param>
        /// <returns></returns>
        public MedDAL.DAL.proveedores Buscar(string sClave)
        {
            return odalProveedores.Buscar(sClave);
        }

        public MedDAL.DAL.proveedores Buscar(int idProveedor)
        {
            return odalProveedores.Buscar(idProveedor);
        }
        /// <summary>
        /// Registra un nuevo proveedor
        /// </summary>
        /// <param name="oProveedor"></param>
        /// <returns></returns>
        public bool NuevoRegistro(MedDAL.DAL.proveedores oProveedor)
        {
            return odalProveedores.NuevoRegistro(oProveedor);            
        }

        /// <summary>
        /// Actualiza un proveedor 
        /// </summary>
        /// <param name="oProveedor"></param>
        /// <returns></returns>
        public bool EditarRegistro(MedDAL.DAL.proveedores oProveedor)
        {            
            return odalProveedores.EditarRegistro(oProveedor);            
        }

        /// <summary>
        /// Intenta eliminar un registro de la base de datos.
        /// </summary>
        /// <param name="oProveedor"></param>
        /// <returns></returns>
        public bool EliminarRegistro(MedDAL.DAL.proveedores oProveedor)
        {
            if (odalContactosProveedores.EliminarRegistro(oProveedor))
            {
                return odalProveedores.EliminarRegistro(oProveedor);
            }
            else
            {
                return false;
            }
        }
    }
}
