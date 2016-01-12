using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Objects;

namespace MedDAL.Facturas
{
    public class DALFacturas : ClsModulo
    {
        DAL.medicuriEntities oMedicuriEntities;

        public DAL.medicuriEntities MedicuriEntities
        {
            get { return this.oMedicuriEntities; }
            set { this.oMedicuriEntities = value; }
        }

        public DALFacturas()
        {
            oMedicuriEntities = new DAL.medicuriEntities();
        }

        /// <summary>
        /// DAL - Insertar nuevo registro
        /// </summary>
        /// <param name="oFactura">factura a guardar</param>
        /// <returns>true registrado, false no registrado</returns>
        public bool NuevoRegistro(DAL.facturas oFactura)
        {
            try
            {
                //Agregar el registro
                oMedicuriEntities.AddTofacturas(oFactura);
                oMedicuriEntities.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool NuevoRegistroFacturacionReceta(DAL.FacturacionDeRecetas oFacturacionRecetas)
        {
            try
            {
                //Agregar el registro
                oMedicuriEntities.AddToFacturacionDeRecetas(oFacturacionRecetas);
                oMedicuriEntities.SaveChanges();

                return true;

            }
            catch
            {
                return false;
            }

        }
        /// <summary>
        /// Buscar mediante filtros parametros
        /// </summary>
        /// <param name="sCadena">Parametro a buscar</param>
        /// <param name="iFiltro">Filtro</param>
        /// <returns>var oQuery</returns>
        public IQueryable<CuentasxCobrarView> Buscar(string sCadena, int iFiltro)
        {
            
            if (iFiltro == 1)
            {
                IQueryable<CuentasxCobrarView> oQuery1 = from q in oMedicuriEntities.facturas
                              where q.Folio.Contains(sCadena)
                              select new CuentasxCobrarView
                              {
                                 idFactura = q.idFactura,
                                 Nombre = q.clientes.Nombre,
                                 Apellidos = q.clientes.Apellidos,
                                 pedido = q.pedidos.Folio,
                                 remision = q.remisiones.Folio,
                                 receta = q.recetas.Folio,
                                 Folio = q.Folio,
                                 Fecha= q.Fecha,
                                 FechaAplicacion = (DateTime)q.FechaAplicacion,
                                 Estatus = q.Estatus
                              };
                return oQuery1;
            }

            if (iFiltro == 2)
            {
                IQueryable<CuentasxCobrarView> oQuery2 = from q in oMedicuriEntities.facturas
                              where q.clientes.Nombre.Contains(sCadena) || q.clientes.Apellidos.Contains(sCadena)
                              select new CuentasxCobrarView
                              {
                                 idFactura = q.idFactura,
                                 Nombre = q.clientes.Nombre,
                                 Apellidos = q.clientes.Apellidos,
                                 pedido = q.pedidos.Folio,
                                 remision = q.remisiones.Folio,
                                 receta = q.recetas.Folio,
                                 Folio = q.Folio,
                                 Fecha= q.Fecha,
                                 FechaAplicacion = (DateTime)q.FechaAplicacion,
                                 Estatus = q.Estatus
                              };
                return oQuery2;


            }

            if (iFiltro == 3)
            {


                DateTime dFecha = Convert.ToDateTime(sCadena);
                IQueryable<CuentasxCobrarView> oQuery3 = from q in oMedicuriEntities.facturas
                             where (q.Fecha == dFecha)
                             select new CuentasxCobrarView
                              {
                                 idFactura = q.idFactura,
                                 Nombre = q.clientes.Nombre,
                                 Apellidos = q.clientes.Apellidos,
                                 pedido = q.pedidos.Folio,
                                 remision = q.remisiones.Folio,
                                 receta = q.recetas.Folio,
                                 Folio = q.Folio,
                                 Fecha= q.Fecha,
                                 FechaAplicacion = (DateTime)q.FechaAplicacion,
                                 Estatus = q.Estatus
                              };
                return oQuery3;

            }

            return null;
        }

        /// <summary>
        /// Recuperar una factura mediante su folio
        /// </summary>
        /// <param name="sFolio">Folio a buscar</param>
        /// <returns></returns>
        public DAL.facturas BuscarFacturaFolio(string sFolio)
        {
            try
            {
                var oQuery = from q in oMedicuriEntities.facturas
                             where q.Folio == sFolio
                             select q;
                return oQuery.First<MedDAL.DAL.facturas>();
            }
            catch
            {
                return null;
            }


        }

        /// <summary>
        /// Recuperar una factura mediante su Id
        /// </summary>
        /// <param name="iIdFactura">Folio a buscar</param>
        /// <returns></returns>
        public DAL.facturas BuscarFactura(int iIdFactura)
        {
            try
            {
                var oQuery = from q in oMedicuriEntities.facturas
                             where q.idFactura == iIdFactura
                             select q;
                return oQuery.First<MedDAL.DAL.facturas>();
            }
            catch
            {
                return null;
            }


        }

       /// <summary>
       /// DAL - Agregar detalle de partida de una factura
       /// </summary>
       /// <param name="oFacturaPartida">Registro de la partida</param>
       /// <returns></returns>
        public bool NuevoDetalleFactura(DAL.facturas_partida oFacturaPartida)
        {
            try
            {
                //Agregar el registro
                oMedicuriEntities.AddTofacturas_partida(oFacturaPartida);
                oMedicuriEntities.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// DAL - Saber si existe un folio repetido
        /// </summary>
        /// <param name="sFolio">Folio a comparar</param>
        /// <returns></returns>
        public bool ValidarFolioRepetido(string sFolio)
        {

            try
            {
                var oQuery = from q in oMedicuriEntities.facturas
                             where q.Folio == sFolio
                             select q;

                if (oQuery.Count() > 0)
                    return false;
                else
                    return true;
            }
            catch
            {
                return false;
            }

        }

        /// <summary>
        /// REcupera la lista de los clientes
        /// </summary>
        /// <returns></returns>
        public IQueryable<CuentasxCobrarView> MostrarLista()
        {
            IQueryable<CuentasxCobrarView> oQuery = (from q in oMedicuriEntities.facturas
                          select new CuentasxCobrarView
                          {
                              idFactura = q.idFactura,
                              Nombre = q.clientes.Nombre,
                              Apellidos = q.clientes.Apellidos,
                              pedido = q.pedidos.Folio,
                              remision = q.remisiones.Folio,
                              receta = q.recetas.Folio,
                              Folio = q.Folio,
                              Fecha = q.Fecha,
                              FechaAplicacion = (DateTime)q.FechaAplicacion,
                              Estatus = q.Estatus
                          });
           return oQuery;
        }

        /// <summary>
        /// REcupera la lista de los clientes
        /// </summary>
        /// <returns></returns>
        public IQueryable<CuentasxCobrarView> MostrarListaCuentasCobrar()
        {
            IQueryable<CuentasxCobrarView> oQuery = (from q in oMedicuriEntities.facturas
                          //where q.Estatus=="3"
                          select new CuentasxCobrarView
                          {
                              idFactura = q.idFactura,
                              Nombre = q.clientes.Nombre,
                              Apellidos = q.clientes.Apellidos,
                              //pedido = q.pedidos.Folio,
                              //remision = q.remisiones.Folio,
                              //receta = q.recetas.Folio,
                              Folio = q.Folio,
                              Fecha = q.Fecha,
                              FechaAplicacion = (DateTime)q.FechaAplicacion,
                              Estatus = q.Estatus
                          });
            return oQuery;
        }

        /// <summary>
        /// Recuperar la partida de una factura
        /// </summary>
        /// <param name="iIdFactura"></param>
        /// <returns></returns>
        public IQueryable<DAL.facturas_partida> RecuperarPartidaFactura(int iIdFactura)
        {
            try
            {
                var oQuery = from q in oMedicuriEntities.facturas_partida
                             where q.idFactura == iIdFactura
                             select q;

                return oQuery;

            }
            catch
            {
                return null;
            }
         }

        /// <summary>
        /// Editar registro
        /// </summary>
        /// <param name="oFactura"></param>
        /// <returns></returns>
        public bool EditarRegistro(DAL.facturas oFactura)
        {
            try
            {
                var oQuery = from q in oMedicuriEntities.facturas.Where("it.idFactura=@idFactura",
                            new ObjectParameter("idFactura", oFactura.idFactura))
                            select q;

                DAL.facturas oFacturaOriginal= oQuery.First<DAL.facturas>();
                oFacturaOriginal.Estatus = oFactura.Estatus;

                oMedicuriEntities.SaveChanges();
                return true;            

            }
            catch
            {
                return false;
            }

        }

        /// <summary>
        /// Eliminar la partida de una factura
        /// </summary>
        /// <param name="iIdFactura"></param>
        /// <returns></returns>
        public bool EliminarFacturaPartida(int iIdFactura)
        {
            try
            {
               var oQuery = from q in oMedicuriEntities.facturas_partida.
                             Where("it.idFactura=@idFactura",
                             new ObjectParameter("idFactura", iIdFactura))
                             select q;

               foreach (DAL.facturas_partida oRegistro in oQuery)
               {
                   oMedicuriEntities.DeleteObject(oRegistro);

               }
                oMedicuriEntities.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Eliminar una factura
        /// </summary>
        /// <param name="iIdFactura">Id factura a eliminar</param>
        /// <returns></returns>
        public bool EliminarRegistro(int iIdFactura)
        {
            try
            {
                var oQuery = from q in oMedicuriEntities.facturas.
                              Where("it.idFactura=@idFactura",
                              new ObjectParameter("idFactura", iIdFactura))
                             select q;

                DAL.facturas oFacturaOriginal = oQuery.First<DAL.facturas>();
                oMedicuriEntities.DeleteObject(oFacturaOriginal);
                oMedicuriEntities.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Metodo para obtener el total facturado por linea de credito (programa)
        /// </summary>
        /// <returns></returns>
        public IQueryable<FacturasxRecetaView> TotalFacturadoPorLineaCredito()
        {
            try
            {
                IQueryable<FacturasxRecetaView> oQuery = (from lc in oMedicuriEntities.lineas_creditos
                              select new FacturasxRecetaView
                              {
                                  idLineaCredito = lc.idLineaCredito,
                                  FuenteCuenta = lc.FuenteCuenta,
                                  Monto = lc.Monto,
                                  FechaVencimiento = lc.FechaVencimiento,
                                  Activo = lc.Activo,
                                  Facturado = ((decimal)(from fr in oMedicuriEntities.FacturacionDeRecetas
                                               where fr.idLineaCredito == lc.idLineaCredito
                                                         select fr.Monto).Sum()) == null ? 0 
                                                         : 
                                              (decimal)(from fr in oMedicuriEntities.FacturacionDeRecetas
                                                where fr.idLineaCredito == lc.idLineaCredito
                                                select fr.Monto).Sum()
                              });
                
                return oQuery;
            }
            catch
            {
                return null;
            }
       }


        /// <summary>
        /// Buscar folios asincronamente para llenar autocomplete extenders
        /// </summary>
        /// <param name="sCadena">Cadena que contenga la clave</param>
        /// <returns></returns>
        public string[] BuscarFolioFacturasAsincrono(string sCadena)
        {
            string[] asResultados;

            try
            {
                var oQuery = from q in oMedicuriEntities.facturas
                             where q.Folio.Contains(sCadena)
                             select q.Folio;

                return asResultados = oQuery.ToArray<string>();

            }
            catch
            {
                return asResultados = new string[0];
            }
        }


        /// <summary>
        /// Buscar folios asincronamente para llenar autocomplete extenders
        /// </summary>
        /// <param name="sCadena">Cadena que contenga la clave</param>
        /// <returns></returns>
        public string[] BuscarVendedorAsincrono(string sCadena)
        {
            string[] asResultados;

            try
            {
                var oQuery = (from q in oMedicuriEntities.facturas
                              where q.Vendedor.Contains(sCadena)
                              select q.Vendedor).Distinct();

                return asResultados = oQuery.ToArray<string>();

            }
            catch
            {
                return asResultados = new string[0];
            }
        }
       
    }
 
}
