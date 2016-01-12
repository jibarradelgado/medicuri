using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MedDAL.DAL;


namespace MedNeg.Inventarios
{
    public class BlInventarios
    {
        MedDAL.Inventarios.DALInventarios dalInventarios;
        MedDAL.Almacenes.DALAlmacenes dalAlmacenes;
        MedDAL.Proveedores.DALProveedores dalProveedores;
        MedDAL.LineasCredito.DALLineasCredito dalLineasCredito;
        MedDAL.Productos.DALProductos dalProductos;
        Bitacora.BlBitacora blBitacora;

        public BlInventarios()
        {
            dalInventarios = new MedDAL.Inventarios.DALInventarios();
            dalAlmacenes = new MedDAL.Almacenes.DALAlmacenes();
            dalProveedores = new MedDAL.Proveedores.DALProveedores();
            dalLineasCredito = new MedDAL.LineasCredito.DALLineasCredito();
            dalProductos = new MedDAL.Productos.DALProductos();
            blBitacora = new Bitacora.BlBitacora();
        }

        public bool EditarRegistro(MedDAL.DAL.inventario oInventario)
        {
            return dalInventarios.EditarRegistro(oInventario);
        }

        public IQueryable<MedDAL.Inventarios.MovimientosView> Buscar(string sCadena, int iFiltro)
        {
            return dalInventarios.Buscar(sCadena, iFiltro);
        }

        public IQueryable<MedDAL.Inventarios.MovimientosView> Buscar(string sCadena, int iFiltro, int idAlmacen)
        {
            return dalInventarios.Buscar(sCadena, iFiltro, idAlmacen);
        }

        public MedDAL.DAL.inventario BuscarPorId(int idInventario)
        {
            return dalInventarios.BuscarPorId(idInventario);
        }

        public IQueryable<MedDAL.DAL.tipo_movientos> ObtenerTiposMovimientos()
        {
            return dalInventarios.ObtenerTiposMovimientos();
        }

        public IQueryable<MedDAL.DAL.almacenes> ObtenerAlmacenes()
        {            
            return dalAlmacenes.BuscarAlmacenesActivos();
        }

        public IQueryable<MedDAL.DAL.proveedores> ObtenerProveedores()
        {
            return dalProveedores.ObtenerProveedoresActivos();
        }

        public IQueryable<MedDAL.DAL.lineas_creditos> ObtenerLineasDeCredito()
        {
            return dalLineasCredito.MostrarListaNoVencidas();
        }

        public MedDAL.DAL.productos buscarProducto(String clave1)
        {
            return dalProductos.buscarProducto(clave1);
        }

        public MedDAL.DAL.productos buscarProductoPorNombre(String sNombre)
        {
            return dalProductos.BuscarProductoNombre(sNombre);
        }

        public IQueryable<String> buscarLotesProducto(int idProducto,int idAlmacen)
        {
            return dalProductos.buscarLotesProducto(idProducto,idAlmacen);
        }

        public IQueryable<String> buscarSeriesProducto(int idProducto, int idAlmacen)
        {
            return dalProductos.buscarSeriesProducto(idProducto, idAlmacen);
        }

        public void GestionarExistenciaProducto(int idAlmacen, int idProducto, String lote, String serie,DateTime fechaCaducidad, decimal cantidad, string modo,int idLineaCredito)
        {
            int iModo = modo.Equals("Entrada") ? 0 : 1;
            try
            {
                //ya existe la existencia el producto con su respectiva serie y/o lote
                dalProductos.ModificarExistenciaProducto(idAlmacen, idProducto, lote, serie, fechaCaducidad, cantidad, iModo);
            }
            catch (Exception ex)
            {
                //NO existe la existencia el producto con su respectiva serie y/o lote, solo si es entrada se agrega, si es salida pues no...
                if (iModo == 0)
                    dalProductos.AgregarExistenciaProducto(idAlmacen, idProducto, lote, serie, fechaCaducidad, cantidad,idLineaCredito);
                else
                    throw ex;
            }
        }

        public void GestionarCancelacion(int idAlmacen, int idProducto, string sLote, string sNoSerie, decimal dCantidad, string sModo)
        {
            int iModo = sModo.Equals("Entrada Cancelada") ? 1 : 0;
            dalProductos.ModificarExistenciaProducto(idAlmacen, idProducto, sLote, sNoSerie, dCantidad, iModo);
        }

        public void EstablecerExistenciaProducto(int idAlmacen, int idProducto, String lote, String serie, decimal cantidad)
        {
            dalProductos.EstablecerExistenciaProducto(idAlmacen, idProducto, lote, serie, cantidad);
        }

        public void BloquearProducto(int idAlmacen, int idProducto, bool bloquear) 
        {
            dalProductos.BloquearProducto(idAlmacen, idProducto, bloquear);
        }

        public void BloquearProducto(int idAlmacen, string clave1Desde, string clave1hasta, bool bloquear) 
        {
            dalProductos.BloquearProducto(idAlmacen, clave1Desde, clave1hasta, bloquear);
        }

