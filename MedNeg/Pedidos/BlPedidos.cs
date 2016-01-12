using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace MedNeg.Pedidos
{
    public class BlPedidos
    {
        MedDAL.Pedidos.DALPedidos odalPedidos;


        public BlPedidos()
        {
            odalPedidos = new MedDAL.Pedidos.DALPedidos();
        }


        /// <summary>
        /// BL - Nuevo registro
        /// </summary>
        /// <param name="oPedido"></param>
        /// <returns></returns>
        public bool NuevoRegistro(MedDAL.DAL.pedidos oPedido)
        {
            return odalPedidos.NuevoRegistro(oPedido);
        }

        /// <summary>
        /// BL - Insertar detalla partida de un pedido
        /// </summary>
        /// <param name="oPedidoPartida">Detalle de partida</param>
        /// <returns></returns>
        public bool NuevoDetallePartida(MedDAL.DAL.pedidos_partida oPedidoPartida)
        {
            return odalPedidos.NuevoDetallePartida(oPedidoPartida);

        }

        /// <summary>
        /// Buscar pedido
        /// </summary>
        /// <param name="sCadena">Parametro a buscar</param>
        /// <param name="iTipo">Filtro</param>
        /// <returns></returns>
        public IQueryable<MedDAL.Pedidos.PedidosView> Buscar(string sCadena, int iTipo)
        {
            //return odalEstados.Buscar(sCadena, iTipo);
            return odalPedidos.Buscar(sCadena, iTipo);
        }

        /// <summary>
        /// BL - Buscar pedido mediante su folio
        /// </summary>
        /// <param name="sFolio">Folio a buscar</param>
        /// <returns></returns>
        public MedDAL.DAL.pedidos BuscarPedidoFolio(string sFolio)
        {
            return odalPedidos.BuscarPedidoFolio(sFolio);
        }

        /// <summary>
        /// Recuperar pedido mediante su id
        /// </summary>
        /// <param name="iIdPedido"></param>
        /// <returns></returns>
        public MedDAL.DAL.pedidos BuscarPedido(int iIdPedido)
        {
            return odalPedidos.BuscarPedido(iIdPedido);

        }
         

        /// <summary>
        /// BL - Validar Folio pedido
        /// </summary>
        /// <param name="sFolio"></param>
        /// <returns></returns>
        public bool ValidarFolioRepetido(string sFolio)
        {
            return odalPedidos.ValidarFolioRepetido(sFolio);
        }

        /// <summary>
        /// Saber si esta activada la opción de folio automatico en pedidos en configuración
        /// </summary>
        public int RecuperaFolioAutomatico(string sRutaArchivoConfig)
        {

            if (File.Exists(sRutaArchivoConfig))
            {
                MedNeg.Configuracion.BlConfiguracion oblConfiguracion = new MedNeg.Configuracion.BlConfiguracion();
                MedDAL.Configuracion.DALConfiguracion odalConfiguracion = new MedDAL.Configuracion.DALConfiguracion();

                odalConfiguracion = (MedDAL.Configuracion.DALConfiguracion)oblConfiguracion.CargaDatos(sRutaArchivoConfig);

                if (odalConfiguracion.iPedidosAutomatico == 1)
                {
                    return odalConfiguracion.iFolioPedidos + 1;
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
         /// Actualizar el folio de pedidos
         /// </summary>
         /// <param name="sRutaArchivoConfig"></param>
        public void ActualizarFolioPedido(string sRutaArchivoConfig)
        {
            MedNeg.Configuracion.BlConfiguracion oblConfiguracion = new MedNeg.Configuracion.BlConfiguracion();
            MedDAL.Configuracion.DALConfiguracion odalConfiguracion = new MedDAL.Configuracion.DALConfiguracion();

            odalConfiguracion = (MedDAL.Configuracion.DALConfiguracion)oblConfiguracion.CargaDatos(sRutaArchivoConfig);

            //Incrementar el folio
            if (odalConfiguracion.iPedidosAutomatico == 1)
            {
                odalConfiguracion.iFolioPedidos++;
                oblConfiguracion.GuardarDatos(odalConfiguracion, sRutaArchivoConfig);
            }
        }

        /// <summary>
        /// Mostrar las lista de clientes que tienen pedidos
        /// </summary>
        /// <returns></returns>
        public IQueryable<MedDAL.Pedidos.PedidosView> MostrarLista()
        {
            return odalPedidos.MostrarLista();
        }


        /// <summary>
        /// Recuperar la partida de un pedido
        /// </summary>
        /// <param name="iIdPedido">Id Pedido a buscar</param>
        /// <returns></returns>
        public IQueryable<MedDAL.DAL.pedidos_partida> RecuperarPartidaPedido(int iIdPedido)
        {
            return odalPedidos.RecuperarPartidaPedido(iIdPedido);
        }

        /// <summary>
        /// Editar Pedido
        /// </summary>
        /// <param name="oPedido"></param>
        /// <returns></returns>
        public bool EditarRegistro(MedDAL.DAL.pedidos oPedido)
        {
            return odalPedidos.EditarRegistro(oPedido);
        }

        /// <summary>
        /// Eliminar partida detalle
        /// </summary>
        /// <param name="iIdPedido"></param>
        /// <returns></returns>
        public bool EliminarPedidoPartida(int iIdPedido)
        {
            return odalPedidos.EliminarPedidoPartida(iIdPedido);
        }

         /// <summary>
        /// Eliminar un pedido
        /// </summary>
        /// <param name="iIdPedido">Id pedido a eliminar</param>
        /// <returns></returns>
        public bool EliminarRegistro(int iIdPedido)
        {
            return odalPedidos.EliminarRegistro(iIdPedido);
        }

    }
}
