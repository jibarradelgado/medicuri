using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using ICSharpCode.SharpZipLib.Zip;

namespace MedNeg.Facturas
{
    public class BlFacturas
    {

        MedDAL.Facturas.DALFacturas odalFacturas;

        //Constructor
        public BlFacturas()
        {
            odalFacturas = new MedDAL.Facturas.DALFacturas();
        }

        /// <summary>
        /// BL - Nuevo registro
        /// </summary>
        /// <param name="oFactura"></param>
        /// <returns></returns>
        public bool NuevoRegistro(MedDAL.DAL.facturas oFactura)
        {
            return odalFacturas.NuevoRegistro(oFactura);
        }

        public bool NuevoRegistroFacturacionReceta(MedDAL.DAL.FacturacionDeRecetas oFacturaRecetas)
        {
            return odalFacturas.NuevoRegistroFacturacionReceta(oFacturaRecetas);
        }

        /// <summary>
        /// BL - Insertar detalla partida de una factura
        /// </summary>
        /// <param name="oRemisionFactura">Detalle de partida</param>
        /// <returns></returns>
        public bool NuevoDetallePartida(MedDAL.DAL.facturas_partida oRemisionFactura)
        {
            return odalFacturas.NuevoDetalleFactura(oRemisionFactura);

        }

        /// <summary>
        /// BL - Buscar factura mediante su folio
        /// </summary>
        /// <param name="sFolio">Folio a buscar</param>
        /// <returns></returns>
        public MedDAL.DAL.facturas BuscarFacturasFolio(string sFolio)
        {
            return odalFacturas.BuscarFacturaFolio(sFolio);
        }

        /// <summary>
        /// Recuperar remision mediante su id
        /// </summary>
        /// <param name="iIdFactura"></param>
        /// <returns></returns>
        public MedDAL.DAL.facturas BuscarFactura(int iIdFactura)
        {
            return odalFacturas.BuscarFactura(iIdFactura);

        }

        /// <summary>
        /// Buscar factura
        /// </summary>
        /// <param name="sCadena">Parametro a buscar</param>
        /// <param name="iTipo">Filtro</param>
        /// <returns></returns>
        //public object Buscar(string sCadena, int iTipo)
        //{
        //    //return odalEstados.Buscar(sCadena, iTipo);
        //    return odalFacturas.Buscar(sCadena, iTipo);
        //}

        public IQueryable<MedDAL.Facturas.CuentasxCobrarView> Buscar(string sCadena, int iTipo)
        {
            return odalFacturas.Buscar(sCadena, iTipo);
        }   

        /// <summary>
        /// BL - Validar Folio pedido
        /// </summary>
        /// <param name="sFolio"></param>
        /// <returns></returns>
        public bool ValidarFolioRepetido(string sFolio)
        {
            return odalFacturas.ValidarFolioRepetido(sFolio);
        }

