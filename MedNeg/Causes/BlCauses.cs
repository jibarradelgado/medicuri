using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MedNeg.Causes
{
    public class BlCauses
    {
        MedDAL.Causes.DALCauses odalCauses;
        MedDAL.CausesCie.DALCausesCie odalCausesCie;
        MedDAL.CausesMedicamento.DALCausesMedicamento odalCausesMedicamento;

        public BlCauses() {
            odalCauses = new MedDAL.Causes.DALCauses();
            odalCausesCie = new MedDAL.CausesCie.DALCausesCie();
            odalCausesMedicamento = new MedDAL.CausesMedicamento.DALCausesMedicamento();
        }

        public IQueryable<MedDAL.DAL.causes> Buscar(string sCadena, int iFiltro)
        {
            return odalCauses.Buscar(sCadena, iFiltro);
        }

        public MedDAL.DAL.causes Buscar(string sClave)
        {
            return odalCauses.Buscar(sClave);
        }

        public MedDAL.DAL.causes_cie BuscarCie(string sClave)
        {
            return odalCausesCie.Buscar(sClave);
        }

        public List<MedDAL.DAL.causes_cie> BuscarCie(string sCadena, int iFiltro)
        {
            return odalCausesCie.Buscar(sCadena, iFiltro);
        }

        public List<MedDAL.DAL.causes_medicamentos> BuscarMedicamento(string sCadena, int iFiltro)
        {
            return odalCausesMedicamento.Buscar(sCadena, iFiltro);
        }

        public bool NuevoRegistro(MedDAL.DAL.causes oCauses)
        {
            return odalCauses.NuevoRegistro(oCauses);
        }

        public bool NuevoRegistro(MedDAL.DAL.causes_cie oCausesCie) 
        {
            return odalCausesCie.NuevoRegistro(oCausesCie);
        }

        public bool NuevoRegistro(MedDAL.DAL.causes_medicamentos oCausesMedicamentos) 
        {
            return odalCausesMedicamento.NuevoRegistro(oCausesMedicamentos);
        }

        public bool EditarRegistro(MedDAL.DAL.causes oCauses)
        {
            return odalCauses.EditarRegistro(oCauses);
        }

        public bool EliminarRegistro(int idCause)
        {
            return odalCauses.EliminarRegistro(idCause);
        }

        public bool EliminarRegistroCie(MedDAL.DAL.causes oCauses)
        {
            return odalCausesCie.EliminarRegistro(oCauses);
        }

        public bool EliminarRegistroMedicamento(MedDAL.DAL.causes oCauses)
        {
            return odalCausesMedicamento.EliminarRegistro(oCauses);
        }

        public MedDAL.DAL.causes Buscar(int idCause)
        {
            return odalCauses.Buscar(idCause);
        }
    }
}
