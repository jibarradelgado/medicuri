using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace MedDAL.Configuracion
{
    [Serializable()]
    public class DALConfiguracion
    {

        public string sRazonSocial;
        public string sRfc;
        public string sRegimenFiscal;
        public string sDomicilio;
        public string sMunicipio;
        public string sEstado;
        public string sPais;
        public string sCodigoPostal;

        public string sRutaCertificado;
        public string sRutaLlave;
        public string sRutaFirma;
        public string sContraseña; //Encriptada

        public string sServidorBD;
        public string sUsuarioBD;
        public string sContraseñaBD; //Encriptada

        public string sColorInterfaz;
        public string sRutaBaner;

        public int iFolioPedidos;
        public byte iPedidosAutomatico;

        public int iFolioRecetas;
        public byte iRecetasAutomatico;

        public int iFolioRemisiones;
        public byte iRemisionesAutomatico;

        public int iFolioFacturas;
        public byte iFacturasAutomatico;

        public byte iVentasNegativas;
        public int iNoMaxRenglonesFactura;

        public string sServidorSmtp;
        public string sPuertoSmtp;
        //public string sUsuarioSmtp;
        public string sCorreoEmisor;
        public string sContraseñaSmtp;

        public int iCaducidad;
        
        

        //Constructor
        public DALConfiguracion()
        {
        }


       public bool SerializarToXml(object obj, string sRutaArchivo) 
        {
           
                XmlSerializer mySerializer = new XmlSerializer(obj.GetType());
                StreamWriter myWriter = new StreamWriter(sRutaArchivo);
                
                mySerializer.Serialize(myWriter, obj);
                myWriter.Close();
           
                return true;
      }

       public object DeserializarXml(string sRutaArchivo)
       {
           
           XmlSerializer xmlSerz = new XmlSerializer(typeof(DALConfiguracion));
           StreamReader strReader = new StreamReader(sRutaArchivo);
                   
           object objConfiguracion = xmlSerz.Deserialize(strReader);
           strReader.Close();
           return objConfiguracion;
       }




    }
}