        /// <summary>
        /// Saber si esta activada la opción de folio automatico en remisiones en configuración
        /// </summary>
        public int RecuperaFolioAutomatico(string sRutaArchivoConfig)
        {

            if (File.Exists(sRutaArchivoConfig))
            {
                MedNeg.Configuracion.BlConfiguracion oblConfiguracion = new MedNeg.Configuracion.BlConfiguracion();
                MedDAL.Configuracion.DALConfiguracion odalConfiguracion = new MedDAL.Configuracion.DALConfiguracion();

                odalConfiguracion = (MedDAL.Configuracion.DALConfiguracion)oblConfiguracion.CargaDatos(sRutaArchivoConfig);

                if (odalConfiguracion.iFacturasAutomatico==1)
                {
                    return odalConfiguracion.iFolioFacturas + 1;
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return 0;
            }

        }

        /// <summary>
        /// Actualizar el folio de facturas
        /// </summary>
        /// <param name="sRutaArchivoConfig"></param>
        public void ActualizarFolioFactura(string sRutaArchivoConfig, bool bIncremento = true)
        {
            MedDAL.Configuracion.DALConfiguracion odalConfiguracion = new MedDAL.Configuracion.DALConfiguracion();
            MedNeg.Configuracion.BlConfiguracion oblConfiguracion = new MedNeg.Configuracion.BlConfiguracion();                
            odalConfiguracion = (MedDAL.Configuracion.DALConfiguracion)oblConfiguracion.CargaDatos(sRutaArchivoConfig);

            if (odalConfiguracion.iFacturasAutomatico == 1)
            {
                //Incrementar el folio
                if (bIncremento)
                {
                    odalConfiguracion.iFolioFacturas++;                    
                }
                else
                {
                    odalConfiguracion.iFolioFacturas--;
                }

                oblConfiguracion.GuardarDatos(odalConfiguracion, sRutaArchivoConfig);
            }
        }

        /// <summary>
        /// Mostrar las lista de las facturas
        /// </summary>
        /// <returns></returns>
        public IQueryable<MedDAL.Facturas.CuentasxCobrarView> MostrarLista()
        {
            return odalFacturas.MostrarLista();
        }

        /// <summary>
        /// Mostrar las lista de las facturas por cobrar
        /// </summary>
        /// <returns></returns>
        public IQueryable<MedDAL.Facturas.CuentasxCobrarView> MostrarListaCuentasCobrar()
        {
            return odalFacturas.MostrarListaCuentasCobrar();
        }

        /// <summary>
        /// Recuperar la partida de una factura
        /// </summary>
        /// <param name="iIdFacturas">Id Factura a buscar</param>
        /// <returns></returns>
        public IQueryable<MedDAL.DAL.facturas_partida> RecuperarPartidaFactura(int iIdFactura)
        {
            return odalFacturas.RecuperarPartidaFactura(iIdFactura);
        }

        /// <summary>
        /// Editar Factura
        /// </summary>
        /// <param name="oFactura"></param>
        /// <returns></returns>
        public bool EditarRegistro(MedDAL.DAL.facturas oFactura)
        {
            return odalFacturas.EditarRegistro(oFactura);
        }

        /// <summary>
        /// Eliminar partida detalle
        /// </summary>
        /// <param name="iIdFactura"></param>
        /// <returns></returns>
        public bool EliminarFacturaPartida(int iIdFactura)
        {
            return odalFacturas.EliminarFacturaPartida(iIdFactura);
        }

        /// <summary>
        /// Eliminar una remision
        /// </summary>
        /// <param name="iIdFactura">Id factura a eliminar</param>
        /// <returns></returns>
        public bool EliminarRegistro(int iIdFactura)
        {
            return odalFacturas.EliminarRegistro(iIdFactura);
        }

        /// <summary>
        /// Metodo para obtener el total facturado por linea de credito (programa)
        /// </summary>
        /// <returns></returns>
        public IQueryable<MedDAL.Facturas.FacturasxRecetaView> TotalFacturadoPorLineaCredito()
        {
            return odalFacturas.TotalFacturadoPorLineaCredito();
        }

        public bool ModificarExistenciaProducto(int idAlmacen, int idProducto, decimal dCantidad, int iModo)
        {
            MedDAL.Productos.DALProductos oProductos = new MedDAL.Productos.DALProductos();

            return oProductos.ModificarExistenciaProducto(idAlmacen, idProducto, dCantidad, iModo);
        }


        /// <summary>
        /// Generacion de factura electronica
        /// </summary>
        /// <param name="iIdFactura"></param>
        /// <param name="sRutaArchivos"></param>
        /// <param name="sUsuario"></param>
        /// <param name="iIdCliente"></param>
        /// <param name="?"></param>
        /// <param name="sFolioFactura"></param>
        public int GenerarFacturaElectronica(int iIdFactura, string sRutaArchivos, string sUsuario,int iIdCliente,string sFolioFactura)
        {
            int iResultado = 0;
            //Datos de la factura
            MedNeg.Facturas.BlFacturas oblFactura = new BlFacturas();
            MedDAL.DAL.facturas oFactura = new MedDAL.DAL.facturas();

            //Datos de la factura
            oFactura = oblFactura.BuscarFactura(iIdFactura);            
            
            //Recuperar la partida de la factura
            List<MedDAL.DAL.facturas_partida> oQuery = new List<MedDAL.DAL.facturas_partida>();
            oQuery.AddRange(oblFactura.RecuperarPartidaFactura(oFactura.idFactura));
            decimal dSubtotal = 0;
            decimal dImpuestosTrasladados = 0;
            decimal dTotal = 0;

            //Recorrer el resultado de la partida para obtener el total
            foreach (MedDAL.DAL.facturas_partida oDetalle in oQuery)
            {
                dSubtotal += oDetalle.Cantidad * oDetalle.Precio;
                dImpuestosTrasladados += (decimal)oDetalle.Iva + (decimal)oDetalle.IEPS;
            }
            //Total
            dTotal = dSubtotal + dImpuestosTrasladados;

            DateTime dtFechaTest = DateTime.Now;
            string sNumeroCertificado = "";
            string sA, sB, sC;

            //Leer archivo de configuracion
            MedNeg.Configuracion.BlConfiguracion oblConfiguracion = new MedNeg.Configuracion.BlConfiguracion();
            MedDAL.Configuracion.DALConfiguracion odalConfiguracion = new MedDAL.Configuracion.DALConfiguracion();

            odalConfiguracion = (MedDAL.Configuracion.DALConfiguracion)oblConfiguracion.CargaDatos(sRutaArchivos+"/Configuracion.xml");
            
            //Comprobante.SelloDigital.leerCER(sRutaArchivos + "/Certificados/aaa010101aaa_csd_06.cer", out sA, out sB, out sC, out sNumeroCertificado);
            Comprobante.SelloDigital.leerCER(sRutaArchivos + "/Facturacion/"+odalConfiguracion.sRutaCertificado, out sA, out sB, out sC, out sNumeroCertificado);

            //Establecer el numero de cuenta
            string sNumeroCuenta = null;
            if (oFactura.metodo_pago.MetodoPago.Equals("depósito en cuenta", StringComparison.InvariantCultureIgnoreCase) ||
                oFactura.metodo_pago.MetodoPago.Equals("traspaso", StringComparison.InvariantCultureIgnoreCase))
            {
                sNumeroCuenta = oFactura.NumeroCuentaPago;
            }
            

            //Debo validar la vigencia del certificado con sa <= FechaEmision <= sB

            #region generarcomprobante

            //Construir objeto factura
            Comprobante.ComprobanteFiscalDigital oComprobanteFiscalDigital = new Comprobante.ComprobanteFiscalDigital(
                   "", //Serie 
                   sFolioFactura, //Folio 
                   dtFechaTest.ToString("yyyy-MM-ddThh:mm:ss"), //Fecha
                   oFactura.tipo_forma_pago.FormaPago, //FormaDePago
                   sNumeroCertificado, //NoCertificado
                   "",//"Condiciones de pago",
                   dSubtotal.ToString(), //SubTotal
                   "0",//Descuento
                   "",//Motivo del descuento
                   dTotal.ToString(), //Total
                   oFactura.metodo_pago.MetodoPago, //Metodo de pago
                   "Ingreso", //Tipo de comprobante
                   null, //noAprobacion
                   null, //anoAprobacion
                   "3.2", //version
                   null,//TipoCambio
                   null, //Moneda
                   odalConfiguracion.sEstado + "," + odalConfiguracion.sMunicipio, //LugarExpedicion
                   sNumeroCuenta //NumCtaPago
                   );

            //Pendiente la parte de pagos parciales

            //Empresa o razon social y direccion fiscal
            Comprobante.t_Emisor Emisor = new Comprobante.t_Emisor(
                odalConfiguracion.sRfc,
                    odalConfiguracion.sRazonSocial,
                    odalConfiguracion.sDomicilio, //Calle
                    "", //No exterior
                    "", //No interior
                    "", //Colonia
                    "", //Localidad
                    "",//Emisor Referencia
                    odalConfiguracion.sMunicipio,
                    odalConfiguracion.sEstado,
                    odalConfiguracion.sPais,
                    odalConfiguracion.sCodigoPostal
                );

            Emisor.addRegimenFiscal(odalConfiguracion.sRegimenFiscal);

            oComprobanteFiscalDigital.setEmisor(Emisor);

            //Establece la expedición de la factura, se toman lo datos del almacen
            MedDAL.DAL.usuarios oUsuario = new MedDAL.DAL.usuarios();
            MedNeg.Usuarios.BlUsuarios oblUsuario = new Usuarios.BlUsuarios();
            oUsuario = (MedDAL.DAL.usuarios)oblUsuario.Buscar(sUsuario);

            oComprobanteFiscalDigital.setExpedicion(
                    oUsuario.almacenes.Calle.ToString(),//Calle
                    oUsuario.almacenes.NumeroExt.ToString(),//NoExt
                    "", //NoInt
                    oUsuario.almacenes.colonias.Nombre, //Colonia
                    oUsuario.almacenes.poblaciones.Nombre.ToString(), //Población
                    "",//Emisor Referencia Expedicion
                    oUsuario.almacenes.municipios.Nombre.ToString(),//Municipio
                    oUsuario.almacenes.estados.Nombre.ToString(),//Estado
                    oUsuario.almacenes.Pais.ToString(),//Pais
                    oUsuario.almacenes.CodigoPostal.ToString());//CP

            //Establecer los datos del cliente
            MedDAL.DAL.clientes oCliente = new MedDAL.DAL.clientes();
            MedNeg.BlClientes.BlClientes oblClientes = new BlClientes.BlClientes();
            oCliente = oblClientes.BuscarCliente(iIdCliente);

            oComprobanteFiscalDigital.setReceptor(
                oCliente.Rfc.ToString(), //RFC
                oCliente.Nombre.ToString() + " " + oCliente.Apellidos.ToString(), //Nombre
                oCliente.Calle.ToString(), //Calle
                oCliente.NumeroExt.ToString(), //No ext
                oCliente.NumeroInt.ToString(),//No int
                oCliente.colonias.Nombre.ToString(),//Colo
                oCliente.poblaciones.Nombre.ToString(),//Ciudad
                "",//ReferenciaReceptor
                oCliente.municipios.Nombre.ToString(),//Municipio
                oCliente.estados.Nombre.ToString(),//Estado
                "Mexico",//Pais
                oCliente.CodigoPostal.ToString() //Codigo postal
                );


            //Recorrer el resultado de la partida para obtener el total
            foreach (MedDAL.DAL.facturas_partida oDetalle in oQuery)
            {
                string Cantidad, Codigo, Descripcion, Unidad, PrecioUnitario, Importe, ImporteNeto;
                
                decimal dImporte=0;
                Cantidad = oDetalle.Cantidad.ToString();
                //Codigo = oDetalle.productos.Clave1.ToString();
                //Descripcion = oDetalle.productos.Nombre.ToString();
                //0087 Identificar si es un producto o un ensamble
                if (oDetalle.idEnsamble.Equals(null))
                {
                    //Datos del producto
                    Codigo = oDetalle.productos.Clave1.ToString();
                }
                else
                {
                    //Datos del ensamble
                    Codigo = oDetalle.ensamble.ClaveBom.ToString();
                }
                
                Descripcion = oDetalle.Descripcion.ToString();
                Unidad = oDetalle.productos.UnidadMedida;
                PrecioUnitario = oDetalle.Precio.ToString();
                dImporte=oDetalle.Cantidad * oDetalle.Precio;
                Importe = dImporte.ToString();
                

                Comprobante.Concepto C = new Comprobante.Concepto(
                    Cantidad,
                    Unidad, 
                    Codigo, 
                    Descripcion, 
                    PrecioUnitario, 
                    Importe);
                oComprobanteFiscalDigital.addConcepto(C);//aqui agregas el elemento a la partida

            }



            #endregion

            //Commpruebo que sea válido
            if (oComprobanteFiscalDigital.Valido() == true)
            {
                string sCadenaOriginal = oComprobanteFiscalDigital.CadenaOriginal();
                oComprobanteFiscalDigital.Sellar(sRutaArchivos + "/Facturacion/" + odalConfiguracion.sRutaLlave.ToString(), sRutaArchivos + "/Facturacion/" + odalConfiguracion.sRutaCertificado.ToString(), odalConfiguracion.sContraseña.ToString());
                oComprobanteFiscalDigital.XML().Save(sRutaArchivos + "/FacturasElectronicas/FacturaE-" + sFolioFactura + ".xml");
                //return a.XML();
                /*
                 * Modificaciones de POJO: 1 de Noviembre 2011 : Agregando el webservice para timbrado, guardaré los archivos en /Facturacion/Timbrados/
                 */
                FacturaService.TimbradoClient svcT = new FacturaService.TimbradoClient();
                FacturaService.RespuestaCFDi RespuestaCFDi = new FacturaService.RespuestaCFDi();
                
                //Aquí, cambiar la funcion a svcT.Timbrar cuando vayan a mostrarlo en producción, de otra manera, no serán válidos los cfdi's que emitan.
                try
                {
                    byte [] aArchivo = File.ReadAllBytes(sRutaArchivos + "/FacturasElectronicas/FacturaE-" + sFolioFactura + ".xml");
                    RespuestaCFDi = svcT.Timbrar("pojo", "a", aArchivo);                    
                    File.WriteAllBytes(sRutaArchivos + "/FacturasElectronicasTimbradas/FacturaE-" + sFolioFactura + ".xml", RespuestaCFDi.Documento);
                    RespuestaCFDi = svcT.PDF("pojo", "a", File.ReadAllBytes(sRutaArchivos + "/FacturasElectronicasTimbradas/FacturaE-" + sFolioFactura + ".xml"), null);
                    File.WriteAllBytes(sRutaArchivos + "/FacturasElectronicasTimbradas/FacturaE-" + sFolioFactura + ".pdf", RespuestaCFDi.Documento);
                    return 0;
                }
                catch (System.Net.WebException ex)
                {
                    //Sale cuando no encuentra al server
                    return 1;
                }
                catch (System.Web.Services.Protocols.SoapHeaderException ex)
                {

                    return 3;
                }
                catch (Exception ex)
                {
                    //Para excepciones no conocidas
                    return 4;
                }
                /*
                 * Falta: Revisar errores con PAC (¿Cómo los regresas? ¿Qué pasa si algo está mal? ¿el xml fue correctamente construido?¿Una función nueva?)
                 */
            }
            else
            {
                //Sale cuando el certificado no es valido
                return 2;
            }
        }

        /// <summary>
        /// Permite la cancelación de facturas [No hay test para esto].
        /// </summary>
        /// <param name="uuid">Es un arreglo de cadenas que contiene los uuid(timbres fiscales) a cancelar.[Por falta de pruebas, cancelen de uno en uno...]</param>
        /// <param name="sRutaArchivos">La ruta a los archivos...</param>
        /// <returns>0 si se canceló, 1 si fallo.</returns>
        public int CancelarFacturaElectronica(string[] uuid,string sRutaArchivos, out string sMensaje)
        {
            MedNeg.Configuracion.BlConfiguracion oblConfiguracion = new MedNeg.Configuracion.BlConfiguracion();
            MedDAL.Configuracion.DALConfiguracion odalConfiguracion = new MedDAL.Configuracion.DALConfiguracion();

            odalConfiguracion = (MedDAL.Configuracion.DALConfiguracion)oblConfiguracion.CargaDatos(sRutaArchivos+"/Configuracion.xml");

            FacturaService.TimbradoClient svcT = new FacturaService.TimbradoClient();
            FacturaService.RespuestaCFDi res = null;
            //El PFX, es un documento creado a partir del CER y KEY, lo ideal es crearlo por fuera, pero se puede crear desde aca con comandos a consola.
            //Es más facil si lo crean y lo cargan, a que lo creen en el sistema.
            res = svcT.Cancelar("medicuri", "a", File.ReadAllBytes(sRutaArchivos + "/Facturacion/" + odalConfiguracion.sRutaFirma.ToString()), uuid, odalConfiguracion.sContraseña);
            if (res.Documento == null)
            { //algo falló, revisa el mensaje y ve que pasó. lo mismo aplica para timbrado, pdf, etc... NUNCA he intentado cancelar más de un UUID, así que no se como funcione eso...
                sMensaje = res.Mensaje; //<- tiene el texto del error...
                return 1;
            }
            else
            {
                //Si se canceló, entonces, el documento escribe un nuevo archivo (un xml, con la información de cancelación, conocido como ack.)

                File.WriteAllBytes(sRutaArchivos + "/FacturasElectronicasTimbradas/FacturaE-" + uuid[0] + ".xml", res.Documento);
                sMensaje = res.Mensaje;
                return 0;
            }
        }

        public void CrearZip(string[] asArchivos, string sArchivo1, string sArchivo2)
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
                        if (file == sArchivo1 || file == sArchivo2)
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

        public void CrearZip(string[] asArchivos, string sArchivo1, string sArchivo2, string sArchivo3, string sArchivo4)
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
                        if (file == sArchivo1 || file == sArchivo2 || file == sArchivo3 || file == sArchivo4)
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

           
    }


