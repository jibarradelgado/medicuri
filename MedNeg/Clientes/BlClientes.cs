using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MedNeg.BlClientes
{
    public class BlClientes
    {

        MedDAL.Clientes.DALClientes odalClientes;

        public BlClientes()
        {
            odalClientes = new MedDAL.Clientes.DALClientes();
        }


        // GT: 17/Mar/2011 Función que recupera un producto mediante su nombre, requerida para llenar autocompletar
        //                 en Pedidos,Remisiones y Facturas
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sNombre"></param>
        /// <returns></returns>
        public MedDAL.DAL.clientes BuscarClienteNombre(string sNombre)
        {
            return odalClientes.BuscarClienteNombre(sNombre);

        }

        /// <summary>
        /// Recuperar cliente por su id
        /// </summary>
        /// <param name="iIdCliente"></param>
        /// <returns></returns>
        public MedDAL.DAL.clientes BuscarCliente(int iIdCliente)
        {
            return odalClientes.BuscarCliente(iIdCliente);
        }

        /// <summary>
        /// BL - Buscar un cliente
        /// </summary>
        /// <param name="sCadena">Parametro a buscar</param>
        /// <param name="iTipo">Filtro</param>
        /// <returns></returns>
        public IQueryable<MedDAL.Clientes.ClientesView> Buscar(string sCadena, int iTipo)
        {
            return odalClientes.Buscar(sCadena, iTipo);
        }

        public MedDAL.DAL.clientes BuscarPorClave(string sClave)
        {
            return odalClientes.BuscarPorClave(sClave);
        }

        public MedDAL.DAL.clientes BuscarPorClaveNombreApellido(string sClave, string sNombre, string sApellido)
        {
            return odalClientes.BuscarPorClaveNombreApellido(sClave, sNombre, sApellido);
        }

        /// <summary>
        /// BL - Buscar un cliente por su nombre de cliente
        /// </summary>
        /// <param name="scliente">cliente</param>
        /// <returns></returns>
        public object Buscar()
        {
            return odalClientes.Buscar();
        }

        /// <summary>
        /// BL - Función que muestra todos los registros de la tabla
        /// </summary>
        /// <returns></returns>
        public IQueryable<MedDAL.Clientes.ClientesView> MostrarLista()
        {
            return odalClientes.MostrarLista();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IQueryable<MedDAL.DAL.clientes> BuscarEnum()
        {
            return odalClientes.BuscarEnum();
        }


        /// <summary>
        ///  BL - Registrar un cliente nuevo
        /// </summary>
        /// <param name="ocliente">cliente a registrar</param>
        /// <returns></returns>
        public bool NuevoRegistro(MedDAL.DAL.clientes oCliente)
        {
            return odalClientes.NuevoRegistro(oCliente);
        }

        /// <summary>
        /// BL - Editar un cliente
        /// </summary>
        /// <param name="ocliente">cliente a editar</param>
        /// <returns></returns>
        public bool EditarRegistro(MedDAL.DAL.clientes oCliente)
        {
            return odalClientes.EditarRegistro(oCliente);
        }

        /// <summary>
        /// BL - Eliminar un cliente
        /// </summary>
        /// <param name="iIdcliente">ID cliente a eliminar</param>
        /// <returns></returns>
        public bool EliminarRegistro(int iIdCliente)
        {
            return odalClientes.EliminarRegistro(iIdCliente);
        }

        /// <summary>
        /// BL -Validar un cliente por su nombre de cliente
        /// </summary>
        /// <param name="claveCliente">cliente</param>
        /// <returns></returns>
        public int ValidarClienteRepetido(string claveCliente)
        {
            return odalClientes.ValidarClienteRepetido(claveCliente);
        }
    }
}
