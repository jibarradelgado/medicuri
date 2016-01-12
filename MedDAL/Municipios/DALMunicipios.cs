using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Objects;
using System.Data.Objects.DataClasses;

namespace MedDAL.Municipios
{
    public class DALMunicipios
    {
        DAL.medicuriEntities oMedicuriEntities;

        public DALMunicipios()
        {
            oMedicuriEntities = new DAL.medicuriEntities();
        }

        /// <summary>
        /// Buscar todos los municipios que coincidan con la búsqueda
        /// </summary>
        /// <param name="sCadena">La cadena por la cual se realiza la búsqueda</param>
        /// <param name="iFiltro">El filtro a aplicar: 1=Todo, 2=Clave, 3=Nombre</param>
        /// <returns></returns>
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

            var oQuery = from q in oMedicuriEntities.municipios.
                                      Where(sConsulta,
                                      new ObjectParameter("Dato", sCadena))
                         select q;

            return oQuery;
        }

        /// <summary>
        /// Busca los municipios que coincidan con el estado dado
        /// </summary>
        /// <param name="sCadena">La cadena por la cual se realiza la búsqueda</param>
        /// <param name="iIdEstado">El Id del estado</param>
        /// <param name="iFiltro">El filtro a aplicar: 1=Todo, 2=Clave, 3=Nombre</param>
        /// <returns></returns>
        public IQueryable<MunicipiosView> Buscar(string sCadena, int iIdEstado, int iFiltro) 
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

            IQueryable<MunicipiosView> oQuery = from q in oMedicuriEntities.municipios.
                            Where(sConsulta,
                            new ObjectParameter("Dato", sCadena))
                         where q.estados.idEstado == iIdEstado
                         select new MunicipiosView 
                         { 
                            idMunicipio = q.idMunicipio,
                            Nombre = q.Nombre,
                            Clave = q.Clave,
                            Activo = q.Activo
                         };

            return oQuery;
        }

        /// <summary>
        /// Obtiene un estado 
        /// </summary>
        /// <param name="iId"></param>
        /// <returns></returns>
        public object Buscar(int iId)
        {
            var oQuery = from q in oMedicuriEntities.municipios
                         where q.idMunicipio == iId
                         select q;

            return (object)oQuery.First<MedDAL.DAL.municipios>();
        }

        /// <summary>
        /// Obtiene todos los Municipios activos
        /// </summary>
        /// <returns></returns>
        public object Buscar() 
        {
            var oQuery = from q in oMedicuriEntities.municipios
                         where q.Activo == true
                         select q;

            return oQuery;
        }

        /// <summary>
        /// Obtiene el municipio que coincida con la clave
        /// </summary>
        /// <param name="sClave"></param>
        /// <returns>Un municipio si coincide con la clave, si no, null</returns>
        public MedDAL.DAL.municipios Buscar(string sClave)
        {
            var oQuery = from q in oMedicuriEntities.municipios
                         where q.Clave.ToUpper() == sClave.ToUpper()
                         select q;

            return oQuery.Count<MedDAL.DAL.municipios>() != 0 ? oQuery.First<MedDAL.DAL.municipios>() : null;
        }

        /// <summary>
        /// Obtiene todos los municipios activos
        /// </summary>
        /// <returns></returns>
        public IQueryable<DAL.municipios> BuscarEnum()
        {
            var oQuery = from q in oMedicuriEntities.municipios
                         where q.Activo == true
                         select q;

            return oQuery;
        }

        public bool NuevoRegistro(DAL.municipios oMunicipio)
        {
            try
            {
                oMedicuriEntities.AddTomunicipios(oMunicipio);                
                oMedicuriEntities.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool EditarRegistro(DAL.municipios oMunicipio)
        {
            try
            {
                var oQuery = from q in oMedicuriEntities.municipios.
                            Where("it.idMunicipio = @idMunicipio",
                            new ObjectParameter("idMunicipio", oMunicipio.idMunicipio))
                             select q;

                DAL.municipios oMunicipioOriginal = oQuery.First<DAL.municipios>();
                oMunicipioOriginal.Nombre = oMunicipio.Nombre;
                oMunicipioOriginal.Activo = oMunicipio.Activo;
                oMunicipioOriginal.idEstado = oMunicipio.idEstado;
                
                oMedicuriEntities.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool EliminarRegistro(DAL.municipios oMunicipio)
        {
            try
            {
                var oQuery = from q in oMedicuriEntities.municipios.
                            Where("it.idMunicipio = @idMunicipio",
                            new ObjectParameter("idMunicipio", oMunicipio.idMunicipio))
                             select q;

                DAL.municipios oMunicipioOriginal = oQuery.First<DAL.municipios>();

                oMedicuriEntities.DeleteObject(oMunicipioOriginal);
                oMedicuriEntities.SaveChanges();
                return true;
            }
            catch
            {
                /*try
                {
                    var oQuery = from q in oMedicuriEntities.municipios.
                            Where("it.idMunicipio = @idMunicipio",
                            new ObjectParameter("idMunicipio", oMunicipio.idMunicipio))
                                 select q;
                    DAL.municipios oMunicipioOriginal = oQuery.First<DAL.municipios>();
                    oMunicipioOriginal.Activo = false;
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

        public IQueryable<DAL.municipios> BuscarEstados(string sCadena, int iIdEstado, int iFiltro)
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

            var oQuery = from q in oMedicuriEntities.municipios.
                            Where(sConsulta,
                            new ObjectParameter("Dato", sCadena))
                         where q.estados.idEstado == iIdEstado
                         select q;

            return oQuery;
        }
    }
}
