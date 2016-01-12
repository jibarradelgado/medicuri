using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Objects;
using System.Data.Objects.DataClasses;

namespace MedDAL.RecetasPartidaFaltantes
{
    public class DALRecetasPartidaFaltantes
    {
        DAL.medicuriEntities oMedicuriEntities;

        public DALRecetasPartidaFaltantes() 
        {
            oMedicuriEntities = new DAL.medicuriEntities();
        }

        /// <summary>
        /// Obtener todos los registros de bitacora
        /// </summary>
        /// <returns></returns>
        public IQueryable<MedDAL.DAL.recetas_partida_faltantes> BuscarTodo()
        {
            var oQuery = from q in oMedicuriEntities.recetas_partida_faltantes
                         select q;

            return oQuery;
        }

        /// <summary>
        /// Regresa los registros de faltantes de recetas que coincidan con el idProducto e idAlmacen
        /// </summary>
        /// <param name="idProducto"></param>
        /// <param name="idAlmacen"></param>
        /// <returns></returns>
        public List<MedDAL.DAL.recetas_partida_faltantes> BuscarPorProductoAlmacen(int idProducto, int idAlmacen)
        { 
            List<MedDAL.DAL.recetas_partida_faltantes> lstRecetasPartidaFaltantes = new List<DAL.recetas_partida_faltantes>();

            var oQuery = from q in oMedicuriEntities.recetas_partida_faltantes
                         where q.idProducto == idProducto && q.idAlmacen == idAlmacen
                         select q;

            lstRecetasPartidaFaltantes.AddRange(oQuery);

            return lstRecetasPartidaFaltantes;
        }

        public bool NuevoRegistro(DAL.recetas_partida_faltantes oRecetasPartidaFaltante)
        {
            try
            {
                oMedicuriEntities.AddTorecetas_partida_faltantes(oRecetasPartidaFaltante);
                oMedicuriEntities.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool EliminarRegistro(DAL.recetas_partida_faltantes oRecetasPartidaFaltante)
        {
            
            try
            {
                //Recuperar objeto original
                var oQueryOriginal = from q in oMedicuriEntities.recetas_partida_faltantes
                                     where q.idRecetasPartidaFaltantes == oRecetasPartidaFaltante.idRecetasPartidaFaltantes
                                     select q;

                DAL.recetas_partida_faltantes oRecetasPartidaFaltantesOriginal = oQueryOriginal.First<DAL.recetas_partida_faltantes>();

                //Comparar cantidad faltante vs cantidad entrante

                // si cantidad entrante >= cantidad faltante: eliminar
                if (oRecetasPartidaFaltante.Cantidad >= oRecetasPartidaFaltantesOriginal.Cantidad)
                {
                    //var oQuery = from q in oMedicuriEntities.recetas_partida_faltantes.
                    //            Where("it.idRecetasPartidaFaltantes = @idRecetasPartidaFaltantes",
                    //            new ObjectParameter("idRecetasPartidaFaltantes", oRecetasPartidaFaltante.idRecetasPartidaFaltantes))
                    //             select q;

                    //DAL.recetas_partida_faltantes oRecetasPartidaFaltantesOriginal = oQuery.First<DAL.recetas_partida_faltantes>();

                    oMedicuriEntities.DeleteObject(oRecetasPartidaFaltantesOriginal);
                
                }

                // si cantidad entrante < cantidad faltante: actualizar
                if (oRecetasPartidaFaltante.Cantidad < oRecetasPartidaFaltantesOriginal.Cantidad)
                {
                    oRecetasPartidaFaltantesOriginal.Cantidad -= oRecetasPartidaFaltante.Cantidad;
                }

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
                foreach (MedDAL.DAL.recetas_partida_faltantes oRecetasPartidaFaltante in BuscarTodo())
                {
                    oMedicuriEntities.DeleteObject(oRecetasPartidaFaltante);
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
