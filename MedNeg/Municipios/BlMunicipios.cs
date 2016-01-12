using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Objects;
using System.Data.Objects.DataClasses;

namespace MedNeg.Municipios
{
    public class BlMunicipios
    {
        MedDAL.Municipios.DALMunicipios odalMunicipios;        

        public BlMunicipios( ) 
        {
            odalMunicipios = new MedDAL.Municipios.DALMunicipios(); 
        }

        /// <summary>
        /// Busca los municipios que coincidan con la cadena de búsqueda
        /// </summary>
        /// <param name="sCadena">El término de búsqueda</param>
        /// <param name="iTipo">Filtro: 1=Todos, 2=Clave, 3=Nombre</param>
        /// <returns></returns>
        public object Buscar(string sCadena, int iTipo)
        {
            return odalMunicipios.Buscar(sCadena, iTipo);    
        }

        /// <summary>
        /// Busca los municipios que coincidan con la cadena de búsqueda y 
        /// con el estado al que pertenecen
        /// </summary>
        /// <param name="sCadena"></param>
        /// <param name="iIdEstado"></param>
        /// <param name="iTipo"></param>
        /// <returns></returns>
        public IQueryable<MedDAL.Municipios.MunicipiosView> Buscar(string sCadena, int iIdEstado, int iTipo) 
        {
            return odalMunicipios.Buscar(sCadena, iIdEstado, iTipo);
        }

        public MedDAL.DAL.municipios Buscar(string sClave)
        {
            return odalMunicipios.Buscar(sClave);
        }

        public object Buscar(int id)
        {
            return odalMunicipios.Buscar(id);
        }

        /// <summary>
        /// Busca los municipios pertenecientes a un estado
        /// </summary>
        /// <param name="iIdEstado"></param>
        /// <returns>Una lista de IQueryables</returns>
        public IQueryable<MedDAL.DAL.municipios> BuscarEstados(int iIdEstado) 
        {
            return (IQueryable<MedDAL.DAL.municipios>)odalMunicipios.BuscarEstados("", iIdEstado, 1);
        }

        public object Buscar()
        {
            return odalMunicipios.Buscar();
        }

        public IQueryable<MedDAL.DAL.municipios> BuscarEnum()
        {
            return odalMunicipios.BuscarEnum();
        }

        public bool NuevoRegistro(MedDAL.DAL.municipios oMunicipio) 
        {
            
            return odalMunicipios.NuevoRegistro(oMunicipio);
        }

        public bool EditarRegistro(MedDAL.DAL.municipios oMunicipio)
        {
            return odalMunicipios.EditarRegistro(oMunicipio);
        }

        public bool EliminarRegistro(MedDAL.DAL.municipios oMunicipio) 
        {
            return odalMunicipios.EliminarRegistro(oMunicipio);
        }
    }
}
