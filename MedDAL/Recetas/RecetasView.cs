using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MedDAL.Recetas
{
    public class RecetasView
    {
        int iIdReceta, iIdEstadoExp,iIdMunicipioExp,iIdPoblacionExp,iIdColoniaExp,iIdEstadoSur,iIdMunicipioSur,iIdPoblacionSur,iIdColoniaSur;
        string sFolio, sPaciente, sTipo, sEstatusMedico, sEstatusFacturacion;
        DateTime tFecha;

        public int idReceta {
            get { return iIdReceta; }
            set { this.iIdReceta = value; }
        }
        public int idEstadoExp
        {
            get { return iIdEstadoExp; }
            set { this.iIdEstadoExp = value; }
        }
        public int idMunicipioExp
        {
            get { return iIdMunicipioExp; }
            set { this.iIdMunicipioExp = value; }
        }
        public int idPoblacionExp
        {
            get { return iIdPoblacionExp; }
            set { this.iIdPoblacionExp = value; }
        }
        public int idColoniaExp
        {
            get { return iIdColoniaExp; }
            set { this.iIdColoniaExp = value; }
        }
        public int idEstadoSur
        {
            get { return iIdEstadoSur; }
            set { this.iIdEstadoSur = value; }
        }
        public int idMunicipioSur
        {
            get { return iIdMunicipioSur; }
            set { this.iIdMunicipioSur = value; }
        }
        public int idPoblacionSur
        {
            get { return iIdPoblacionSur; }
            set { this.iIdPoblacionSur = value; }
        }
        public int idColoniaSur
        {
            get { return iIdColoniaSur; }
            set { this.iIdColoniaSur = value; }
        }
        public string Folio
        {
            get { return sFolio; }
            set { this.sFolio = value; }
        }
        public string Paciente
        {
            get { return sPaciente; }
            set { this.sPaciente = value; }
        }        
        public string Tipo
        {
            get { return sTipo; }
            set { this.sTipo = value; }
        }
        public string EstatusMedico
        {
            get { return sEstatusMedico; }
            set { this.sEstatusMedico = value; }
        }
        public string Estatus
        {
            get { return sEstatusFacturacion; }
            set { this.sEstatusFacturacion = value; }
        }
        public DateTime Fecha
        {
            get { return tFecha; }
            set { this.tFecha = value; }
        }

        public RecetasView() { }
    }
}
