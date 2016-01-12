using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Objects;

namespace MedDAL.Productos
{
    public class DALProductos
    {
        DAL.medicuriEntities oMedicuriEntities;

        public DALProductos()
        {
            oMedicuriEntities = new DAL.medicuriEntities();
        }

        /// <summary>
        /// DAL - Insertar nuevo registro
        /// </summary>
        /// <param name="oProducto">Registro a guardar</param>
        /// <returns>true registrado, false no registrado</returns>
        public bool NuevoRegistro(DAL.productos oProducto)
        {
            try
            {
                //Agregar el registro
                oMedicuriEntities.productos.AddObject(oProducto);
                oMedicuriEntities.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }

        //comentada por EL
        /*   public DAL.productos buscarProducto(String clave1)
           {
               try
               {
                   var oQuery = from q in oMedicuriEntities.productos
                                where q.Clave1 == clave1
                                select q;
                   return oQuery.First<MedDAL.DAL.productos>();
               }
               catch 
               {
                   return null;
               }
           }
           */

        /// <summary>
        /// Busca un producto dada su clave1
        /// </summary>
        /// <param name="clave1">clave del producto a buscar</param>
        /// <returns></returns>
        public DAL.productos buscarProducto(String clave1)
        {
            try
            {
                var oQuery = from q in oMedicuriEntities.productos.
                                    Where("it.Clave1 = @clave1",
                                    new ObjectParameter("clave1", clave1))
                             select q;
                return oQuery.First<MedDAL.DAL.productos>();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// Busca un producto por su clave2
        /// </summary>
        /// <param name="sClave2"></param>
        /// <returns></returns>
        public DAL.productos buscarProductoClave2(String sClave2)
        {
            try
            {
                var oQuery = from q in oMedicuriEntities.productos.
                                    Where("it.Clave2 = @clave2",
                                    new ObjectParameter("clave2", sClave2))
                             select q;
                return oQuery.First<MedDAL.DAL.productos>();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// Busca un producto dado su id
        /// </summary>
        /// <param name="id">id del producto a buscar</param>
        /// <returns></returns>
        public DAL.productos buscarProducto(int id)
        {
            try
            {
                var oQuery = from q in oMedicuriEntities.productos
                                    where q.idProducto==id
                             select q;
                return oQuery.First<MedDAL.DAL.productos>();
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public IQueryable<DAL.productos> buscarProducto(string clave1Desde,string clave1hasta)
        {
            var oQuery = from q in oMedicuriEntities.productos
                         where (
                                    q.Clave1.CompareTo(clave1Desde) >= 0 &&
                                    q.Clave1.CompareTo(clave1hasta) <= 0
                               )
                         select q;
            return oQuery;
        }

        public decimal obtenerExistenciaProducto(int idAlmacen, int idProducto,string lote,string serie)
        {
            var oQuery = (from q in oMedicuriEntities.productos_almacen
                          where (q.idProducto == idProducto && q.idAlmacen == idAlmacen && q.Lote.Equals(lote) && q.NoSerie.Equals(serie))
                          select q.Cantidad).Sum();
            return oQuery;
        }
        
        public IQueryable<String> buscarLotesProducto(int idProducto,int idAlmacen)
        {
            var oQuery = (from q in oMedicuriEntities.productos_almacen
                         where (q.idProducto == idProducto && q.idAlmacen==idAlmacen && q.Bloqueado==0 && !q.Lote.Equals(String.Empty) && q.Cantidad != 0)
                         select q.Lote).Distinct();
            return  oQuery;
        }

        public IQueryable<String> buscarSeriesProducto(int idProducto, int idAlmacen)
        {
            //var oQuery = (from q in oMedicuriEntities.productos_almacen
            //              where (q.idProducto == idProducto && q.idAlmacen == idAlmacen && q.Bloqueado == 0 && !q.NoSerie.Equals(String.Empty) && q.Cantidad != 0)
            //             select q.NoSerie).Distinct();
            //&& q.productos.productos_almacen.Where(b => b.Bloqueado == 0) && q.productos.productos_almacen.Where(c => c.Cantidad != 0)
            var oQuery = (from q in oMedicuriEntities.inventario_partida
                          where (q.idProducto == idProducto && q.inventario.idAlmacen == idAlmacen && !q.NoSerie.Equals(string.Empty) 
                          && q.productos.productos_almacen.Where(c => c.Cantidad != 0).Count() > 0
                          && q.productos.productos_almacen.Where(b => b.Bloqueado == 0).Count() > 0)
                          select q.NoSerie).Distinct();

            return oQuery;
        }

        public int buscarLineaCredito(int idAlmacen, int idProducto, string serie, string lote)
        {
            var oQuery = (from q in oMedicuriEntities.productos_almacen
                          where (q.idProducto == idProducto && q.idAlmacen == idAlmacen 
                          && q.Bloqueado == 0 && q.NoSerie.Equals(serie) && q.Lote.Equals(lote) )
                          select q.idLineaCredito);
            return (int) oQuery.First();
        }

        //public decimal buscarCostoProducto(int idAlmacen, int idProducto)
        //{
        //    var oQuery = (from q in oMedicuriEntities.productos_almacen
        //                  where (q.idProducto == idProducto && q.idAlmacen == idAlmacen && q.Bloqueado == 0)
        //                  select q.Costo);
        //    return oQuery.First();
        //}

        public IQueryable<DAL.productos_almacen> ObtenerExistenciaProducto(int idProducto, int idAlmacen)
        {
            var oQuery = from q in oMedicuriEntities.productos_almacen
                          where (q.idProducto == idProducto && q.idAlmacen == idAlmacen ) && q.Cantidad > 0
                          select q;
            return oQuery;
        }

        public IQueryable<DAL.productos_almacen> ObtenerExistenciaProducto(int idProducto, int idAlmacen, string sLote, string sNoSerie)
        {
            var oQuery = from q in oMedicuriEntities.productos_almacen
                         where (q.idProducto == idProducto && q.idAlmacen == idAlmacen && q.Lote == sLote && q.NoSerie == sNoSerie)
                         select q;
            return oQuery;
        }

        /// <summary>
        /// EL: Función que modifica la existencia de un producto
        /// </summary>
        /// <param name="idAlmacen">id del almacén cuya existencia se modificará</param>
        /// <param name="idProducto">id del producto cuya existencia se modificará</param>
        /// <param name="cantidad">cantidad a modificar</param>
        /// <param name="modo">0 aumentar, 1 disminuir</param>
        /// <returns></returns>
        public void ModificarExistenciaProducto(int idAlmacen, int idProducto,String lote,String serie, decimal cantidad, int modo)
        {
            var oQuery = from q in oMedicuriEntities.productos_almacen
                         where q.idAlmacen == idAlmacen && q.idProducto == idProducto && q.Lote.Equals(lote) && q.NoSerie.Equals(serie)
                         select q;

            DAL.productos_almacen productoAlmacen = oQuery.First<DAL.productos_almacen>();
            if (modo == 0)
                productoAlmacen.Cantidad += cantidad;
            else if (modo == 1)
                productoAlmacen.Cantidad -= cantidad;


            oMedicuriEntities.SaveChanges();
        }

        /// <summary>
        /// EL: Función que modifica la existencia de un producto
        /// </summary>
        /// <param name="idAlmacen">id del almacén cuya existencia se modificará</param>
        /// <param name="idProducto">id del producto cuya existencia se modificará</param>
        /// <param name="cantidad">cantidad a modificar</param>
        /// <param name="modo">0 aumentar, 1 disminuir</param>
        /// <returns></returns>
        public void ModificarExistenciaProducto(int idAlmacen, int idProducto, String lote, String serie,DateTime fechaCaducidad, decimal cantidad, int modo)
        {
            var oQuery = from q in oMedicuriEntities.productos_almacen
                         where q.idAlmacen == idAlmacen && q.idProducto == idProducto && q.Lote.Equals(lote)//Se elimina a petición del cliente && q.NoSerie.Equals(serie)
                         select q;

            DAL.productos_almacen productoAlmacen = oQuery.First<DAL.productos_almacen>();
            if (modo == 0)
                productoAlmacen.Cantidad += cantidad;
            else if (modo == 1)
                productoAlmacen.Cantidad -= cantidad;

            productoAlmacen.FechaCaducidad = fechaCaducidad;

            oMedicuriEntities.SaveChanges();
        }

        /// <summary>
        /// 2013/03/10 JID Modificar la existencia de un producto en el almacen
        /// </summary>
        /// <param name="idAlmacen">id del almacén cuya existencia se modificará</param>
        /// <param name="idProducto">id del producto cuya existencia se modificará</param>
        /// <param name="dCantidad">cantidad a modificar</param>
        /// <param name="modo">0 aumentar, 1 disminuir</param>
        public bool ModificarExistenciaProducto(int idAlmacen, int idProducto, decimal dCantidad, int iModo, MedDAL.DAL.medicuriEntities oEntities)
        {
            try
            {
                var oQuery = from q in oEntities.productos_almacen
                             where q.idAlmacen == idAlmacen && q.idProducto == idProducto
                             select q;

                DAL.productos_almacen productoAlmacen = oQuery.First<DAL.productos_almacen>();

                if (iModo == 0)
                    productoAlmacen.Cantidad += dCantidad;
                else if (iModo == 1)
                    productoAlmacen.Cantidad -= dCantidad;

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// GT: Función que modifica la existencia de un producto
        /// </summary>
        /// <param name="idAlmacen">id del almacén cuya existencia se modificará</param>
        /// <param name="idProducto">id del producto cuya existencia se modificará</param>
        /// <param name="dCantidad">cantidad a modificar</param>
        /// <param name="modo">0 aumentar, 1 disminuir</param>
        public bool ModificarExistenciaProducto(int idAlmacen,int idProducto,decimal dCantidad,int iModo)
        {
            try
            {
                var oQuery = from q in oMedicuriEntities.productos_almacen
                             where q.idAlmacen == idAlmacen && q.idProducto == idProducto
                             select q;

                DAL.productos_almacen productoAlmacen = oQuery.First<DAL.productos_almacen>();

                if (iModo == 0)
                    productoAlmacen.Cantidad += dCantidad;
                else if (iModo == 1)
                    productoAlmacen.Cantidad -= dCantidad;

                oMedicuriEntities.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public void AgregarExistenciaProducto(int idAlmacen, int idProducto, String lote, String serie, DateTime fechaCaducidad, decimal cantidad,int idLineaCredito)
        {
            MedDAL.DAL.productos_almacen producto = new DAL.productos_almacen();
            producto.idAlmacen = idAlmacen;
            producto.idProducto = idProducto;
            producto.Lote = lote;
            producto.NoSerie = serie;
            producto.Cantidad = cantidad;
            producto.Bloqueado = 0;
            producto.FechaCaducidad = fechaCaducidad;
            producto.idLineaCredito = idLineaCredito;
            oMedicuriEntities.AddToproductos_almacen(producto);
            oMedicuriEntities.SaveChanges();

        }

       
        public void EstablecerExistenciaProducto(int idAlmacen, int idProducto, String lote, String serie, decimal cantidad)
        {
            var oQuery = from q in oMedicuriEntities.productos_almacen
                         where q.idAlmacen == idAlmacen && q.idProducto == idProducto && q.Lote.Equals(lote) && q.NoSerie.Equals(serie)
                         select q;

            DAL.productos_almacen producto = oQuery.First();
            producto.Cantidad = cantidad;

            oMedicuriEntities.SaveChanges();

        }

        public MedDAL.DAL.productos_almacen ObtenerProductoLote(int idAlmacen, int idProducto, string sLote)
        {
            var oquery = from q in oMedicuriEntities.productos_almacen
                         where q.idAlmacen == idAlmacen && q.idProducto == idProducto && q.Lote.Equals(sLote)
                         select q;

            return oquery.First<MedDAL.DAL.productos_almacen>();
        }



        public MedDAL.DAL.productos_almacen_stocks ObtenerProductoAlmacenStock(int idAlmacen, int idProducto)
        {
            var oQuery = from q in oMedicuriEntities.productos_almacen_stocks
                         where q.idAlmacen == idAlmacen && q.idProducto == idProducto
                         select q;

            return oQuery.First<MedDAL.DAL.productos_almacen_stocks>();
        }
       
        public void BloquearProducto(int idAlmacen, int idProducto,bool bloquear)
        {
            var oQuery = from q in oMedicuriEntities.productos_almacen
                         where q.idAlmacen == idAlmacen && q.idProducto == idProducto
                         select q;
            
            foreach( DAL.productos_almacen producto in oQuery)
                producto.Bloqueado = (bloquear) ? 1 : 0;

            oMedicuriEntities.SaveChanges();
        }

        public void BloquearProducto(int idAlmacen, string clave1Desde, string clave1hasta, bool bloquear)
        { 
         var oQuery = from q in oMedicuriEntities.productos
                         where (
                                    q.Clave1.CompareTo(clave1Desde) > 0 && q.Clave1.CompareTo(clave1Desde) == 0 &&
                                    q.Clave1.CompareTo(clave1hasta) < 0 && q.Clave1.CompareTo(clave1hasta) == 0
                               )
                         select q;
            List<MedDAL.Inventarios.ErrorCambioPrecio> errores = new List<Inventarios.ErrorCambioPrecio>();
            foreach (DAL.productos producto in oQuery)
            {
                BloquearProducto(idAlmacen, producto.idProducto, bloquear);
            }
        
        }


        //public decimal buscarCostoProducto(int idAlmacen,int idProducto)
        //{
        //    var oQuery = (from q in oMedicuriEntities.productos_almacen
        //                  where (q.idProducto == idProducto && q.idAlmacen == idAlmacen && q.Bloqueado == 0)
        //                  select q.Costo);
        //    return oQuery.First();
        //}

       /*comentada por EL
        /// <summary>
        /// EL - Establece la existencia de un producto
        /// </summary>
        /// <param name="idAlmacen">id del Almacen afectado</param>
        /// <param name="idProducto">id del producto afectado</param>
        /// <param name="cantidad">nueva cantidad</param>
        public void EstablecerExistenciaProducto(int idAlmacen, int idProducto, decimal cantidad)
        {
            var oQuery = from q in oMedicuriEntities.productos_almacen
                         where q.idAlmacen == idAlmacen && q.idProducto == idProducto
                         select q;

            DAL.productos_almacen productoAlmacen = oQuery.First<DAL.productos_almacen>();
                productoAlmacen.Cantidad = cantidad;
            
            oMedicuriEntities.SaveChanges();

        }
        */
        #region  modificarPreciosProducto

        public bool evaluaRequerimientosPrecios(decimal? precioMinimo, decimal? precio1, decimal? precio2, decimal? precio3, decimal? precioPublico)
        {
            if ((
                 ((precio1 != 0) ? (precioMinimo <= precio1) : true) && 
                 ((precio2 != 0) ? (precioMinimo <= precio2) : true) &&
                 ((precio3 != 0) ? (precioMinimo <= precio3) : true) && 
                 precioMinimo <= precioPublico &&
                 ((precio1 != 0) ? ( precioPublico >= precio1) : true) && 
                 ((precio2 != 0) ? (precioPublico >= precio2 ) : true) &&
                 ((precio3 != 0) ? (precioPublico >= precio3) : true)
                ))
                return true;
            else
                throw new Exception("Violación a la jerarquía de precios");
        }

        /// <summary>
        /// aumentar en un monto los precios de TODOS los productos en la lista de precios señalada
        /// </summary>
        /// <param name="cant">cantidad a aumentar</param>
        /// <param name="listaPrecio">
        /// 1: Precio1
        /// 2: Precio2
        /// 3: Precio3
        /// 4: PrecioMinimo
        /// 5: PrecioPublico
        /// </param>
        public List<MedDAL.Inventarios.ErrorCambioPrecio> PreciosProductosAumentarMonto(decimal cant, int listaPrecio)
        {
            var oQuery = from q in oMedicuriEntities.productos
                         select q;

            List<MedDAL.Inventarios.ErrorCambioPrecio> errores = new List<Inventarios.ErrorCambioPrecio>();

            foreach (DAL.productos producto in oQuery)
            {
                try
                {
                    switch (listaPrecio)
                    {
                        case 1: if (evaluaRequerimientosPrecios(producto.PrecioMinimo, producto.Precio1 + cant, producto.Precio2, producto.Precio3, producto.PrecioPublico))
                                producto.Precio1 += cant;
                            break;
                        case 2: if (evaluaRequerimientosPrecios(producto.PrecioMinimo, producto.Precio1, producto.Precio2 + cant, producto.Precio3, producto.PrecioPublico))
                                producto.Precio2 += cant;
                            break;
                        case 3: if (evaluaRequerimientosPrecios(producto.PrecioMinimo, producto.Precio1, producto.Precio2, producto.Precio3 + cant, producto.PrecioPublico))
                                producto.Precio3 += cant;
                            break;
                        case 4: if (evaluaRequerimientosPrecios(producto.PrecioMinimo + cant, producto.Precio1, producto.Precio2, producto.Precio3, producto.PrecioPublico))
                                producto.PrecioMinimo += cant;
                            break;
                        case 5: if (evaluaRequerimientosPrecios(producto.PrecioMinimo, producto.Precio1, producto.Precio2, producto.Precio3, producto.PrecioPublico + cant))
                                producto.PrecioPublico += cant;
                            break;
                        default: throw new Exception("Error en el parámetro listaPrecio");
                    }
                }
                catch (Exception ex)
                { 
                    if(ex.Message.Equals("Violación a la jerarquía de precios"))
                        errores.Add(new Inventarios.ErrorCambioPrecio(producto.Clave1,"Violación a la jerarquía de precios"));
                }
            }

            oMedicuriEntities.SaveChanges();
            if (errores.Count > 0)
                return errores;
            else
                return null;
        }

        /// <summary>
        /// disminuir en un monto los precios de TODOS los productos en la lista de precios señalada
        /// </summary>
        /// <param name="cant">cantidad a disminuir</param>
        /// <param name="listaPrecio">
        /// 1: Precio1
        /// 2: Precio2
        /// 3: Precio3
        /// 4: PrecioMinimo
        /// 5: PrecioPublico
        /// </param>
        public List<MedDAL.Inventarios.ErrorCambioPrecio> PreciosProductosDisminuirMonto(decimal cant, int listaPrecio)
        {
            var oQuery = from q in oMedicuriEntities.productos
                         select q;
            List<MedDAL.Inventarios.ErrorCambioPrecio> errores = new List<Inventarios.ErrorCambioPrecio>();
            foreach (DAL.productos producto in oQuery)
            {
             try
             {

                switch (listaPrecio)
                {
                    case 1: if (evaluaRequerimientosPrecios(producto.PrecioMinimo, producto.Precio1-cant, producto.Precio2, producto.Precio3, producto.PrecioPublico))
                        producto.Precio1 -= cant;
                        break;
                    case 2: if (evaluaRequerimientosPrecios(producto.PrecioMinimo, producto.Precio1, producto.Precio2 - cant, producto.Precio3, producto.PrecioPublico))
                        producto.Precio2 -= cant;
                        break;
                    case 3: if (evaluaRequerimientosPrecios(producto.PrecioMinimo, producto.Precio1, producto.Precio2, producto.Precio3 - cant, producto.PrecioPublico))
                        producto.Precio3 -= cant;
                        break;
                    case 4: if (evaluaRequerimientosPrecios(producto.PrecioMinimo - cant, producto.Precio1, producto.Precio2, producto.Precio3, producto.PrecioPublico))
                        producto.PrecioMinimo -= cant;
                        break;
                    case 5: if (evaluaRequerimientosPrecios(producto.PrecioMinimo, producto.Precio1, producto.Precio2, producto.Precio3, producto.PrecioPublico - cant))
                        producto.PrecioPublico -= cant;
                        break;
                    default: throw new Exception("Error en el parámetro listaPrecio");
                }
             }
             catch (Exception ex)
             {
                 if (ex.Message.Equals("Violación a la jerarquía de precios"))
                     errores.Add(new Inventarios.ErrorCambioPrecio(producto.Clave1, "Violación a la jerarquía de precios"));
             }
            }

            oMedicuriEntities.SaveChanges();
            if (errores.Count > 0)
                return errores;
            else
                return null;
        }

        /// <summary>
        /// aumentar en un porcentaje los precios de TODOS los productos en la lista de precios señalada
        /// </summary>
        /// <param name="cant">cantidad a aumentar</param>
        /// <param name="listaPrecio">
        /// 1: Precio1
        /// 2: Precio2
        /// 3: Precio3
        /// 4: PrecioMinimo
        /// 5: PrecioPublico
        /// </param>
        public List<MedDAL.Inventarios.ErrorCambioPrecio> PreciosProductosAumentarPorcentaje(decimal cant, int listaPrecio)
        {
            var oQuery = from q in oMedicuriEntities.productos
                         select q;
            decimal pctje = 1 + cant / 100;
            List<MedDAL.Inventarios.ErrorCambioPrecio> errores = new List<Inventarios.ErrorCambioPrecio>();
            foreach (DAL.productos producto in oQuery)
            {
               try{
                switch (listaPrecio)
                {
                    case 1: if (evaluaRequerimientosPrecios(producto.PrecioMinimo, producto.Precio1*pctje, producto.Precio2, producto.Precio3, producto.PrecioPublico))
                        producto.Precio1 *= pctje;
                        break;
                    case 2: if (evaluaRequerimientosPrecios(producto.PrecioMinimo, producto.Precio1, producto.Precio2*pctje, producto.Precio3, producto.PrecioPublico))
                        producto.Precio2 *= pctje;
                        break;
                    case 3: if (evaluaRequerimientosPrecios(producto.PrecioMinimo, producto.Precio1, producto.Precio2, producto.Precio3*pctje, producto.PrecioPublico))
                        producto.Precio3 *= pctje;
                        break;
                    case 4: if (evaluaRequerimientosPrecios(producto.PrecioMinimo*pctje, producto.Precio1, producto.Precio2, producto.Precio3, producto.PrecioPublico))
                        producto.PrecioMinimo *= pctje;
                        break;
                    case 5: if (evaluaRequerimientosPrecios(producto.PrecioMinimo, producto.Precio1, producto.Precio2, producto.Precio3, producto.PrecioPublico*pctje))
                        producto.PrecioPublico *= pctje;
                        break;
                    default: throw new Exception("Error en el parámetro listaPrecio");
                }
               }
               catch (Exception ex)
               {
                   if (ex.Message.Equals("Violación a la jerarquía de precios"))
                       errores.Add(new Inventarios.ErrorCambioPrecio(producto.Clave1, "Violación a la jerarquía de precios"));
               }
            }

            oMedicuriEntities.SaveChanges();
            if (errores.Count > 0)
                return errores;
            else
                return null;
        }

        /// <summary>
        /// Disminuir en un porcentaje los precios de TODOS los productos en la lista de precios señalada
        /// </summary>
        /// <param name="cant">cantidad a Disminuir</param>
        /// <param name="listaPrecio">
        /// 1: Precio1
        /// 2: Precio2
        /// 3: Precio3
        /// 4: PrecioMinimo
        /// 5: PrecioPublico
        /// </param>
        public List<MedDAL.Inventarios.ErrorCambioPrecio> PreciosProductosDisminuirPorcentaje(decimal cant, int listaPrecio)
        {
            var oQuery = from q in oMedicuriEntities.productos
                         select q;
            decimal pctje = 1 - cant / 100;
            List<MedDAL.Inventarios.ErrorCambioPrecio> errores = new List<Inventarios.ErrorCambioPrecio>();
            foreach (DAL.productos producto in oQuery)
            {
                try{
                switch (listaPrecio)
                {
                    case 1: if (evaluaRequerimientosPrecios(producto.PrecioMinimo, producto.Precio1 * pctje, producto.Precio2, producto.Precio3, producto.PrecioPublico))
                            producto.Precio1 *= pctje;
                        break;
                    case 2: if (evaluaRequerimientosPrecios(producto.PrecioMinimo, producto.Precio1, producto.Precio2 * pctje, producto.Precio3, producto.PrecioPublico))
                            producto.Precio2 *= pctje;
                        break;
                    case 3: if (evaluaRequerimientosPrecios(producto.PrecioMinimo, producto.Precio1, producto.Precio2, producto.Precio3 * pctje, producto.PrecioPublico))
                            producto.Precio3 *= pctje;
                        break;
                    case 4: if (evaluaRequerimientosPrecios(producto.PrecioMinimo * pctje, producto.Precio1, producto.Precio2, producto.Precio3, producto.PrecioPublico))
                            producto.PrecioMinimo *= pctje;
                        break;
                    case 5: if (evaluaRequerimientosPrecios(producto.PrecioMinimo, producto.Precio1, producto.Precio2, producto.Precio3, producto.PrecioPublico * pctje))
                            producto.PrecioPublico *= pctje;
                        break;
                    default: throw new Exception("Error en el parámetro listaPrecio");
                }
                }
                catch (Exception ex)
                {
                    if (ex.Message.Equals("Violación a la jerarquía de precios"))
                        errores.Add(new Inventarios.ErrorCambioPrecio(producto.Clave1, "Violación a la jerarquía de precios"));
                }
            }

            oMedicuriEntities.SaveChanges();
            if (errores.Count > 0)
                return errores;
            else
                return null;
        }


       /// <summary>
       /// EL : Método para aumentar los precios en un rango de clave1, en un intervalo cerrado
       /// </summary>
       /// <param name="clave1Desde"></param>
       /// <param name="clave1hasta"></param>
       /// <param name="cant"></param>
       /// <param name="listaPrecio">
        /// 1: Precio1
        /// 2: Precio2
        /// 3: Precio3
        /// 4: PrecioMinimo
        /// 5: PrecioPublico
       /// </param>
        public List<MedDAL.Inventarios.ErrorCambioPrecio> PreciosProductosAumentarMonto(string clave1Desde, string clave1hasta, decimal cant, int listaPrecio)
        {
            var oQuery = from q in oMedicuriEntities.productos
                         where (
                                    q.Clave1.CompareTo(clave1Desde) >= 0  &&
                                    q.Clave1.CompareTo(clave1hasta) <= 0                      
                               )
                         select q;

            List<MedDAL.Inventarios.ErrorCambioPrecio> errores = new List<Inventarios.ErrorCambioPrecio>();
            foreach (DAL.productos producto in oQuery)
            {
               try{
                switch (listaPrecio)
                {
                    case 1: if (evaluaRequerimientosPrecios(producto.PrecioMinimo, producto.Precio1 + cant, producto.Precio2, producto.Precio3, producto.PrecioPublico))
                            producto.Precio1 += cant;
                        break;
                    case 2: if (evaluaRequerimientosPrecios(producto.PrecioMinimo, producto.Precio1, producto.Precio2 + cant, producto.Precio3, producto.PrecioPublico))
                            producto.Precio2 += cant;
                        break;
                    case 3: if (evaluaRequerimientosPrecios(producto.PrecioMinimo, producto.Precio1, producto.Precio2, producto.Precio3 + cant, producto.PrecioPublico))
                            producto.Precio3 += cant;
                        break;
                    case 4: if (evaluaRequerimientosPrecios(producto.PrecioMinimo + cant, producto.Precio1, producto.Precio2, producto.Precio3, producto.PrecioPublico))
                            producto.PrecioMinimo += cant;
                        break;
                    case 5: if (evaluaRequerimientosPrecios(producto.PrecioMinimo, producto.Precio1, producto.Precio2, producto.Precio3, producto.PrecioPublico + cant))
                            producto.PrecioPublico += cant;
                        break;
                    default: throw new Exception("Error en el parámetro listaPrecio");
                }
               }
               catch (Exception ex)
               {
                   if (ex.Message.Equals("Violación a la jerarquía de precios"))
                       errores.Add(new Inventarios.ErrorCambioPrecio(producto.Clave1, "Violación a la jerarquía de precios"));
               }
            }

            oMedicuriEntities.SaveChanges();
            if (errores.Count > 0)
                return errores;
            else
                return null;
        }

        /// <summary>
        /// disminuir en un monto los precios de TODOS los productos en la lista de precios señalada
        /// </summary>
        /// <param name="cant">cantidad a disminuir</param>
        /// <param name="listaPrecio">
        /// 1: Precio1
        /// 2: Precio2
        /// 3: Precio3
        /// 4: PrecioMinimo
        /// 5: PrecioPublico
        /// </param>
        public List<MedDAL.Inventarios.ErrorCambioPrecio> PreciosProductosDisminuirMonto(string clave1Desde, string clave1hasta, decimal cant, int listaPrecio)
        {
            var oQuery = from q in oMedicuriEntities.productos
                         where (
                                    q.Clave1.CompareTo(clave1Desde) > 0 && q.Clave1.CompareTo(clave1Desde) == 0 &&
                                    q.Clave1.CompareTo(clave1hasta) < 0 && q.Clave1.CompareTo(clave1hasta) == 0
                               )
                         select q;
            List<MedDAL.Inventarios.ErrorCambioPrecio> errores = new List<Inventarios.ErrorCambioPrecio>();
            foreach (DAL.productos producto in oQuery)
            {
                try{
                switch (listaPrecio)
                {
                    case 1: if (evaluaRequerimientosPrecios(producto.PrecioMinimo, producto.Precio1 - cant, producto.Precio2, producto.Precio3, producto.PrecioPublico))
                            producto.Precio1 -= cant;
                        break;
                    case 2: if (evaluaRequerimientosPrecios(producto.PrecioMinimo, producto.Precio1, producto.Precio2 - cant, producto.Precio3, producto.PrecioPublico))
                            producto.Precio2 -= cant;
                        break;
                    case 3: if (evaluaRequerimientosPrecios(producto.PrecioMinimo, producto.Precio1, producto.Precio2, producto.Precio3 - cant, producto.PrecioPublico))
                            producto.Precio3 -= cant;
                        break;
                    case 4: if (evaluaRequerimientosPrecios(producto.PrecioMinimo - cant, producto.Precio1, producto.Precio2, producto.Precio3, producto.PrecioPublico))
                            producto.PrecioMinimo -= cant;
                        break;
                    case 5: if (evaluaRequerimientosPrecios(producto.PrecioMinimo, producto.Precio1, producto.Precio2, producto.Precio3, producto.PrecioPublico - cant))
                            producto.PrecioPublico -= cant;
                        break;
                    default: throw new Exception("Error en el parámetro listaPrecio");
                }
                }
                catch (Exception ex)
                {
                    if (ex.Message.Equals("Violación a la jerarquía de precios"))
                        errores.Add(new Inventarios.ErrorCambioPrecio(producto.Clave1, "Violación a la jerarquía de precios"));
                }
            }

            oMedicuriEntities.SaveChanges();
            if (errores.Count > 0)
                return errores;
            else
                return null;
        }

        /// <summary>
        /// aumentar en un porcentaje los precios de TODOS los productos en la lista de precios señalada
        /// </summary>
        /// <param name="cant">cantidad a aumentar</param>
        /// <param name="listaPrecio">
        /// 1: Precio1
        /// 2: Precio2
        /// 3: Precio3
        /// 4: PrecioMinimo
        /// 5: PrecioPublico
        /// </param>
        public List<MedDAL.Inventarios.ErrorCambioPrecio> PreciosProductosAumentarPorcentaje(string clave1Desde, string clave1hasta, decimal cant, int listaPrecio)
        {
            var oQuery = from q in oMedicuriEntities.productos
                         where (
                                    q.Clave1.CompareTo(clave1Desde) > 0 && q.Clave1.CompareTo(clave1Desde) == 0 &&
                                    q.Clave1.CompareTo(clave1hasta) < 0 && q.Clave1.CompareTo(clave1hasta) == 0
                               )
                         select q;
            decimal pctje = 1 + cant / 100;
            List<MedDAL.Inventarios.ErrorCambioPrecio> errores = new List<Inventarios.ErrorCambioPrecio>();
            foreach (DAL.productos producto in oQuery)
            {
              try{  switch (listaPrecio)
                {
                    case 1: if (evaluaRequerimientosPrecios(producto.PrecioMinimo, producto.Precio1 * pctje, producto.Precio2, producto.Precio3, producto.PrecioPublico))
                            producto.Precio1 *= pctje;
                        break;
                    case 2: if (evaluaRequerimientosPrecios(producto.PrecioMinimo, producto.Precio1, producto.Precio2 * pctje, producto.Precio3, producto.PrecioPublico))
                            producto.Precio2 *= pctje;
                        break;
                    case 3: if (evaluaRequerimientosPrecios(producto.PrecioMinimo, producto.Precio1, producto.Precio2, producto.Precio3 * pctje, producto.PrecioPublico))
                            producto.Precio3 *= pctje;
                        break;
                    case 4: if (evaluaRequerimientosPrecios(producto.PrecioMinimo * pctje, producto.Precio1, producto.Precio2, producto.Precio3, producto.PrecioPublico))
                            producto.PrecioMinimo *= pctje;
                        break;
                    case 5: if (evaluaRequerimientosPrecios(producto.PrecioMinimo, producto.Precio1, producto.Precio2, producto.Precio3, producto.PrecioPublico * pctje))
                            producto.PrecioPublico *= pctje;
                        break;
                    default: throw new Exception("Error en el parámetro listaPrecio");
                }
              }
              catch (Exception ex)
              {
                  if (ex.Message.Equals("Violación a la jerarquía de precios"))
                      errores.Add(new Inventarios.ErrorCambioPrecio(producto.Clave1, "Violación a la jerarquía de precios"));
              }
            }

            oMedicuriEntities.SaveChanges();
            if (errores.Count > 0)
                return errores;
            else
                return null;
        }

        /// <summary>
        /// Disminuir en un porcentaje los precios de TODOS los productos en la lista de precios señalada
        /// </summary>
        /// <param name="cant">cantidad a Disminuir</param>
        /// <param name="listaPrecio">
        /// 1: Precio1
        /// 2: Precio2
        /// 3: Precio3
        /// 4: PrecioMinimo
        /// 5: PrecioPublico
        /// </param>
        public List<MedDAL.Inventarios.ErrorCambioPrecio> PreciosProductosDisminuirPorcentaje(string clave1Desde, string clave1hasta, decimal cant, int listaPrecio)
        {
            var oQuery = from q in oMedicuriEntities.productos
                         where (
                                    q.Clave1.CompareTo(clave1Desde) > 0 && q.Clave1.CompareTo(clave1Desde) == 0 &&
                                    q.Clave1.CompareTo(clave1hasta) < 0 && q.Clave1.CompareTo(clave1hasta) == 0
                               )
                         select q;
            decimal pctje = 1 - cant / 100;
            List<MedDAL.Inventarios.ErrorCambioPrecio> errores = new List<Inventarios.ErrorCambioPrecio>();
            foreach (DAL.productos producto in oQuery)
            {
              try{  switch (listaPrecio)
                {
                    case 1: if (evaluaRequerimientosPrecios(producto.PrecioMinimo, producto.Precio1 * pctje, producto.Precio2, producto.Precio3, producto.PrecioPublico))
                            producto.Precio1 *= pctje;
                        break;
                    case 2: if (evaluaRequerimientosPrecios(producto.PrecioMinimo, producto.Precio1, producto.Precio2 * pctje, producto.Precio3, producto.PrecioPublico))
                            producto.Precio2 *= pctje;
                        break;
                    case 3: if (evaluaRequerimientosPrecios(producto.PrecioMinimo, producto.Precio1, producto.Precio2, producto.Precio3 * pctje, producto.PrecioPublico))
                            producto.Precio3 *= pctje;
                        break;
                    case 4: if (evaluaRequerimientosPrecios(producto.PrecioMinimo * pctje, producto.Precio1, producto.Precio2, producto.Precio3, producto.PrecioPublico))
                            producto.PrecioMinimo *= pctje;
                        break;
                    case 5: if (evaluaRequerimientosPrecios(producto.PrecioMinimo, producto.Precio1, producto.Precio2, producto.Precio3, producto.PrecioPublico * pctje))
                            producto.PrecioPublico *= pctje;
                        break;
                    default: throw new Exception("Error en el parámetro listaPrecio");
                }
              }
              catch (Exception ex)
              {
                  if (ex.Message.Equals("Violación a la jerarquía de precios"))
                      errores.Add(new Inventarios.ErrorCambioPrecio(producto.Clave1, "Violación a la jerarquía de precios"));
              }
            }

            oMedicuriEntities.SaveChanges();
            if (errores.Count > 0)
                return errores;
            else
                return null;
        }
        
        #endregion



        public IQueryable<DAL.productos> ObtenerTodosProductos()
        {
            var oQuery = from q in oMedicuriEntities.productos
                         select q;
            return oQuery;
        }

        /// <summary>
        /// Busca todos los Productos que estén activos
        /// </summary>
        /// <returns>La coleccion de productos</returns>
        public IQueryable<DAL.productos> BuscarEnum()
        {
            var oQuery = from q in oMedicuriEntities.productos
                         where q.Activo == true
                         select q;

            return oQuery;
        }

        /// <summary>
        /// DAL - Muestra todos los registros de la tabla productos
        /// </summary>
        /// <returns>Objeto</returns>
        public object MostrarLista()
        {

            var oQuery = from q in oMedicuriEntities.productos select q;
            return oQuery;

        }

        /// <summary>
        /// DAL - Muestra todos los registros de la tabla productos
        /// </summary>
        /// <returns>Objeto</returns>
        public object MostrarListaFiltradaAlmacen(int iIdAlmacen)
        {

            var oQuery = from q in oMedicuriEntities.productos_almacen
                         where q.idAlmacen==iIdAlmacen
                         select new
                         {
                             q.idProducto,
                             q.productos.Clave1,
                             q.productos.Nombre,
                             q.productos.Presentacion,
                             q.productos.StockMinimo,
                             q.productos.StockMaximo,
                             q.productos.PrecioMinimo,
                             q.productos.PrecioPublico,
                             Tipo=q.productos.tipos.Nombre,
                             q.productos.Activo
                         };
            
                        return oQuery;

        }


        // GT: 17/Mar/2011 Función que recupera un producto mediante su nombre, requerida para llenar autocompletar
        //                 en Pedidos,Remisiones y Facturas
        /// <summary>
        /// Recuperar un producto mediante su nombre
        /// </summary>
        /// <param name="sNombre">Nombre a buscar</param>
        /// <returns></returns>
        public DAL.productos BuscarProductoNombre(string sNombre)
        {
            try
            {
                var oQuery = from q in oMedicuriEntities.productos
                             where q.Nombre == sNombre
                             select q;
                return oQuery.First<MedDAL.DAL.productos>();
            }
            catch
            {
                return null;
            }


        }


        // GT: 17/Mar/2011 Función que recupera un producto mediante su nombre, requerida para llenar autocompletar
        //                 en Pedidos,Remisiones y Facturas
        /// <summary>
        /// DAL - Buscar productos mediante el nombre
        /// </summary>
        /// <param name="sCadena">Parametro de busqueda</param>
        /// <returns></returns>
        public string[] BuscarProductoAsincrono(string sCadena)
        {
            string[] asResultados;

            try
            {
                var oQuery = from q in oMedicuriEntities.productos
                             where q.Nombre.Contains(sCadena)
                             select q.Nombre;

                return asResultados = oQuery.ToArray<string>();

            }
            catch
            {
                return asResultados = new string[0];
            }
        }

        /// <summary>
        /// DAL - Buscar productos mediante el nombre y ensambles mediante el nombre
        /// </summary>
        /// <param name="sCadena">Parametro de busqueda</param>
        /// <returns></returns>
        public string[] BuscarProductoEnsambleAsincrono(string sCadena)
        {
            
            try
            {
                var oQuery1 = from q in oMedicuriEntities.productos
                              where q.Nombre.Contains(sCadena)
                              select new { q.Nombre };

                var oQuery2 = from q in oMedicuriEntities.ensamble
                              where q.Descripcion.Contains(sCadena)
                              select new { q.Descripcion };



                int iContador = 0;
                string[] asResultados = new string[oQuery1.Count() + oQuery2.Count()];

                foreach (var registro in oQuery1)
                {
                    asResultados[iContador] = registro.Nombre.ToString();
                    iContador++;
                }

                foreach (var registro in oQuery2)
                {
                    asResultados[iContador] = registro.Descripcion.ToString();
                    iContador++;
                }


                return asResultados;

            }
            catch
            {
                string[] asResultados = new string[0];
                return asResultados;
            }
        }

        // GT: 17/Mar/2011 Función que recupera un producto mediante su clave, requerida para llenar autocompletar
        //                 en Pedidos,Remisiones y Facturas
        /// <summary>
        /// DAL - Buscar productos mediante el nombre
        /// </summary>
        /// <param name="sCadena">Parametro de busqueda</param>
        /// <returns></returns>
        public string[] BuscarProductoClave1Asincrono(string sCadena)
        {
            string[] asResultados;

            try
            {
                var oQuery = from q in oMedicuriEntities.productos
                             where q.Clave1.Contains(sCadena)
                             select q.Clave1;

                return asResultados = oQuery.ToArray<string>();

            }
            catch
            {
                return asResultados = new string[0];
            }
        }

        /// <summary>
        /// JID 21/09/2011 Funcion que recupera un producto mediante su clave 2
        /// </summary>
        /// <param name="sCadena"></param>
        /// <returns></returns>
        public string[] BuscarProductoClave2Asincrono(string sCadena)
        {
            string[] asResultados;

            try
            {
                var oQuery = from q in oMedicuriEntities.productos
                             where q.Clave2.Contains(sCadena)
                             select q.Clave2;

                return asResultados = oQuery.ToArray<string>();

            }
            catch
            {
                return asResultados = new string[0];
            }
        }

        // GT: 17/Mar/2011 Función que recupera un producto mediante su clave, requerida para llenar autocompletar
        //                 en Pedidos,Remisiones y Facturas
        /// <summary>
        /// DAL - Buscar productos mediante el nombre
        /// </summary>
        /// <param name="sCadena">Parametro de busqueda</param>
        /// <returns></returns>
        public string[] BuscarProductoEnsambleClaveAsincrono(string sCadena)
        {
            //string[] asResultados= new string[1];

            try
            {
                var oQuery1 = from q in oMedicuriEntities.productos
                             where q.Clave1.Contains(sCadena)
                             select new { q.Clave1 };

                var oQuery2 = from q in oMedicuriEntities.ensamble
                              where q.ClaveBom.Contains(sCadena)
                              select new { q.ClaveBom };


                int iContador = 0;
                string[] asResultados = new string[oQuery1.Count()+oQuery2.Count()];

                foreach (var registro in oQuery1)
                {
                    asResultados[iContador] = registro.Clave1.ToString();
                    iContador++;
                }
                                
                foreach (var registro in oQuery2)
                {
                    asResultados[iContador] = registro.ClaveBom.ToString();
                    iContador++;
                }

                
                return asResultados;

            }
            catch
            {
                string[] asResultados = new string[0];
                return asResultados;
            }
        }

        /// <summary>
        /// DAL - Metodo que regresa el id de un producto mediante su clave1
        /// </summary>
        /// <param name="sClave">Valor de la clave1</param>
        /// <returns></returns>
        public int RecuperarIdProducto(string sClave)
        {
            MedDAL.DAL.productos oProducto;

            try
            {
                var oQuery = from q in oMedicuriEntities.productos
                             where q.Clave1 == sClave
                             select q;
                oProducto = oQuery.First<MedDAL.DAL.productos>();

                return oProducto.idProducto;

            }
            catch
            {
                return 0;
            }

        }

        /// <summary>
        /// Eliminar un producto
        /// </summary>
        /// <param name="iIdProducto">Id producto a eliminar</param>
        /// <returns></returns>
        public bool EliminarRegistro(int iIdproducto)
        {
            try
            {
                var oQuery = from q in oMedicuriEntities.productos.
                              Where("it.idProducto=@idProducto",
                              new ObjectParameter("idProducto", iIdproducto))
                             select q;

                DAL.productos oProductoOriginal = oQuery.First<DAL.productos>();
                oMedicuriEntities.DeleteObject(oProductoOriginal);
                oMedicuriEntities.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Validar producto
        /// </summary>REVISAR SEVERAMENTE
        /// <param name="idProducto">Nombre de productos</param>
        /// <returns>Object</returns>
        public int ValidarProductoRepetido(string claveProducto)
        {
            try
            {

                //Recuperar el objeto a editar
                var oQuery = from q in oMedicuriEntities.productos.
                             Where("it.Clave1=@Clave1",
                             new ObjectParameter("Clave1", claveProducto))
                             select q;

                return oQuery.Count();

            }
            catch
            {
                return 0;
            }
        }

        public int ValidarProductoRepetido(string sClaveProducto, int IdProducto)
        {
            try
            {
                var oQuery = from q in oMedicuriEntities.productos
                             where q.Clave1 == sClaveProducto && q.idProducto != IdProducto
                             select q;
                return oQuery.Count();
            }
            catch
            {
                return 0;
            }
        }
        /// <summary>
        /// Acutualiza un producto en la DB
        /// </summary>
        /// <param name="oProducto"> Producto a actualizar</param>
        /// <returns></returns>
        public bool EditarRegistro(DAL.productos oProducto)
        {
            try
            {

                //Recuperar el objeto a editar
                var oQuery = from q in oMedicuriEntities.productos.
                             Where("it.idProducto=@idProducto",
                             new ObjectParameter("idProducto", oProducto.idProducto))
                             select q;


                DAL.productos oProductoOriginal = oQuery.First<DAL.productos>();

                //Datos de identificacion - Claves
                oProductoOriginal.Clave2 = oProducto.Clave2;
                oProductoOriginal.Clave3 = oProducto.Clave3;
                oProductoOriginal.Clave4 = oProducto.Clave4;
                oProductoOriginal.Nombre = oProducto.Nombre;
                oProductoOriginal.Descripcion = oProducto.Descripcion;
                oProductoOriginal.Activo = oProducto.Activo;

                //Datos de identificacion - Presentacion
                oProductoOriginal.Presentacion = oProducto.Presentacion;
                oProductoOriginal.UnidadMedida = oProducto.UnidadMedida;
                oProductoOriginal.DescripcionAdicional = oProducto.DescripcionAdicional;

                //Datos del proveedor - general
                //oProductoOriginal.proveedores_productos. = oProducto.Calle;


                //Campos opcionales
                oProductoOriginal.Campo1 = oProducto.Campo1;
                oProductoOriginal.Campo2 = oProducto.Campo2;
                oProductoOriginal.Campo3 = oProducto.Campo3;
                oProductoOriginal.Campo4 = oProducto.Campo4;
                oProductoOriginal.Campo5 = oProducto.Campo5;
                oProductoOriginal.Campo6 = oProducto.Campo6;
                oProductoOriginal.Campo7 = oProducto.Campo7;
                oProductoOriginal.Campo8 = oProducto.Campo8;
                oProductoOriginal.Campo9 = oProducto.Campo9;
                oProductoOriginal.Campo10 = oProducto.Campo10;

                //Precios
                oProductoOriginal.PrecioPublico = oProducto.PrecioPublico;
                oProductoOriginal.PrecioMinimo = oProducto.PrecioMinimo;
                oProductoOriginal.Precio1 = oProducto.Precio1;
                oProductoOriginal.Precio2 = oProducto.Precio2;
                oProductoOriginal.Precio3 = oProducto.Precio3;
                oProductoOriginal.TipoMoneda = oProducto.TipoMoneda;
                oProductoOriginal.idTipoIva = oProducto.idTipoIva;
                oProductoOriginal.TasaIeps = oProducto.TasaIeps;
                oProductoOriginal.Costeo = oProducto.Costeo;

                //Inventarios
                oProductoOriginal.ManejaLote = oProducto.ManejaLote;
                oProductoOriginal.ManejaSeries = oProducto.ManejaSeries;
                oProductoOriginal.StockMaximo = oProducto.StockMaximo;
                oProductoOriginal.StockMinimo = oProducto.StockMinimo;
                oProductoOriginal.DiasResurtido = oProducto.DiasResurtido;

                oMedicuriEntities.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Obtiene los estados que coincidan con la búsqueda y el filtro
        /// </summary>
        /// <param name="sCadena">La cadena por la cual buscar</param>
        /// <param name="iFiltro">El filtro a aplicar: 1=todo, 2=Clave, 3=Nombre</param>
        /// <returns></returns>
        public object Buscar(string sCadena, int iFiltro)
        {
            //GT 27-Abril
            //string sConsulta = "";
            switch (iFiltro)
            {
                case 1:
                    //GT 27-Abril
                    //var oQuery = sCadena != "" ? from q in oMedicuriEntities.productos where q.tipos.Nombre.Contains(sCadena) select q : from q in oMedicuriEntities.productos select q;
                    //return oQuery;
                    //break;
                    if (sCadena != "")
                    {
                        IQueryable<ProductoView> oQuery = from q in oMedicuriEntities.productos
                                     where q.tipos.Nombre.Contains(sCadena)
                                        select new ProductoView
                                        {
                                            idProducto = q.idProducto,
                                            Clave1 = q.Clave1,
                                            Clave2 = q.Clave2,
                                            Nombre = q.Nombre,
                                            Presentacion = q.Presentacion,
                                            StockMinimo = (int)q.StockMinimo,
                                            StockMaximo = (int)q.StockMaximo,
                                            PrecioMinimo = (decimal)q.PrecioMinimo,
                                            PrecioPublico = (decimal)q.PrecioPublico,
                                            TipoProducto = (string)q.tipos.Nombre,
                                            Activo = q.Activo
                                        };
                        return oQuery;
                    }
                    else
                    {
                        IQueryable<ProductoView> oQuery = from q in oMedicuriEntities.productos
                                        select new ProductoView
                                        {
                                            idProducto = q.idProducto,
                                            Clave1 = q.Clave1,
                                            Clave2 = q.Clave2,
                                            Nombre = q.Nombre,
                                            Presentacion = q.Presentacion,
                                            StockMinimo = q.StockMinimo == null? 0 : (int)q.StockMinimo,
                                            StockMaximo = q.StockMaximo == null? 0 : (int)q.StockMaximo,
                                            PrecioMinimo = (decimal)q.PrecioMinimo,
                                            PrecioPublico = (decimal)q.PrecioPublico,
                                            TipoProducto = (string)q.tipos.Nombre,
                                            Activo = q.Activo
                                        };
                        return oQuery;
                    }
                case 2:
                    //GT 27-Abril
                    //sConsulta = "it.Clave1 LIKE '%'+@Dato+'%'";
                    //oQuery = from q in oMedicuriEntities.productos.Where(sConsulta, new ObjectParameter("Dato", sCadena)) select q;
                    //return oQuery;
                    //break;
                    IQueryable<ProductoView> oQuery2 = from q in oMedicuriEntities.productos
                                     where q.Clave1.Contains(sCadena)
                                        select new ProductoView
                                        {
                                            idProducto = q.idProducto,
                                            Clave1 = q.Clave1,
                                            Clave2 = q.Clave2,
                                            Nombre = q.Nombre,
                                            Presentacion = q.Presentacion,
                                            StockMinimo = q.StockMinimo == null ? 0 : (int)q.StockMinimo,
                                            StockMaximo = q.StockMaximo == null ? 0 : (int)q.StockMaximo,
                                            PrecioMinimo = (decimal)q.PrecioMinimo,
                                            PrecioPublico = (decimal)q.PrecioPublico,
                                            TipoProducto = (string)q.tipos.Nombre,
                                            Activo = q.Activo
                                        };
                        return oQuery2;
                case 3:
                    //GT 27-Abril
                    //sConsulta = "it.Nombre LIKE '%'+@Dato+'%'";
                    //oQuery = from q in oMedicuriEntities.productos.Where(sConsulta, new ObjectParameter("Dato", sCadena)) select q;
                    //return oQuery;
                    //break;
                    IQueryable<ProductoView> oQuery3 = from q in oMedicuriEntities.productos
                                     where q.Nombre.Contains(sCadena)
                                  select new ProductoView
                                  {
                                      idProducto = q.idProducto,
                                      Clave1 = q.Clave1,
                                      Clave2 = q.Clave2,
                                      Nombre = q.Nombre,
                                      Presentacion = q.Presentacion,
                                      StockMinimo = q.StockMinimo == null ? 0 : (int)q.StockMinimo,
                                      StockMaximo = q.StockMaximo == null ? 0 : (int)q.StockMaximo,
                                      PrecioMinimo = (decimal)q.PrecioMinimo,
                                      PrecioPublico = (decimal)q.PrecioPublico,
                                      TipoProducto = (string)q.tipos.Nombre,
                                      Activo = q.Activo
                                  };
                        return oQuery3;
                case 4:
                        IQueryable<ProductoView> oQuery4 = from q in oMedicuriEntities.productos
                                where q.Clave2.Contains(sCadena)
                                select new ProductoView
                                {
                                    idProducto = q.idProducto,
                                    Clave1 = q.Clave1,
                                    Clave2 = q.Clave2,
                                    Nombre = q.Nombre,
                                    Presentacion = q.Presentacion,
                                    StockMinimo = q.StockMinimo == null ? 0 : (int)q.StockMinimo,
                                    StockMaximo = q.StockMaximo == null ? 0 : (int)q.StockMaximo,
                                    PrecioMinimo = (decimal)q.PrecioMinimo,
                                    PrecioPublico = (decimal)q.PrecioPublico,
                                    TipoProducto = (string)q.tipos.Nombre,
                                    Activo = q.Activo
                                };
                        return oQuery4;
                default:
                    return null;
            }            
        }

        /// <summary>
        /// Obtiene los productos filtrados por almacen
        /// </summary>
        /// <param name="sCadena">La cadena por la cual buscar</param>
        /// <param name="iFiltro">El filtro a aplicar: 1=todo, 2=Clave, 3=Nombre</param>
        /// <param name="iIdAlmacen">El almacen a buscar</param>
        /// <returns></returns>
        public object BuscarFiltradaAlmacen(string sCadena, int iFiltro,int iIdAlmacen)
        {
            //string sConsulta = "";
            switch (iFiltro)
            {
                case 1:
                    if (sCadena != "")
                    {
                        IQueryable<ProductoView> oQuery = from q in oMedicuriEntities.productos_almacen
                                     where q.idAlmacen == iIdAlmacen && q.productos.tipos.Nombre.Contains(sCadena)
                                     select new ProductoView {
                                         idProducto = q.idProducto,
                                         Clave1 = q.productos.Clave1,
                                         Clave2 = q.productos.Clave2,
                                         Nombre = q.productos.Nombre,
                                         Presentacion = q.productos.Presentacion,
                                         StockMinimo = q.productos.StockMinimo == null? 0 : (int)q.productos.StockMinimo,
                                         StockMaximo = q.productos.StockMaximo == null? 0 : (int)q.productos.StockMaximo,
                                         PrecioMinimo = (decimal)q.productos.PrecioMinimo,
                                         PrecioPublico = (decimal)q.productos.PrecioPublico,
                                         TipoProducto = (string)q.productos.tipos.Nombre,
                                         Activo = q.productos.Activo
                                     };
                        
                        return oQuery;
                    }
                    else
                    {
                        IQueryable<ProductoView> oQuery = from q in oMedicuriEntities.productos_almacen
                                     where q.idAlmacen == iIdAlmacen
                                        select new ProductoView
                                        {
                                            idProducto = q.idProducto,
                                            Clave1 = q.productos.Clave1,
                                            Clave2 = q.productos.Clave2,
                                            Nombre = q.productos.Nombre,
                                            Presentacion = q.productos.Presentacion,
                                            StockMinimo = q.productos.StockMinimo == null ? 0 : (int)q.productos.StockMinimo,
                                            StockMaximo = q.productos.StockMaximo == null ? 0 : (int)q.productos.StockMaximo,
                                            PrecioMinimo = (decimal)q.productos.PrecioMinimo,
                                            PrecioPublico = (decimal)q.productos.PrecioPublico,
                                            TipoProducto = (string)q.productos.tipos.Nombre,
                                            Activo = q.productos.Activo
                                        };
                        return oQuery;
                    }
                        
                    
                //break;
                case 2:
                    IQueryable<ProductoView> oQuery2 = from q in oMedicuriEntities.productos_almacen
                                     where q.idAlmacen == iIdAlmacen && q.productos.Clave1.Contains(sCadena)
                                        select new ProductoView
                                        {
                                            idProducto = q.idProducto,
                                            Clave1 = q.productos.Clave1,
                                            Clave2 = q.productos.Clave2,
                                            Nombre = q.productos.Nombre,
                                            Presentacion = q.productos.Presentacion,
                                            StockMinimo = q.productos.StockMinimo == null ? 0 : (int)q.productos.StockMinimo,
                                            StockMaximo = q.productos.StockMaximo == null ? 0 : (int)q.productos.StockMaximo,
                                            PrecioMinimo = (decimal)q.productos.PrecioMinimo,
                                            PrecioPublico = (decimal)q.productos.PrecioPublico,
                                            TipoProducto = (string)q.productos.tipos.Nombre,
                                            Activo = q.productos.Activo
                                        };
                        return oQuery2;
                //break;
                case 3:
                    IQueryable<ProductoView> oQuery3 = from q in oMedicuriEntities.productos_almacen
                                     where q.idAlmacen == iIdAlmacen && q.productos.Nombre.Contains(sCadena)
                                        select new ProductoView
                                        {
                                            idProducto = q.idProducto,
                                            Clave1 = q.productos.Clave1,
                                            Clave2 = q.productos.Clave2,
                                            Nombre = q.productos.Nombre,
                                            Presentacion = q.productos.Presentacion,
                                            StockMinimo = q.productos.StockMinimo == null ? 0 : (int)q.productos.StockMinimo,
                                            StockMaximo = q.productos.StockMaximo == null ? 0 : (int)q.productos.StockMaximo,
                                            PrecioMinimo = (decimal)q.productos.PrecioMinimo,
                                            PrecioPublico = (decimal)q.productos.PrecioPublico,
                                            TipoProducto = (string)q.productos.tipos.Nombre,
                                            Activo = q.productos.Activo
                                        };
                        return oQuery3;
                //break;
                case 4: 
                    IQueryable<ProductoView> oQuery4 = from q in oMedicuriEntities.productos_almacen
                                     where q.idAlmacen == iIdAlmacen && q.productos.Clave2.Contains(sCadena)
                                        select new ProductoView
                                        {
                                            idProducto = q.idProducto,
                                            Clave1 = q.productos.Clave1,
                                            Clave2 = q.productos.Clave2,
                                            Nombre = q.productos.Nombre,
                                            Presentacion = q.productos.Presentacion,
                                            StockMinimo = q.productos.StockMinimo == null ? 0 : (int)q.productos.StockMinimo,
                                            StockMaximo = q.productos.StockMaximo == null ? 0 : (int)q.productos.StockMaximo,
                                            PrecioMinimo = (decimal)q.productos.PrecioMinimo,
                                            PrecioPublico = (decimal)q.productos.PrecioPublico,
                                            TipoProducto = (string)q.productos.tipos.Nombre,
                                            Activo = q.productos.Activo
                                        };
                        return oQuery4;
                default:
                    return null;
            }
        }

        public decimal RecuperarPrecioPublico(int iIdProducto)
        {
            var oQuery = from q in oMedicuriEntities.productos
                         where q.idProducto == iIdProducto
                         select q.PrecioPublico;

                return Convert.ToDecimal(oQuery);
        }

        public decimal RecuperarPrecio1(int iIdProducto)
        {
            var oQuery = from q in oMedicuriEntities.productos
                         where q.idProducto == iIdProducto
                         select q.Precio1;

            return Convert.ToDecimal(oQuery);
        }

        public decimal RecuperarPrecio2(int iIdProducto)
        {
            var oQuery = from q in oMedicuriEntities.productos
                         where q.idProducto == iIdProducto
                         select q.Precio2;

            return Convert.ToDecimal(oQuery);
        }

        public decimal RecuperarPrecio3(int iIdProducto)
        {
            var oQuery = from q in oMedicuriEntities.productos
                         where q.idProducto == iIdProducto
                         select q.Precio3;

            return Convert.ToDecimal(oQuery);
        }

        public decimal RecuperarPrecioMinimo(int iIdProducto)
        {
            var oQuery = from q in oMedicuriEntities.productos
                         where q.idProducto == iIdProducto
                         select q.PrecioMinimo;

            return Convert.ToDecimal(oQuery);
        }

    }
}