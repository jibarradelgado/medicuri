using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Objects;
using System.Data.Objects.DataClasses;

namespace MedNeg.LineasCredito
{
    public class BlLineasCredito
    {

        MedDAL.LineasCredito.DALLineasCredito odalLineaCredito;


        /// <summary>
        /// Constructor
        /// </summary>
        public BlLineasCredito()
        {
            odalLineaCredito = new MedDAL.LineasCredito.DALLineasCredito();
        }

        /// <summary>
        /// Buscar Línea Crédito
        /// </summary>
        /// <param name="sCadena">Parametro a buscar</param>
        /// <param name="iTipo">Filtro</param>
        /// <returns></returns>
        public object Buscar(string sCadena, int iTipo)
        {
            //return odalEstados.Buscar(sCadena, iTipo);
            return odalLineaCredito.Buscar(sCadena, iTipo);
        }

        /// <summary>
        /// BL - Función que muestra todos los registros de la tabla
        /// </summary>
        /// <returns></returns>
        public object MostrarLista()
        {
            return odalLineaCredito.MostrarLista();
        }

        /// <summary>
        /// Buscar un registro por su llave primaria
        /// </summary>
        /// <param name="id">Llave primaria</param>
        /// <returns></returns>
        public object Buscar(int id)
        {
            //return odalEstados.Buscar(id);
            return odalLineaCredito.Buscar(id);
        }

        /// <summary>
        ///  Registrar nueva línea crédito
        /// </summary>
        /// <param name="oLineaCredito">Línea crédito a registrar</param>
        /// <returns></returns>
        public bool NuevoRegistro(MedDAL.DAL.lineas_creditos oLineaCredito)
        {
            return odalLineaCredito.NuevoRegistro(oLineaCredito);
        }

        /// <summary>
        /// Editar una línea de crédito
        /// </summary>
        /// <param name="oLineaCredito">Línea de crédito a editar</param>
        /// <returns></returns>
        public bool EditarRegistro(MedDAL.DAL.lineas_creditos oLineaCredito)
        {
            return odalLineaCredito.EditarRegistro(oLineaCredito);
        }

        /// <summary>
        /// BL - Eliminar una línea de crédito
        /// </summary>
        /// <param name="oLineaCredito">Línea de crédito a eliminar</param>
        /// <returns></returns>
        public bool EliminarRegistro(MedDAL.DAL.lineas_creditos oLineaCredito)
        {
            return odalLineaCredito.EliminarRegistro(oLineaCredito);
        }


        public IQueryable<MedDAL.DAL.lineas_creditos> MostrarListaActivos()
        {
            return odalLineaCredito.MostrarListaActivos();
        }

        public int ValidarClaveRepetida(string sClave)
        {
            return odalLineaCredito.ValidarClaveRepetida(sClave);
        }
    }
}