        public IQueryable<MedDAL.DAL.productos> buscarTodosProductos()
        {
            return dalProductos.ObtenerTodosProductos();
        }

        public List<MedDAL.Inventarios.ErrorCambioPrecio> FinalizaCambioPrecios(string listaPrecios, string operacion, string tipo, decimal cantidad)
        {
        
        if(operacion.Contains("Aumento"))
        {
           if(tipo.Contains("Monto"))
           {
               return dalProductos.PreciosProductosAumentarMonto(cantidad, listaPrecios2int(listaPrecios));
           }
           else if(tipo.Contains("Porcentaje"))
           {
               return dalProductos.PreciosProductosAumentarPorcentaje(cantidad, listaPrecios2int(listaPrecios));
           }
        }
        else if(operacion.Contains("Disminuci"))
                {
                    if(tipo.Contains("Monto"))
                    {
                        return dalProductos.PreciosProductosDisminuirMonto(cantidad, listaPrecios2int(listaPrecios));
                    }
                    if (tipo.Contains("Porcentaje"))
                    {
                        return dalProductos.PreciosProductosDisminuirPorcentaje(cantidad, listaPrecios2int(listaPrecios));
                    }
                }
        throw new Exception("BlInventarios.FinalizaCambioPrecios - Error en los parámetros");
        }

        public List<MedDAL.Inventarios.ErrorCambioPrecio> FinalizaCambioPrecios(string clave1Desde, string clave1Hasta, string listaPrecios, string operacion, string tipo, decimal cantidad)
        {
            if (operacion.Contains("Aumento"))
            {
                if (tipo.Contains("Monto"))
                {
                   return dalProductos.PreciosProductosAumentarMonto(clave1Desde, clave1Hasta, cantidad, listaPrecios2int(listaPrecios));
                }
                else if (tipo.Contains("Porcentaje"))
                {
                    return dalProductos.PreciosProductosAumentarPorcentaje(clave1Desde, clave1Hasta, cantidad, listaPrecios2int(listaPrecios));
                }
            }
            else if (operacion.Contains("Disminuci"))
            {
                if (tipo.Contains("Monto"))
                {
                    return dalProductos.PreciosProductosDisminuirMonto(clave1Desde, clave1Hasta, cantidad, listaPrecios2int(listaPrecios));
                }
                if (tipo.Contains("Porcentaje"))
                {
                    return dalProductos.PreciosProductosDisminuirPorcentaje(clave1Desde, clave1Hasta, cantidad, listaPrecios2int(listaPrecios));
                }
            }
            throw new Exception("BlInventarios.FinalizaCambioPrecios - Error en los parámetros");
        }

        public void agregarRegistroInventario(MedDAL.DAL.inventario inventario, List<MedDAL.DAL.inventario_partida> partida)
        {
            dalInventarios.agregarRegistroInventario(inventario, partida);
        }
        
        private int listaPrecios2int(string listaprecios)
        {
            switch (listaprecios)
            {
                case "Precio público": return 5;
                case "Precio 1": return 1;
                case "Precio 2": return 2;
                case "Precio 3": return 3;
                case "Precio mínimo": return 4;
                default: throw new Exception("Error en el parámetro listaPrecio");
            }
        }


        //public string buscarCostoProducto(int idAlmacen, int idProducto)
        //{
        //    try {
        //        return dalProductos.buscarCostoProducto(idAlmacen, idProducto).ToString();
        //    }catch(Exception ex)
        //    {
        //        return string.Empty;
        //    }
        //}

        public void DescontarCreditoALinea(int idLineaCredito, decimal cantidad)
        {
            dalLineasCredito.DescontarCreditoALinea(idLineaCredito, cantidad);
        }

        public void AumentarCreditoALinea(int idLineaCredito, decimal cantidad)
        {
            dalLineasCredito.AumentarCreditoALinea(idLineaCredito, cantidad);
        }

        public decimal BuscarCantCreditoLineaCredito(int idLineaCredito)
        {
            return dalLineasCredito.BuscarCantidadCreditoLineaDeCredito(idLineaCredito);
        }

        public DateTime BuscarFechaCaducidad(int idProducto, int idAlmacen, string lote)
        {
            return dalInventarios.BuscarFechaCaducidad(idProducto, idAlmacen, lote);
        }

        public Producto ObtenerExistenciaTeorica(int idAlmacen, int idProducto,String lote,String serie)
        {
            MedDAL.DAL.productos dbProd = dalProductos.buscarProducto(idProducto);
            return  new Producto(
                                            dbProd,
                                            dbProd.Clave1,
                                            dbProd.Nombre,
                                            lote,
                                            serie,
                                            dalProductos.obtenerExistenciaProducto(idAlmacen, idProducto, lote, serie)
                                            );
        }