    /// <summary>
    /// GT: 17/Mar/2011 Clase que se utiliza para generar los detalles de la facturas,Pedidos y Remisiones
    /// </summary>
    public class BlDetallePartida : MedDAL.DAL.productos
    {
        public int iIdProducto;
        public string sClave;
        public string sProducto;
        public decimal dCantidad;
        //public decimal dDesc1;
        //public decimal dDesc2;
        public decimal dIeps;
        //public decimal dImp1;
        //public decimal dImp2;
        public decimal dIva;
        //public decimal dComision;
        public decimal dPrecio;
        public string sObservaciones;
        public decimal dTotal;
        public string sDescripcion;
        public bool bEsEnsamble;

        //Para que funcione el binding al data grid view
        public int IIdProducto
        {
            get { return this.iIdProducto; } set { this.iIdProducto = value; }
        }
        public string SClave 
        {
            get { return this.sClave; }set { this.sClave = value;}
        }
        public string SProducto
        {
            get { return this.sProducto; }
            set { this.sProducto = value; }
        }
        public decimal DCantidad
        {
            get { return this.dCantidad; }
            set { this.dCantidad = value; }
        }
        //public decimal DDesc1
        //{
        //    get { return this.dDesc1; }
        //    set { this.dDesc1 = value; }
        //}
        //public decimal DDesc2
        //{
        //    get { return this.dDesc2; }
        //    set { this.dDesc2 = value; }
        //}
        public decimal DIeps
        {
            get { return this.dIeps; }
            set { this.dIeps = value; }
        }
        //public decimal DImp1
        //{
        //    get { return this.dImp1; }
        //    set { this.dImp1 = value; }
        //}
        //public decimal DImp2
        //{
        //    get { return this.dImp2; }
        //    set { this.dImp2 = value; }
        //}
        public decimal DIva
        {
            get { return this.dIva; }
            set { this.dIva = value; }
        }
        //public decimal DComision
        //{
        //    get { return this.dComision; }
        //    set { this.dComision = value; }
        //}
        public decimal DPrecio
        {
            get { return this.dPrecio; }
            set { this.dPrecio = value; }
        }
        public string SObservaciones
        {
            get { return this.sObservaciones; }
            set { this.sObservaciones = value; }
        }
        public decimal DTotal
        {
            get { return this.dTotal; }
            set { this.dTotal = value; }
        }
        public string SDescripcion
        {
            get { return this.sDescripcion; }
            set { this.sDescripcion = value; }
        }

