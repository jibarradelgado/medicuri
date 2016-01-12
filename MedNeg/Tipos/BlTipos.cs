using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Objects;
using System.Data.Objects.DataClasses;

namespace MedNeg.Tipos
{
    public class BlTipos
    {

        MedDAL.Tipos.DALTipos odalTipos;


        public BlTipos()
        {
            odalTipos = new MedDAL.Tipos.DALTipos();
        }


        /// <summary>
        /// BL - Registrar Tipo
        /// </summary>
        /// <param name="oTipo">Tipo a registrar</param>
        /// <returns></returns>
        public bool NuevoRegistro(MedDAL.DAL.tipos oTipo)
        {
            return odalTipos.NuevoRegistro(oTipo);
        }


        /// <summary>
        /// BL - Buscar tipo mediante filtros
        /// </summary>
        /// <param name="sCadena">Cadena a buscar</param>
        /// <param name="iFiltro">Filtro a utilizar</param>
        /// <returns></returns>
        public object Buscar(string sCadena, int iFiltro)
        {
            return odalTipos.Buscar(sCadena, iFiltro);
        }

        public string Buscar(int idTipo)
        {
            return odalTipos.Buscar(idTipo);
        }

        /// <summary>
        /// BL - Mostrar todos los registros de las tablas
        /// </summary>
        /// <returns></returns>
        public object MostrarLista()
        {
            return odalTipos.MostrarLista();
        }

        /// <summary>
        /// Recupera todos los tipos
        /// </summary>
        /// <returns></returns>
        public List<MedDAL.DAL.tipos> RecuperarTipos()
        {
            return odalTipos.RecuperarTipos();
        }

        /// <summary>
        /// BL - Recuperar los tipos de Almacenes
        /// </summary>
        /// <returns></returns>
        public List<MedDAL.DAL.tipos> RecuperarTiposAlmacen()
        {
            return odalTipos.RecuperarTiposAlmacen();                 
        }

        /// <summary>
        /// BL - Recuperar los tipos de Clientes
        /// </summary>
        /// <returns></returns>
        public List<MedDAL.DAL.tipos> RecuperarTiposClientes()
        {
            return odalTipos.RecuperarTiposClientes();
        }

        /// <summary>
        /// BL - Recuperar los tipos de Productos
        /// </summary>
        /// <returns></returns>
        public List<MedDAL.DAL.tipos> RecuperarTiposProductos()
        {
            return odalTipos.RecuperarTiposProductos();
        }

        /// <summary>
        /// BL - Recuperar los tipos de Proveedores
        /// </summary>
        /// <returns></returns>
        public List<MedDAL.DAL.tipos> RecuperarTiposProveedores()
        {
            return odalTipos.RecuperarTiposProveedores();
        }


        /// <summary>
        /// BL - Recuperar los tipos de Vendedores
        /// </summary>
        /// <returns></returns>
        public List<MedDAL.DAL.tipos> RecuperarTiposVendedores()
        {
            return odalTipos.RecuperarTiposVendedores();
        }

        /// <summary>
        /// BL - Recuperar los tipos de Recetas
        /// </summary>
        /// <returns></returns>
        public List<MedDAL.DAL.tipos> RecuperarTiposRecetas()
        {
            return odalTipos.RecuperarTiposRecetas();
        }

        /// <summary>
        /// Editar registro
        /// </summary>
        /// <param name="oTipo">Tipo a eliminar</param>
        /// <returns></returns>
        public bool EditarRegistro(MedDAL.DAL.tipos oTipo)
        {
            return odalTipos.EditarRegistro(oTipo);
        }

        /// <summary>
        /// BL - Eliminar Registro
        /// </summary>
        /// <param name="iIdTipo">Id de tipo a eliminar</param>
        /// <returns></returns>
        public bool EliminarRegistro(int iIdTipo)
        {
            return odalTipos.EliminarRegistro(iIdTipo);

        }
    }

}
