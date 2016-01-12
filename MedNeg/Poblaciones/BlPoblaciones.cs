using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Objects;
using System.Data.Objects.DataClasses;

namespace MedNeg.Poblaciones
{
    public class BlPoblaciones
    {
        MedDAL.Poblaciones.DALPoblaciones odalPoblaciones;        

        public BlPoblaciones( ) 
        {
            odalPoblaciones = new MedDAL.Poblaciones.DALPoblaciones();
        }

        public object BuscarPoblaciones() 
        {
            return odalPoblaciones.BuscarPoblaciones();
        }

        public object Buscar(string sCadena, int iTipo)
        {
            return odalPoblaciones.Buscar(sCadena, iTipo);
        }

        public MedDAL.DAL.poblaciones Buscar(string sClave)
        {
            return odalPoblaciones.Buscar(sClave);
        }

        /// <summary>
        /// Obtiene las poblaciones contenidas en este estado
        /// </summary>
        /// <param name="sCadena"></param>
        /// <param name="iIdEstado"></param>
        /// <param name="iTipo"></param>
        /// <returns></returns>
        public object BuscarEstados(string sCadena, int iIdEstado, int iTipo)
        {
            return odalPoblaciones.BuscarEstados(sCadena, iIdEstado, iTipo);
        }

        /// <summary>
        /// Obtiene las poblaciones contenidas en el estado
        /// </summary>
        /// <param name="iIdEstado"></param>
        /// <returns></returns>
        public IQueryable<MedDAL.DAL.poblaciones> BuscarEstados(int iIdEstado) 
        {
            return (IQueryable<MedDAL.DAL.poblaciones>)odalPoblaciones.BuscarEstados("", iIdEstado, 1);
        }

        /// <summary>
        /// Obtiene las poblaciones contenidas en el municipio
        /// </summary>
        /// <param name="sCadena"></param>
        /// <param name="iIdMunicipio"></param>
        /// <param name="iTipo"></param>
        /// <returns></returns>
        public object BuscarMunicipios(string sCadena, int iIdMunicipio, int iTipo)
        {
            return odalPoblaciones.BuscarMunicipios(sCadena, iIdMunicipio, iTipo);
        }

        public IQueryable<MedDAL.Poblaciones.PoblacionesView> Buscar(string sCadena, int iIdMunicipio, int iTipo)
        {
            return odalPoblaciones.Buscar(sCadena, iIdMunicipio, iTipo);
        }

        /// <summary>
        /// Obtiene las poblaciones contenidas en el municipio
        /// </summary>
        /// <param name="iIdMunicipio"></param>
        /// <returns></returns>
        public IQueryable<MedDAL.DAL.poblaciones> BuscarMunicipios(int iIdMunicipio) 
        {
            return (IQueryable<MedDAL.DAL.poblaciones>)odalPoblaciones.BuscarMunicipios("", iIdMunicipio, 1);
        }

        public object Buscar(int id)
        {
            return odalPoblaciones.Buscar(id);
        }

        public object Buscar()
        {
            return odalPoblaciones.Buscar();
        }

        public IQueryable<MedDAL.DAL.poblaciones> BuscarEnum()
        {
            return odalPoblaciones.BuscarEnum();
        }

        public bool NuevoRegistro(MedDAL.DAL.poblaciones oPoblacion)
        {

            return odalPoblaciones.NuevoRegistro(oPoblacion);
        }

        public bool EditarRegistro(MedDAL.DAL.poblaciones oPoblacion)
        {
            return odalPoblaciones.EditarRegistro(oPoblacion);
        }

        public bool EliminarRegistro(MedDAL.DAL.poblaciones oPoblacion)
        {
            return odalPoblaciones.EliminarRegistro(oPoblacion);
        }

    }
}
