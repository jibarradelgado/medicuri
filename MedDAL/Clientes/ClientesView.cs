using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MedDAL.Clientes
{
    public class ClientesView
    {
        int iIdCliente, iIdEstados, iIdMunicipios, iIdPoblaciones, iIdColonias;
        string sClave1, sNombre, sApellidos, sRFC, sPersona, sTelefono, sCelular, sCorreoElectronico, sTipoCliente;
        DateTime tFechaAlta;
        bool bActivo;

        public int idCliente {
            get { return iIdCliente; }
            set { this.iIdCliente = value; }
        }
        public int idEstado
        {
            get { return iIdEstados; }
            set { this.iIdEstados = value; }
        }
        public int idMunicipio
        {
            get { return iIdMunicipios; }
            set { this.iIdMunicipios = value; }
        }
        public int idPoblacion
        {
            get { return iIdPoblaciones; }
            set { this.iIdPoblaciones = value; }
        }
        public int idColonia
        {
            get { return iIdColonias; }
            set { this.iIdColonias = value; }
        }        
        public string Clave1 
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
        public string TipoCliente
        {
            get { return sTipoCliente; }
            set { this.sTipoCliente = value; }
        }
        public DateTime FechaAlta
        {
            get { return tFechaAlta; }
            set { this.tFechaAlta = value; }
        }
        public bool Activo {
            get { return bActivo; }
            set { this.bActivo = value; }
        }

        public ClientesView() { }
    }
}