        public bool BEsEnsamble
        {
            get { return this.bEsEnsamble; }
            set { this.bEsEnsamble = value; }
        }

        /// <summary>
        /// BL - Nuevo detalle de partida
        /// </summary>
        /// <param name="iIdProducto">IdProducto Producto</param>
        /// <param name="sClave">Clave Producto</param>
        /// <param name="sProducto">Descripción del producto</param>
        /// <param name="iCantidad">Cantidad</param>
        /// <param name="iIeps">IEPS</param>
        /// <param name="iImp1">Impuesto 1</param>
        /// <param name="iImp2">Impuesto 2</param>
        /// <param name="iIva">IVA</param>
        /// <param name="dPrecio">Precio</param>
        /// <param name="sObservaciones">Observaciones del detalle</param>
        /// <param name="sDescripcion">Descripcion del detalle</param>
        /// <param name="BEsEnsamble">Saber si es un ensamble el detalle o un producto solo</param>
       
        public BlDetallePartida(int iIdProducto,string sClave, string sProducto, decimal dCantidad, decimal dIeps,
             decimal dIva, decimal dPrecio, string sObservaciones,decimal dTotal,string sDescripcion="",bool bEnsamble=false)
        {
            this.iIdProducto = iIdProducto;
            this.sClave=sClave;
            this.sProducto = sProducto;
            this.dCantidad = dCantidad;
            //this.dDesc1 = dDesc1;
            //this.dDesc2 = dDesc2;
            this.dIeps = dIeps;
            //this.dImp1 = dImp1;
            //this.dImp2 = dImp2;
            this.dIva = dIva;
            //this.dComision = dComision;
            this.dPrecio = dPrecio;
            this.sObservaciones = sObservaciones;
            this.dTotal = dTotal;
            this.sDescripcion = sDescripcion;
            this.bEsEnsamble = bEnsamble;
        }
   }

