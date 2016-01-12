using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MedNeg.Productos
{
    public class BlProductos
    {

        MedDAL.Productos.DALProductos odalProducto;
        MedDAL.ProveedoresProductos.DALProveedoresProductos odalProveedorProducto;

        public BlProductos()
        {
            odalProducto = new MedDAL.Productos.DALProductos();
            odalProveedorProducto = new MedDAL.ProveedoresProductos.DALProveedoresProductos();
        }
                
        /// <summary>
        /// BL - REcuperar un producto mediante su Clave1
        /// </summary>
        /// <param name="clave1"></param>
        /// <returns></returns>
        public MedDAL.DAL.productos buscarProducto(String sClave1)
        {
            return odalProducto.buscarProducto(sClave1);
        }

        /// <summary>
        /// BL - REcuperar un producto mediante su Clave2
        /// </summary>
        /// <param name="sClave2"></param>
        /// <returns></returns>
        public MedDAL.DAL.productos buscarProductoClave2(string sClave2)
        {
            return odalProducto.buscarProductoClave2(sClave2);
        }


        // GT: 17/Mar/2011 Función que recupera un producto mediante su nombre, requerida para llenar autocompletar
        //                 en Pedidos,Remisiones y Facturas
        /// <summary>
        /// BL - Recuperar un producto mediante su nombre
        /// </summary>
        /// <param name="sNombre"></param>
        /// <returns></returns>
        public MedDAL.DAL.productos BuscarProductoNombre(string sNombre)
        {
            return odalProducto.BuscarProductoNombre(sNombre);

        }

        /// <summary>
        /// BL - Recuperar un producto mediante su Clave1
        /// </summary>
        /// <param name="sClave">Clave1 del producto</param>
        /// <returns></returns>
        public int RecuperarIdProducto(string sClave)
        {
            return odalProducto.RecuperarIdProducto(sClave);
        }

        /// <summary>
        /// BL - Buscar un Producto
        /// </summary>
        /// <param name="sCadena">Parametro a buscar</param>
        /// <param name="iTipo">Filtro</param>
        /// <returns></returns>
        public object Buscar(string sCadena, int iTipo)
        {
            return odalProducto.Buscar(sCadena, iTipo);
        }

        /// <summary>
        /// BL - Buscar productos filtrada por almacenes
        /// </summary>
        /// <param name="sCadena"></param>
        /// <param name="iFiltro"></param>
        /// <param name="iIdAlmacen"></param>
        /// <returns></returns>
        public object BuscarFiltradaAlmacen(string sCadena, int iFiltro, int iIdAlmacen)
        {
            return odalProducto.BuscarFiltradaAlmacen(sCadena, iFiltro, iIdAlmacen);
        }



        public MedDAL.DAL.productos Buscar(int idProducto)
        {
            return odalProducto.buscarProducto(idProducto);
        }

        /// <summary>
        /// BL - Función que muestra todos los registros de la tabla
        /// </summary>
        /// <returns></returns>
        public object MostrarLista()
        {
            return odalProducto.MostrarLista();
        }

        public IQueryable<MedDAL.DAL.productos_almacen> ObtenerExistenciaProducto(int idProducto, int idAlmacen, string sLote, string sNoSerie)
        {
            return odalProducto.ObtenerExistenciaProducto(idProducto, idAlmacen, sLote, sNoSerie);
        }

        public IQueryable<MedDAL.DAL.productos_almacen> ObtenerExistenciaProducto(int idProducto, int idAlmacen)
        {
            return odalProducto.ObtenerExistenciaProducto(idProducto, idAlmacen);
        }

        /// <summary>
        /// Obtiene los valores producto-almacen de los productos relacionados por el lote
        /// </summary>
        /// <param name="idAlmacen"></param>
        /// <param name="idProducto"></param>
        /// <param name="sLote"></param>
        /// <returns></returns>
        public MedDAL.DAL.productos_almacen ObtenerProductoLote(int idAlmacen, int idProducto, string sLote)
        {
            return odalProducto.ObtenerProductoLote(idAlmacen, idProducto, sLote);
        }

        public MedDAL.DAL.productos_almacen_stocks ObtenerProductoAlmacenStock(int idAlmacen, int idProducto)
        {

            return odalProducto.ObtenerProductoAlmacenStock(idAlmacen, idProducto);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IQueryable<MedDAL.DAL.productos> BuscarEnum()
        {
            return odalProducto.BuscarEnum();
        }

        /// <summary>
        ///  BL - Registrar un producto nuevo
        /// </summary>
        /// <param name="oProducto">producto a registrar</param>
        /// <returns></returns>
        public bool NuevoRegistro(MedDAL.DAL.productos oProducto)
        {
            return odalProducto.NuevoRegistro(oProducto);
        }

        /// <summary>
        /// BL - Editar un Producto
        /// </summary>
        /// <param name="oProducto">Producto a editar</param>
        /// <returns></returns>
        public bool EditarRegistro(MedDAL.DAL.productos oProducto)
        {
            return odalProducto.EditarRegistro(oProducto);
        }

        /// <summary>
        /// BL - Eliminar un Producto
        /// </summary>
        /// <param name="iIdProducto">ID Producto a eliminar</param>
        /// <returns></returns>
        public bool EliminarRegistro(int iIdProducto)
        {
            return odalProducto.EliminarRegistro(iIdProducto);
        }

        /// <summary>
        /// BL -Validar un Producto por su calve1
        /// </summary>
        /// <param name="claveProducto">Producto</param>
        /// <returns></returns>
        public int ValidarProductoRepetido(string claveProducto)
        {
            return odalProducto.ValidarProductoRepetido(claveProducto);
        }

        public int ValidarProductoRepetido(string sClaveProducto, int iIdProducto)
        {
            return odalProducto.ValidarProductoRepetido(sClaveProducto, iIdProducto);
        }

        /// <summary>
        /// Añade un nuevo proveedor/producto
        /// </summary>
        /// <param name="oProveedorProducto"></param>
        /// <returns></returns>
        public bool NuevoProveedorProducto(MedDAL.DAL.proveedores_productos oProveedorProducto) {
            return odalProveedorProducto.NuevoRegistro(oProveedorProducto);
        }

        /// <summary>
        /// Elimina un proveedor/producto
        /// </summary>
        /// <param name="idProducto"></param>
        /// <returns></returns>
        public bool EliminarProveedorProducto(int idProducto) {
            return odalProveedorProducto.EliminarRegistro(idProducto);
        }

        public object BuscarProveedor(int idProducto) {
            return odalProveedorProducto.Buscar(idProducto);
        }


        public decimal RecuperarPrecioPublico(int iIdProducto)
        {
            return odalProducto.RecuperarPrecioPublico(iIdProducto);
        }

        public decimal RecuperarPrecio1(int iIdProducto)
        {
           return odalProducto.RecuperarPrecio1(iIdProducto);
        }

        public decimal RecuperarPrecio2(int iIdProducto)
        {
            return odalProducto.RecuperarPrecio2(iIdProducto);
        }

        public decimal RecuperarPrecio3(int iIdProducto)
        {
            return odalProducto.RecuperarPrecio3(iIdProducto);
        }

        public decimal RecuperarPrecioMinimo(int iIdProducto)
        {
            return odalProducto.RecuperarPrecioMinimo(iIdProducto);
        }
    }
}
