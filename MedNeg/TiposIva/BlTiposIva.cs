using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Objects;
using System.Data.Objects.DataClasses;

namespace MedNeg.TiposIva
{
    public class BlTiposIva
    {
        MedDAL.TiposIva.DALTiposIva odalTiposIva;

        public BlTiposIva() 
        {
            odalTiposIva = new MedDAL.TiposIva.DALTiposIva();
        }

        public List<MedDAL.DAL.tipo_iva> Buscar()
        {
            return odalTiposIva.Buscar();
        }

        public object Buscar(string sCadena, int iFiltro)
        {
            return odalTiposIva.Buscar(sCadena, iFiltro);
        }

        public IQueryable<MedDAL.DAL.tipo_iva> BuscarGral() {
            return odalTiposIva.BuscarGral();
        }

        public bool NuevoRegistro(MedDAL.DAL.tipo_iva oTipoIva)
        {
            return odalTiposIva.NuevoRegistro(oTipoIva);
        }

        public bool EditarRegistro(MedDAL.DAL.tipo_iva oTipoIva)
        {
            return odalTiposIva.EditarRegistro(oTipoIva);
        }

        public bool EliminarRegistro(MedDAL.DAL.tipo_iva oTipoIva)
        {
            return odalTiposIva.EliminarRegistro(oTipoIva);
        }
    }
}
