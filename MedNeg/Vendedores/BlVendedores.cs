using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MedNeg.Vendedores
{
    public class BlVendedores
    {

        MedDAL.Vendedores.DALVendedores odalVendedores;

        /// <summary>
        /// Constructor
        /// </summary>
        public BlVendedores() {
            odalVendedores = new MedDAL.Vendedores.DALVendedores();
        }

        /// <summary>
        /// BL - Buscar un vendedor
        /// </summary>
        /// <param name="sCadena">Parametro a buscar</param>
        /// <param name="iTipo">Filtro</param>
        /// <returns></returns>
        public object Buscar(string sCadena, int iTipo)
        {
            return odalVendedores.Buscar(sCadena, iTipo);
        }

        /// <summary>
        /// BL - Buscar un registro por su llave primaria
        /// </summary>
        /// <param name="id">Llave primaria</param>
        /// <returns></returns>
        public object Buscar(int id)
        {
            return odalVendedores.Buscar(id);
        }

        /// <summary>
        /// BL - Buscar un vendedor por su nombre de usuario
        /// </summary>
        /// <param name="sVendedor">Vendedor</param>
        /// <returns></returns>
        public object Buscar()
        {
            return odalVendedores.Buscar();
        }

        /// <summary>
        /// BL - Función que muestra todos los registros de la tabla
        /// </summary>
        /// <returns></returns>
        public object MostrarLista()
        {
            return odalVendedores.MostrarLista();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IQueryable<MedDAL.DAL.vendedores> BuscarEnum()
        {
            return odalVendedores.BuscarEnum();
        }


        /// <summary>
        ///  BL - Registrar un vendedor nuevo
        /// </summary>
        /// <param name="oVendedor">Usuario a registrar</param>
        /// <returns></returns>
        public bool NuevoRegistro(MedDAL.DAL.vendedores oVendedor)
        {
            return odalVendedores.NuevoRegistro(oVendedor);
        }

        /// <summary>
        /// BL - Editar un vendedor
        /// </summary>
        /// <param name="oVendedor">Vendedor a editar</param>
        /// <returns></returns>
        public bool EditarRegistro(MedDAL.DAL.vendedores oVendedor)
        {
            return odalVendedores.EditarRegistro(oVendedor);
        }

        /// <summary>
        /// BL - Eliminar un vendedor
        /// </summary>
        /// <param name="iIdVendedor">ID vendedor a eliminar</param>
        /// <returns></returns>
        public bool EliminarRegistro(int iIdVendedor)
        {
            return odalVendedores.EliminarRegistro(iIdVendedor);
        }

        /// <summary>
        /// BL -Validar un usuario por su nombre de usuario
        /// </summary>
        /// <param name="claveVendedor">Vendedor</param>
        /// <returns></returns>
        public int ValidarVendedorRepetido(string claveVendedor)
        {
            return odalVendedores.ValidarVendedorRepetido(claveVendedor);
        }
    }
}
