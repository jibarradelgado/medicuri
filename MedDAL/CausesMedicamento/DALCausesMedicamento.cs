using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Objects;

namespace MedDAL.CausesMedicamento
{
    public class DALCausesMedicamento
    {
        DAL.medicuriEntities oMedicuriEntities;

        public DALCausesMedicamento() {
            oMedicuriEntities = new DAL.medicuriEntities();
        }

        /// <summary>
        /// DAL - Insertar nuevo registro
        /// </summary>
        /// <param name="oCausesMedicamento">Registro a guardar</param>
        /// <returns>true registrado, false no registrado</returns>
        public bool NuevoRegistro(DAL.causes_medicamentos oCausesMedicamento)
        {
            try
            {
                //Agregar el registro
                oMedicuriEntities.AddTocauses_medicamentos(oCausesMedicamento);
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
                var oQuery = from q in oMedicuriEntities.causes_medicamentos
                             where q.idCause == oCause.idCause
                             select q;

                foreach (MedDAL.DAL.causes_medicamentos oCausesMedicamentos in oQuery)
                {
                    oMedicuriEntities.DeleteObject(oCausesMedicamentos);
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
        /// Obtiene los causes que coincidan con la búsqueda y el filtro
        /// </summary>
        /// <param name="sCadena">La cadena por la cual buscar</param>
        /// <param name="iFiltro">El filtro a aplicar: 1=todo, 2=Clave, 3=Nombre</param>
        /// <returns></returns>
        public List<MedDAL.DAL.causes_medicamentos> Buscar(string sCadena, int iFiltro)
        {
            List<MedDAL.DAL.causes_medicamentos> lstCausesMedicamentos = new List<DAL.causes_medicamentos>();

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

            var oQuery = from q in oMedicuriEntities.causes_medicamentos.
                                      Where(sConsulta,
                                      new ObjectParameter("Dato", sCadena))
                         select q;

            lstCausesMedicamentos.AddRange(oQuery);

            return lstCausesMedicamentos;
        }


    }
}
