using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Objects;
using System.Data.Objects.DataClasses;

namespace MedDAL.Estados
{
    public class DALEstados
    {
        DAL.medicuriEntities oMedicuriEntities;

        public DALEstados()
        {
            oMedicuriEntities = new DAL.medicuriEntities();
        }

        /// <summary>
        /// Obtiene los estados que coincidan con la búsqueda y el filtro
        /// </summary>
        /// <param name="sCadena">La cadena por la cual buscar</param>
        /// <param name="iFiltro">El filtro a aplicar: 1=todo, 2=Clave, 3=Nombre</param>
        /// <returns></returns>
        public object Buscar(string sCadena, int iFiltro)
        {
            string sConsulta = "";
            switch (iFiltro) 
            { 
                case 1:
                    sConsulta = "(it.Nombre LIKE '%'+@Dato+'%' OR it.Clave LIKE '%'+@Dato+'%')";
                    break;
                case 2:
                    sConsulta = "it.Clave LIKE '%'+@Dato+'%'";                    
                    break;
                case 3:
                    sConsulta = "it.Nombre LIKE '%'+@Dato+'%'";
                    break;
            }

            var oQuery = from q in oMedicuriEntities.estados.
                                      Where(sConsulta,
                                      new ObjectParameter("Dato", sCadena))
                         select q;

            return oQuery;
        }

        /// <summary>
        /// Obtiene un estado. 
        /// </summary>
        /// <param name="iId">El id del estado</param>
        /// <returns></returns>
        public object Buscar(int iId) 
        {
            var oQuery = from q in oMedicuriEntities.estados
                         where q.idEstado == iId
                         select q;

            return (object)oQuery.First<MedDAL.DAL.estados>();
        }

        /// <summary>
        /// Busca todos los estados que estén activos
        /// </summary>
        /// <returns>La coleccion de estados</returns>
        public IQueryable<DAL.estados> BuscarEnum()
        {
            var oQuery = from q in oMedicuriEntities.estados
                         where q.Activo == true
                         select q;

            return oQuery;
        }

        /// <summary>
        /// Busca todos los estados que estén activos
        /// </summary>
        /// <return>El resultado  de la búsqueda</return></returns>
        public object Buscar() 
        {
            var oQuery = from q in oMedicuriEntities.estados
                         where q.Activo == true
                         select q;

            return oQuery;
        }

        /// <summary>
        /// Obtiene el estado que coincida con la clave
        /// </summary>
        /// <param name="sClave"></param>
        /// <returns>Un estado si coincide con la clave, si no, null</returns>
        public MedDAL.DAL.estados Buscar(string sClave)
        {
            var oQuery = from q in oMedicuriEntities.estados
                         where q.Clave.ToUpper() == sClave.ToUpper()
                         select q;

            return oQuery.Count<MedDAL.DAL.estados>() != 0 ? oQuery.First<MedDAL.DAL.estados>() : null;
        }

        /// <summary>
        /// Inserta un nuevo estado
        /// </summary>
        /// <param name="oEstado"></param>
        /// <returns></returns>
        public bool NuevoRegistro(DAL.estados oEstado) 
        {
            try
            {
                oMedicuriEntities.AddToestados(oEstado);
                oMedicuriEntities.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Actualiza un estado
        /// </summary>
        /// <param name="oEstado"></param>
        /// <returns></returns>
        public bool EditarRegistro(DAL.estados oEstado)
        {
            try
            {     
                var oQuery = from q in oMedicuriEntities.estados.
                         Where("it.idEstado = @idEstado",
                         new ObjectParameter("idEstado", oEstado.idEstado))
                         select q;

                DAL.estados oEstadoOriginal = oQuery.First<DAL.estados>();               
                oEstadoOriginal.Nombre = oEstado.Nombre;
                oEstadoOriginal.Activo = oEstado.Activo;
                
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
        /// <param name="oEstado"></param>
        /// <returns></returns>
        public bool EliminarRegistro(DAL.estados oEstado) 
        {
            try
            {
                var oQuery = from q in oMedicuriEntities.estados.
                            Where("it.idEstado = @idEstado",
                            new ObjectParameter("idEstado", oEstado.idEstado))
                             select q;

                DAL.estados oEstadoOriginal = oQuery.First<DAL.estados>();

                oMedicuriEntities.DeleteObject(oEstadoOriginal);
                oMedicuriEntities.SaveChanges();
                return true;
            }
            catch 
            {
                /*try
                {                    
                    var oQuery = from q in oMedicuriEntities.estados.
                            Where("it.idEstado = @idEstado",
                            new ObjectParameter("idEstado", oEstado.idEstado))
                                 select q;
                    DAL.estados oEstadoOriginal = oQuery.First<DAL.estados>();
                    oMedicuriEntities.Refresh(RefreshMode.StoreWins, oEstadoOriginal);
                    oEstadoOriginal.Activo = false;
                    oMedicuriEntities.SaveChanges();
                    return true;
                }
                catch 
                {
                    return false;
                }*/
                return false;
            }
        }


    }
}
