using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Objects;
using System.Data.Objects.DataClasses;

namespace MedDAL.Poblaciones
{
    public class DALPoblaciones
    {
        DAL.medicuriEntities oMedicuriEntities;

        public DALPoblaciones()
        {
            oMedicuriEntities = new DAL.medicuriEntities();
        }

        /// <summary>
        /// Busca las poblaciones activas en la base de datos
        /// </summary>
        /// <param name="sCadena">La cadena por la cual se va a realizar la búsqueda</param>
        /// <param name="iFiltro">1=Nombre y Clave, 2=Clabe, 3=Nombre</param>
        /// <returns>IQueryable Resultado de la búsqueda</returns>
        public object Buscar(string sCadena, int iFiltro)
        {
            string sConsulta = "";
            switch (iFiltro)
            {
                case 1:
                    sConsulta = "(it.Nombre LIKE '%'+@Dato+'%' OR it.Clave LIKE '%'+@Dato+'%') and it.Activo = true";
                    break;
                case 2:
                    sConsulta = "it.Clave LIKE '%'+@Dato+'%' and it.Activo = true";
                    break;
                case 3:
                    sConsulta = "it.Nombre LIKE '%'+@Dato+'%' and it.Activo = true";
                    break;
            }

            var oQuery = from q in oMedicuriEntities.poblaciones.
                                      Where(sConsulta,
                                      new ObjectParameter("Dato", sCadena))
                         select q;

            return oQuery;
        }

        /// <summary>
        /// Buscar las poblaciones pertenecientes a un Estado
        /// </summary>
        /// <param name="sCadena">La cadena por la cual se va a realizar la búsqueda</param>
        /// <param name="iIdEstado">El id del estado con el cual está ligada la población</param>
        /// <param name="iFiltro">1=Nombre y Clave, 2=Clave, 3=Nombre</param>
        /// <returns>IQueryable Resultado de la búsqueda</returns>
        public object BuscarEstados(string sCadena, int iIdEstado, int iFiltro)
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

            var oQuery = from q in oMedicuriEntities.poblaciones.
                            Where(sConsulta,
                            new ObjectParameter("Dato", sCadena))
                         where q.estados.idEstado == iIdEstado
                         select q;

            return oQuery;
        }

        /// <summary>
        /// Buscar las poblaciones pertenecientes a un Municipio
        /// </summary>
        /// <param name="sCadena">La cadena por la cual se va a realizar la búsqueda</param>
        /// <param name="iIdMunicipio">El id del municipio con el cual está ligada la población</param>
        /// <param name="iFiltro">1=Nombre y Clave, 2=Clave, 3=Nombre</param>
        /// <returns>IQueryable Resultado de la búsqueda</returns>
        public object BuscarMunicipios(string sCadena, int iIdMunicipio, int iFiltro)
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

            var oQuery = from q in oMedicuriEntities.poblaciones.
                            Where(sConsulta,
                            new ObjectParameter("Dato", sCadena))
                            where q.municipios.idMunicipio == iIdMunicipio
                            select q;

            return oQuery;
        }

        public IQueryable<MedDAL.Poblaciones.PoblacionesView> Buscar(string sCadena, int iIdMunicipio, int iFiltro)
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

            IQueryable<MedDAL.Poblaciones.PoblacionesView> oQuery = from q in oMedicuriEntities.poblaciones.
                            Where(sConsulta,
                            new ObjectParameter("Dato", sCadena))
                                                                    where q.municipios.idMunicipio == iIdMunicipio
                                                                    select new PoblacionesView
                                                                    {
                                                                        idPoblacion = q.idPoblacion,
                                                                        Clave = q.Clave,
                                                                        Nombre = q.Nombre,
                                                                        Activo = q.Activo
                                                                    };

            return oQuery;
        }

        /// <summary>
        /// Busca todas las poblaciones
        /// </summary>
        /// <returns>Un una lista de las propiedades de poblaciones</returns>
        public object BuscarPoblaciones() 
        {
            var oQuery = (from q in oMedicuriEntities.poblaciones
                          select new
                          {
                              q.Clave,
                              q.Nombre,
                              q.Activo                              
                          });
            return oQuery;
        }

        /// <summary>
        /// Obtener una poblacion
        /// </summary>
        /// <param name="iId"></param>
        /// <returns></returns>
        public object Buscar(int iId)
        {
            var oQuery = from q in oMedicuriEntities.poblaciones
                         where q.idPoblacion == iId
                         select q;

            return (object)oQuery.First<MedDAL.DAL.poblaciones>();
        }

        /// <summary>
        /// Obtener las poblaciones Activas
        /// </summary>
        /// <returns></returns>
        public object Buscar()
        {
            var oQuery = from q in oMedicuriEntities.poblaciones
                         where q.Activo == true
                         select q;

            return oQuery;
        }

        /// <summary>
        /// Obtiene la poblacion que coincida con la clave
        /// </summary>
        /// <param name="sClave"></param>
        /// <returns>Una poblacion si coincide con la clave, si no, null</returns>
        public MedDAL.DAL.poblaciones Buscar(string sClave)
        {
            var oQuery = from q in oMedicuriEntities.poblaciones
                         where q.Clave.ToUpper() == sClave.ToUpper()
                         select q;

            return oQuery.Count<MedDAL.DAL.poblaciones>() != 0 ? oQuery.First<MedDAL.DAL.poblaciones>() : null;
        }

        /// <summary>
        /// Obtener las poblaciones Activas
        /// </summary>
        /// <returns></returns>
        public IQueryable<DAL.poblaciones> BuscarEnum()
        {
            var oQuery = from q in oMedicuriEntities.poblaciones
                         where q.Activo == true
                         select q;

            return oQuery;
        }

        public bool NuevoRegistro(DAL.poblaciones oPoblacion)
        {
            try
            {
                oMedicuriEntities.AddTopoblaciones(oPoblacion);
                oMedicuriEntities.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool EditarRegistro(DAL.poblaciones oPoblacion)
        {
            try
            {
                var oQuery = from q in oMedicuriEntities.poblaciones.
                            Where("it.idPoblacion = @idPoblacion",
                            new ObjectParameter("idPoblacion", oPoblacion.idPoblacion))
                             select q;

                DAL.poblaciones oPoblacionOriginal = oQuery.First<DAL.poblaciones>();
                oPoblacionOriginal.Nombre = oPoblacion.Nombre;
                oPoblacionOriginal.Activo = oPoblacion.Activo;
                oPoblacionOriginal.idMunicipio = oPoblacion.idMunicipio;
                oPoblacionOriginal.idEstado = oPoblacion.idEstado;

                oMedicuriEntities.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool EliminarRegistro(DAL.poblaciones oPoblacion)
        {
            try
            {
                var oQuery = from q in oMedicuriEntities.poblaciones.
                            Where("it.idPoblacion = @idPoblacion",
                            new ObjectParameter("idPoblacion", oPoblacion.idPoblacion))
                             select q;

                DAL.poblaciones oPoblacionOriginal = oQuery.First<DAL.poblaciones>();

                oMedicuriEntities.DeleteObject(oPoblacionOriginal);
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
