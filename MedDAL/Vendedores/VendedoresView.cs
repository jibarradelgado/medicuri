using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MedDAL.Vendedores
{
    public class VendedoresView
    {
        int iIdVendedor, iIdEstado, iIdMunicipio, iIdPoblacion, iIdColonia;
        string sClave1, sNombre, sApellidos, sRFC, sPersona, sTelefono, sCelular, sCorreoElectronico, sTipoVendedor;
        DateTime tFechaAlta;
        bool bActivo;

        public int idVendedor
        {
            get { return iIdVendedor; }
            set { this.iIdVendedor = value; }
        }
        public int idEstado
        {
            get { return iIdEstado; }
            set { this.iIdEstado = value; }
        }
        public int idMunicipio
        {
            get { return iIdMunicipio; }
            set { this.iIdMunicipio = value; }
        }
        public int idPoblacion
        {
            get { return iIdPoblacion; }
            set { this.iIdPoblacion = value; }
        }
        public int idColonia
        {
            get { return iIdColonia; }
            set { this.iIdColonia = value; }
        }
        public string Clave
        {
            get { return sClave1; }
            set { this.sClave1 = value; }
        }
        public string Nombre
        {
            get { return sNombre; }
            set { this.sNombre = value; }
        }
        public string Apellidos
        {
            get { return sApellidos; }
            set { this.sApellidos = value; }
        }
        public string RFC
        {
            get { return sRFC; }
            set { this.sRFC = value; }
        }
        public string TipoPersona
        {
            get { return sPersona; }
            set { this.sPersona = value; }
        }
        public string Telefono
        {
            get { return sTelefono; }
            set { this.sTelefono = value; }
        }
        public string Celular
        {
            get { return sCelular; }
            set { this.sCelular = value; }
        }
        public string CorreoElectronico
        {
            get { return sCorreoElectronico; }
            set { this.sCorreoElectronico = value; }
        }
        public string TipoVendedor
        {
            get { return sTipoVendedor; }
            set { this.sTipoVendedor = value; }
        }
        public DateTime FechaAlta
        {
            get { return tFechaAlta; }
            set { this.tFechaAlta = value; }
        }
        public bool Activo
        {
            get { return bActivo; }
            set { this.bActivo = value; }
        }

        public VendedoresView() { }
    }
}