    public class BlDetalleFacturaReceta
    {
        public int iIdProducto;
        public string sClave;
        public string sProducto;
        public decimal dCantidad;
        //public decimal dDesc1;
        //public decimal dDesc2;
        public decimal dIeps;
        public decimal dImp1;
        //public decimal dImp2;
        public decimal dIva;
        //public decimal dComision;
        public decimal dPrecio;
        public string sObservaciones;
        public decimal dTotal;
        public int iIdLineaCredito;
        public int iIdReceta;

        //Para que funcione el binding al data grid view
        public int IIdProducto
        {
            get { return this.iIdProducto; }
            set { this.iIdProducto = value; }
        }
        public string SClave
        {
            get { return this.sClave; }
            set { this.sClave = value; }
        }
        public string SProducto
        {
            get { return this.sProducto; }
            set { this.sProducto = value; }
        }
        public decimal DCantidad
        {
            get { return this.dCantidad; }
            set { this.dCantidad = value; }
        }
        //public decimal DDesc1
        //{
        //    get { return this.dDesc1; }
        //    set { this.dDesc1 = value; }
        //}
        //public decimal DDesc2
        //{
        //    get { return this.dDesc2; }
        //    set { this.dDesc2 = value; }
        //}
        public decimal DIeps
        {
            get { return this.dIeps; }
            set { this.dIeps = value; }
        }
        public decimal DImp1
        {
            get { return this.dImp1; }
            set { this.dImp1 = value; }
        }
        //public decimal DImp2
        //{
        //    get { return this.dImp2; }
        //    set { this.dImp2 = value; }
        //}
        public decimal DIva
        {
            get { return this.dIva; }
            set { this.dIva = value; }
        }
        //public decimal DComision
        //{
        //    get { return this.dComision; }
        //    set { this.dComision = value; }
        //}
        public decimal DPrecio
        {
            get { return this.dPrecio; }
            set { this.dPrecio = value; }
        }
        public string SObservaciones
        {
            get { return this.sObservaciones; }
            set { this.sObservaciones = value; }
        }
        public decimal DTotal
        {
            get { return this.dTotal; }
            set { this.dTotal = value; }
        }
        public int IIdLineaCredito
        {
            get { return this.iIdLineaCredito; }
            set{this.iIdLineaCredito=value;}
        }
        public int IIdReceta
        {
            get { return this.iIdReceta; }
            set { this.iIdReceta = value; }
        }

