using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MedNeg.VendedorVinculacion
{
    public class BlVendedorVinculacion
    {
        MedDAL.VendedoresVinculacion.DALVendedoresVinculacion oVinculacion;

        public BlVendedorVinculacion() {
            oVinculacion = new MedDAL.VendedoresVinculacion.DALVendedoresVinculacion();
        }
        /// <summary>
        /// recupera una vinculacion registrada
        /// </summary>
        /// <param name="sCadena">nombre de la vinculacion</param>
        /// <returns></returns>
        public object Buscar(string sCadena)
        {
            return oVinculacion.Buscar(sCadena);
        }

        /// <summary>
        /// recupera una vinculacion registrada
        /// </summary>
        /// <param name="idVinculacion">clave de vinculacion</param>
        /// <returns></returns>
        public object Buscar(int idVinculacion)
        {
            return oVinculacion.Buscar(idVinculacion);
        }

        /// <summary>
        /// Inserta un uevo registro
        /// </summary>
        /// <param name="oVendedorVinculacion">obj vinculacion</param>
        /// <returns></returns>
        public object NuevoRegistro(MedDAL.DAL.vendedores_vinculacion oVendedorVinculacion)
        {
            return oVinculacion.NuevoRegistro(oVendedorVinculacion);
        }
    }
}
