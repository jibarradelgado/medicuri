using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Objects;
using System.Data.Objects.DataClasses;

namespace MedDAL.BitacoraFaltantes
{
    public class DALBitacoraFaltantes
    {
        DAL.medicuriEntities oMedicuriEntities;

        public DALBitacoraFaltantes()
        {
            oMedicuriEntities = new DAL.medicuriEntities();
        }

        /// <summary>
        /// Obtener todos los registros de bitacora
        /// </summary>
        /// <returns></returns>
        public IQueryable<MedDAL.DAL.bitacora_faltantes> BuscarTodo()
        {
            var oQuery = from q in oMedicuriEntities.bitacora_faltantes
                         select q;

            return oQuery;
        }

        public bool NuevoRegistro(DAL.bitacora_faltantes oBitacoraFaltantes)
        {
            try
            {
                oMedicuriEntities.AddTobitacora_faltantes(oBitacoraFaltantes);
                oMedicuriEntities.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool EliminarRegistro(DAL.bitacora_faltantes oBitacoraFaltantes)
        {
            try
            {
                var oQuery = from q in oMedicuriEntities.bitacora_faltantes.
                            Where("it.idBitacoraFaltantes = @idBitacoraFaltantes",
                            new ObjectParameter("idBitacoraFaltantes", oBitacoraFaltantes.idBitacoraFaltantes))
                            select q;

                DAL.bitacora_faltantes oBitacoraFaltantesOriginal = oQuery.First<DAL.bitacora_faltantes>();

                oMedicuriEntities.DeleteObject(oBitacoraFaltantesOriginal);
                oMedicuriEntities.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool EliminarTodo()
        {
            try
            {
                foreach (MedDAL.DAL.bitacora_faltantes oBitacoraFaltantes in BuscarTodo())
                {
                    oMedicuriEntities.DeleteObject(oBitacoraFaltantes);
                }
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