        public BlDetalleFacturaReceta(int iIdProducto, string sClave, string sProducto, decimal dCantidad, decimal dIeps,
             decimal dIva,decimal dImp1, decimal dPrecio, string sObservaciones,decimal dTotal,int iIdLineaCredito,int idReceta)
        {
            this.iIdProducto = iIdProducto;
            this.sClave=sClave;
            this.sProducto = sProducto;
            this.dCantidad = dCantidad;
            //this.dDesc1 = dDesc1;
            //this.dDesc2 = dDesc2;
            this.dIeps = dIeps;
            this.dImp1 = dImp1;
            //this.dImp2 = dImp2;
            this.dIva = dIva;
            //this.dComision = dComision;
            this.dPrecio = dPrecio;
            this.sObservaciones = sObservaciones;
            this.dTotal = dTotal;
            this.iIdLineaCredito = iIdLineaCredito;
            this.iIdReceta = idReceta;
        }
    }

    public class BlFacturacionDeLineas
    {

        public int iIdLineaCredito;
        public DateTime dtFecha;
        public decimal dMonto;

        public int IIdLineaCredito
        {
            get { return this.iIdLineaCredito; }
            set { this.iIdLineaCredito = value; }
        }
        public DateTime DtFecha
        {
            get { return this.dtFecha; }
            set { this.dtFecha = value; }
        }
        public decimal DMonto
        {
            get { return this.dMonto; }
            set { this.dMonto = value; }
        }



    }
}
