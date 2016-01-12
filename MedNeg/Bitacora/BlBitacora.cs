using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Xml;
using System.Text;
using System.IO;
using System.Data;
using MedNeg.Serializacion;
using ICSharpCode.SharpZipLib.Zip;
using System.Globalization;

namespace MedNeg.Bitacora
{
    public class BlBitacora
    {
        MedDAL.Bitacora.DALBitacora odalBitacora;

        public BlBitacora() 
        {
            odalBitacora = new MedDAL.Bitacora.DALBitacora();
        }

        public List<MedDAL.DAL.bitacora> Buscar(XDocument xmlDoc, string sFechaInicio, string sFechaFin, int iFiltro, string sCadena) 
        {            
            DateTime tFechaInicio = DateTime.Parse(sFechaInicio);
            DateTime tFechaFin = DateTime.Parse(sFechaFin);

            switch (iFiltro)
            {
                case 1:
                    var oQuery = from c in xmlDoc.Descendants("bitacora")
                                 where (c.Element("Usuario").Value.ToString().ToUpper().Contains(sCadena.ToUpper())) ||
                                       (c.Element("Modulo").Value.ToString().ToUpper().Contains(sCadena.ToUpper())) &&   
                                       (DateTime.Parse(c.Element("FechaEntradaSrv").Value.ToString(), CultureInfo.CurrentCulture) >= tFechaInicio &&
                                       DateTime.Parse(c.Element("FechaEntradaSrv").Value.ToString(), CultureInfo.CurrentCulture) <= tFechaFin)
                                 select c;

                    List<MedDAL.DAL.bitacora> lstBitacora = ObtenerBitacoraQuery(oQuery);

                    return lstBitacora;
                case 2:
                    oQuery = from c in xmlDoc.Descendants("bitacora")
                             where (c.Element("Usuario").Value.ToString().ToUpper().Contains(sCadena.ToUpper())) &&
                                       (DateTime.Parse(c.Element("FechaEntradaSrv").Value.ToString()) >= tFechaInicio &&
                                       DateTime.Parse(c.Element("FechaEntradaSrv").Value.ToString()) <= tFechaFin)
                             select c;

                    lstBitacora = ObtenerBitacoraQuery(oQuery);

                    return lstBitacora;
                case 3:                   
                    oQuery = from c in xmlDoc.Descendants("bitacora")
                             where (c.Element("Modulo").Value.ToString().ToUpper().Contains(sCadena.ToUpper())) &&
                                       (DateTime.Parse(c.Element("FechaEntradaSrv").Value.ToString()) >= tFechaInicio &&
                                       DateTime.Parse(c.Element("FechaEntradaSrv").Value.ToString()) <= tFechaFin)
                             select c;

                    lstBitacora = ObtenerBitacoraQuery(oQuery);

                    return lstBitacora;
                default:
                    return null;
            }
        }

        public List<MedDAL.DAL.bitacora> Buscar(XDocument xmlDoc, int iFiltro, string sCadena) 
        {            
            switch (iFiltro) 
            { 
                case 1:
                    var oQuery = from c in xmlDoc.Descendants("bitacora")
                             where (c.Element("Usuario").Value.ToString().ToUpper().Contains(sCadena.ToUpper()))
                             where (c.Element("Modulo").Value.ToString().ToUpper().Contains(sCadena.ToUpper()))
                             select c;

                    List<MedDAL.DAL.bitacora> lstBitacora = ObtenerBitacoraQuery(oQuery);

                    return lstBitacora;
                case 2:                   
                    oQuery = from c in xmlDoc.Descendants("bitacora")
                             where (c.Element("Usuario").Value.ToString().ToUpper().Contains(sCadena.ToUpper()))                             
                             select c;

                    lstBitacora = ObtenerBitacoraQuery(oQuery);

                    return lstBitacora;                    
                case 3:                    
                    oQuery = from c in xmlDoc.Descendants("bitacora")
                             where (c.Element("Modulo").Value.ToString().ToUpper().Contains(sCadena.ToUpper()))
                             select c;

                    lstBitacora = ObtenerBitacoraQuery(oQuery);
                                            
                    return lstBitacora;
                default:
                    return null;
            }
             
        }

        public IQueryable<MedDAL.DAL.bitacora> Buscar(string sCadena, int iFiltro)
        {
            return odalBitacora.Buscar(sCadena, iFiltro);
        }

        public IQueryable<MedDAL.DAL.bitacora> Buscar(string sCadena, int iFiltro, string sFecha1, string sFecha2)
        {
            return odalBitacora.Buscar(sCadena, iFiltro, sFecha1, sFecha2);
        }

        public string ObtenerXML(string sCadenaGuardar) 
        {
            List<MedDAL.DAL.bitacora> lstBitacora = new List<MedDAL.DAL.bitacora>();//odalBitacora.Buscar();
            lstBitacora.AddRange(odalBitacora.Buscar());

            string sXML = lstBitacora.ToXml();

            StreamWriter swrXml = new StreamWriter(sCadenaGuardar);
            swrXml.Write(sXML);
            swrXml.Flush();
            swrXml.Close();

            return sCadenaGuardar;
        }

