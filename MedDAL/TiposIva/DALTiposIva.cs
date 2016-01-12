using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Objects;
using System.Data.Objects.DataClasses;

namespace MedDAL.TiposIva
{
    public class DALTiposIva
    {
        DAL.medicuriEntities oMedicuriEntities;

        public DALTiposIva()
        {
            oMedicuriEntities = new DAL.medicuriEntities();
        }

        public List<MedDAL.DAL.tipo_iva> Buscar() 
        {
            List<MedDAL.DAL.tipo_iva> lstTiposIva = new List<DAL.tipo_iva>();

            var oQuery = from q in oMedicuriEntities.tipo_iva
                         select q;

            lstTiposIva.AddRange(oQuery); //oQuery.ToList<MedDAL.DAL.tipo_iva> XD

            return lstTiposIva;
        }

        public object Buscar(string sCadena,int iFiltro)
        {
            //List<MedDAL.DAL.tipo_iva> lstTiposIva = new List<DAL.tipo_iva>();

            string sConsulta = "";
            switch (iFiltro)
            {
                case 1:
                    sConsulta = "(it.Zona LIKE '%'+@Dato+'%' OR it.Iva LIKE '%'+@Dato+'%')";
                    break;
                case 2:
                    sConsulta = "it.Zona LIKE '%'+@Dato+'%'";
                    break;
                case 3:
                    sConsulta = "it.Iva LIKE '%'+@Dato+'%'";
                    break;
            }

            var oQuery = from q in oMedicuriEntities.tipo_iva.
                                      Where(sConsulta,
                                      new ObjectParameter("Dato", sCadena))
                         select q;

            

            return oQuery;
        }

        // VM: 29/Mar/2011 05:34 a.m. Función que recupera los tipos de iva
        // pero los regresa en un IQueryable (no further changes here)
        /// <summary>
        /// Recupera tipos de iva
        /// </summary>
        /// <returns></returns>
        public IQueryable<MedDAL.DAL.tipo_iva> BuscarGral() {
            var oQuery = from q in oMedicuriEntities.tipo_iva
                         select q;
            return (IQueryable<MedDAL.DAL.tipo_iva>)oQuery;
        }

        /// <summary>
        /// Inserta un nuevo Tipo de Iva
        /// </summary>
        /// <param name="oTipoIva"></param>
        /// <returns></returns>
        public bool NuevoRegistro(DAL.tipo_iva oTipoIva)
        {
            try
            {
                oMedicuriEntities.AddTotipo_iva(oTipoIva);
                oMedicuriEntities.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Actualiza un Tipo de Iva
        /// </summary>
        /// <param name="oTipoIva"></param>
        /// <returns></returns>
        public bool EditarRegistro(DAL.tipo_iva oTipoIva)
        {
            try
            {
                var oQuery = from q in oMedicuriEntities.tipo_iva.
                         Where("it.idTipoIva = @idTipoIva",
                         new ObjectParameter("idTipoIva", oTipoIva.idTipoIva))
                             select q;

                DAL.tipo_iva oTipoIvaOriginal = oQuery.First<DAL.tipo_iva>();
                oTipoIvaOriginal.Zona = oTipoIva.Zona;
                oTipoIvaOriginal.Iva = oTipoIva.Iva;
                oTipoIvaOriginal.Activo = oTipoIva.Activo;

                oMedicuriEntities.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Intenta eliminar un registro de la base de datos.
        /// </summary>
        /// <param name="oTipoIva"></param>
        /// <returns></returns>
        public bool EliminarRegistro(DAL.tipo_iva oTipoIva)
        {
            try
            {
                var oQuery = from q in oMedicuriEntities.tipo_iva.
                            Where("it.idTipoIva = @idTipoIva",
                            new ObjectParameter("idTipoIva", oTipoIva.idTipoIva))
                             select q;

                DAL.tipo_iva oTipoIvaOriginal = oQuery.First<DAL.tipo_iva>();

                oMedicuriEntities.DeleteObject(oTipoIvaOriginal);
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
