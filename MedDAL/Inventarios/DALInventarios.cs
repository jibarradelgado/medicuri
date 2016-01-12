using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Objects;
using System.Data.Objects.DataClasses;

namespace MedDAL.Inventarios
{
    public class DALInventarios
    {
        DAL.medicuriEntities oMedicuriEntities= new DAL.medicuriEntities();

        public IQueryable<DAL.tipo_movientos> ObtenerTiposMovimientos()
        {
            var oQuery = from q in oMedicuriEntities.tipo_movientos
                         
                         select q;
            return oQuery;
        }

        public IQueryable<MovimientosView> Buscar(string sCadena, int iFiltro)
        {
            string sConsulta = "";
            switch (iFiltro)
            {
                case 1:
                    sConsulta = "(it.Concepto LIKE '%'+@Dato+'%')";
                    break;
                case 2:
                    sConsulta = "it.Pedimento LIKE '%'+@Dato+'%'";
                    break;
                /*case 3:
                    sConsulta = "it.Nombre LIKE '%'+@Dato+'%'";
                    break;*/
            }

            IQueryable<MovimientosView> oQuery = from q in oMedicuriEntities.inventario.
                                      Where(sConsulta,
                                      new ObjectParameter("Dato", sCadena)) where !q.Concepto.Equals("Entrada Cancelada") && !q.Concepto.Equals("Salida Cancelada") 
                         select new MovimientosView
                            {
                                idInventarioMov = q.idInventarioMov,
                                Concepto = q.Concepto,
                                Fecha = q.Fecha,
                                Observaciones = q.Observaciones,
                                Pedimento = q.Pedimento
                            };

            return oQuery;
        }

        public IQueryable<MovimientosView> Buscar(string sCadena, int iFiltro, int idAlmacen)
        {
            string sConsulta = "";
            switch (iFiltro)
            {
                case 1:
                    sConsulta = "(it.Concepto LIKE '%'+@Dato+'%')";
                    break;
                case 2:
                    sConsulta = "it.Pedimento LIKE '%'+@Dato+'%'";
                    break;
                /*case 3:
                    sConsulta = "it.Nombre LIKE '%'+@Dato+'%'";
                    break;*/
            }

            IQueryable<MovimientosView> oQuery = from q in oMedicuriEntities.inventario.
                                      Where(sConsulta,
                                      new ObjectParameter("Dato", sCadena))
                                                 where !q.Concepto.Equals("Entrada Cancelada") && !q.Concepto.Equals("Salida Cancelada") && q.almacenes.idAlmacen == idAlmacen
                                                 select new MovimientosView
                                                 {
                                                     idInventarioMov = q.idInventarioMov,
                                                     Concepto = q.Concepto,
                                                     Fecha = q.Fecha,
                                                     Observaciones = q.Observaciones,
                                                     Pedimento = q.Pedimento
                                                 };

            return oQuery;
        }

        public DAL.inventario BuscarPorId(int idInventario)
        {
            return (from q in oMedicuriEntities.inventario where q.idInventarioMov == idInventario select q).First();
        }

        public void agregarRegistroInventario(DAL.inventario inventario, List<DAL.inventario_partida> partida)
        {
            oMedicuriEntities.AddToinventario(inventario);
            oMedicuriEntities.SaveChanges();

            foreach (DAL.inventario_partida p in partida)
            {
                p.idInventarioMov= inventario.idInventarioMov;
                oMedicuriEntities.AddToinventario_partida(p);
            }
         
            oMedicuriEntities.SaveChanges();
        }

