using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Objects;
using System.Data.Objects.DataClasses;

namespace MedDAL.Bitacora
{
    public class DALBitacora
    {
        DAL.medicuriEntities oMedicuriEntities;

        public DALBitacora() 
        {
            oMedicuriEntities = new DAL.medicuriEntities();
        }

        /// <summary>
        /// Obtener todos los registros de bitacora
        /// </summary>
        /// <returns></returns>
        public IQueryable<MedDAL.DAL.bitacora> Buscar()
        {            
            var oQuery = from q in oMedicuriEntities.bitacora                                      
                         select q;

            return oQuery;
        }

        public IQueryable<MedDAL.DAL.bitacora> Buscar(string sCadena, int iFiltro) 
        {
            string sConsulta = "";

            switch (iFiltro)
            {
                case 1:
                    sConsulta = "it.Usuario LIKE '%'+@Dato+'%' OR it.Modulo LIKE '%'+@Dato+'%'";
                    break;
                case 2:
                    sConsulta = "it.Usuario LIKE '%'+@Dato+'%'";
                    break;
                case 3:
                    sConsulta = "it.Modulo LIKE '%'+@Dato+'%'";
                    break;
            }

            var oQuery = from q in oMedicuriEntities.bitacora.
                                      Where(sConsulta,
                                      new ObjectParameter("Dato", sCadena))
                         select q;
            
            return oQuery;
        }

        public IQueryable<MedDAL.DAL.bitacora> Buscar(string sCadena, int iFiltro, string sFecha1, string sFecha2)
        {
            string sConsulta = "";

            switch (iFiltro)
            { 
                case 1:
                    sConsulta = "(it.Usuario LIKE '%'+@Dato+'%' OR it.Modulo LIKE '%'+@Dato+'%') AND (it.FechaEntradaSrv between @Fecha1 AND @Fecha2)";
                    break;
                case 2:
                    sConsulta = "(it.Usuario LIKE '%'+@Dato+'%') AND (it.FechaEntradaSrv between @Fecha1 AND @Fecha2)";
                    break;
                case 3:
                    sConsulta = "(it.Modulo LIKE '%'+@Dato+'%') AND (it.FechaEntradaSrv between @Fecha1 AND @Fecha2)";
                    break;
            }

            var oQuery = from q in oMedicuriEntities.bitacora.
                            Where(sConsulta,
                            new ObjectParameter("Dato", sCadena),
                            new ObjectParameter("Fecha1", DateTime.Parse(sFecha1)),
                            new ObjectParameter("Fecha2", DateTime.Parse(sFecha2)))
                         select q;

            return oQuery;
        }

        public void Respaldar() 
        {
            
        }

        /// <summary>
        /// DAL - Muestra todos los registros de la tabla bitacora
        /// </summary>
        /// <returns>Objeto</returns>
        public object MostrarLista()
        {
            var oQuery = from q in oMedicuriEntities.bitacora select q;//orderby q.FechaEntradaSrv select q;
            return oQuery;

        }


        public bool NuevoRegistro(DAL.bitacora oBitacora) 
        {
            try
            {
                oMedicuriEntities.AddTobitacora(oBitacora);
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
                foreach(MedDAL.DAL.bitacora oBitacora in Buscar())
                {
                    oMedicuriEntities.DeleteObject(oBitacora);    
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
