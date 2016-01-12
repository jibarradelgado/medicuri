using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MedNeg.ClientesContactos
{
    public class BlClientesContactos
    {
        MedDAL.ClientesContactos.DALClientesContactos odalClientesContactos;

        public BlClientesContactos(){
            odalClientesContactos = new MedDAL.ClientesContactos.DALClientesContactos();
        }

        /// <summary>
        /// Registra los contactos del cliente
        /// </summary>
        /// <param name="lstClientesContactos">lista de contactos a agregar</param>
        /// <returns>TRUE si se pudieron eliminar todos, FALSE si uno falla</returns>
        public bool NuevoRegistro(List<MedDAL.DAL.clientes_contacto> lstClientesContactos, int idCliente)
        {
            MedDAL.DAL.clientes_contacto nvo;
            bool result = true;
            foreach (MedDAL.DAL.clientes_contacto contacto in lstClientesContactos)
            {
                nvo = new MedDAL.DAL.clientes_contacto();
                nvo.idCliente = idCliente;
                nvo.Nombre = contacto.Nombre;
                nvo.Apellidos = contacto.Apellidos;
                nvo.Celular = contacto.Celular;
                nvo.Telefono = contacto.Telefono;
                nvo.CorreoElectronico = contacto.CorreoElectronico;
                result = result & odalClientesContactos.NuevoRegistro(nvo);
            }
            return result;
        }

        /// <summary>
        /// Elimina los contactos de un cliente
        /// </summary>
        /// <param name="lstClientesContactos">lista de contactos a eliminar</param>
        /// <returns>TRUE si se pudieron eliminar todos, FALSE si uno falla</returns>
        public bool EliminarRegistroContactos(List<MedDAL.DAL.clientes_contacto> lstClientesContactos) {
            bool result = true;
            foreach (MedDAL.DAL.clientes_contacto contacto in lstClientesContactos)
                result = result & odalClientesContactos.EliminarRegistro(contacto);
            return result;
        }

        /// <summary>
        /// Elimina los contactos de un cliente a partir de su ID
        /// </summary>
        /// <param name="idCliente">id del cliente</param>
        /// <returns></returns>
        public bool EliminarSimultaneos(int idCliente) {
            return odalClientesContactos.EliminarSimultaneos(idCliente);
        }

        /// <summary>
        /// Busca todos los contactos de un cliente
        /// </summary>
        /// <param name="idCliente">id del cliente</param>
        /// <returns>lista de contactos</returns>
        public List<MedDAL.DAL.clientes_contacto> BuscarContactos(int idCliente) {
            return odalClientesContactos.Buscar(idCliente);
        }

    }
}
