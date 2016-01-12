using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Objects;
using System.Data.Objects.DataClasses;

namespace MedNeg.Colonias
{
    public class BlColonias
    {
        MedDAL.Colonias.DALColonias odalColonias;        

        public BlColonias( ) 
        {
            odalColonias = new MedDAL.Colonias.DALColonias();
        }

        public object Buscar(string sCadena, int iTipo)
        {
            return odalColonias.Buscar(sCadena, iTipo);
        }

        public MedDAL.DAL.colonias Buscar(string sClave)
        {
            return odalColonias.Buscar(sClave);
        }

        /// <summary>
        /// Obtiene las colonias contenidas en este estado
        /// </summary>
        /// <param name="sCadena"></param>
        /// <param name="iIdEstado"></param>
        /// <param name="iTipo"></param>
        /// <returns></returns>
        public object BuscarEstados(string sCadena, int iIdEstado, int iTipo)
        {
            return odalColonias.BuscarEstados(sCadena, iIdEstado, iTipo);
        }

        /// <summary>
        /// Obtiene las colonias contenidas en el estado
        /// </summary>
        /// <param name="iIdEstado"></param>
        /// <returns></returns>
        public IQueryable<MedDAL.DAL.colonias> BuscarEstados(int iIdEstado)
        {
            return (IQueryable < MedDAL.DAL.colonias >) odalColonias.BuscarEstados("", iIdEstado, 1);
        }

        /// <summary>
        /// Obtiene las colonias contenidas en el municipio
        /// </summary>
        /// <param name="sCadena"></param>
        /// <param name="iIdMunicipio"></param>
        /// <param name="iTipo"></param>
        /// <returns></returns>
        public object BuscarMunicipios(string sCadena, int iIdMunicipio, int iTipo)
        {
            return odalColonias.BuscarMunicipios(sCadena, iIdMunicipio, iTipo);
        }

        /// <summary>
        /// Obtiene las colonias contenidas en el municipio
        /// </summary>
        /// <param name="iIdMunicipio"></param>
        /// <returns></returns>
        public IQueryable<MedDAL.DAL.colonias> BuscarMunicipios(int iIdMunicipio)
        {
            return (IQueryable<MedDAL.DAL.colonias>)odalColonias.BuscarMunicipios("", iIdMunicipio, 1);
        }

        /// <summary>
        /// Obtiene las colonias contenidas en la población
        /// </summary>
        /// <param name="sCadena"></param>
        /// <param name="iIdPoblacion"></param>
        /// <param name="iTipo"></param>
        /// <returns></returns>
        public object BuscarPoblaciones(string sCadena, int iIdPoblacion, int iTipo)
        {
            return odalColonias.BuscarPoblaciones(sCadena, iIdPoblacion, iTipo);
        }

        public IQueryable<MedDAL.Colonias.ColoniasView> Buscar(string sCadena, int idPoblacion, int iFiltro)
        {
            return odalColonias.Buscar(sCadena, idPoblacion, iFiltro);
        }

        /// <summary>
        /// Obtiene las colonias contenidas en la población
        /// </summary>
        /// <param name="iIdPoblacion"></param>
        /// <returns></returns>
        public IQueryable<MedDAL.DAL.colonias> BuscarPoblaciones(int iIdPoblacion)
        {
            return (IQueryable<MedDAL.DAL.colonias>)odalColonias.BuscarPoblaciones("", iIdPoblacion, 1);
        }

        public object Buscar(int id)
        {
            return odalColonias.Buscar(id);
        }

        public object Buscar()
        {
            return odalColonias.Buscar();
        }

        /// <summary>
        /// Obtiene las colonias activas
        /// </summary>
        /// <returns></returns>
        public IQueryable<MedDAL.DAL.colonias> BuscarEnum()
        {
            return odalColonias.BuscarEnum();
        }

        public bool NuevoRegistro(MedDAL.DAL.colonias oColonia)
        {

            return odalColonias.NuevoRegistro(oColonia);
        }

        public bool EditarRegistro(MedDAL.DAL.colonias oColonia)
        {
            return odalColonias.EditarRegistro(oColonia);
        }

        public bool EliminarRegistro(MedDAL.DAL.colonias oColonia)
        {
            return odalColonias.EliminarRegistro(oColonia);
        }
    }
}