        public List<Producto> ObtenerExistenciaTeorica(int idAlmacen, string clave1Desde, string clave1Hasta)
        {
            IQueryable<MedDAL.DAL.productos> iProductos = dalProductos.buscarProducto(clave1Desde, clave1Hasta);
            List<MedDAL.DAL.productos> lstProductos = new List<productos>();
            lstProductos.AddRange(iProductos);
            List<Producto> rProductos= new List<Producto>();
            foreach (MedDAL.DAL.productos producto in lstProductos)
            {
                IQueryable<MedDAL.DAL.productos_almacen> p_a = dalProductos.ObtenerExistenciaProducto(producto.idProducto, idAlmacen);
                foreach (MedDAL.DAL.productos_almacen pa in p_a)
                {
                    rProductos.Add(
                                    new Producto(
                                                    producto,
                                                    producto.Clave1,
                                                    producto.Nombre,
                                                    pa.Lote,
                                                    pa.NoSerie,
                                                    pa.Cantidad
                                                 )
                                    );
                }

            }
            return rProductos;
        }

        public List<MedNeg.Inventarios.Producto> ObtenerExistenciaTeorica(int idAlmacen)
        {
            IQueryable<MedDAL.DAL.productos> iProductos = dalProductos.ObtenerTodosProductos();
            List<MedDAL.DAL.productos> lstProductos = new List<productos>();
            lstProductos.AddRange(iProductos);
            List<MedNeg.Inventarios.Producto> rProductos = new List<MedNeg.Inventarios.Producto>();
            foreach (MedDAL.DAL.productos oProducto in lstProductos)
            {
                IQueryable<MedDAL.DAL.productos_almacen> p_a = dalProductos.ObtenerExistenciaProducto(oProducto.idProducto, idAlmacen);
                foreach (MedDAL.DAL.productos_almacen pa in p_a)
                {
                    rProductos.Add(
                                    new MedNeg.Inventarios.Producto(
                                                    oProducto,
                                                    oProducto.Clave1,
                                                    oProducto.Nombre,
                                                    pa.Lote,
                                                    pa.NoSerie,
                                                    pa.Cantidad
                                                 )
                                    );
                }

            }
            return rProductos;
        }



        public void NuevoRegistroBitacora(MedDAL.DAL.bitacora bitacora)
        {
            blBitacora.NuevoRegistro(bitacora);
        }

        public void EstablecerExistenciaProducto(List<Producto> lProductos, int idAlmacen)
        {
            foreach (Producto p in lProductos)
            {
                dalProductos.EstablecerExistenciaProducto(idAlmacen, p.dbProducto.idProducto, p.lote, p.serie, p.existenciaReal);
            }
        }

        //Codigo Chorcho
        public IQueryable<MedDAL.Inventarios.InventariosView> BuscarProductosAlmacen(string sCadena, int iFiltro) 
        {
            return dalInventarios.BuscarProductosAlmacen(sCadena, iFiltro);
        }

        public bool EditarProductosAlmacenStock(MedDAL.DAL.productos_almacen_stocks oProductosAlmacenStock) 
        {
            return dalInventarios.EditarProductosAlmacenStock(oProductosAlmacenStock);
        }

        public bool BuscarAlmacenProductoStocks(int idAlmacen, int idProducto) 
        {
            return dalInventarios.BuscarAlmacenProductoStocks(idAlmacen, idProducto);
        }

        public bool NuevoProductosAlmacenStock(MedDAL.DAL.productos_almacen_stocks oProductosAlmacenStock)
        {
            return dalInventarios.NuevoProductosAlmacenStock(oProductosAlmacenStock);
        }

        /// <summary>
       /// Metodo que va a mostrar solo las existencias de un almacen en especifico
       /// </summary>
       /// <param name="sCadena">Valor a buscar</param>
       /// <param name="iFiltro">PArametro</param>
       /// <param name="iIdAlmacen">ID almacen a buscar</param>
       /// <returns></returns>
        public IQueryable<MedDAL.Inventarios.InventariosView> BuscarProductosAlmacenFiltradaAlmacen(string sCadena, int iFiltro, int iIdAlmacen)
        {
            return dalInventarios.BuscarProductosAlmacenFiltradaAlmacen(sCadena, iFiltro, iIdAlmacen);
        }

        /// <summary>
        /// JID 21/09/2011 Buscar un almacen filtrado
        /// </summary>
        /// <param name="iIdAlmacen"></param>
        /// <returns></returns>
        public IQueryable<MedDAL.DAL.almacenes> BuscarAlmacenesFiltrado(int iIdAlmacen)
        {
            return dalAlmacenes.BuscarAlmacenesActivosFiltrado(iIdAlmacen);
        }

        public bool BuscarProductosBajoStock() 
        {
            return dalInventarios.BuscarProductosBajoStock();
        }

        public bool BuscarProductosBajoStock(int iIdAlmacen) 
        {
            return dalInventarios.BuscarProductosBajoStock(iIdAlmacen);    
        }

        public bool BuscarProductosCaducos() 
        {
            return dalInventarios.BuscarProductosCaducos();
        }

        public bool BuscarProductosCaducos(int iIdAlmacen)
        {
            return dalInventarios.BuscarProductosCaducos(iIdAlmacen);
        }

        public MedDAL.DAL.inventario ObtenerUltimoMovimiento()
        {
            return dalInventarios.ObtenerUltimoMovimiento();
        }
    }
}
