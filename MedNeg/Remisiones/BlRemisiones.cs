using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace MedNeg.Remisiones
{
    public class BlRemisiones
    {
        MedDAL.Remisiones.DALRemisiones odalRemisiones;


        public BlRemisiones()
        {
            odalRemisiones = new  MedDAL.Remisiones.DALRemisiones();
        }


        /// <summary>
        /// BL - Nuevo registro
        /// </summary>
        /// <param name="oRemision"></param>
        /// <returns></returns>
        public bool NuevoRegistro(MedDAL.DAL.remisiones oRemision)
        {
            return odalRemisiones.NuevoRegistro(oRemision);
        }

        /// <summary>
        /// BL - Insertar detalla partida de un pedido
        /// </summary>
        /// <param name="oRemisionPartida">Detalle de partida</param>
        /// <returns></returns>
        public bool NuevoDetallePartida(MedDAL.DAL.remisiones_partida oRemisionPartida)
        {
            return odalRemisiones.NuevoDetalleRemision(oRemisionPartida);

        }

        /// <summary>
        /// BL - Buscar remision mediante su folio
        /// </summary>
        /// <param name="sFolio">Folio a buscar</param>
        /// <returns></returns>
        public MedDAL.DAL.remisiones BuscarRemisionFolio(string sFolio)
        {
            return odalRemisiones.BuscarRemisionFolio(sFolio);
        }

        /// <summary>
        /// Recuperar remision mediante su id
        /// </summary>
        /// <param name="iIdRemision"></param>
        /// <returns></returns>
        public MedDAL.DAL.remisiones BuscarRemision(int iIdRemision)
        {
            return odalRemisiones.BuscarRemision(iIdRemision);

        }
         

        /// <summary>
        /// BL - Validar Folio pedido
        /// </summary>
        /// <param name="sFolio"></param>
        /// <returns></returns>
        public bool ValidarFolioRepetido(string sFolio)
        {
            return odalRemisiones.ValidarFolioRepetido(sFolio);
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

                if (odalConfiguracion.iRemisionesAutomatico==1)
                {
                    return odalConfiguracion.iFolioRemisiones+1;
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
         /// Actualizar el folio de remisiones
         /// </summary>
         /// <param name="sRutaArchivoConfig"></param>
        public void ActualizarFolioRemision(string sRutaArchivoConfig)
        {
            MedNeg.Configuracion.BlConfiguracion oblConfiguracion = new MedNeg.Configuracion.BlConfiguracion();
            MedDAL.Configuracion.DALConfiguracion odalConfiguracion = new MedDAL.Configuracion.DALConfiguracion();

            odalConfiguracion = (MedDAL.Configuracion.DALConfiguracion)oblConfiguracion.CargaDatos(sRutaArchivoConfig);

            if (odalConfiguracion.iRemisionesAutomatico == 1)
            {
                //Incrementar el folio
                odalConfiguracion.iFolioRemisiones++;
                oblConfiguracion.GuardarDatos(odalConfiguracion, sRutaArchivoConfig);
            }

        }

        /// <summary>
        /// Mostrar las lista de clientes que tienen pedidos
        /// </summary>
        /// <returns></returns>
        public IQueryable<MedDAL.Remisiones.RemisionesView> MostrarLista()
        {
            return odalRemisiones.MostrarLista();
        }

        /// <summary>
        /// Buscar pedido
        /// </summary>
        /// <param name="sCadena">Parametro a buscar</param>
        /// <param name="iTipo">Filtro</param>
        /// <returns></returns>
        public IQueryable<MedDAL.Remisiones.RemisionesView> Buscar(string sCadena, int iTipo)
        {
            //return odalEstados.Buscar(sCadena, iTipo);
            return odalRemisiones.Buscar(sCadena, iTipo);
        }


        /// <summary>
        /// Recuperar la partida de una remision
        /// </summary>
        /// <param name="iIdRemision">Id Remision a buscar</param>
        /// <returns></returns>
        public IQueryable<MedDAL.DAL.remisiones_partida> RecuperarPartidaRemision(int iIdRemision)
        {
            return odalRemisiones.RecuperarPartidaRemision(iIdRemision);
        }

        /// <summary>
        /// Editar Remision
        /// </summary>
        /// <param name="oRemision"></param>
        /// <returns></returns>
        public bool EditarRegistro(MedDAL.DAL.remisiones oRemision)
        {
            return odalRemisiones.EditarRegistro(oRemision);
        }

        /// <summary>
        /// Eliminar partida detalle
        /// </summary>
        /// <param name="iIdRemision"></param>
        /// <returns></returns>
        public bool EliminarRemisionPartida(int iIdRemision)
        {
            return odalRemisiones.EliminarRemisionPartida(iIdRemision);
        }

         /// <summary>
        /// Eliminar una remision
        /// </summary>
        /// <param name="iIdRemision">Id Remision a eliminar</param>
        /// <returns></returns>
        public bool EliminarRegistro(int iIdRemision)
        {
            return odalRemisiones.EliminarRegistro(iIdRemision);
        }

        public bool ModificarExistenciaProducto(int idAlmacen, int idProducto, decimal dCantidad, int iModo)
        {
            MedDAL.Productos.DALProductos oProductos = new MedDAL.Productos.DALProductos();

            return oProductos.ModificarExistenciaProducto(idAlmacen, idProducto, dCantidad, iModo);
       }
            


    }
    
}
