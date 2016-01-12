using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Objects;
using System.Globalization;


namespace MedDAL.Pedidos
{
    public class DALPedidos:ClsModulo
    {
        DAL.medicuriEntities oMedicuriEntities = new DAL.medicuriEntities();

        public DALPedidos()
        {
            oMedicuriEntities = new DAL.medicuriEntities();
        }

        /// <summary>
        /// DAL - Insertar nuevo registro
        /// </summary>
        /// <param name="oPerfil">Pedido a guardar</param>
        /// <returns>true registrado, false no registrado</returns>
        public bool NuevoRegistro(DAL.pedidos oPedido)
        {
            try
            {
                //Agregar el registro
                oMedicuriEntities.AddTopedidos(oPedido);
                oMedicuriEntities.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }


        /// <summary>
        /// Recuperar un pedido mediante su folio
        /// </summary>
        /// <param name="sFolio">Folio a buscar</param>
        /// <returns></returns>
        public DAL.pedidos BuscarPedidoFolio(string sFolio)
        {
            try
            {
                var oQuery = from q in oMedicuriEntities.pedidos
                             where q.Folio == sFolio
                             select q;
                return oQuery.First<MedDAL.DAL.pedidos>();
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Buscar mediante filtros parametros
        /// </summary>
        /// <param name="sCadena">Parametro a buscar</param>
        /// <param name="iFiltro">Filtro</param>
        /// <returns>var oQuery</returns>
        public IQueryable<PedidosView> Buscar(string sCadena, int iFiltro)
        {
            string sConsulta = "";
            
            switch (iFiltro)
            {
               
                case 1:
                    sConsulta = "it.Folio LIKE '%'+@Dato+'%' ";
                    break;
              
                case 2:
                    sConsulta = "it.clientes.nombre LIKE '%'+@Dato+'%'";
                    break;
               
                //case 3:
                //    //sConsulta = "it.Fecha LIKE '%'+@Dato+'%'";
                //    sConsulta = "it.Fecha='@Dato'";
                    
                //    break;


            }

            if (iFiltro == 3)
            {


                DateTime dFecha = Convert.ToDateTime(sCadena);
                IQueryable<PedidosView> oQuery = from q in oMedicuriEntities.pedidos
                              where (q.Fecha == dFecha)
                              select new PedidosView {
                              idPedido = q.idPedido, 
                              idCliente = q.idCliente, 
                              Nombre = q.clientes.Nombre, 
                              Apellidos = q.clientes.Apellidos,
                              Folio = q.Folio,
                              Fecha = q.Fecha,
                              Estatus = q.Estatus};
                return oQuery;

            }
            else
            {
                IQueryable<PedidosView> oQuery = from q in oMedicuriEntities.pedidos.Where(sConsulta, new ObjectParameter("Dato", sCadena))
                                                 select new PedidosView
                                                 {
                                                     idPedido = q.idPedido,
                                                     idCliente = q.idCliente,
                                                     Nombre = q.clientes.Nombre,
                                                     Apellidos = q.clientes.Apellidos,
                                                     Folio = q.Folio,
                                                     Fecha = q.Fecha,
                                                     Estatus = q.Estatus
                                                 };
                return oQuery;
            }
            
        }
             

        /// <summary>
        /// Recuperar un pedido mediante su Id
        /// </summary>
        /// <param name="iIdPedido">Folio a buscar</param>
        /// <returns></returns>
        public DAL.pedidos BuscarPedido(int iIdPedido)
        {
            try
            {
                var oQuery = from q in oMedicuriEntities.pedidos
                             where q.idPedido == iIdPedido
                             select q;
                return oQuery.First<MedDAL.DAL.pedidos>();
            }
            catch
            {
                return null;
            }


        }

       /// <summary>
       /// DAL - Agregar detalle de partida de un pedido
       /// </summary>
       /// <param name="oPedidoPartida">Registro de la partida</param>
       /// <returns></returns>
        public bool NuevoDetallePartida(DAL.pedidos_partida oPedidoPartida)
        {
            try
            {
                //Agregar el registro
                oMedicuriEntities.AddTopedidos_partida(oPedidoPartida);
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
                var oQuery = from q in oMedicuriEntities.pedidos
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
        public IQueryable<PedidosView> MostrarLista()
        {
            IQueryable<PedidosView> oQuery = (from q in oMedicuriEntities.pedidos
                          select new PedidosView {
                              idPedido = q.idPedido, 
                              idCliente = q.idCliente, 
                              Nombre = q.clientes.Nombre, 
                              Apellidos = q.clientes.Apellidos,
                              Folio = q.Folio,
                              Fecha = q.Fecha,
                              Estatus = q.Estatus});
            return oQuery;
        }


        /// <summary>
        /// Recuperar la partida de un pedido
        /// </summary>
        /// <param name="iIdPedido"></param>
        /// <returns></returns>
        public IQueryable<DAL.pedidos_partida> RecuperarPartidaPedido(int iIdPedido)
        {
            try
            {
                var oQuery = from q in oMedicuriEntities.pedidos_partida
                             where q.idPedido == iIdPedido
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
        /// <param name="oPedido"></param>
        /// <returns></returns>
        public bool EditarRegistro(DAL.pedidos oPedido)
        {
            try
            {
                var oQuery= from q in oMedicuriEntities.pedidos.Where("it.idPedido=@idPedido",
                            new ObjectParameter("idPedido",oPedido.idPedido))
                            select q;

                DAL.pedidos oPedidoOriginal= oQuery.First<DAL.pedidos>();
                oPedidoOriginal.Estatus=oPedido.Estatus;

                oMedicuriEntities.SaveChanges();
                return true;            

            }
            catch
            {
                return false;
            }

        }

        /// <summary>
        /// Eliminar la partida de un pedido
        /// </summary>
        /// <param name="iIdPedido"></param>
        /// <returns></returns>
        public bool EliminarPedidoPartida(int iIdPedido)
        {
            try
            {
               var oQuery = from q in oMedicuriEntities.pedidos_partida.
                             Where("it.idPedido=@idPedido",
                             new ObjectParameter("idPedido",iIdPedido))
                             select q;

               foreach (DAL.pedidos_partida oRegistro in oQuery)
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
        /// Eliminar un pedido
        /// </summary>
        /// <param name="iIdPedido">Id pedido a eliminar</param>
        /// <returns></returns>
        public bool EliminarRegistro(int iIdPedido)
        {
            try
            {
                var oQuery = from q in oMedicuriEntities.pedidos.
                              Where("it.idPedido=@idPedido",
                              new ObjectParameter("idPedido", iIdPedido))
                             select q;

                DAL.pedidos oPedidoOriginal = oQuery.First<DAL.pedidos>();
                oMedicuriEntities.DeleteObject(oPedidoOriginal);
                oMedicuriEntities.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }


        /// <summary>
        /// Buscar folios asincronamente para llenar
        /// </summary>
        /// <param name="sCadena"></param>
        /// <returns></returns>
        public string[] BuscarFolioAsincrono(string sCadena)
        {
            string[] asResultados;

            try
            {
                var oQuery = from q in oMedicuriEntities.pedidos
                             where q.Folio.Contains(sCadena) && q.Estatus=="1"
                             select q.Folio;
                
                return asResultados = oQuery.ToArray<string>();

            }
            catch
            {
                return asResultados = new string[0];
            }
        }
    }
}