        public List<MedDAL.DAL.bitacora> ObtenerBitacoraQuery(IEnumerable<XElement> oQuery)
        { 
            string sContenido;            

            List<MedDAL.DAL.bitacora> lstBitacora = new List<MedDAL.DAL.bitacora>();

            foreach(object c in oQuery)
            {
                sContenido = c.ToString();
                sContenido = sContenido.Replace("<bitacora>", "<bitacora xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"> \r\n");
                //sResto = sContenido.Substring(sContenido.IndexOf("<bitacora>" + 10), sContenido.Length - 1);

                sContenido = "<?xml version=\"1.0\" encoding=\"utf-16\"?> \r\n" + sContenido;                
                MedDAL.DAL.bitacora oBitacora = sContenido.ConvertTo<MedDAL.DAL.bitacora>();
                
                lstBitacora.Add(oBitacora);
            }

            return lstBitacora;
        }

        public List<MedDAL.DAL.bitacora> ObtenerBitacora(string sArchivo) 
        {            
            List<MedDAL.DAL.bitacora> lstBitacora = sArchivo.ConvertTo<List<MedDAL.DAL.bitacora>>();
            return lstBitacora;
        } 

        public string ObtenerSQL(string sCadenaGuardar)
        {
            List<MedDAL.DAL.bitacora> lstBitacora = new List<MedDAL.DAL.bitacora>();//odalBitacora.Buscar();
            lstBitacora.AddRange(odalBitacora.Buscar());

            StreamWriter swrSql = new StreamWriter(sCadenaGuardar, false, System.Text.Encoding.Unicode);
            swrSql.WriteLine("IdEntradaBitacora,FechaEntradaSrv,FechaEntradaCte,Modulo,Usuario,Nombre,Accion,Descripcion");

            foreach (MedDAL.DAL.bitacora oBitacora in lstBitacora) 
            {
                swrSql.Write(oBitacora.IdEntradaBitacora.ToString() + ",");
                swrSql.Write(oBitacora.FechaEntradaSrv.ToString() + ",");
                swrSql.Write(oBitacora.FechaEntradaCte.ToString() + ",");
                swrSql.Write(oBitacora.Modulo.ToString() + ",");
                swrSql.Write(oBitacora.Usuario.ToString() + ",");
                swrSql.Write(oBitacora.Nombre.ToString() + ",");
                swrSql.Write(oBitacora.Accion.ToString() + ",");
                swrSql.WriteLine(oBitacora.Descripcion.ToString());
            }

            swrSql.Flush();
            swrSql.Close();

            return sCadenaGuardar;
        }

        public void CrearZip(string[] asArchivos) 
        {
            if (!Directory.Exists(asArchivos[0]))
            {
                Console.WriteLine("Cannot find directory '{0}'", asArchivos[0]);
                return;
            }

            try
            {
                // Depending on the directory this could be very large and would require more attention
                // in a commercial package.
                string[] filenames = Directory.GetFiles(asArchivos[0]);

                

                // 'using' statements guarantee the stream is closed properly which is a big source
                // of problems otherwise.  Its exception safe as well which is great.
                using (ZipOutputStream s = new ZipOutputStream(File.Create(asArchivos[1])))
                {

                    s.SetLevel(0); // 0 - store only to 9 - means best compression

                    byte[] buffer = new byte[4096];

                    foreach (string file in filenames)
                    {
                        // Using GetFileName makes the result compatible with XP
                        // as the resulting path is not absolute.
                        ZipEntry entry = new ZipEntry(Path.GetFileName(file));

                        // Setup the entry data as required.

                        // Crc and size are handled by the library for seakable streams
                        // so no need to do them here.

                        // Could also use the last write time or similar for the file.
                        entry.DateTime = DateTime.Now;
                        s.PutNextEntry(entry);

                        using (FileStream fs = File.OpenRead(file))
                        {

                            // Using a fixed size buffer here makes no noticeable difference for output
                            // but keeps a lid on memory usage.
                            int sourceBytes;
                            do
                            {
                                sourceBytes = fs.Read(buffer, 0, buffer.Length);
                                s.Write(buffer, 0, sourceBytes);
                            } while (sourceBytes > 0);
                        }
                    }

                    // Finish/Close arent needed strictly as the using statement does this automatically

                    // Finish is important to ensure trailing information for a Zip file is appended.  Without this
                    // the created file would be invalid.
                    s.Finish();

                    // Close is important to wrap things up and unlock the file.
                    s.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception during processing {0}", ex);

                // No need to rethrow the exception as for our purposes its handled.
            }
        }

        public bool NuevoRegistro(MedDAL.DAL.bitacora oBitacora) 
        {
            return odalBitacora.NuevoRegistro(oBitacora);
        }

        public bool EliminarTodo() 
        {
            return odalBitacora.EliminarTodo();
        }

        public object MostrarLista()
        {
            return odalBitacora.MostrarLista();
        }
    }
}
