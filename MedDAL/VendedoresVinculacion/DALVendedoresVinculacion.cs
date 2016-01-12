using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Objects;

namespace MedDAL.VendedoresVinculacion
{
    public class DALVendedoresVinculacion
    {
        DAL.medicuriEntities oMedicuriEntities;

        public DALVendedoresVinculacion()
        {
            oMedicuriEntities = new DAL.medicuriEntities();
        }

        // GT: 21/Mar/2011 Función que recupera la vinculacion de un vendedor mediante su nombre
        /// <summary>
        /// DAL - Buscar vinculacion mediante el nombre
        /// </summary>
        /// <param name="sCadena">Parametro de busqueda</param>
        /// <returns></returns>
        public string[] BuscarVinculacionAsincrono(string sCadena)
        {
            string[] asResultados;

            try
            {
                var oQuery = from q in oMedicuriEntities.vendedores_vinculacion
                             where q.Vinculacion.Contains(sCadena)
                             select q.Vinculacion;

                return asResultados = oQuery.ToArray<string>();

            }
            catch
            {
                return asResultados = new string[0];
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sVinculacion"></param>
        /// <returns></returns>
        public object Buscar(string sVinculacion)
        {
            var oQuery = from q in oMedicuriEntities.vendedores_vinculacion
                         where q.Vinculacion == sVinculacion
                         select q;

            if (oQuery.Count<MedDAL.DAL.vendedores_vinculacion>() > 0)
            {
                return (object)oQuery.First<MedDAL.DAL.vendedores_vinculacion>();
            }
            else return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sVinculacion"></param>
        /// <returns></returns>
        public object Buscar(int idVinculacion)
        {
            var oQuery = from q in oMedicuriEntities.vendedores_vinculacion
                         where q.idVinculacion == idVinculacion
                         select q;

            if (oQuery.Count<MedDAL.DAL.vendedores_vinculacion>() > 0)
            {
                return (object)oQuery.First<MedDAL.DAL.vendedores_vinculacion>();
            }
            else return null;
        }

        /// <summary>
        /// DAL - Insertar nuevo registro
        /// </summary>
        /// <param name="oVinculacion">Registro a guardar</param>
        /// <returns>true registrado, false no registrado</returns>
        public bool NuevoRegistro(DAL.vendedores_vinculacion oVinculacion)
        {
            try
            {
                //Agregar el registro
                oMedicuriEntities.AddTovendedores_vinculacion(oVinculacion);
                oMedicuriEntities.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
