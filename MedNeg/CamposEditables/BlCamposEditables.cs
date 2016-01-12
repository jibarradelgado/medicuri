using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Objects;
using System.Data.Objects.DataClasses;

namespace MedNeg.CamposEditables
{
    public class BlCamposEditables
    {
        MedDAL.CamposEditables.DALCamposEditables odalCamposEditables;

        public BlCamposEditables()
        {
            odalCamposEditables = new MedDAL.CamposEditables.DALCamposEditables();
        }

        public List<MedDAL.DAL.campos_editables> Buscar() 
        {
            return odalCamposEditables.Buscar();
        }

        public List<MedDAL.DAL.campos_editables> Buscar(string sModulo)
        {
            return odalCamposEditables.Buscar(sModulo);
        }

        public bool EditarRegistro(MedDAL.DAL.campos_editables oCampoEditable)
        {
            return odalCamposEditables.EditarRegistro(oCampoEditable);
        }
    }
}
