using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Objects;
using System.Data.Objects.DataClasses;

namespace MedDAL.Colonias
{
    public class DALColonias
    {
        DAL.medicuriEntities oMedicuriEntities;

        public DALColonias()
        {
            oMedicuriEntities = new DAL.medicuriEntities();
        }

        /// <summary>
        /// Busca las colonias activas en la base de datos
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

            var oQuery = from q in oMedicuriEntities.colonias.
                                      Where(sConsulta,
                                      new ObjectParameter("Dato", sCadena))
                         select q;

            return oQuery;
        }

        /// <summary>
        /// Buscar las Colonias pertenecientes a un Estado
        /// </summary>
        /// <param name="sCadena">La cadena por la cual se va a realizar la búsqueda</param>
        /// <param name="iIdEstado">El id del estado con el cual está ligada la colonia</param>
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

            var oQuery = from q in oMedicuriEntities.colonias.
                            Where(sConsulta,
                            new ObjectParameter("Dato", sCadena))
                         where q.estados.idEstado == iIdEstado
                         select q;

            return oQuery;
        }

        /// <summary>
        /// Buscar las Colonias pertenecientes a un Municipio
        /// </summary>
        /// <param name="sCadena">La cadena por la cual se va a realizar la búsqueda</param>
        /// <param name="iIdMunicipio">El id del municipio con el cual está ligada la colonia</param>
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

            var oQuery = from q in oMedicuriEntities.colonias.
                            Where(sConsulta,
                            new ObjectParameter("Dato", sCadena))
                         where q.municipios.idMunicipio == iIdMunicipio
                         select q;

            return oQuery;
        }

        /// <summary>
        /// Buscar las Colonias pertenecientes a una Población
        /// </summary>
        /// <param name="sCadena">La cadena por la cual se va a realiar la búsqueda</param>
        /// <param name="iIdPoblacion">El id de la poblacion con la cual está ligada la colonia</param>
        /// <param name="iFiltro">1=Nombre y Clave, 2=Clave, 3=Nombre</param>
        /// <returns>IQueryable Resultado de la búsqueda</returns>
        public object BuscarPoblaciones(string sCadena, int iIdPoblacion, int iFiltro)
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

            var oQuery = from q in oMedicuriEntities.colonias.
                            Where(sConsulta,
                            new ObjectParameter("Dato", sCadena))
                         where q.poblaciones.idPoblacion == iIdPoblacion
                         select q;

            return oQuery;
        }

        public IQueryable<ColoniasView> Buscar(string sCadena, int idPoblacion, int iFiltro) 
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

            IQueryable<ColoniasView> oQuery = from q in oMedicuriEntities.colonias.
                            Where(sConsulta,
                            new ObjectParameter("Dato", sCadena))
                                              where q.poblaciones.idPoblacion == idPoblacion
                                              select new ColoniasView
                                              { 
                                                idColonia = q.idColonia,
                                                Clave =q.Clave,
                                                Nombre = q.Nombre,
                                                Activo = q.Activo
                                              };

            return oQuery;
        }

        /// <summary>
        /// Obtener una colonia
        /// </summary>
        /// <param name="iId"></param>
        /// <returns></returns>
        public object Buscar(int iId)
        {
            var oQuery = from q in oMedicuriEntities.colonias
                         where q.idColonia == iId
                         select q;

            return (object)oQuery.First<MedDAL.DAL.colonias>();
        }

        /// <summary>
        /// Obtener todas las Colonias activas
        /// </summary>
        /// <returns></returns>
        public object Buscar()
        {
            var oQuery = from q in oMedicuriEntities.colonias
                         where q.Activo == true
                         select q;

            return oQuery;
        }

        /// <summary>
        /// Obtiene la colonia que coincida con la clave
        /// </summary>
        /// <param name="sClave"></param>
        /// <returns>Una colonia si coincide con la clave, si no, null</returns>
        public MedDAL.DAL.colonias Buscar(string sClave)
        {
            var oQuery = from q in oMedicuriEntities.colonias
                         where q.Clave.ToUpper() == sClave.ToUpper()
                         select q;

            return oQuery.Count<MedDAL.DAL.colonias>() != 0 ? oQuery.First<MedDAL.DAL.colonias>() : null;
        }

        /// <summary>
        /// Obtener las Colonias Activas
        /// </summary>
        /// <returns></returns>
        public IQueryable<DAL.colonias> BuscarEnum()
        {
            var oQuery = from q in oMedicuriEntities.colonias
                         where q.Activo == true
                         select q;

            return oQuery;
        }

        public bool NuevoRegistro(DAL.colonias oColonia)
        {
            try
            {
                oMedicuriEntities.AddTocolonias(oColonia);
                oMedicuriEntities.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool EditarRegistro(DAL.colonias oColonia)
        {
            try
            {
                var oQuery = from q in oMedicuriEntities.colonias.
                            Where("it.idColonia = @idColonia",
                            new ObjectParameter("idColonia", oColonia.idColonia))
                             select q;

                DAL.colonias oColoniaOriginal = oQuery.First<DAL.colonias>();
                oColoniaOriginal.Nombre = oColonia.Nombre;
                oColoniaOriginal.Activo = oColonia.Activo;
                oColoniaOriginal.IdPoblacion = oColonia.IdPoblacion;
                oColoniaOriginal.IdMunicipio = oColonia.IdMunicipio;
                oColoniaOriginal.IdEstado = oColonia.IdEstado;

                oMedicuriEntities.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool EliminarRegistro(DAL.colonias oColonia)
        {
            try
            {
                var oQuery = from q in oMedicuriEntities.colonias.
                            Where("it.idColonia = @idColonia",
                            new ObjectParameter("idColonia", oColonia.idColonia))
                             select q;

                DAL.colonias oColoniaOriginal = oQuery.First<DAL.colonias>();

                oMedicuriEntities.DeleteObject(oColoniaOriginal);
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
