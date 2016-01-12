using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Objects;
using System.Data.Objects.DataClasses;

namespace MedNeg.Estados
{
    public class BlEstados
    {
        MedDAL.Estados.DALEstados odalEstados;

        public BlEstados()
        {
            odalEstados = new MedDAL.Estados.DALEstados();
        }

        public object Buscar(string sCadena, int iTipo)
        {
            return odalEstados.Buscar(sCadena, iTipo);
        }

        public MedDAL.DAL.estados Buscar(string sClave)
        {
            return odalEstados.Buscar(sClave);
        }

        public object Buscar(int id)
        {
            return odalEstados.Buscar(id);
        }

        public object Buscar()
        {
            return odalEstados.Buscar();
        }

        /// <summary>
        /// Busca todos los estados activos
        /// </summary>
        /// <returns></returns>
        public IQueryable<MedDAL.DAL.estados> BuscarEnum() 
        {
            return odalEstados.BuscarEnum();
        }

        public bool NuevoRegistro(MedDAL.DAL.estados oEstado)
        {

            return odalEstados.NuevoRegistro(oEstado);
        }

        public bool EditarRegistro(MedDAL.DAL.estados oEstado)
        {
            return odalEstados.EditarRegistro(oEstado);
        }

        public bool EliminarRegistro(MedDAL.DAL.estados oEstado)
        {
            return odalEstados.EliminarRegistro(oEstado);
        }

    }
}
