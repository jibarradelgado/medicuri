using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Objects;

namespace MedDAL.CausesCie
{
    public class DALCausesCie
    {
        DAL.medicuriEntities oMedicuriEntities;

        public DALCausesCie() {
            oMedicuriEntities = new DAL.medicuriEntities();
        }

        /// <summary>
        /// DAL - Insertar nuevo registro
        /// </summary>
        /// <param name="oCausesCie">Registro a guardar</param>
        /// <returns>true registrado, false no registrado</returns>
        public bool NuevoRegistro(DAL.causes_cie oCausesCie)
        {
            try
            {
                //Agregar el registro
                oMedicuriEntities.AddTocauses_cie(oCausesCie);
                oMedicuriEntities.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Elimina todos los registros que contengan relacion con el cause
        /// </summary>
        /// <param name="oAlmacen"></param>
        /// <returns>true si se realizo la eliminación, false si no</returns>
        public bool EliminarRegistro(MedDAL.DAL.causes oCause)
        {
            try
            {
                var oQuery = from q in oMedicuriEntities.causes_cie
                             where q.idCause == oCause.idCause
                             select q;

                foreach (MedDAL.DAL.causes_cie oCausesCie in oQuery)
                {
                    oMedicuriEntities.DeleteObject(oCausesCie);
                }

                oMedicuriEntities.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Validar cause
        /// </summary>REVISAR SEVERAMENTE
        /// <param name="idCauseCie">Nombre de causeCie</param>
        /// <returns>true si esta repetido, false si no</returns>
        public bool ValidarCauseRepetido(string claveCauseCie)
        {          
            //Recuperar el objeto a editar
            var oQuery = from q in oMedicuriEntities.causes_cie.
                            Where("it.Clave=@Clave",
                            new ObjectParameter("Clave", claveCauseCie))
                            select q;

            return oQuery.Count() > 0 ? true : false;

            
        }

        /// <summary>
        /// Obtiene los causes que coincidan con la búsqueda y el filtro
        /// </summary>
        /// <param name="sCadena">La cadena por la cual buscar</param>
        /// <param name="iFiltro">El filtro a aplicar: 1=todo, 2=Clave, 3=Nombre</param>
        /// <returns></returns>
        public List<MedDAL.DAL.causes_cie> Buscar(string sCadena, int iFiltro)
        {
            List<MedDAL.DAL.causes_cie> lstCausesCie = new List<DAL.causes_cie>();

            string sConsulta = "";
            switch (iFiltro)
            {
                case 1:
                    sConsulta = "(it.Tipo LIKE '%'+@Dato+'%' OR it.Clave LIKE '%'+@Dato+'%')";
                    break;
                case 2:
                    sConsulta = "it.Clave LIKE '%'+@Dato+'%'";
                    break;
                case 3:
                    sConsulta = "it.Tipo LIKE '%'+@Dato+'%'";
                    break;
            }

            var oQuery = from q in oMedicuriEntities.causes_cie.
                                      Where(sConsulta,
                                      new ObjectParameter("Dato", sCadena))
                         select q;

            lstCausesCie.AddRange(oQuery);

            return lstCausesCie;
        }

        public MedDAL.DAL.causes_cie Buscar(string sCadena)
        {
            var oQuery = from q in oMedicuriEntities.causes_cie
                         where q.Clave == sCadena
                         select q;

            if (oQuery.Count<MedDAL.DAL.causes_cie>() > 0)
            {
                return oQuery.First<MedDAL.DAL.causes_cie>();
            }
            else
            {
                return null;
            }
        }
    }
}
