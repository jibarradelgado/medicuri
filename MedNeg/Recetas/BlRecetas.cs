using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace MedNeg.Recetas
{
    public class BlRecetas
    {
        MedDAL.Recetas.DALRecetas oRecetas;
        MedNeg.Productos.BlProductos blProductos;
        MedDAL.Productos.DALProductos dalProductos;
        MedDAL.Usuarios.DALUsuarios dalUsuarios;
        MedDAL.Tipos.DALTipos dalTipos;
        MedDAL.Vendedores.DALVendedores dalVendedores;
        MedDAL.Recetas.DALRecetas dalRecetas;

        public BlRecetas()
        {
            oRecetas = new MedDAL.Recetas.DALRecetas();
            blProductos = new Productos.BlProductos();
            dalProductos = new MedDAL.Productos.DALProductos();
            dalUsuarios = new MedDAL.Usuarios.DALUsuarios();
            dalTipos = new MedDAL.Tipos.DALTipos();
            dalVendedores = new MedDAL.Vendedores.DALVendedores();
            dalRecetas = new MedDAL.Recetas.DALRecetas();
        }

        /// <summary>
        /// BL - Buscar receta mediante su folio
        /// </summary>
        /// <param name="sFolio">Folio a buscar</param>
        /// <returns></returns>
        public MedDAL.DAL.recetas BuscarRecetaFolio(string sFolio)
        {
            return oRecetas.BuscarRecetaFolio(sFolio);
        }

        /// <summary>
        /// BL - Buscar receta mediante su folio
        /// </summary>
        /// <param name="sFolio">Folio a buscar</param>
        /// <returns></returns>
        public MedDAL.DAL.recetas BuscarRecetaFolioRepetido(string sFolio)
        {
            return oRecetas.BuscarRecetaFolioRepetido(sFolio);
        }

        public IQueryable<MedDAL.Recetas.RecetasView> BuscarTodasRecetas()
        {
            return dalRecetas.BuscarTodasRecetas();  
        }

        public IQueryable<MedDAL.Recetas.RecetasView> BuscarTodasRecetas(int idAlmacen)
        {
            return dalRecetas.BuscarTodasRecetas(idAlmacen);
        }
        
        /// <summary>
        /// Recuperar la partida de una receta
        /// </summary>
        /// <param name="iIdReceta"></param>
        /// <returns></returns>
        public IQueryable<MedDAL.DAL.recetas_partida> RecuperarPartidaRecetas(int iIdReceta)
        {
            return oRecetas.RecuperarPartidaRecetas(iIdReceta);
        }

         /// <summary>
        /// Editar registro
        /// </summary>
        /// <param name="oReceta"></param>
        /// <returns></returns>
        public bool EditarRegistro(MedDAL.DAL.recetas oReceta)
        {
            return oRecetas.EditarRegistro(oReceta);
        }

        /// <summary>
        /// Recuperar un receta mediante su id
        /// </summary>
        /// <param name="iIdReceta">Id a buscar</param>
        /// <returns></returns>
        public MedDAL.DAL.recetas BuscarReceta(int iIdReceta)
        {
            return oRecetas.BuscarReceta(iIdReceta);
        }

        public IQueryable<MedDAL.Recetas.RecetasView> BuscarReceta(string sCadena, int iFiltro)
        {
            return dalRecetas.Buscar(sCadena, iFiltro);
        }

        public IQueryable<MedDAL.Recetas.RecetasView> BuscarReceta(string sCadena,int iFiltro, int iIdAlmacen)
        {
            return dalRecetas.Buscar(sCadena, iFiltro, iIdAlmacen);
        }

        /// <summary>
        /// Obtener todas las recetas
        /// </summary>
        /// <returns></returns>
        public object BuscarReceta()
        {
            return oRecetas.BuscarReceta();
        }

    

        /// <summary>
        /// BL - Recuperar el monto total de las recetas
        /// </summary>
        /// <param name="idReceta"></param>
        /// <returns></returns>
        public object RecetaPartidaMontoTotal(string sFolioReceta)
        {
            return oRecetas.RecetaPartidaMontoTotal(sFolioReceta);
        }

        public MedDAL.DAL.productos buscaProducto(string clave1)
        {
            return blProductos.buscarProducto(clave1);
        }

        public MedDAL.DAL.productos buscaProducto(int id)
        {
            return dalProductos.buscarProducto(id);
        }

        public MedDAL.DAL.productos buscaProductoNombre(string Nombre)
        {
            return blProductos.BuscarProductoNombre(Nombre);
        }



        public IQueryable<string> buscaLotesProducto(int idProducto,int idAlmacen)
        {
            return dalProductos.buscarLotesProducto(idProducto, idAlmacen);
        }

        public IQueryable<string> buscaSeriesProducto(int idProducto, int idAlmacen)
        {
            return dalProductos.buscarSeriesProducto(idProducto, idAlmacen);
        }

        public MedDAL.DAL.usuarios buscarUsuario(string nombreUsuario)
        {
            return (MedDAL.DAL.usuarios)dalUsuarios.Buscar(nombreUsuario);
        }

        public List<MedDAL.DAL.tipos> buscarTiposRecetas()
        {
            return dalTipos.RecuperarTiposRecetas();
        }

        public MedDAL.DAL.vendedores buscarVendedor(string cedProf)
        {
            return dalVendedores.Buscar(cedProf);
        }

        public MedDAL.DAL.vendedores buscarVendedorClave(string sClave)
        {
            return dalVendedores.BuscarVendedorClave(sClave);
        }

        //0415 GT 15-Ago-2011
        public MedDAL.DAL.vendedores buscarVendedorNombre(string sNombreVendedor)
        {
            return dalVendedores.BuscarVendedorNombre(sNombreVendedor);
        }

        public int buscarLineaCredito(int idAlmacen, int idProducto, string serie, string lote)
        {
            return dalProductos.buscarLineaCredito(idAlmacen, idProducto, serie, lote);
        }

        public void guardarReceta(MedDAL.DAL.recetas receta)
        {
            dalRecetas.agregarReceta(receta);
        }

        public void guardarRecetasPartida(List<MedDAL.DAL.recetas_partida> lPartida)
        {
            dalRecetas.agregaRecetaPartida(lPartida);
        }

        public void eliminaRecetasPartida(int idReceta)
        {
            dalRecetas.eliminaPartidaReceta(idReceta);
        }

        public void aumentarExistencias(int idAlmacen, List<MedDAL.DAL.recetas_partida> lstProductos)
        {
            foreach (MedDAL.DAL.recetas_partida oRecetasPartida in lstProductos)
            {
                dalProductos.ModificarExistenciaProducto(idAlmacen, Convert.ToInt32(oRecetasPartida.idProducto), oRecetasPartida.Lote, oRecetasPartida.NoSerie, oRecetasPartida.CantidaSurtida.Value, 0);
            }
        }

        public void disminuirExistencias(int idAlmacen,List<MedDAL.DAL.recetas_partida> lstProductos)
        {
            foreach (MedDAL.DAL.recetas_partida oRecetasPartida in lstProductos)
            {
                dalProductos.ModificarExistenciaProducto(idAlmacen, Convert.ToInt32(oRecetasPartida.idProducto), oRecetasPartida.Lote, oRecetasPartida.NoSerie, oRecetasPartida.CantidaSurtida.Value, 1);
            }
        }

        public void EditarReceta(MedDAL.DAL.recetas receta)
        {
            dalRecetas.EditarReceta(receta);
        }

        public void EliminarReceta(MedDAL.DAL.recetas receta)
        {
            dalRecetas.EliminarReceta(receta);
        }


        /// <summary>
        /// GT Se requiere para recuperar las recetas de facturacion por programa
        /// </summary>
        /// <returns></returns>
         public object RecetasFacturarPorPrograma()
         {
             return oRecetas.RecetasFacturarPorPrograma();
         }

        /// <summary>
        /// GT Se requiere para recuperar las recetas de facturacion por programa 
        /// </summary>
        /// <param name="iIdAlmacen"></param>
        /// <returns></returns>
         public object RecetasFacturarPorPrograma(int iIdAlmacen)
         {
             return oRecetas.RecetasFacturarPorPrograma(iIdAlmacen);
         }

        /// <summary>
         /// GT Se requiere para recuperar las recetas de facturacion por programa 
        /// </summary>
        /// <param name="sFecha1">Fecha desde</param>
        /// <param name="sFecha2">Fecha hasta</param>
        /// <returns></returns>
         public object RecetasFacturarPorProgramaFecha(string sFecha1,string sFecha2)
         {
             return oRecetas.RecetasFacturarPorProgramaFecha(sFecha1, sFecha2);
         }

        /// <summary>
         /// GT Se requiere para recuperar las recetas de facturacion por programa 
        /// </summary>
        /// <param name="sFolio1"></param>
        /// <param name="sFolio2"></param>
        /// <returns></returns>
         public object RecetasFacturarPorProgramaFolio(string sFolio1, string sFolio2)
         {
             return oRecetas.RecetasFacturarPorProgramaFolio(sFolio1, sFolio2);
         }

        /// <summary>
         /// GT Se requiere para recuperar las recetas de facturacion por programa 
        /// </summary>
        /// <param name="sFecha1"></param>
        /// <param name="sFecha2"></param>
        /// <param name="iIdAlmacen"></param>
        /// <returns></returns>
         public object RecetasFacturarPorProgramaFecha(string sFecha1, string sFecha2,int iIdAlmacen)
         {
             return oRecetas.RecetasFacturarPorProgramaFecha(sFecha1, sFecha2,iIdAlmacen);
         }

        /// <summary>
         /// GT Se requiere para recuperar las recetas de facturacion por programa 
        /// </summary>
        /// <param name="sFolio1"></param>
        /// <param name="sFolio2"></param>
        /// <param name="iIdAlmacen"></param>
        /// <returns></returns>
         public object RecetasFacturarPorProgramaFolio(string sFolio1, string sFolio2,int iIdAlmacen)
         {
             return oRecetas.RecetasFacturarPorProgramaFolio(sFolio1, sFolio2,iIdAlmacen);
         }

         public bool NuevoRegistroPartida(MedDAL.DAL.recetas_partida oRecetasPartida)
         {
             return oRecetas.NuevoRegistroPartida(oRecetasPartida);
         }

         public bool EliminarRegistroPartida(MedDAL.DAL.recetas oReceta)
         {
             return oRecetas.EliminarRegistroPartida(oReceta);
         }

         /// <summary>
         /// Saber si esta activada la opción de folio automatico de recetas en configuración
         /// </summary>
         public int RecuperaFolioAutomatico(string sRutaArchivoConfig)
         {

             if (File.Exists(sRutaArchivoConfig))
             {
                 MedNeg.Configuracion.BlConfiguracion oblConfiguracion = new MedNeg.Configuracion.BlConfiguracion();
                 MedDAL.Configuracion.DALConfiguracion odalConfiguracion = new MedDAL.Configuracion.DALConfiguracion();

                 odalConfiguracion = (MedDAL.Configuracion.DALConfiguracion)oblConfiguracion.CargaDatos(sRutaArchivoConfig);

                 if (odalConfiguracion.iRecetasAutomatico == 1)
                 {
                     return odalConfiguracion.iFolioRecetas + 1;
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
         /// Actualizar el folio de Recetas
         /// </summary>
         /// <param name="sRutaArchivoConfig"></param>
         public void ActualizarFolioReceta(string sRutaArchivoConfig)
         {
             MedNeg.Configuracion.BlConfiguracion oblConfiguracion = new MedNeg.Configuracion.BlConfiguracion();
             MedDAL.Configuracion.DALConfiguracion odalConfiguracion = new MedDAL.Configuracion.DALConfiguracion();

             odalConfiguracion = (MedDAL.Configuracion.DALConfiguracion)oblConfiguracion.CargaDatos(sRutaArchivoConfig);

             //Incrementar el folio
             if (odalConfiguracion.iRecetasAutomatico == 1)
             {
                 odalConfiguracion.iFolioRecetas++;
                 oblConfiguracion.GuardarDatos(odalConfiguracion, sRutaArchivoConfig);
             }
         }
    }
}