        /// <summary>
        /// Edita un registro de movimientos
        /// </summary>
        /// <param name="oCliente"> Cliente a actualizar</param>
        /// <returns></returns>
        public bool EditarRegistro(DAL.inventario oInventario)
        {
            try
            {
                //Recuperar el objeto a editar
                var oQuery = from q in oMedicuriEntities.inventario.
                             Where("it.idInventarioMov=@idInventarioMov",
                             new ObjectParameter("idInventarioMov", oInventario.idInventarioMov))
                             select q;


                DAL.inventario oInventarioOriginal = oQuery.First<DAL.inventario>();

                oInventarioOriginal.Concepto = oInventario.Concepto;

                oMedicuriEntities.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }

        //Codigo Chorcho
        public IQueryable<InventariosView> BuscarProductosAlmacen(string sCadena, int iFiltro)
        {
            if (sCadena != "")
            {
                switch (iFiltro)
                {
                    case 1:
                        IQueryable<InventariosView> oQuery = (from pas in oMedicuriEntities.productos_almacen_stocks
                                      where pas.productos.Nombre.Contains(sCadena) ||
                                            pas.almacenes.Nombre.Contains(sCadena)
                                                              select new InventariosView
                                      {
                                          Almacen = pas.almacenes.Nombre,
                                          Clave = pas.productos.Clave1,
                                          Producto = pas.productos.Nombre,
                                          StockMin = (int)pas.StockMin,
                                          StockMax = (int)pas.StockMax,
                                          idProAlmStocks = pas.idProAlmStocks,
                                          Cantidad = ((from pa in oMedicuriEntities.productos_almacen
                                                       where pa.idProducto == pas.idProducto
                                                             && pa.idAlmacen == pas.idAlmacen

                                                       select pa.Cantidad).Sum()) == null ? 0
                                                      :
                                                      (from pa in oMedicuriEntities.productos_almacen
                                                       where pa.idProducto == pas.idProducto
                                                             && pa.idAlmacen == pas.idAlmacen

                                                       select pa.Cantidad).Sum()                                    
                                      });
                        return oQuery;
                    case 2:
                        oQuery = (from pas in oMedicuriEntities.productos_almacen_stocks
                                  where pas.productos.Nombre.Contains(sCadena)
                                  select new InventariosView
                                  {
                                      Almacen = pas.almacenes.Nombre,
                                      Clave = pas.productos.Clave1,
                                      Producto = pas.productos.Nombre,
                                      StockMin = (int)pas.StockMin,
                                      StockMax = (int)pas.StockMax,
                                      idProAlmStocks = pas.idProAlmStocks,
                                      Cantidad = ((from pa in oMedicuriEntities.productos_almacen
                                                   where pa.idProducto == pas.idProducto
                                                         && pa.idAlmacen == pas.idAlmacen

                                                   select pa.Cantidad).Sum()) == null ? 0
                                                  :
                                                  (from pa in oMedicuriEntities.productos_almacen
                                                   where pa.idProducto == pas.idProducto
                                                         && pa.idAlmacen == pas.idAlmacen

                                                   select pa.Cantidad).Sum()                                    
                                  });
                        return oQuery;
                    case 3:
                        oQuery = (from pas in oMedicuriEntities.productos_almacen_stocks
                                  where pas.almacenes.Nombre.Contains(sCadena)
                                  select new InventariosView
                                  {
                                      Almacen = pas.almacenes.Nombre,
                                      Clave = pas.productos.Clave1,
                                      Producto = pas.productos.Nombre,
                                      StockMin = (int)pas.StockMin,
                                      StockMax = (int)pas.StockMax,
                                      idProAlmStocks = pas.idProAlmStocks,
                                      Cantidad = ((from pa in oMedicuriEntities.productos_almacen
                                                   where pa.idProducto == pas.idProducto
                                                         && pa.idAlmacen == pas.idAlmacen

                                                   select pa.Cantidad).Sum()) == null ? 0
                                                  :
                                                  (from pa in oMedicuriEntities.productos_almacen
                                                   where pa.idProducto == pas.idProducto
                                                         && pa.idAlmacen == pas.idAlmacen

                                                   select pa.Cantidad).Sum()                                    
                                  });
                        return oQuery;
                }
                return null;
            }
            else
            {
                IQueryable<InventariosView> oQuery = (from pas in oMedicuriEntities.productos_almacen_stocks
                                                      select new InventariosView
                              {
                                  Almacen = pas.almacenes.Nombre,
                                  Clave = pas.productos.Clave1,
                                  Producto = pas.productos.Nombre,
                                  StockMin = (int)pas.StockMin,
                                  StockMax = (int)pas.StockMax,
                                  idProAlmStocks = pas.idProAlmStocks,
                                  Cantidad = ((from pa in oMedicuriEntities.productos_almacen
                                               where pa.idProducto == pas.idProducto
                                                     && pa.idAlmacen == pas.idAlmacen

                                               select pa.Cantidad).Sum()) == null ? 0
                                              :
                                              (from pa in oMedicuriEntities.productos_almacen
                                               where pa.idProducto == pas.idProducto
                                                     && pa.idAlmacen == pas.idAlmacen

                                               select pa.Cantidad).Sum()                                    
                              });
                return oQuery;
            }
        }

        public bool BuscarProductosBajoStock() 
        {            
            var oQuery = from q in oMedicuriEntities.productos_almacen
                            from q2 in oMedicuriEntities.productos_almacen_stocks
                            where q.Cantidad <= q2.StockMin
                            select q;

            if (oQuery.Count<MedDAL.DAL.productos_almacen>() == 0)
            {
                return false;
            }
            else
            {
                return true;
            }         
        }

        public bool BuscarProductosBajoStock(int iIdAlmacen)
        {
            var oQuery = from q in oMedicuriEntities.productos_almacen
                         from q2 in oMedicuriEntities.productos_almacen_stocks
                         where q.Cantidad <= q2.StockMin && q.idAlmacen == iIdAlmacen
                         select q;

            if (oQuery.Count<MedDAL.DAL.productos_almacen>() == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public bool BuscarProductosCaducos()
        {
            var oQuery = from q in oMedicuriEntities.productos_almacen
                         where q.FechaCaducidad <= DateTime.Today
                         select q;
            if (oQuery.Count<MedDAL.DAL.productos_almacen>() == 0)
            {
                return false;
            }
            else
            {
                return true;
            } 
        }

        public bool BuscarProductosCaducos(int iIdAlmacen)
        {
            var oQuery = from q in oMedicuriEntities.productos_almacen
                         where q.FechaCaducidad <= DateTime.Today && q.idAlmacen == iIdAlmacen
                         select q;
            if (oQuery.Count<MedDAL.DAL.productos_almacen>() == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

       /// <summary>
       /// Metodo que va a mostrar solo las existencias de un almacen en especifico
       /// </summary>
       /// <param name="sCadena">Valor a buscar</param>
       /// <param name="iFiltro">PArametro</param>
       /// <param name="iIdAlmacen">ID almacen a buscar</param>
       /// <returns></returns>
        public IQueryable<InventariosView> BuscarProductosAlmacenFiltradaAlmacen(string sCadena, int iFiltro, int iIdAlmacen)
        {
            if (sCadena != "")
            {
                switch (iFiltro)
                {
                    case 1:
                        IQueryable<InventariosView> oQuery = (from pas in oMedicuriEntities.productos_almacen_stocks
                                      where (pas.productos.Nombre.Contains(sCadena) ||
                                            pas.almacenes.Nombre.Contains(sCadena)) && pas.idAlmacen==iIdAlmacen
                                                              select new InventariosView
                                      {
                                          Almacen = pas.almacenes.Nombre,
                                          Clave = pas.productos.Clave1,
                                          Producto = pas.productos.Nombre,
                                          StockMin = (int)pas.StockMin,
                                          StockMax = (int)pas.StockMax,
                                          idProAlmStocks = pas.idProAlmStocks,
                                          Cantidad = ((from pa in oMedicuriEntities.productos_almacen
                                                      where pa.idProducto == pas.idProducto
                                                            && pa.idAlmacen == pas.idAlmacen
                                                           
                                                      select pa.Cantidad).Sum()) == null? 0 
                                                      :
                                                      (from pa in oMedicuriEntities.productos_almacen
                                                       where pa.idProducto == pas.idProducto
                                                             && pa.idAlmacen == pas.idAlmacen

                                                       select pa.Cantidad).Sum()
                                      });
                        return oQuery;
                    case 2:
                        oQuery = (from pas in oMedicuriEntities.productos_almacen_stocks
                                  where pas.productos.Nombre.Contains(sCadena) && pas.idAlmacen == iIdAlmacen
                                  select new InventariosView
                                  {
                                      Almacen = pas.almacenes.Nombre,
                                      Clave = pas.productos.Clave1,
                                      Producto = pas.productos.Nombre,
                                      StockMin = (int)pas.StockMin,
                                      StockMax = (int)pas.StockMax,
                                      idProAlmStocks = pas.idProAlmStocks,
                                      Cantidad = ((from pa in oMedicuriEntities.productos_almacen
                                                   where pa.idProducto == pas.idProducto
                                                         && pa.idAlmacen == pas.idAlmacen

                                                   select pa.Cantidad).Sum()) == null ? 0
                                                  :
                                                  (from pa in oMedicuriEntities.productos_almacen
                                                   where pa.idProducto == pas.idProducto
                                                         && pa.idAlmacen == pas.idAlmacen

                                                   select pa.Cantidad).Sum()
                                  });
                        return oQuery;
                    case 3:
                        oQuery = (from pas in oMedicuriEntities.productos_almacen_stocks
                                  where pas.almacenes.Nombre.Contains(sCadena) && pas.idAlmacen == iIdAlmacen
                                  select new InventariosView
                                  {
                                      Almacen = pas.almacenes.Nombre,
                                      Clave = pas.productos.Clave1,
                                      Producto = pas.productos.Nombre,
                                      StockMin = (int)pas.StockMin,
                                      StockMax = (int)pas.StockMax,
                                      idProAlmStocks = pas.idProAlmStocks,
                                      Cantidad = ((from pa in oMedicuriEntities.productos_almacen
                                                   where pa.idProducto == pas.idProducto
                                                         && pa.idAlmacen == pas.idAlmacen

                                                   select pa.Cantidad).Sum()) == null ? 0
                                                  :
                                                  (from pa in oMedicuriEntities.productos_almacen
                                                   where pa.idProducto == pas.idProducto
                                                         && pa.idAlmacen == pas.idAlmacen

                                                   select pa.Cantidad).Sum()
                                  });
                        return oQuery;
                }
                return null;
            }
            else
            {
                IQueryable<InventariosView> oQuery = (from pas in oMedicuriEntities.productos_almacen_stocks
                              where pas.idAlmacen==iIdAlmacen
                                                      select new InventariosView
                              {
                                  Almacen = pas.almacenes.Nombre,
                                  Clave = pas.productos.Clave1,
                                  Producto = pas.productos.Nombre,
                                  StockMin = (int)pas.StockMin,
                                  StockMax = (int)pas.StockMax,
                                  idProAlmStocks = pas.idProAlmStocks,
                                  Cantidad = ((from pa in oMedicuriEntities.productos_almacen
                                               where pa.idProducto == pas.idProducto
                                                     && pa.idAlmacen == pas.idAlmacen

                                               select pa.Cantidad).Sum()) == null ? 0
                                              :
                                              (from pa in oMedicuriEntities.productos_almacen
                                               where pa.idProducto == pas.idProducto
                                                     && pa.idAlmacen == pas.idAlmacen

                                               select pa.Cantidad).Sum()
                              });
                return oQuery;
            }
        }

        public bool EditarProductosAlmacenStock(DAL.productos_almacen_stocks oProductosAlmacenStock) 
        {
            try
            {
                var oQuery = from q in oMedicuriEntities.productos_almacen_stocks.
                         Where("it.idProAlmStocks = @idProAlmStocks",
                         new ObjectParameter("idProAlmStocks", oProductosAlmacenStock.idProAlmStocks))
                             select q;

                DAL.productos_almacen_stocks oProductosAlmacenStockOriginal = oQuery.First<DAL.productos_almacen_stocks>();
                oProductosAlmacenStockOriginal.StockMax = oProductosAlmacenStock.StockMax;
                oProductosAlmacenStockOriginal.StockMin = oProductosAlmacenStock.StockMin;

                oMedicuriEntities.SaveChanges();
                return true;
            }
            catch 
            {
                return false;
            }
        }

        public bool BuscarAlmacenProductoStocks(int idAlmacen, int idProducto)
        {
            bool bExiste;

            var oQuery = from q in oMedicuriEntities.productos_almacen_stocks
                         where q.idAlmacen == idAlmacen && q.idProducto == idProducto
                         select q;

            return bExiste = oQuery.Count<MedDAL.DAL.productos_almacen_stocks>() != 0 ? true : false;
        }

        public bool NuevoProductosAlmacenStock(DAL.productos_almacen_stocks oProductosAlmacenStock)
        {
            try
            {
                oMedicuriEntities.AddToproductos_almacen_stocks(oProductosAlmacenStock);
                oMedicuriEntities.SaveChanges();
                return true;
            }
            catch 
            {
                return false;
            }
        }

        public DateTime BuscarFechaCaducidad(int idProducto, int idAlmacen, string lote)
        { 
           var oQuery = from q in oMedicuriEntities.productos_almacen
                        where q.idProducto == idProducto && q.idAlmacen == idAlmacen && q.Lote == lote
                        select q.FechaCaducidad;

           return (DateTime)oQuery.First();
        }

        /// <summary>
        /// DAL Metodo que regresa las claves que contengan el parametro
        /// </summary>
        /// <param name="sCadena">Cadena que contenga la clave</param>
        /// <returns></returns>
        public string[] BuscarPedimentosAsincrono(string sCadena)
        {
            string[] asResultados;

            try
            {
                var oQuery = from q in oMedicuriEntities.inventario
                             where q.Pedimento.Contains(sCadena)
                             select q.Pedimento;


                return asResultados = oQuery.ToArray<string>();

            }
            catch
            {
                return asResultados = new string[0];
            }
        }

        public DAL.inventario ObtenerUltimoMovimiento()
        {
            try
            {
                return oMedicuriEntities.inventario.OrderByDescending(X => X.idInventarioMov).First();
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
