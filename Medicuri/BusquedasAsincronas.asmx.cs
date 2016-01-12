using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using System.Collections;

namespace Medicuri
{
     //GT: 17/Mar/2011
     // Web service creado para crear funciones que seran invocadas por los controles AutoComplete 
     // para cuando se desea utilizar una función asincrona en la busqueda, en especial para Pedidos,Facturas,Recetas Y Remisiones
     
     /// <summary>
    /// Summary description for BusquedasAsincronas
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]

    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class BusquedasAsincronas : System.Web.Services.WebService
    {

        /// <summary>
        /// Recuperar Clave1 de producto
        /// </summary>
        /// <param name="prefixText"></param>
        /// <returns></returns>
        [WebMethod]
        public string[] RecuperarClave1Producto(string prefixText)
        {
            MedDAL.Productos.DALProductos odalProductos = new MedDAL.Productos.DALProductos();

            string[] asProducto = odalProductos.BuscarProductoClave1Asincrono(prefixText);

            return asProducto.ToArray();
        }

        /// <summary>
        /// Recuperar Clave1 de producto
        /// </summary>
        /// <param name="prefixText"></param>
        /// <returns></returns>
        [WebMethod]
        public string[] RecuperarClave2Producto(string prefixText)
        {
            MedDAL.Productos.DALProductos odalProductos = new MedDAL.Productos.DALProductos();

            string[] asProducto = odalProductos.BuscarProductoClave2Asincrono(prefixText);

            return asProducto.ToArray();
        }

        [WebMethod]
        public string[] Nada(string prefixText)
        {
            return null;
        }

        /// <summary>
        /// Recuperar la clave1 de productos y la claveBOM de ensambles
        /// </summary>
        /// <param name="prefixText"></param>
        /// <returns></returns>
        [WebMethod]
        public string[] RecuperarClave1ProductoEnsambles(string prefixText)
        {
            MedDAL.Productos.DALProductos odalProductos = new MedDAL.Productos.DALProductos();

            string[] asProducto = odalProductos.BuscarProductoEnsambleClaveAsincrono(prefixText);

            return asProducto.ToArray();
        }

        /// <summary>
        /// Recuperar Nombre de producto
        /// </summary>
        /// <param name="prefixText"></param>
        /// <returns></returns>
        [WebMethod]
        public string[] RecuperarNombreProducto(string prefixText)
        {
            MedDAL.Productos.DALProductos odalProductos = new MedDAL.Productos.DALProductos();

            string[] asProducto = odalProductos.BuscarProductoAsincrono(prefixText);

            return asProducto.ToArray();
        }

        /// <summary>
        /// Recuperar Nombre de producto y nombre de ensambles
        /// </summary>
        /// <param name="prefixText"></param>
        /// <returns></returns>
        [WebMethod]
        public string[] RecuperarNombreEnsambleProducto(string prefixText)
        {
            MedDAL.Productos.DALProductos odalProductos = new MedDAL.Productos.DALProductos();

            string[] asProducto = odalProductos.BuscarProductoEnsambleAsincrono(prefixText);

            return asProducto.ToArray();
        }

        /// <summary>
        /// Recuperar nombre del cliente
        /// </summary>
        /// <param name="prefixText"></param>
        /// <returns></returns>
        [WebMethod]
        public string[] RecuperarNombreCliente(string prefixText)
        {
            MedDAL.Clientes.DALClientes odalClientes = new MedDAL.Clientes.DALClientes();
            string[] asClientes = odalClientes.BuscarClienteAsincrono(prefixText);

            return asClientes.ToArray();
        }

        /// <summary>
        /// Recuperar la clave del cliente
        /// </summary>
        /// <param name="prefixText"></param>
        /// <returns></returns>
        [WebMethod]
        public string[] RecuperarClave1Cliente(string prefixText)
        {
            MedDAL.Clientes.DALClientes odalClientes = new MedDAL.Clientes.DALClientes();
            string[] asClientes = odalClientes.BuscarClave1ClienteAsincrono(prefixText);

            return asClientes.ToArray();
        }
        /// <summary>
        /// Recuperar nombre del cliente
        /// </summary>
        /// <param name="prefixText"></param>
        /// <returns></returns>
        [WebMethod]
        public string[] RecuperarNombreProveedor(string prefixText)
        {
            MedDAL.Proveedores.DALProveedores odalProveedor = new MedDAL.Proveedores.DALProveedores();
            string[] asClientes = odalProveedor.BuscarProveedorAsincrono(prefixText);

            return asClientes.ToArray();
        }

        /// <summary>
        /// Recuperar vendedores vinculados
        /// </summary>
        /// <param name="prefixText"></param>
        /// <returns></returns>
        [WebMethod]
        public string[] RecuperarVendedoresVinculacion(string prefixText)
        {
            MedDAL.VendedoresVinculacion.DALVendedoresVinculacion odalVinculacion = new MedDAL.VendedoresVinculacion.DALVendedoresVinculacion();
            string[] asVendedores = odalVinculacion.BuscarVinculacionAsincrono(prefixText);

            return asVendedores.ToArray();
        }

        /// <summary>
        /// Recuperar vendedores especialidad
        /// </summary>
        /// <param name="prefixText"></param>
        /// <returns></returns>
        [WebMethod]
        public string[] RecuperarVendedoresEspecialidad(string prefixText)
        {
            MedDAL.VendedorEspecialidad.DALVendedorEspecialidad odalEspecialidad = new MedDAL.VendedorEspecialidad.DALVendedorEspecialidad();
            string[] asVendedores = odalEspecialidad.BuscarEspecialidadAsincrono(prefixText);

            return asVendedores.ToArray();
        }

        /// <summary>
        /// Recuperar folios asincronamente
        /// </summary>
        /// <param name="prefixText"></param>
        /// <returns></returns>
        [WebMethod]
        public string[] RecuperarFolioPedidos(string prefixText)
        {

            MedDAL.Pedidos.DALPedidos odalPedidos = new MedDAL.Pedidos.DALPedidos();
            string[] asFolios = odalPedidos.BuscarFolioAsincrono(prefixText);

            return asFolios.ToArray();

        }

        /// <summary>
        /// Recuperar folios asincronamente
        /// </summary>
        /// <param name="prefixText"></param>
        /// <returns></returns>
        [WebMethod]
        public string[] RecuperarFolioRemisiones(string prefixText)
        {
            MedDAL.Remisiones.DALRemisiones odalRemisiones = new MedDAL.Remisiones.DALRemisiones();
            string[] asRemisiones = odalRemisiones.BuscarFolioAsincrono(prefixText);

            return asRemisiones.ToArray();
        }

        /// <summary>
        /// Recuperar folios asincronamente
        /// </summary>
        /// <param name="prefixText"></param>
        /// <returns></returns>
        [WebMethod]
        public string[] RecuperarFolioRecetas(string prefixText)
        {
            MedDAL.Recetas.DALRecetas odalRecetas = new MedDAL.Recetas.DALRecetas();
            string[] asRecetas = odalRecetas.BuscarFolioAsincrono(prefixText);

            return asRecetas.ToArray();
        }

        /// <summary>
        /// Recuperar medicos mediante su cedula profesional
        /// </summary>
        /// <param name="prefixText"></param>
        /// <returns></returns>
        [WebMethod]
        public string[] RecuperarCedulaVendedores(string prefixText)
        {
            MedDAL.Vendedores.DALVendedores odalVendedores = new MedDAL.Vendedores.DALVendedores();
            string[] asVendedores = odalVendedores.BuscarVendedorCedulaAsincrono(prefixText);

            return asVendedores.ToArray();
        }


        /// <summary>
        /// Recuperar las claves de los vendedores
        /// </summary>
        /// <param name="prefixText">Cadena contenida en la clave</param>
        /// <returns></returns>
        [WebMethod]
        public string[] RecuperarClaveVendedores(string prefixText)
        {
            MedDAL.Vendedores.DALVendedores odalVendedores = new MedDAL.Vendedores.DALVendedores();
            string[] asVendedores = odalVendedores.BuscarClaveVendedorAsincrono(prefixText);

            return asVendedores.ToArray();
        }

        /// <summary>
        /// Recuperar las claves y nombres de los CAUSES
        /// </summary>
        /// <param name="prefixText">Cadena contenida en la clave</param>
        /// <returns></returns>
        [WebMethod]
        public string[] RecuperarClaveCauses(string prefixText)
        {
            MedDAL.Causes.DALCauses odalCauses = new MedDAL.Causes.DALCauses();
            string[] asCauses = odalCauses.BuscarClaveCausesAsincrono(prefixText);

            return asCauses.ToArray();
        }

        /// <summary>
        /// Recuperar las claves y descripciones de los CIE
        /// </summary>
        /// <param name="prefixText">Cadena contenida en la clave</param>
        /// <returns></returns>
        [WebMethod]
        public string[] RecuperarClaveCausesCIE(string prefixText)
        {
            MedDAL.Causes.DALCauses odalCauses = new MedDAL.Causes.DALCauses();
            string[] asCauses = odalCauses.BuscarClaveDescripcionCausesCie(prefixText);

            return asCauses.ToArray();
        }

        /// <summary>
        /// Recuperar las claves de los proveedores
        /// </summary>
        /// <param name="prefixText">Cadena contenida en la clave</param>
        /// <returns></returns>
        [WebMethod]
        public string[] RecuperarClaveProveedores(string prefixText)
        {
            MedDAL.Proveedores.DALProveedores odalProveedores = new MedDAL.Proveedores.DALProveedores();
            string[] asVendedores = odalProveedores.BuscarProveedorAsincrono(prefixText);

            return asVendedores.ToArray();
        }

        /// <summary>
        /// Recuperar las claves de las lineas de credito
        /// </summary>
        /// <param name="prefixText">Cadena contenida en la clave</param>
        /// <returns></returns>
        [WebMethod]
        public string[] RecuperarClaveLineasCredito(string prefixText)
        {
            MedDAL.LineasCredito.DALLineasCredito odalLineasCreditos= new MedDAL.LineasCredito.DALLineasCredito();
            string[] asLineasCreditos= odalLineasCreditos.BuscarClaveLineaCreditoAsincrono(prefixText);

            return asLineasCreditos.ToArray();
        }

        /// <summary>
        /// Recuperar las claves de los almacenes
        /// </summary>
        /// <param name="prefixText">Cadena contenida en la clave</param>
        /// <returns></returns>
        [WebMethod]
        public string[] RecuperarClaveAlmacenes(string prefixText)
        {            
            MedDAL.Almacenes.DALAlmacenes odalAlmacenes = new MedDAL.Almacenes.DALAlmacenes();
            string[] asAlmacenes = odalAlmacenes.BuscarClaveAlmacenesAsincrono(prefixText);

            return asAlmacenes.ToArray();
        }

        /// <summary>
        /// Recuperar los Pedimentos de los movimientos
        /// </summary>
        /// <param name="prefixText">Cadena contenida en la clave</param>
        /// <returns></returns>
        [WebMethod]
        public string[] RecuperarPedimentos(string prefixText)
        {
            MedDAL.Inventarios.DALInventarios oDALInventarios = new MedDAL.Inventarios.DALInventarios();
            string[] asPedimentos = oDALInventarios.BuscarPedimentosAsincrono(prefixText);

            return asPedimentos.ToArray();
        }

        /// <summary>
        /// Recuperar las claves de los almacenes
        /// </summary>
        /// <param name="prefixText">Cadena contenida en la clave</param>
        /// <returns></returns>
        [WebMethod]
        public string[] RecuperarClaveAlmacenesUsuario(string prefixText)
        {
            MedDAL.Almacenes.DALAlmacenes odalAlmacenes = new MedDAL.Almacenes.DALAlmacenes();
            string[] asAlmacenes = odalAlmacenes.BuscarClaveAlmacenesAsincrono(prefixText);

            return asAlmacenes.ToArray();
        }

        /// <summary>
        /// Recuperar los folios de las factruas
        /// </summary>
        /// <param name="prefixText">Cadena contenida en el folio</param>
        /// <returns></returns>
        [WebMethod]
        public string[] RecuperarFolioFacturas(string prefixText)
        {
            MedDAL.Facturas.DALFacturas odalFacturas = new MedDAL.Facturas.DALFacturas();
            string[] asFacturas = odalFacturas.BuscarFolioFacturasAsincrono(prefixText);

            return asFacturas.ToArray();
        }

        /// <summary>
        /// Recuperar los tipos de almacen
        /// </summary>
        /// <param name="prefixText">Cadena contenida en el tipo</param>
        /// <returns></returns>
        [WebMethod]
        public string[] RecuperarTiposAlmacenes(string prefixText)
        {
            MedDAL.Tipos.DALTipos odalTipos = new MedDAL.Tipos.DALTipos();
            string[] asTipos= odalTipos.BuscarTipoAlmacenesAsincrono(prefixText);

            return asTipos.ToArray();
        }

        /// <summary>
        /// Recuperar los tipos de clientes
        /// </summary>
        /// <param name="prefixText">Cadena contenida en el tipo</param>
        /// <returns></returns>
        [WebMethod]
        public string[] RecuperarTiposClientes(string prefixText)
        {
            MedDAL.Tipos.DALTipos odalTipos = new MedDAL.Tipos.DALTipos();
            string[] asTipos = odalTipos.BuscarTipoClientesAsincrono(prefixText);

            return asTipos.ToArray();
        }

        /// <summary>
        /// Recuperar los tipos de productos
        /// </summary>
        /// <param name="prefixText">Cadena contenida en el tipo</param>
        /// <returns></returns>
        [WebMethod]
        public string[] RecuperarTiposProductos(string prefixText)
        {
            MedDAL.Tipos.DALTipos odalTipos = new MedDAL.Tipos.DALTipos();
            string[] asTipos = odalTipos.BuscarTipoProductosAsincrono(prefixText);

            return asTipos.ToArray();
        }

        /// <summary>
        /// Recuperar los tipos de proveedores
        /// </summary>
        /// <param name="prefixText">Cadena contenida en el tipo</param>
        /// <returns></returns>
        [WebMethod]
        public string[] RecuperarTiposProveedores(string prefixText)
        {
            MedDAL.Tipos.DALTipos odalTipos = new MedDAL.Tipos.DALTipos();
            string[] asTipos = odalTipos.BuscarTipoProveedoresAsincrono(prefixText);

            return asTipos.ToArray();
        }

        /// <summary>
        /// Recuperar los tipos de vendedores
        /// </summary>
        /// <param name="prefixText">Cadena contenida en el tipo</param>
        /// <returns></returns>
        [WebMethod]
        public string[] RecuperarTiposVendedores(string prefixText)
        {
            MedDAL.Tipos.DALTipos odalTipos = new MedDAL.Tipos.DALTipos();
            string[] asTipos = odalTipos.BuscarTipoVendedoresAsincrono(prefixText);

            return asTipos.ToArray();
        }

        /// <summary>
        /// Recuperar los tipos de recetas
        /// </summary>
        /// <param name="prefixText">Cadena contenida en el tipo</param>
        /// <returns></returns>
        [WebMethod]
        public string[] RecuperarTiposRecetas(string prefixText)
        {
            MedDAL.Tipos.DALTipos odalTipos = new MedDAL.Tipos.DALTipos();
            string[] asTipos = odalTipos.BuscarTipoRecetasAsincrono(prefixText);

            return asTipos.ToArray();
        }

        /// <summary>
        /// Recuperar el nombre los usuarios(vendedores en facturas)
        /// </summary>
        /// <param name="prefixText"></param>
        /// <returns></returns>
        [WebMethod]
        public string[] RecuperarFacturasVendedores(string prefixText)
        {
            MedDAL.Facturas.DALFacturas odalFacturas = new MedDAL.Facturas.DALFacturas();
            string[] asTipos = odalFacturas.BuscarVendedorAsincrono(prefixText);

            return asTipos.ToArray();
        }

        /// <summary>
        /// 0415 GT 15-Ago-11
        /// Recuperar el nombre los vendedores medicos
        /// </summary>
        /// <param name="prefixText"></param>
        /// <returns></returns>
        [WebMethod]
        public string[] RecuperarNombreVendedores(string prefixText)
        {
           
            MedDAL.Vendedores.DALVendedores odalVendedores = new MedDAL.Vendedores.DALVendedores();
            string[] asNombres = odalVendedores.BuscarNombreVendedorAsincrono2(prefixText);

            return asNombres.ToArray();
        }

    }
}
