using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Objects;
using System.Data.Objects.DataClasses;

namespace MedDAL.CamposEditables
{
    public class DALCamposEditables
    {
        DAL.medicuriEntities oMedicuriEntities;

        public DALCamposEditables() 
        {
            oMedicuriEntities = new DAL.medicuriEntities();
        }

        public List<MedDAL.DAL.campos_editables> Buscar()
        {
            List<MedDAL.DAL.campos_editables> lstCamposEditables = new List<DAL.campos_editables>();

            var oQuery = from q in oMedicuriEntities.campos_editables
                         select q;

            lstCamposEditables.AddRange(oQuery);

            return lstCamposEditables;
        }

        public List<MedDAL.DAL.campos_editables> Buscar(string sModulo)
        {
            List<MedDAL.DAL.campos_editables> lstCamposEditables = new List<DAL.campos_editables>();

            var oQuery = from q in oMedicuriEntities.campos_editables
                         where q.Modulo == sModulo
                         select q;

            lstCamposEditables.AddRange(oQuery);

            return lstCamposEditables;
        }

        public bool EditarRegistro(DAL.campos_editables oCampoEditable)
        {
            try
            {
                var oQuery = from q in oMedicuriEntities.campos_editables.
                             Where ("it.idCampoEditable = @idCampoEditable",
                             new ObjectParameter("idCampoEditable", oCampoEditable.idCampoEditable))
                             select q;

                DAL.campos_editables oCampoEditableOriginal = oQuery.First<DAL.campos_editables>();
                oCampoEditableOriginal.Valor = oCampoEditable.Valor;

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
